using UnityEngine;
using System.Collections;

public class CraftControl : MonoBehaviour 
{
	public ToneGenerator Alien;
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
		if ( m_timeToTrigger > 0 )
		{
			m_timeToTrigger -= Time.deltaTime;

			if ( m_timeToTrigger <= 0 )
			{
				Alien.Trigger();
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

		if ( Input.GetKeyDown("a") )
		{
			Ship.mod.Frequency = Mathf.Clamp( Ship.mod.Frequency - 0.1f, 0.1f, 4 );
		}
		else if ( Input.GetKeyDown("s") )
		{
			Ship.mod.Frequency = Mathf.Clamp( Ship.mod.Frequency + 0.1f, 0.1f, 4 );
		}

		FreqDisplay.Text = Ship.oscil.Frequency + "";
		RateDisplay.Text = Ship.mod.Frequency.ToString("0.0");
	}
}
