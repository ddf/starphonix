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
	public LowPassGlitch    Glitch;
	public ChromaticGlitch  ChromaGlitch;
	public CameraMeltdown 	Meltdown;
	public AudioSource 		ThinkSound;
	public AudioSource 	    AcceptSound;
	public AudioClip[] 		WarScreams;
	public AudioClip 		DeathScream;
	public Color 			HappyColor;
	public Color 			AngryColor;
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
	Lerper 		m_lightLerper = new Lerper(EasingType.ExpoEaseIn);

	Vector3     m_startPos;
	Vector3 	m_wanderTo;
	Vector3		m_wanderFrom;
	float 		m_wanderTimer;
	Lerper 		m_wanderLerp = new Lerper(EasingType.ExpoEaseInOut);

	int 		m_screamIdx;

	static float MAX_PATIENCE = 180;

	// Use this for initialization
	void Start () 
	{
		m_light = GetComponentInChildren<Light>();
		m_startPos = transform.position;
		m_wanderTimer = 3;

		m_litColor = MeterSections[0].material.color;
		Tone.oscil.Wave = new Square();
		
		m_planetIdx = -1;
		m_patience  = MAX_PATIENCE;

		transmitting = false;

		m_thinkString = "EST COMM";
		m_timer  	  = 4;

		WarScreams.Shuffle();

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
			if ( m_patience < 20 )
			{
				Console.PushLine( "WARNING!" );
				Console.PushLine( "COMM BREAKDOWN IMMINENT!" );
			}

			yield return new WaitForSeconds( waitFor );

			Tone.oscil.Frequency = Random.Range(MinFreq, MaxFreq);
			Tone.mod.Frequency   = Mathf.Round( Random.Range(MinMod, MaxMod)*10 ) / 10;

			m_duration  	  = m_timer = m_patience;
			
			m_planetIdx++;
			Console.PushLine( "SYS: " + Planets[m_planetIdx] );
			Console.PushLine( "POP: " + Random.Range( 7, 950 ) + ".000.000.000" );
			m_thinkString = "CALIBRATING";
			BeginThink();
			
			transmitting = true;
		}
		else 
		{
			Glitch.LongGlitch();
			ChromaGlitch.Break();
			Meltdown.Trigger();
			audio.PlayOneShot( DeathScream );
			StartCoroutine( DeferredLevelLoad() );
		}
	}

	IEnumerator DeferredLevelLoad()
	{
		yield return new WaitForSeconds(2.075f);

		Application.LoadLevel( "Main" );
	}

	void Fail()
	{
		if ( m_planetIdx >= 0 )
		{
			Flavor.Failure();

			Console.ReplaceLastLine( "OUTCOME: WAR" );
			Console.PushLine( "CASUALTIES: " + Random.Range( 60, 101 ) + "%" );
			m_patience -= 5;

			if ( m_patience > 0 )
			{
				Glitch.War();
				ChromaGlitch.War();

				audio.PlayOneShot( WarScreams[m_screamIdx] );
				m_screamIdx = (m_screamIdx+1) % WarScreams.Length;
			}

			m_lightLerper.Begin( 5, 0.84f, 1 );

			StartCoroutine( Configure(1.2f) );
		}
		else 
		{
			Console.PushLine( "COMM BEGIN" );
			Console.PushLine( "XMIT WHEN SYNC'D" );

			StartCoroutine( Configure(0) );
		}
	}

	void Success()
	{
		Flavor.Success();
		Console.ReplaceLastLine( "OUTCOME: CEASE FIRE" );
		AcceptSound.Play();
		StartCoroutine( Configure(0.5f) );
	}

	void RenderJudgement()
	{		
		switch( m_xmitSync )
		{
			// if player transmits a bad sync it's even worse than just timing out
			case SyncQuality.Bad:
			{
				m_patience -= 5;
				Fail();
			}
			break;

			case SyncQuality.Acceptable:
			{
				// just a little miffed
				m_patience -= 3;
				Success();
			}
			break;

			case SyncQuality.Perfect:
			{
				m_patience -= 1;
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

			m_light.color = Color.Lerp( AngryColor, HappyColor, m_patience / MAX_PATIENCE );

			if ( !m_lightLerper.done )
			{
				m_lightLerper.Update();

				m_light.intensity = m_lightLerper.value;
			}

			//float rotSpeed = Extensions.Map( m_patience, MAX_PATIENCE, 0, 1, 100 );
			float rotSpeed = Easing.Expo.easeIn( 1 - m_patience/MAX_PATIENCE, 1, 200, 1 );

			transform.Rotate( new Vector3( 0.5f, 2, 0.2f ) * Time.deltaTime * rotSpeed );

			if ( m_wanderTimer > 0 )
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
				Fail();
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
