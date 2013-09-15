using UnityEngine;
using System.Collections;

public class CraftControl : MonoBehaviour 
{
	public AlienAI 		 Alien;
	public ToneGenerator Ship;
	public SyncFeedback  Feedback;

	public TextGlitch FreqDisplay;
	public TextGlitch RateDisplay;

	public AudioSource IncrementSound;
	public AudioSource DecrementSound;

	float m_timeToTrigger = 0.01f;

	class ValueController
	{
		public static AudioSource IncrementSound;
		public static AudioSource DecrementSound;

		string m_decKey;
		string m_incKey;

		float m_minVal;
		float m_maxVal;
		float m_incVal;

		enum Direction
		{
			Down,
			Up,
			None
		} 

		Direction m_direction;

		float m_timer;

		public ValueController( string decKey, string incKey, float minVal, float maxVal, float incVal )
		{
			m_decKey = decKey;
			m_incKey = incKey;
			m_minVal = minVal;
			m_maxVal = maxVal;
			m_incVal = incVal;
			m_timer  = 0.01f;
		}

		public float Update( float currentValue )
		{
			if ( Input.GetKey(m_decKey) )
			{
				m_direction = Direction.Down;

				if ( Input.GetKeyDown(m_decKey) )
				{
					m_timer = 0.01f;
				}
			}
			else if ( Input.GetKey(m_incKey) )
			{
				m_direction = Direction.Up;

				if ( Input.GetKeyDown(m_incKey) )
				{
					m_timer = 0.01f;
				}
			}
			else
			{
				m_direction = Direction.None;
			}

			if ( m_timer > 0 )
			{
				m_timer -= Time.deltaTime;

				if ( m_timer <= 0 )
				{
					if ( m_direction == Direction.Down && currentValue > m_minVal )
					{
						currentValue -= m_incVal;
						DecrementSound.Play();
					}
					else if ( m_direction == Direction.Up && currentValue < m_maxVal )
					{
						currentValue += m_incVal;
						IncrementSound.Play();
					}

					currentValue = Mathf.Clamp( currentValue, m_minVal, m_maxVal );

					m_timer = 0.15f;
				}
			}

			return currentValue;
		}

	}

	ValueController	 	m_freqController;
	ValueController   	m_rateController;

	// Use this for initialization
	void Start () 
	{
		Ship.oscil.Wave = new Square();

		ValueController.IncrementSound = IncrementSound;
		ValueController.DecrementSound = DecrementSound;

		m_freqController = new ValueController( "q", "w", AlienAI.MinFreq - 10, AlienAI.MaxFreq + 10, 1 );
		m_rateController = new ValueController( "a", "s", AlienAI.MinMod - 0.2f, AlienAI.MaxMod + 0.2f, 0.1f );
	}
	
	// Update is called once per frame
	void Update () 
	{
		if ( !Alien.transmitting ) return;

		if ( m_timeToTrigger > 0 )
		{
			m_timeToTrigger -= Time.deltaTime;

			if ( m_timeToTrigger <= 0 )
			{
				Alien.Tone.Trigger();
				Ship.Trigger();

				m_timeToTrigger = 6;
			}
		}

		Ship.oscil.Frequency = m_freqController.Update( (int)Ship.oscil.Frequency );
		Ship.mod.Frequency   = m_rateController.Update( Ship.mod.Frequency );

		FreqDisplay.Text = Ship.oscil.Frequency + "";
		FreqDisplay.color = Feedback.freqColor;

		RateDisplay.Text = Ship.mod.Frequency.ToString("0.0");
		RateDisplay.color = Feedback.rateColor;

		if ( Input.GetKeyDown("space") )
		{
			audio.Play();
			Alien.ReceiveXMit();
			Ship.Stop();
			
			FreqDisplay.Text = "###";
			FreqDisplay.color = Color.white;

			RateDisplay.Text = "###";
			RateDisplay.color = Color.white;

			m_timeToTrigger = 0.5f;
		}
	}
}
