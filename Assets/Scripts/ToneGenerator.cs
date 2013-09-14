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

	bool m_bGenerate;

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

		audio.Play();
	}
	
	void OnAudioFilterRead ( float[] output, int channels ) 
	{
		if ( m_bGenerate )
		{
			int samples = output.Length / channels;

			for( int i = 0; i < samples; ++i )
			{
				float s = oscil.Generate();
				for( int c = 0; c < channels; ++c )
				{
					output[i*channels + c] = s;
				}
			}
		}
	}
}
