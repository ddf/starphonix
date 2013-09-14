using UnityEngine;
using System.Collections;

public class Generator : MonoBehaviour 
{
	public float value { get; protected set; }
	
	public virtual float Generate()
	{
		value = 0;
		
		return value;
	}
}
