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
	MissionDelta
}

public class Intro : FlavorText 
{
	Program m_program = Program.Boot;

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
						RunProgram( Program.MissionInfo );
					}
				}
				break;

				case Program.MissionInfo:
				{
					if ( Input.anyKeyDown )
					{
						RunProgram( Program.Directory );
					}
				}
				break;

				case Program.MissionAlpha:
				{
					if ( Input.GetKeyDown("y") )
					{
						Application.LoadLevel( "Alien" );
					}
					else if ( Input.GetKeyDown("n") )
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
				PushLine( "    1 - UNSSC_STARPHONIX.ALPHA.LOG");
				PushLine( "    2 - UNSSC_STARPHONIX.INFO" );
				//PushLine( "2 - UNSSC_STARPHONIX.BETA.LOG" );

			}
			break;

			case Program.MissionAlpha:
			{
				PushLine( "SYS//NEGOTIATION//PROBELOG" );
				PushLine( "SYS//SUB.NET//NEGOTIATION.LOG.BLACKBOX" );
				PushLine( "SYS//ID//UNSSC_STARPHONIX" );
				PushLine( "    QUERY//ID" );
				PushLine( "    //DES.SONICNEGOTIATIONPROBEMODULE.WARPHASE/TREATY");
				PushLine( "SYS//ID//UNSSC STARPHONIX//LOGFEED" );
				PushLine( "SYS//INIT//LAZARUS//UNSSC_STARPHONIX//LOGFEED" );
				PushLine( "SYS//INIT//REPLAY" );
				PushLine( "Y or N" );
			}
			break;

			case Program.MissionInfo:
			{
				PushLine( "SYS//ID//PROTOCOL_FRENZY" );
				PushLine( "    QUERY//ID" );
				PushLine( "    //DES.MISSION.DIPLOMATICPROBE.TIMECRITICAL" );
				PushLine( "SYS//ID//UNENG_ROYER" );
				PushLine( "    QUERY//ID" );
				PushLine( "    //DES.MISSION.DESIGNMODULE.FIRSTCLASS" );
				PushLine( "SYS//ID//UNENG_DIFEDE" );
				PushLine( "    QUERY//ID" );
				PushLine( "    //DES.MISSION.FABRICATIONMODULE.FIRSTCLASS" );
			}
			break;

			default:
			{
				PushLine( "SYS//ERROR//PROGRAM//UNRECOGNIZED");
				RunProgram( Program.Directory );
			}
			break;
		}

		m_program = prg;
	}
}
