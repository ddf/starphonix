using UnityEngine;
using System.Collections;

public class RandomUV : MonoBehaviour 
{

	// Use this for initialization
	void Start () {
	
	}
	


	void Update() 
	{
    	renderer.material.mainTextureOffset = new Vector2 (Random.value, Random.value);
	}
}
