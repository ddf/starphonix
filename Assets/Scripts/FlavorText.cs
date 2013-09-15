using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FlavorText : MonoBehaviour 
{
	public ConsoleReadout Console;
	public float 		  OutputRate = 0.3f;

	Queue<string> m_feed = new Queue<string>();
	float 		  m_feedTimer;
	float 		  m_linkCheck;

	protected int feedCount
	{
		get 
		{
			return m_feed.Count;
		}
	}

	// Use this for initialization
	void Start () 
	{
		PushLine( "SYS//ID//UNSSC_STARPHONIX" );
		PushLine( "INIT//NEGOTIATION//" );
		PushLine( "SUB.SYS//QUARDRARIAN.EMISSARY" );
		PushLine( "INIT//COMM" );

		m_linkCheck = 5;
	}

	protected virtual void Update()
	{
		if ( m_feedTimer > 0 )
		{
			m_feedTimer -= Time.deltaTime;

			if ( m_feedTimer <= 0 )
			{
				Console.PushLine( m_feed.Dequeue() );

				if ( m_feed.Count > 0 )
				{
					m_feedTimer = OutputRate;
				}
			}
		}

		if ( m_linkCheck > 0 )
		{
			m_linkCheck -= Time.deltaTime;

			if ( m_linkCheck <= 0 )
			{
				PushLine( "COMM//INTEG.CHECK.LINK" );
				m_linkCheck = 5;
			}
		}
	}

	protected void PushLine( string line )
	{
		m_feed.Enqueue( line );

		if ( m_feed.Count == 1 )
		{
			m_feedTimer = 0.1f;
		}
	}
	
	public void Success()
	{
		PushLine( "COMM//SIGNALLOCK//STARPHONIX" );
		PushLine( "COMM//SUB//TRANSMIT.SUCCESS" );
		PushLine( "COMM//SUSTAIN.LINK" );
	}

	public void Failure()
	{
		PushLine( "COMM//CRITICALFAILURE//STARPHONIX" );
		PushLine( "COMM//SUB//TRANSMIT.EVAC" );
		PushLine( "COMM//RE.INIT.LINK" );
	}

	public void Disconnect()
	{
		PushLine( "COMM//SIGNALLOST//STARPHONIX" );
		PushLine( "COMM//SUB//TRASMIT.RENDEZVOUS" );
		PushLine( "COMM//KILL.LINK" );
	}
}
