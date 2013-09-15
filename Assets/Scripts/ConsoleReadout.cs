using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ConsoleReadout : MonoBehaviour 
{
	public int MaxLines = 10;

	TextMesh 	  m_text;
	List<string>  m_lines = new List<string>();

	// Use this for initialization
	void Start () 
	{
		m_text = GetComponent<TextMesh>();
	}

	public void PushLine( string input )
	{
		m_lines.Add( input );

		if ( audio )
		{
			audio.Play();
		}

		if ( m_lines.Count > MaxLines )
		{
			m_lines.RemoveAt( 0 );
		}

		RefreshOutput();
	}

	public void ReplaceLastLine( string input )
	{
		if ( m_lines.Count > 0 )
		{
			m_lines[ m_lines.Count-1 ] = input;
			RefreshOutput();
		}
		else 
		{
			PushLine( input );
		}
	}

	void RefreshOutput()
	{
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
	}
}
