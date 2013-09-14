using UnityEngine;
using System.Collections;

public class CraftControl : MonoBehaviour 
{
	public AlienAI 		 Alien;
	public ToneGenerator Ship;

	public TextGlitch FreqDisplay;
	public TextGlitch RateDisplay;

	float m_timeToTrigger = 0.01f;

	// Use this for initialization
	void Start () 
	{
		Ship.oscil.Wave = new Square();
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

		if ( Input.GetKeyDown("q") )
		{
			Ship.oscil.Frequency -= 1;
		}
		else if ( Input.GetKeyDown("w") )
		{
			Ship.oscil.Frequency += 1;
		}

		Ship.oscil.Frequency = Mathf.Clamp( Ship.oscil.Frequency, AlienAI.MinFreq - 10, AlienAI.MaxFreq + 10 );

		if ( Input.GetKeyDown("a") )
		{
			Ship.mod.Frequency = Mathf.Clamp( Ship.mod.Frequency - 0.1f, AlienAI.MinMod - 0.2f, AlienAI.MaxMod + 0.2f );
		}
		else if ( Input.GetKeyDown("s") )
		{
			Ship.mod.Frequency = Mathf.Clamp( Ship.mod.Frequency + 0.1f, AlienAI.MinMod - 0.2f, AlienAI.MaxMod + 0.2f );
		}

		FreqDisplay.Text = Ship.oscil.Frequency + "";
		RateDisplay.Text = Ship.mod.Frequency.ToString("0.0");

		if ( Input.GetKeyDown("space") )
		{
			audio.Play();
			Alien.ReceiveXMit();
			Ship.Stop();
			FreqDisplay.Text = "###";
			RateDisplay.Text = "###";

			m_timeToTrigger = 0.5f;
		}
	}
}
