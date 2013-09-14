using UnityEngine;
using System.Collections;

public class LowPassGlitch : MonoBehaviour 
{
	AudioLowPassFilter m_filter;

	float m_defaultCutoff;
	float m_timer;
	float m_glitchTimer;

	// Use this for initialization
	void Start () 
	{
		m_filter = GetComponent<AudioLowPassFilter>();
		m_defaultCutoff = m_filter.cutoffFrequency;

		m_timer = 10;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if ( m_timer > 0 )
		{
			m_timer -= Time.deltaTime;

			if ( m_timer <= 0 )
			{
				m_filter.cutoffFrequency = Random.Range( 4000, 8000 );
				m_glitchTimer = Random.Range( 0.05f, 0.2f );
				renderer.enabled = true;
			}
		}

		if ( m_glitchTimer > 0 )
		{
			m_glitchTimer -= Time.deltaTime;

			if ( m_glitchTimer <= 0 )
			{
				m_filter.cutoffFrequency = m_defaultCutoff;
				if ( Random.value > 0.2f )
				{
					m_timer = Random.Range( 8, 16 );
				}
				else 
				{
					m_timer = Random.Range( 0.25f, 0.5f );
				}

				renderer.enabled = false;
			}
		}
	}
}
