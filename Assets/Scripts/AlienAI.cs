using UnityEngine;
using System.Collections;

public class AlienAI : MonoBehaviour 
{
	public ToneGenerator 	Tone;
	public Renderer[] 	 	MeterSections;
	public ConsoleReadout 	Console;
	public SyncFeedback     Feedback;
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

		Configure();
	}

	void Configure()
	{
		Tone.oscil.Frequency = Random.Range(MinFreq, MaxFreq);
		Tone.mod.Frequency   = Mathf.Round( Random.Range(MinMod, MaxMod)*10 ) / 10;

		m_duration  	  = m_timer = m_patience;
		m_thinkAnimTimer  = 1;
		m_thinkAnimIdx    = 0;

		m_planetIdx++;

		Console.PushLine( Planets[m_planetIdx] + ": " + m_thinkAnim[0] );

		transmitting = true;
	}

	void Fail()
	{
		Console.ReplaceLastLine( Planets[m_planetIdx] + ": WAR" );

		m_patience -= 5;

		Configure();
	}

	void Success()
	{
		Console.ReplaceLastLine( Planets[m_planetIdx] + ": CEASE FIRE" );
		Configure();	
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
					Console.ReplaceLastLine( Planets[m_planetIdx] + ": " + m_thinkAnim[m_thinkAnimIdx] );

					m_thinkAnimTimer = 1;
				}
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
					Console.ReplaceLastLine( Planets[m_planetIdx] + ": " + m_processAnim[m_processAnimIdx] );
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

		Console.PushLine( Planets[m_planetIdx] + ": " + m_processAnim[m_processAnimIdx] );

		transmitting = false;
	}
}
