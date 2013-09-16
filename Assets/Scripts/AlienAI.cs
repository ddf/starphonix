using UnityEngine;
using System.Collections;

public class AlienAI : MonoBehaviour 
{
	public ToneGenerator 	Tone;
	public Renderer[] 	 	MeterSections;
	public TextGlitch 		MeterText;
	public ConsoleReadout 	Console;
	public FlavorText 		Flavor;
	public SyncFeedback     Feedback;
	public CommLog 			CommLog;
	public LowPassGlitch    Glitch;
	public ChromaticGlitch  ChromaGlitch;
	public CameraMeltdown 	Meltdown;
	public AudioSource 		ThinkSound;
	public AudioSource 	    AcceptSound;
	public AudioSource 		BeginNegoshSound;
	public AudioClip[] 		WarScreams;
	public AudioClip 		DeathScream;
	public AudioClip		TruceScream;
	public AudioClip 		PeaceScream;
	public Color 			HappyColor;
	public Color 			AngryColor;
	public Color 			PeaceColor;
	public string[] 		Planets;

	public static int   MinFreq = 350;
	public static int   MaxFreq = 500;
	public static float MinMod  = 0.5f;
	public static float MaxMod  = 3.8f;

	public bool transmitting 
	{
		get; private set;
	}

	public float m_duration;
	public float m_timer;


	Color 	 	m_litColor;

	float 		m_thinkAnimTimer;
	int   		m_thinkAnimIdx;
	string[]	m_thinkAnim = new string[] { "|", "/", "-", "\\", };

	float 		m_processAnimTimer;
	int   		m_processAnimIdx;
	string[]	m_processAnim = new string[] { "#", "##", "###", "####", };

	// what are we thinking about
	string m_thinkString;

	// which planet we are negotiating about
	int   m_planetIdx;
	// how long we wait for the player to get it right
	float m_patience;
	// what the judgement of latest transmission was
	SyncQuality m_xmitSync;

	Light 		m_light;
	Lerper 		m_lightLerper 	 = new Lerper(EasingType.ExpoEaseIn);
	Color 		m_lightColor;
	Color 		m_lightColorTarget;
	Lerper  	m_lightColorLerp = new Lerper(EasingType.BackEaseOut);

	Vector3     m_startPos;
	Vector3 	m_wanderTo;
	Vector3		m_wanderFrom;
	float 		m_wanderTimer;
	Lerper 		m_wanderLerp = new Lerper(EasingType.ExpoEaseInOut);

	int 		m_screamIdx;

	public static float MAX_PATIENCE 		   = 100;

	public static float INACTION_PENALTY 	   = 10;
	public static float POOR_SYNC_PENALTY	   = 20;
	public static float PASS_SYNC_PENALTY	   = 6;
	public static float PERF_SYNC_PENALTY 	   = 2;

	public static int 	 MAX_FAILS     		   = 6;
	public static int   MAX_SUCCESS  		   = 6;
	public static int   MIN_SUCCESS_FOR_TRUCE  = 2;
	public static int 	 MAX_FAILS_FOR_PEACE   = 2;

	public static int   TREATIES_WITH_FREQ_COLOR = 2;
	public static int   TREATIES_WITH_RATE_COLOR = 4;

	int m_totalFails   = 0;
	int m_totalSuccess = 0;

	bool m_bNegotiationComplete;

	// Use this for initialization
	void Start () 
	{
		m_light = GetComponentInChildren<Light>();
		m_lightColor = HappyColor;
		m_light.color = HappyColor;

		m_startPos = transform.position;
		m_wanderTimer = 3;

		m_litColor = MeterSections[0].material.color;
		Tone.oscil.Wave = new Square();
		
		m_planetIdx = -1;
		m_patience  = MAX_PATIENCE;

		transmitting = false;

		Console.PushLine( "PROTOCOL: SONIC SYNC" );
		Console.PushLine( "- TUNE FREQ AND RATE" );
		Console.PushLine( "  TO SYNC SONICS" );
		Console.PushLine( "- XMIT PASS/PERF SYNC" );
		Console.PushLine( "- DO NOT XMIT POOR SYNC!" );
		Console.PushLine( "" );

		m_thinkString = "EST COMM";
		m_timer  	  = 4;

		WarScreams.Shuffle();
		Planets.Shuffle();

		BeginThink();
	}

	void BeginThink()
	{
		m_thinkAnimTimer  = 1;
		m_thinkAnimIdx    = 0;
		Console.PushLine( m_thinkString + ": " + m_thinkAnim[0] );
	}

	IEnumerator Configure( float waitFor )
	{
		if ( m_patience > 0 )
		{
			// if we failed to much, we will either meltdown 
			// or temporarily cease hostilities
			// depending on how many success there were
			if ( m_totalFails == MAX_FAILS )
			{
				if ( m_totalSuccess >= MIN_SUCCESS_FOR_TRUCE )
				{
					DoTruce();
				}
				else 
				{
					DoMeltdown();
				}
			}
			// if the player succeed enough, we will either
			// declare truce or declare peace
			// depending on how much they failed
			else if ( m_totalSuccess == MAX_SUCCESS )
			{
				if ( m_totalFails <= MAX_FAILS_FOR_PEACE )
				{
					DoPeace();
				}
				else 
				{
					DoTruce();
				}
			}
			else 
			{	
				if ( m_patience < 20 )
				{
					Console.PushLine( "WARNING!" );
					Console.PushLine( "COMM BREAKDOWN IMMINENT!" );
				}

				m_lightColor = m_light.color;
				m_lightColorTarget = Color.Lerp( AngryColor, HappyColor, m_patience / MAX_PATIENCE );
				m_lightColorLerp.Begin( 0, 1, 0.5f );

				yield return new WaitForSeconds( waitFor );

				Tone.oscil.Frequency = Random.Range(MinFreq, MaxFreq);
				Tone.mod.Frequency   = Mathf.Round( Random.Range(MinMod, MaxMod)*10 ) / 10;
				
				m_planetIdx++;
				Console.PushLine( " " );
				Console.PushLine( "SYS: " + Planets[m_planetIdx] );
				Console.PushLine( "POP: " + Random.Range( 7, 950 ) + ".000.000.000" );
				m_thinkString = "TUNING";
				BeginThink();

				BeginNegoshSound.Play();

				int treatiesCompleted = m_totalFails+m_totalSuccess;

				Feedback.hideFreqColor = treatiesCompleted >= TREATIES_WITH_FREQ_COLOR;
				Feedback.hideRateColor = treatiesCompleted >= TREATIES_WITH_RATE_COLOR;
				Feedback.glitchAmount  = Extensions.Map( treatiesCompleted, 0, MAX_FAILS + MAX_SUCCESS, 0.1f, 0.8f );

				yield return new WaitForSeconds( 0.75f );
				
				transmitting = true;

				m_duration = m_timer = m_patience;

				print( "timer set to " + m_timer );				
			}
		}
		// run out of patience always meltsdown
		else 
		{
			DoMeltdown();
		}
	}

	void DoMeltdown()
	{
		m_bNegotiationComplete = true;

		Glitch.LongGlitch();
		ChromaGlitch.Break();
		Meltdown.Trigger();

		m_lightColor = m_light.color;
		m_lightColorTarget = AngryColor;
		m_lightLerper.Begin( 0, 1, 0.1f );

		audio.PlayOneShot( DeathScream );

		StartCoroutine( DeferredLevelLoad(DeathScream.length) );
	}

	void DoTruce()
	{
		m_bNegotiationComplete = true;

		Glitch.War();
		ChromaGlitch.War();

		audio.PlayOneShot( TruceScream );

		Flavor.Disconnect();
		Console.PushLine( "COMM TERMINATED" );
		Console.PushLine( "HOSTILITIES TEMPORARILY" );
		Console.PushLine( "SUSPENDED");

		m_wanderFrom = transform.position;
		m_wanderTo   = m_startPos + new Vector3( 10, 30, 0.1f );
		m_wanderLerp.Begin( 0, 1, 4 );

		StartCoroutine( DeferredLevelLoad( 4 ) );
	}

	void DoPeace()
	{
		m_bNegotiationComplete = true;

		m_lightColor = m_light.color;
		m_lightColorTarget = PeaceColor;
		m_lightColorLerp.Begin( 0, 1, 1.0f );

		audio.PlayOneShot( PeaceScream );

		Flavor.Disconnect();
		Console.PushLine( "COMM TERMINATED" );
		Console.PushLine( "PEACEFUL RESOLUTION" );

		m_wanderFrom = transform.position;
		m_wanderTo   = m_startPos + new Vector3( -15, 30, 0.1f );
		m_wanderLerp.Begin( 0, 1, 4 );

		StartCoroutine( DeferredLevelLoad( 4 ) );
	}

	IEnumerator DeferredLevelLoad( float waitFor )
	{
		yield return new WaitForSeconds(waitFor);

		Application.LoadLevel( "Main" );
	}

	void Fail()
	{
		if ( m_planetIdx >= 0 )
		{
			Flavor.Failure();

			Console.ReplaceLastLine( "OUTCOME: WAR" );
			Console.PushLine( "CASUALTIES: " + Random.Range( 60, 101 ) + "%" );
			m_totalFails++;

			// only do normal vfx and sound if we aren't going to trigger an endgame
			if ( m_patience > 0 && m_totalFails < MAX_FAILS )
			{
				Glitch.War();
				ChromaGlitch.War();

				audio.PlayOneShot( WarScreams[m_screamIdx] );
				m_screamIdx = (m_screamIdx+1) % WarScreams.Length;
			}

			m_lightLerper.Begin( 5, 0.84f, 1 );

			CommLog.AddFailure();

			StartCoroutine( Configure(1.2f) );
		}
	}

	void Success()
	{
		Flavor.Success();
		Console.ReplaceLastLine( "OUTCOME: CEASE FIRE" );
		AcceptSound.Play();
		CommLog.AddSuccess();

		m_totalSuccess++;

		StartCoroutine( Configure(1.0f) );
	}

	void RenderJudgement()
	{		
		switch( m_xmitSync )
		{
			// if player transmits a bad sync it's even worse than just timing out
			// transmitting bad syncs should cause the alien to lose patience
			// before the player reaches 10 failures.
			case SyncQuality.Bad:
			{
				m_patience -= POOR_SYNC_PENALTY;
				Fail();
			}
			break;

			// the alien still gets annoyed with this
			// but it's not as bad as being ignored.
			case SyncQuality.Acceptable:
			{
				m_patience -= PASS_SYNC_PENALTY;
				Success();
			}
			break;

			// and, finally, a perfect sync doesn't hurt
			// much at all, but needs to put a bit 
			// of a dent so difficulty ramps a bit
			case SyncQuality.Perfect:
			{
				m_patience -= PERF_SYNC_PENALTY;
				Success();
			}
			break;
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		// animation
		{
			transform.localScale = Vector3.one + Vector3.one * Tone.mod.value * 0.25f;

			if ( !m_lightColorLerp.done )
			{
				m_lightColorLerp.Update();

				m_light.color = Color.Lerp( m_lightColor, m_lightColorTarget, m_lightColorLerp.value );
			}

			if ( !m_lightLerper.done )
			{
				m_lightLerper.Update();

				m_light.intensity = m_lightLerper.value;
			}

			//float rotSpeed = Extensions.Map( m_patience, MAX_PATIENCE, 0, 1, 100 );
			float rotSpeed = Easing.Expo.easeIn( 1 - m_patience/MAX_PATIENCE, 1, 200, 1 );

			transform.Rotate( new Vector3( 0.5f, 2, 0.2f ) * Time.deltaTime * rotSpeed );

			// don't wander again 
			if ( !m_bNegotiationComplete && m_wanderTimer > 0 )
			{
				m_wanderTimer -= Time.deltaTime;

				if ( m_wanderTimer <= 0 )
				{
					m_wanderFrom = transform.position;
					m_wanderTo   = m_startPos + new Vector3( Random.Range(-1,1), 0, Random.Range(-1,1) );
					m_wanderLerp.Begin( 0, 1, 3.25f );
				}
			}

			if ( !m_wanderLerp.done )
			{
				m_wanderLerp.Update();

				transform.position = Extensions.Lerp( m_wanderFrom, m_wanderTo, m_wanderLerp.value );

				if ( m_wanderLerp.done )
				{
					m_wanderTimer = 4;
				}
			}
		}

		if ( !m_bNegotiationComplete )
		{
			if ( m_timer > 0 )
			{
				m_timer -= Time.deltaTime;

				if ( m_thinkAnimTimer > 0 )
				{
					m_thinkAnimTimer -= Time.deltaTime;

					if ( m_thinkAnimTimer <= 0 )
					{
						m_thinkAnimIdx = (m_thinkAnimIdx + 1) % m_thinkAnim.Length;
						Console.ReplaceLastLine( m_thinkString + ": " + m_thinkAnim[m_thinkAnimIdx] );
						ThinkSound.Play();

						m_thinkAnimTimer = 1;
					}
				}

				if ( transmitting )
				{
					MeterText.Text = m_timer.ToString("000");
				}

				if ( m_timer <= 0 )
				{
					if ( m_planetIdx < 0 )
					{
						Console.ReplaceLastLine( "COMM BEGIN" );
						StartCoroutine( Configure(1) );
					}
					else 
					{
						m_patience -= INACTION_PENALTY;
						Fail();
					}
				}
			}

			if ( m_processAnimTimer > 0 )
			{
				m_processAnimTimer -= Time.deltaTime;

				if ( m_processAnimTimer <= 0 )
				{
					m_processAnimIdx++;
					if ( m_processAnimIdx == m_processAnim.Length )
					{
						RenderJudgement();
					}
					else
					{
						Console.ReplaceLastLine( "OUTCOME: " + m_processAnim[m_processAnimIdx] );
						m_processAnimTimer = 0.5f;
						ThinkSound.Play();
					}
				}
			}

			int litPanels = (int)((MeterSections.Length+1) * (m_timer/m_duration));
			for( int p = 0; p < MeterSections.Length; ++p )
			{
				if ( litPanels > p )
				{
					MeterSections[p].material.color = m_litColor;
				}
				else 
				{
					MeterSections[p].material.color = m_litColor * 0.4f;
				}
			}
		}
	}

	public void ReceiveXMit()
	{
		Console.PushLine( "EXEC XMIT" );
		Tone.Stop();

		m_xmitSync = Feedback.state;
		m_timer    = 0;
		m_processAnimTimer = 0.5f;
		m_processAnimIdx   = 0;
		MeterText.Text = "###";

		Console.PushLine( "OUTCOME: " + m_processAnim[m_processAnimIdx] );

		transmitting = false;
	}
}
