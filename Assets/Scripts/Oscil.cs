using UnityEngine;

public class Oscil : Generator
{
	public float 	 Frequency;
	public float	 Phase;
	public Generator Amplitude;
	public Waveform  Wave = new Sine();

	// where we will sample our waveform, moves between [0,1]
	private float		step;
	// the step size we will use to advance our step
	private float		stepSize;
	// what was our frequency from the last time we updated our step size
	// stashed so that we don't do more math than necessary
	private float		prevFreq;
	// 1 / sampleRate, which is used to calculate stepSize
	private float		oneOverSampleRate;

	void Start()
	{
		oneOverSampleRate = 1.0f / (float)AudioSettings.outputSampleRate;
		// don't call updateStepSize because it checks for frequency change
		stepSize = Frequency * oneOverSampleRate;
		prevFreq = Frequency;
	}

	// updates our step size based on the current frequency
	void updateStepSize()
	{
		float currFreq = Frequency;
		if ( prevFreq != currFreq )
		{
			stepSize = currFreq * oneOverSampleRate;
			prevFreq = currFreq;
		}
	}

	public void Reset()
	{
		step = 0;
	}

	public override float Generate()
	{
			// start with our base amplitude
		float outAmp = Amplitude.Generate();

		// temporary step location with phase offset.
		float tmpStep = step + Phase;
		// don't be less than zero
		if ( tmpStep < 0.0f )
		{
			tmpStep -= (int)tmpStep - 1f;
		}
		// don't exceed 1.
		// we don't use Math.floor because that involves casting up
		// to a double and then back to a float.
		if ( tmpStep > 1.0f )
		{
			tmpStep -= (int)tmpStep;
		}

		// calculate the sample value
		float sample = outAmp * Wave.Value( tmpStep );

		// update our step size.
		// this will check to make sure the frequency has changed.
		updateStepSize();

		// increase time
		// NOT THIS FROM BEFORE: step += stepSize + fPhase;
		step += stepSize;

		// don't be less than zero
		if ( step < 0.0f )
		{
			step -= (int)step - 1f;
		}

		// don't exceed 1.
		// we don't use Math.floor because that involves casting up
		// to a double and then back to a float.
		if ( step > 1.0f )
		{
			step -= (int)step;
		}

		return sample;
	}
}