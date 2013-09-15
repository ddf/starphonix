using UnityEngine;
using System.Collections;

public class Intro : MonoBehaviour 
{
	public ConsoleReadout Console;
	public string[] 	  Output;
	public float 		  OutputRate;

	float	m_outputTimer; 
	int  	m_outputIndx;

	// Use this for initialization
	void Start () 
	{
		m_outputTimer = 10.5f;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if ( m_outputTimer > 0 )
		{
			m_outputTimer -= Time.deltaTime;

			if ( m_outputTimer <= 0 )
			{
				Console.ReplaceLastLine( Output[m_outputIndx] );
				Console.PushLine( " " );
				m_outputIndx++;

				if ( m_outputIndx < Output.Length )
				{
					Console.PushLine( "_" );
					m_outputTimer = 0.5f;
				}
			}
		}

		if ( m_outputIndx == Output.Length )
		{
			if ( Input.GetKeyDown("y") )
			{
				Application.LoadLevel( "Alien" );
			}
			else if ( Input.GetKeyDown("n") )
			{
				Application.Quit();
			}
		}
	}
}
