using UnityEngine;
using System.Collections;

public class TextGlitch : MonoBehaviour 
{
	public string[] Glitches;
	public string 	Text;
	public float    Frequency;

	public Color color 
	{
		set 
		{
			renderer.SetColor( value );
		}
	}

	TextMesh m_text;

	// Use this for initialization
	void Start () 
	{
		m_text = GetComponent<TextMesh>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		m_text.text = Random.value > Frequency ? Text : Glitches[ Random.Range(0, Glitches.Length) ];
	}
}
