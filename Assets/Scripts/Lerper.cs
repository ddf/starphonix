using UnityEngine;
using System.Collections;

public enum EasingType
{
	Linear,
	ExpoEaseIn,
	ExpoEaseOut,
	ExpoEaseInOut,
	BounceEaseIn,
	BounceEaseOut,
	BounceEaseInOut,
	BackEaseIn,
	BackEaseOut,
	BackEaseInOut
}

public class Lerper 
{
	private float m_begin;
	private float m_change;
	private float m_duration;
	
	private float m_time;
	private float m_value;
	
	private Easing.Function m_easingFunction;
	
	public Lerper()
	{
		m_easingFunction = new Easing.Four( Easing.Linear.easeNone );
	}
	
	public Lerper( Easing.Function easingFunction )
	{
		m_easingFunction = easingFunction;
	}
	
	public Lerper( EasingType easingType )
	{
		switch( easingType )
		{
			case EasingType.Linear:
			{
				m_easingFunction = new Easing.Four( Easing.Linear.easeNone );
			}
			break;
			
			case EasingType.ExpoEaseIn: 
			{
				m_easingFunction = new Easing.Four( Easing.Expo.easeIn ); 
			}
			break;

			case EasingType.ExpoEaseOut:
			{
				m_easingFunction = new Easing.Four( Easing.Expo.easeOut );
			}
			break;

			case EasingType.ExpoEaseInOut:
			{
				m_easingFunction = new Easing.Four( Easing.Expo.easeInOut );
			}
			break;
			
			case EasingType.BounceEaseIn: 
			{
				m_easingFunction = new Easing.Four( Easing.Bounce.easeIn ); 
			}
			break;

			case EasingType.BounceEaseOut:
			{
				m_easingFunction = new Easing.Four( Easing.Bounce.easeOut );
			}
			break;

			case EasingType.BounceEaseInOut:
			{
				m_easingFunction = new Easing.Four( Easing.Bounce.easeInOut );
			}
			break;

			case EasingType.BackEaseIn:
			{
				m_easingFunction = new Easing.Five( Easing.Back.easeIn );
			}
			break;

			case EasingType.BackEaseOut:
			{
				m_easingFunction = new Easing.Five( Easing.Back.easeOut );
			}
			break;

			case EasingType.BackEaseInOut:
			{
				m_easingFunction = new Easing.Five( Easing.Back.easeInOut );
			}
			break;
		}
	}
	
	public void Begin( float startVal, float endVal, float duration )
	{
		m_begin = startVal;
		m_change = endVal - startVal;
		m_duration = duration;
		m_value = m_begin;
		m_time = 0;
		
		//If duration is 0, snap to end value
		if (m_duration == 0)
		{
			m_value = endVal;
		}
	}
	
	public void Update()
	{
		if ( m_time < m_duration )
		{
			m_time += Time.deltaTime;
			if ( m_time > m_duration )
			{
				m_time = m_duration;
			}
			m_value = m_easingFunction.ease( m_time, m_begin, m_change, m_duration );
		}
	}
	
	public Easing.Function ease
	{
		get
		{
			return m_easingFunction;
		}
		
		set
		{
			if ( value != null )
			{
				m_easingFunction = value;
			}
			else
			{
				m_easingFunction = new Easing.Four( Easing.Linear.easeNone );
			}
		}
	}
	
	public float time
	{
		get
		{
			return m_time;
		}
	}
	
	public float normalizedTime
	{
		get
		{
			return m_time/m_duration;
		}
	}

	public float begin 
	{
		get 
		{
			return m_begin;
		}
	}

	public float end 
	{
		get
		{
			return m_begin + m_change;
		}
	}
	
	public float value
	{
		get
		{
			return m_value;
		}
	}

	public float duration 
	{
		get 
		{
			return m_duration;
		}
	}
	
	public bool done
	{
		get
		{
			return m_time == m_duration;
		}
	}
	
	public bool IsAtEnd()
	{
		return m_time == m_duration;
	}
}
