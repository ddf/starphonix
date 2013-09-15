using UnityEngine;
using System.Collections;

public class CommLogList : MonoBehaviour 
{
	public Renderer[] Squares;
	public Color 	  FillColor;

	int m_nextSquare;

	public void LightNext()
	{
		Squares[m_nextSquare].SetColor( FillColor );
		m_nextSquare++;
	}
}
