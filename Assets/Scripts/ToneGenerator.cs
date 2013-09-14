using UnityEngine;
using System.Collections;

public class ToneGenerator : MonoBehaviour 
{
	public Oscil oscil;

	public Oscil mod 
	{
		get
		{
			return (Oscil)oscil.Amplitude;
		}
	}

	bool 	m_bGenerate;
	float[]	m_samples = new float[0];

	void Start()
	{
		mod.Wave = new Ramp();
	}

	void Update()
	{
		m_bGenerate = audio.isPlaying;
	}

	public void Trigger()
	{
		oscil.Reset();
		mod.Reset();

		audio.Play(0);
	}

	public void Stop()
	{
		audio.Stop();
		m_bGenerate = false;
		for( int i = 0; i < m_samples.Length; ++i )
		{
			m_samples[i] = 0;
		}
	}

	public float[] GetOutputData()
	{
		return m_samples;
	}
	
	void OnAudioFilterRead ( float[] output, int channels ) 
	{
		int scount = output.Length / channels;
		if ( m_samples.Length != scount )
		{
			m_samples = new float[scount];
		}

		if ( m_bGenerate )
		{
			for( int i = 0; i < scount; ++i )
			{
				m_samples[i] = oscil.Generate();
				for( int c = 0; c < channels; ++c )
				{
					output[i*channels + c] = m_samples[i];
				}
			}
		}
		else
		{
			for( int i = 0; i < scount; ++i )
			{
				m_samples[i] = 0;
				for( int c = 0; c < channels; ++c )
				{
					output[i*channels + c] = m_samples[i];
				}
			}
		}
	}
}
