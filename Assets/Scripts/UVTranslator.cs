using UnityEngine;
using System.Collections;

public class UVTranslator : MonoBehaviour 
{
	public Vector2 	Rate 				= Vector2.zero;
	public string 	TexturePropertyName = "_MainTex";
	public int 		MaterialIndex 		= 0;
	public Vector2  InitialOffset 		= Vector2.zero;
	public bool 	ResetUVsOnDisable 	= false;	
	
	private Vector2		m_offset;
	private string		m_texturePropertyName;
	
	// Use this for initialization
	void Start () 
	{
		m_texturePropertyName	= TexturePropertyName;
		m_offset				= InitialOffset;
		renderer.materials[ MaterialIndex ].SetTextureOffset( m_texturePropertyName, m_offset );
	}
	
	void OnDisable()
	{
		if (ResetUVsOnDisable)
		{
			renderer.materials[ MaterialIndex ].SetTextureOffset(m_texturePropertyName, Vector2.zero );
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		if ( Rate != Vector2.zero )
		{
			m_offset += Time.deltaTime * Rate;
			m_offset.x %= 1.0f;
			m_offset.y %= 1.0f;
			
			renderer.materials[ MaterialIndex ].SetTextureOffset(m_texturePropertyName, m_offset );
		}
	}
}
