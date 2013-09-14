using UnityEngine;
using System.Collections;

public class CraftControl : MonoBehaviour 
{
	public ToneGenerator Alien;
	public ToneGenerator Ship;

	public TextMesh FreqDisplay;
	public TextMesh RateDisplay;

	float m_timeToTrigger = 0.01f;

	// Use this for initialization
	void Start () 
	{
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

		FreqDisplay.text = ( Random.value > 0.1f ? Ship.oscil.Frequency + "" : "*x&#-" );
		RateDisplay.text = ( Random.value > 0.1f ? Ship.mod.Frequency + "" : "*x&#-" );
	}
}
