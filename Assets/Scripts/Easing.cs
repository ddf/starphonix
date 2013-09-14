using UnityEngine;
using System.Collections;

// good ole robert penner's easing equations
// t is time
// b is beginning value
// c is change in value
// d is total duration
// s is sigmoid, I think.
public static class Easing 
{
	public delegate float EasingFunctionFour( float t, float b, float c, float d );
	public delegate float EasingFunctionFive( float t, float b, float c, float d, float s );
	public delegate float EasingFunctionSix( float t, float b, float c, float d, float a, float p );
	
	public interface Function
	{
		float ease( float t, float b, float c, float d );
	}
	
	public class Four : Function
	{
		EasingFunctionFour m_ease;
		
		public Four( EasingFunctionFour func )
		{
			m_ease = func;
		}
		
		public float ease( float t, float b, float c, float d )
		{
			return m_ease( t, b, c, d );
		}
	}
	
	public class Five : Function
	{
		EasingFunctionFive m_ease;
		float			   m_s;
		
		public Five( EasingFunctionFive func, float s = 1.70158f )
		{
			m_ease = func;
			m_s = s;
		}
		
		public float ease( float t, float b, float c, float d )
		{
			return m_ease( t, b, c, d, m_s );
		}
	}
	
	public class Six : Function
	{
		EasingFunctionSix m_ease;
		float			  m_a, m_p;
		
		public Six( EasingFunctionSix func, float a = 0, float p = 0 )
		{
			m_ease = func;
			m_a = a;
			m_p = p;
		}
		
		public float ease( float t, float b, float c, float d )
		{
			return m_ease( t, b, c, d, m_a, m_p );
		}
	}
		
	public static class Back 
	{
		public static float easeIn (float t, float b, float c, float d, float s)
		{
			return c*(t/=d)*t*((s+1)*t - s) + b;
		}
		
		public static float easeOut (float t, float b, float c, float d, float s)
		{
			return c*((t=t/d-1)*t*((s+1)*t + s) + 1) + b;
		}
		
		public static float easeInOut (float t, float b, float c, float d, float s)
		{
			if ((t/=d/2) < 1) return c/2*(t*t*(((s*=(1.525f))+1)*t - s)) + b;
			return c/2*((t-=2)*t*(((s*=(1.525f))+1)*t + s) + 2) + b;
		}
	}
	
	public static class Bounce 
	{
		public static float easeOut (float t, float b, float c, float d)
		{
			if ((t/=d) < (1/2.75f)) {
				return c*(7.5625f*t*t) + b;
			} else if (t < (2/2.75f)) {
				return c*(7.5625f*(t-=(1.5f/2.75f))*t + .75f) + b;
			} else if (t < (2.5f/2.75f)) {
				return c*(7.5625f*(t-=(2.25f/2.75f))*t + .9375f) + b;
			} else {
				return c*(7.5625f*(t-=(2.625f/2.75f))*t + .984375f) + b;
			}
		}
		
		public static float easeIn (float t, float b, float c, float d)
		{
			return c - easeOut (d-t, 0, c, d) + b;
		}
		
		public static float easeInOut (float t, float b, float c, float d)
		{
			if (t < d/2) return easeIn (t*2, 0, c, d) * .5f + b;
			else return easeOut (t*2-d, 0, c, d) * .5f + c*.5f + b;
		}
	}
	
	public static class Circ 
	{
		public static float easeIn (float t, float b, float c, float d)
		{
			return -c * (Mathf.Sqrt(1 - (t/=d)*t) - 1) + b;
		}
		
		public static float easeOut (float t, float b, float c, float d)
		{
			return c * Mathf.Sqrt(1 - (t=t/d-1)*t) + b;
		}
		
		public static float easeInOut (float t, float b, float c, float d)
		{
			if ((t/=d/2) < 1) return -c/2 * (Mathf.Sqrt(1 - t*t) - 1) + b;
			return c/2 * (Mathf.Sqrt(1 - (t-=2)*t) + 1) + b;
		}
	}
	
	public static class Cubic 
	{
		public static float easeIn (float t, float b, float c, float d)
		{
			return c*(t/=d)*t*t + b;
		}
		
		public static float easeOut (float t, float b, float c, float d)
		{
			return c*((t=t/d-1)*t*t + 1) + b;
		}
		
		public static float easeInOut (float t, float b, float c, float d)
		{
			if ((t/=d/2) < 1) return c/2*t*t*t + b;
			return c/2*((t-=2)*t*t + 2) + b;
		}
	}
	
	public static class Elastic 
	{
		public static float easeIn (float t, float b, float c, float d, float a, float p)
		{
			if (t==0) return b;  
			if ((t/=d)==1) return b+c;  
			if (p==0) p=d*.3f;
			float s = 0;
			if (a==0 || a < Mathf.Abs(c)) { a=c; s=p/4; }
			else s = p/(2*Mathf.PI) * Mathf.Asin (c/a);
			return -(a*Mathf.Pow(2,10*(t-=1)) * Mathf.Sin( (t*d-s)*(2*Mathf.PI)/p )) + b;
		}
		
		public static float easeOut (float t, float b, float c, float d, float a, float p)
		{
			if (t==0) return b;  
			if ((t/=d)==1) return b+c;  
			if (p==0) p=d*.3f;
			float s = 0;
			if (a==0 || a < Mathf.Abs(c)) { a=c; s=p/4; }
			else s = p/(2*Mathf.PI) * Mathf.Asin (c/a);
			return (a*Mathf.Pow(2,-10*t) * Mathf.Sin( (t*d-s)*(2*Mathf.PI)/p ) + c + b);
		}
		
		public static float easeInOut (float t, float b, float c, float d, float a, float p)
		{
			if (t==0) return b;  
			if ((t/=d/2)==2) return b+c;  
			if (p==0) p=d*(.3f*1.5f);
			float s = 0;
			if (a==0 || a < Mathf.Abs(c)) { a=c; s=p/4; }
			else s = p/(2*Mathf.PI) * Mathf.Asin (c/a);
			if (t < 1) return -.5f*(a*Mathf.Pow(2,10*(t-=1)) * Mathf.Sin( (t*d-s)*(2*Mathf.PI)/p )) + b;
			return a*Mathf.Pow(2,-10*(t-=1)) * Mathf.Sin( (t*d-s)*(2*Mathf.PI)/p )*.5f + c + b;
		}
	}
	
	public static class Expo
	{
		public static float easeIn (float t, float b, float c, float d)
		{
			return (t==0) ? b : c * Mathf.Pow(2, 10 * (t/d - 1)) + b;
		}
		
		public static float easeOut (float t, float b, float c, float d)
		{
			return (t==d) ? b+c : c * (-Mathf.Pow(2, -10 * t/d) + 1) + b;
		}
		
		public static float easeInOut (float t, float b, float c, float d)
		{
			if (t==0) return b;
			if (t==d) return b+c;
			if ((t/=d/2) < 1) return c/2 * Mathf.Pow(2, 10 * (t - 1)) + b;
			return c/2 * (-Mathf.Pow(2, -10 * --t) + 2) + b;
		}
	}
	
	public static class Linear
	{
		public static float easeNone (float t, float b, float c, float d)
		{
			return c*t/d + b;
		}
		
		public static float easeIn (float t, float b, float c, float d)
		{
			return c*t/d + b;
		}
		
		public static float easeOut (float t, float b, float c, float d)
		{
			return c*t/d + b;
		}
		
		public static float easeInOut (float t, float b, float c, float d)
		{
			return c*t/d + b;
		}
	}
	
	public static class Quad
	{
		public static float easeIn (float t, float b, float c, float d)
		{
			return c*(t/=d)*t + b;
		}
		
		public static float easeOut (float t, float b, float c, float d)
		{
			return -c *(t/=d)*(t-2) + b;
		}
		
		public static float easeInOut (float t, float b, float c, float d)
		{
			if ((t/=d/2) < 1) return c/2*t*t + b;
			return -c/2 * ((--t)*(t-2) - 1) + b;
		}
	}
	
	public static class Quart 
	{
		public static float easeIn (float t, float b, float c, float d)
		{
			return c*(t/=d)*t*t*t + b;
		}
		
		public static float easeOut (float t, float b, float c, float d)
		{
			return -c * ((t=t/d-1)*t*t*t - 1) + b;
		}
		
		public static float easeInOut (float t, float b, float c, float d)
		{
			if ((t/=d/2) < 1) return c/2*t*t*t*t + b;
			return -c/2 * ((t-=2)*t*t*t - 2) + b;
		}
	}
	
	public static class Quint
	{
		public static float easeIn (float t, float b, float c, float d)
		{
			return c*(t/=d)*t*t*t*t + b;
		}
		
		public static float easeOut (float t, float b, float c, float d)
		{
			return c*((t=t/d-1)*t*t*t*t + 1) + b;
		}
		
		public static float easeInOut (float t, float b, float c, float d)
		{
			if ((t/=d/2) < 1) return c/2*t*t*t*t*t + b;
			return c/2*((t-=2)*t*t*t*t + 2) + b;
		}
	}
	
	public static class Sine 
	{
		public static float easeIn (float t, float b, float c, float d)
		{
			return -c * Mathf.Cos(t/d * (Mathf.PI/2)) + c + b;
		}
		
		public static float easeOut (float t, float b, float c, float d)
		{
			return c * Mathf.Sin(t/d * (Mathf.PI/2)) + b;
		}
		
		public static float easeInOut (float t, float b, float c, float d)
		{
			return -c/2 * (Mathf.Cos(Mathf.PI*t/d) - 1) + b;
		}
	}
}
