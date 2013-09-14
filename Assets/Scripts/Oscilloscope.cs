using UnityEngine;
using System.Collections;

public class Oscilloscope : MonoBehaviour 
{
	public ToneGenerator source;

	LineRenderer m_line;
	float[] 	 m_samples = new float[2048];
	Color 		 m_lineColor;

	// Use this for initialization
	void Start () 
	{
		m_line = GetComponent<LineRenderer>();
		m_lineColor = m_line.material.color;
	}
	
	// Update is called once per frame
	void Update () 
	{
		m_samples = source.GetOutputData();
		m_line.SetVertexCount( m_samples.Length );

		for( int i = 0; i < m_samples.Length; ++i )
		{
			float xPos  = -1 + (float)i/m_samples.Length*2;
			Vector3 pos = new Vector3(xPos, m_samples[i]*0.1f, 0);
			m_line.SetPosition(i, pos);
		}

		m_lineColor.a = source.mod.value * 2;
		m_line.material.color = m_lineColor;
	}
}
