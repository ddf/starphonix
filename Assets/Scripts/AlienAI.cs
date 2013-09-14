using UnityEngine;
using System.Collections;

public class AlienAI : MonoBehaviour 
{
	public ToneGenerator Alien;
	public Renderer[] 	 MeterSections;

	public static int   MinFreq = 350;
	public static int   MaxFreq = 500;
	public static float MinMod  = 0.5f;
	public static float MaxMod  = 3.8f;

	float m_duration;
	float m_timer;
	Color m_litColor;

	// Use this for initialization
	void Start () 
	{
		m_litColor = MeterSections[0].material.color;
		Alien.oscil.Wave = new Square();
		Configure();
	}

	void Configure()
	{
		Alien.oscil.Frequency = Random.Range(MinFreq, MaxFreq);
		Alien.mod.Frequency   = Mathf.Round( Random.Range(MinMod, MaxMod)*10 ) / 10;

		m_duration = m_timer = 120;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if ( m_timer > 0 )
		{
			m_timer -= Time.deltaTime;

			if ( m_timer <= 0 )
			{
				Configure();
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
