using UnityEngine;
using System.Collections;

public enum SyncQuality
{
	Bad,
	Acceptable,
	Perfect
}

public class SyncFeedback : MonoBehaviour 
{
	public ToneGenerator Alien;
	public ToneGenerator Ship;
	public TextGlitch 	 Text;
	public Renderer 	 Render;

	static float FreqTolerance = 10;
	static float ModTolerance  = 0.8f;


	public SyncQuality state 
	{
		get; private set;
	}

	public bool hideFreqColor
	{
		get; set;
	}

	public bool hideRateColor
	{
		get; set;
	}

	public float glitchAmount
	{
		get 
		{
			return Text.Frequency;
		}

		set 
		{
			Text.Frequency = value;
		}
	}

	public Color freqColor
	{
		get 
		{
			if ( hideFreqColor ) return Color.grey;

			if ( WithinTolerance(Alien.oscil.Frequency, Ship.oscil.Frequency, 0) )
			{
				return Color.green;
			}

			if ( WithinTolerance(Alien.oscil.Frequency, Ship.oscil.Frequency, FreqTolerance) )
			{
				return Color.yellow;
			}

			return Color.red;
		}
	}

	public Color rateColor
	{
		get 
		{
			if ( hideRateColor ) return Color.grey;

			if ( WithinTolerance(Alien.mod.Frequency, Ship.mod.Frequency, 0.01f) )
			{
				return Color.green;
			}

			if ( WithinTolerance(Alien.mod.Frequency, Ship.mod.Frequency, ModTolerance) )
			{
				return Color.yellow;
			}

			return Color.red;
		}
	}

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
		if ( 	WithinTolerance(Alien.oscil.Frequency, Ship.oscil.Frequency, 0)
			&&  WithinTolerance(Alien.mod.Frequency, Ship.mod.Frequency, 0.01f) )
		{
			Render.material.color = Color.green;
			Text.Text = "SYNC: PERF";
			state = SyncQuality.Perfect;
		}
		else if ( WithinTolerance(Alien.oscil.Frequency, Ship.oscil.Frequency, FreqTolerance) 
			   && WithinTolerance(Alien.mod.Frequency, Ship.mod.Frequency, ModTolerance) )
		{
			Render.material.color = Color.yellow;
			Text.Text = "SYNC: PASS";
			state = SyncQuality.Acceptable;
		}
		else
		{
			Render.material.color = Color.red;
			Text.Text = "SYNC: POOR";
			state = SyncQuality.Bad;
		}

	}
}
