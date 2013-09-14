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

public class Square : Waveform
{
	public float Value( float at )
	{
		return at < 0.5 ? Extensions.Map(at, 0, 0.5f, 1.0f, 0.7f) : Extensions.Map(at, 0.5f, 1.0f, -0.7f, -1.0f);
	}
}

public class Saw : Waveform
{
	public float Value( float at )
	{
		return 1 - at*2;
	}
}

public class Ramp : Waveform
{
	public float Value( float at )
	{
		return at < 0.2f ? 1.0f - at*5 : 0;
	}
}