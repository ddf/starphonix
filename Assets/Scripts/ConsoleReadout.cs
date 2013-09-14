using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ConsoleReadout : MonoBehaviour 
{
	public int MaxLines = 10;

	TextMesh 	  m_text;
	Queue<string> m_lines = new Queue<string>();

	// Use this for initialization
	void Start () 
	{
		m_text = GetComponent<TextMesh>();
	}

	public void PushLine( string input )
	{
		m_lines.Enqueue( input );

		if ( m_lines.Count > MaxLines )
		{
			m_lines.Dequeue();
		}

		string output = "";
		foreach( string line in m_lines )
		{
			output += line + "\n";
		}

		m_text.text = output;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if ( Random.value < 0.1f )
		{
			PushLine( Random.Range( 12000.0f, 334954.0f ) + "" );
		}
	}
}
