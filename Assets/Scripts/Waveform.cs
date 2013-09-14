using UnityEngine;

public interface Waveform 
{
	float Value( float at );
}

public class Sine : Waveform
{
	public float Value( float at )
	{
		return Mathf.Sin(at*Mathf.PI*2);
	}
}

public class Ramp : Waveform
{
	public float Value( float at )
	{
		return 1.0f - at;
	}
}