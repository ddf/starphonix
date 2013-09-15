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
	Color m_litColor;

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

	// Use this for initialization
	void Start () 
	{
		m_litColor = MeterSections[0].material.color;
		Tone.oscil.Wave = new Square();
		
		m_planetIdx = -1;
		m_patience  = 120;

		transmitting = false;

		m_thinkString = "EST COMM";
		m_timer  	  = 4;

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
			Console.PushLine( "POP: " + Random.Range( 7, 950 ) + "000000000" );
			m_thinkString = "CALIBRATING";
			BeginThink();
			
			transmitting = true;
		}
		else 
		{
			Glitch.LongGlitch();
			ChromaGlitch.Break();
			StartCoroutine( DeferredLevelLoad() );
		}
	}

	IEnumerator DeferredLevelLoad()
	{
		yield return new WaitForSeconds(1);

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
			}

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
		Console.PushLine( "OUTCOME: CEASE FIRE" );
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
