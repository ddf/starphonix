using UnityEngine;
using System.Collections;

public enum Program
{
	Boot,
	Directory,
	MissionInfo,
	MissionAlpha,
	MissionBeta,
	MissionGamma,
}

public class Intro : FlavorText 
{
	Program m_program 	 = Program.Boot;
	float   m_loadTimer;

	// Use this for initialization
	IEnumerator Start () 
	{
		PushLine( "SYS//BOOT" );
		PushLine( "XBIOS V3.10.2.51.9" );

		yield return new WaitForSeconds( 9.5f );

		RunProgram( Program.MissionAlpha );
	}
	
	// Update is called once per frame
	protected override void Update () 
	{
		base.Update();

		if ( feedCount == 0 )
		{
			switch( m_program )
			{
				case Program.Directory:
				{
					if ( Input.GetKeyDown("1") )
					{
						RunProgram( Program.MissionAlpha );
					}
					else if ( Input.GetKeyDown("2") )
					{
						RunProgram( Program.MissionBeta );
					}
					else if ( Input.GetKeyDown("3" ) )
					{
						RunProgram( Program.MissionGamma );
					}
					else if ( Input.GetKeyDown("4") )
					{
						RunProgram( Program.MissionInfo );
					}
				}
				break;

				case Program.MissionInfo:
				{
					m_loadTimer -= Time.deltaTime;
					if ( m_loadTimer <= 0 )
					{
						RunProgram( Program.Directory );
					}
				}
				break;

				case Program.MissionAlpha:
				{
					m_loadTimer -= Time.deltaTime;
					Console.ReplaceLastLine( "REPLAY IN " + Mathf.Ceil(m_loadTimer).ToString("0") + " SECONDS. PRESS Q TO ABORT." );
					if ( m_loadTimer <= 0 )
					{
						Console.ReplaceLastLine( "BEGINNING REPLAY..." );
						AlienAI.MAX_PATIENCE = 180;
						AlienAI.TREATIES_WITH_FREQ_COLOR = 2;
						AlienAI.TREATIES_WITH_RATE_COLOR = 4;
						Application.LoadLevel( "Alien" );
					}
					
					if ( Input.GetKeyDown("q") )
					{
						RunProgram( Program.Directory );
					}
				}
				break;

				case Program.MissionBeta:
				{
					m_loadTimer -= Time.deltaTime;
					Console.ReplaceLastLine( "REPLAY IN " + Mathf.Ceil(m_loadTimer).ToString("0") + " SECONDS. PRESS Q TO ABORT." );
					if ( m_loadTimer <= 0 )
					{
						Console.ReplaceLastLine( "BEGINNING REPLAY..." );
						AlienAI.MAX_PATIENCE = 100;
						AlienAI.TREATIES_WITH_FREQ_COLOR = 1;
						AlienAI.TREATIES_WITH_RATE_COLOR = 2;
						Application.LoadLevel( "Alien" ); 
					}

					if ( Input.GetKeyDown("q") )
					{
						RunProgram( Program.Directory );
					}
				}
				break;

				case Program.MissionGamma:
				{
					m_loadTimer -= Time.deltaTime;
					Console.ReplaceLastLine( "REPLAY IN " + Mathf.Ceil(m_loadTimer).ToString("0") + " SECONDS. PRESS Q TO ABORT." );
					if ( m_loadTimer <= 0 )
					{
						Console.ReplaceLastLine( "BEGINNING REPLAY..." );
						AlienAI.MAX_PATIENCE = 50;
						AlienAI.TREATIES_WITH_FREQ_COLOR = 0;
						AlienAI.TREATIES_WITH_RATE_COLOR = 0;
						Application.LoadLevel( "Alien" ); 
					}

					if ( Input.GetKeyDown("q") )
					{
						RunProgram( Program.Directory );
					}
				}
				break;

				default: break;
			}
		}
	}

	void RunProgram( Program prg )
	{
		PushLine( "EXEC//" + prg.ToString().ToUpper() );

		switch ( prg )
		{
			case Program.Directory:
			{
				PushLine( "DIR//CONTENTS" );
				PushLine( "    1 - UNSSC_STARPHONIX.ALPHA.LOG" );
				PushLine( "    2 - UNSSC_STARPHONIX.BETA.LOG" );
				PushLine( "    3 - UNSSC_STARPHONIX.GAMMA.LOG" );
				PushLine( "    4 - UNSSC_STARPHONIX.INFO" );
			}
			break;

			case Program.MissionAlpha:
			{
				PushLine( "SYS//NEGOTIATION//PROBELOG" );
				PushLine( "SYS//SUB.NET//NEGOTIATION.LOG.BLACKBOX" );
				PushLine( "SYS//ID//UNSSC_STARPHONIX" );
				PushLine( "    QUERY//ID" );
				PushLine( "    //DES.SONICNEGOTIATIONPROBEMODULE.WARPHASE/TREATY");
				PushLine( "    //DES.CODENAME.FIRSTENCOUNTER" );
				PushLine( "SYS//ID//UNSSC STARPHONIX//LOGFEED" );
				PushLine( "SYS//INIT//LAZARUS//UNSSC_STARPHONIX//LOGFEED" );
				PushLine( "SYS//INIT//REPLAY" );
				PushLine( "REPLAY IN 5 SECONDS. PRESS Q TO ABORT." );

				m_loadTimer = 5;
			}
			break;

			case Program.MissionBeta:
			{
				PushLine( "SYS//NEGOTIATION//PROBELOG" );
				PushLine( "SYS//SUB.NET//NEGOTIATION.LOG.BLACKBOX" );
				PushLine( "SYS//ID//UNSSC_STARPHONIX" );
				PushLine( "    QUERY//ID" );
				PushLine( "    //DES.SONICNEGOTIATIONPROBEMODULE.WARPHASE/TREATY");
				PushLine( "    //DES.CODENAME.THREATASSESS" );
				PushLine( "SYS//ID//UNSSC STARPHONIX//LOGFEED" );
				PushLine( "SYS//INIT//LAZARUS//UNSSC_STARPHONIX//LOGFEED" );
				PushLine( "SYS//INIT//REPLAY" );
				PushLine( "REPLAY IN 5 SECONDS. PRESS Q TO ABORT." );

				m_loadTimer = 5;
			}
			break;

			case Program.MissionGamma:
			{
				PushLine( "SYS//NEGOTIATION//PROBELOG" );
				PushLine( "SYS//SUB.NET//NEGOTIATION.LOG.BLACKBOX" );
				PushLine( "SYS//ID//UNSSC_STARPHONIX" );
				PushLine( "    QUERY//ID" );
				PushLine( "    //DES.SONICNEGOTIATIONPROBEMODULE.WARPHASE/TREATY");
				PushLine( "    //DES.CODENAME.LASTDITCH" );
				PushLine( "SYS//ID//UNSSC STARPHONIX//LOGFEED" );
				PushLine( "SYS//INIT//LAZARUS//UNSSC_STARPHONIX//LOGFEED" );
				PushLine( "SYS//INIT//REPLAY" );
				PushLine( "REPLAY IN 5 SECONDS. PRESS Q TO ABORT." );

				m_loadTimer = 5;
			}
			break;

			case Program.MissionInfo:
			{
				PushLine( "SYS//ID//PROTOCOL_FRENZY" );
				PushLine( "    QUERY//ID" );
				PushLine( "    //DES.MISSION.DIPLOMATICPROBE.TIMECRITICAL" );
				PushLine( "SYS//ID//UNENG_ROYER" );
				PushLine( "    QUERY//ID" );
				PushLine( "    //DES.MISSION.DESIGNVISUALS" );
				PushLine( "    //SECURITY.MAGIC.ULTRA" );
				PushLine( "SYS//ID//UNENG_DIFEDE" );
				PushLine( "    QUERY//ID" );
				PushLine( "    //DES.MISSION.CORESYSTEMSONICS" );
				PushLine( "    //SECURITY.MAGIC.ULTRA" );

				m_loadTimer = 2;
			}
			break;

			default:
			{
				PushLine( "SYS//ERROR//PROGRAM//UNRECOGNIZED");
				RunProgram( Program.Directory );
				prg = Program.Directory;
			}
			break;
		}

		m_program = prg;
	}
}
