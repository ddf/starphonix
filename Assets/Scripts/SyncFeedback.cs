using UnityEngine;
using System.Collections;

public class SyncFeedback : MonoBehaviour 
{
	public ToneGenerator Alien;
	public ToneGenerator Ship;
	public Renderer 	 Render;

	static float FreqTolerance = 6;
	static float ModTolerance  = 0.5f;

	// Use this for initialization
	void Start () 
	{
		
	}

	bool WithinTolerance( float v1, float v2, float t )
	{
		if ( Mathf.Abs(v1-v2) <= t )
		{
			return true;
		}

		return false;
	}

	// Update is called once per frame
	void Update () 
	{
		if ( 	WithinTolerance(Alien.oscil.Frequency, Ship.oscil.Frequency, 2 )
			&&  WithinTolerance(Alien.mod.Frequency, Ship.mod.Frequency, 0.1f ) )
		{
			Render.material.color = Color.green;
		}
		else if ( WithinTolerance(Alien.oscil.Frequency, Ship.oscil.Frequency, FreqTolerance) 
			   && WithinTolerance(Alien.mod.Frequency, Ship.mod.Frequency, ModTolerance) )
		{
			Render.material.color = Color.yellow;
		}
		else
		{
			Render.material.color = Color.red;
		}

	}
}
