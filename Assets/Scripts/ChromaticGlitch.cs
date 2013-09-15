using UnityEngine;
using System.Collections;

public class ChromaticGlitch : MonoBehaviour 
{
	Vignetting m_vignette;

	float m_defaultChroma;
	float m_timer;

	Lerper m_chromaLerp = new Lerper(EasingType.ExpoEaseOut);

	public void Break()
	{
		m_defaultChroma = 50;
		m_chromaLerp.Begin( 50, 75, 0.25f );
		m_vignette.chromaticAberration = 50;
		m_timer = 0;
	}

	public void War()
	{
		m_chromaLerp.Begin( 40, 55, 0.25f );
		m_timer = 0;
	}

	// Use this for initialization
	void Start () 
	{
		m_vignette = GetComponent<Vignetting>();

		m_defaultChroma = m_vignette.chromaticAberration;

		Glitch();
	}

	void Glitch()
	{
		float offset = Random.Range( 2.5f, 10.0f );
		m_vignette.chromaticAberration = m_defaultChroma + offset;
		m_chromaLerp.Begin( offset, 0, Random.Range(0.1f, 0.25f) );
	}
	
	// Update is called once per frame
	void Update () 
	{
		if ( !m_chromaLerp.done )
		{
			m_chromaLerp.Update();

			m_vignette.chromaticAberration = m_defaultChroma + m_chromaLerp.value;

			if ( m_chromaLerp.done )
			{
				if ( m_chromaLerp.begin == 40 )
				{
					m_timer = 0.75f;
				}
				else if ( m_chromaLerp.begin == 50 )
				{
					m_defaultChroma = 0;
					m_chromaLerp.Begin( 75, 0, 0.75f );
				}
				else if ( m_chromaLerp.begin < 11 )
				{
					m_timer = Random.Range( 0.5f, 1.5f );
				}
			}
		}

		if ( m_timer > 0 )
		{
			m_timer -= Time.deltaTime;

			if ( m_timer <= 0 )
			{
				Glitch();
			}
		}
	}
}
