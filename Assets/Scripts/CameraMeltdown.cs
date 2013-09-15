using UnityEngine;
using System.Collections;

public class CameraMeltdown : MonoBehaviour 
{
	ContrastEnhance m_constrast;

	Lerper m_blurLerp = new Lerper();

	// Use this for initialization
	void Start () 
	{
		m_constrast = GetComponent<ContrastEnhance>();
	}

	public void Trigger()
	{
		m_constrast.enabled = true;
		m_blurLerp.Begin( 7.5f, 200, 1 );
	}
	
	// Update is called once per frame
	void Update () 
	{
		if ( !m_blurLerp.done )
		{
			m_blurLerp.Update();

			m_constrast.blurSpread = m_blurLerp.value;
		}
	}
}
