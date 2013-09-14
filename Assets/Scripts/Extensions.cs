using UnityEngine;
using System.Collections.Generic;
using System;

public delegate bool WhereCondition<T>( T item );

public static class Extensions 
{
	public static void Shuffle<T>(this IList<T> list)  
	{  
	    System.Random rng = new System.Random();  
	    int n = list.Count;  
	    while (n > 1) {  
	        n--;  
	        int k = rng.Next(n + 1);  
	        T value = list[k];  
	        list[k] = list[n];  
	        list[n] = value;  
	    }  
	}
	
	public static void Shuffle<T>(this T[] list)  
	{  
	    System.Random rng = new System.Random();  
	    int n = list.Length;  
	    while (n > 1) {  
	        n--;  
	        int k = rng.Next(n + 1);  
	        T value = list[k];  
	        list[k] = list[n];  
	        list[n] = value;  
	    }  
	}

	public static T Any<T>(this T[] list)
	{
        if ( list.Length > 0 )
        {
		  return list[ UnityEngine.Random.Range(0,list.Length) ];
        }

        return default(T);
	}

    public static IEnumerable<T> Where<T>( this T[] list, WhereCondition<T> condition )
    {
        List<T> filtered = new List<T>();
        foreach( T item in list )
        {
            if ( condition(item) )
            {
                filtered.Add( item );
            }
        }

        return filtered as IEnumerable<T>;
    }

    public static IEnumerable<T> Where<T>( this IEnumerable<T> list, WhereCondition<T> condition )
    {
        List<T> filtered = new List<T>();
        foreach( T item in list )
        {
            if ( condition(item) )
            {
                filtered.Add( item );
            }
        }

        return filtered as IEnumerable<T>;
    }

    public static bool Contains<T>( this T[] list, T obj )
    {
		if ( list == null )
		{
			return false;
		}
		
        foreach( T item in list )
        {
            if ( item != null && item.Equals( obj ) )
            {
                return true;
            }

            if ( item == null && obj == null )
            {
                return true;
            }
        }

        return false;
    }

    public static void SetColor( this Renderer renderer, Color color, int MaterialIndex = 0 )
    {
        Material m = renderer.materials[MaterialIndex];
        if ( m.HasProperty("_TintColor") )
        {
            m.SetColor( "_TintColor", color );
        }
        else
        {
            m.color = color;
        }
    }

    public static Color GetColor( this Renderer renderer, int materialIndex = 0 )
    {
        Material m = renderer.materials[materialIndex];

        if ( m.HasProperty("_TintColor") )
        {
            return m.GetColor("_TintColor");
        }
        
        return m.color;
    }

    public static void SetAlpha( this Renderer renderer, float alpha )
    {
        if ( renderer.material.HasProperty("_Color") )
        {
            Color c = renderer.material.color;
            c.a     = alpha;
            renderer.material.color = c;

            return;
        }

        MeshFilter meshComponent = renderer.GetComponent<MeshFilter>();
        if ( meshComponent )
        {
            Color[] colors = meshComponent.mesh.colors;
            for( int i = 0; i < colors.Length; ++i )
            {
                colors[i].a = alpha;
            }
            meshComponent.mesh.colors = colors;
        }
    }

    public static void SetScale( this Transform transform, Vector3 scale )
    {
    	transform.localScale = scale;
    }

    public static Vector3 GetScale( this Transform transform )
    {
    	return transform.localScale;
    }
	
	public static void LogAtCurrentTime( string s )
	{
		Debug.Log(Time.time + ": " + s);
	}
	
	public static float Map( float value, float vlow, float vhigh, float tlow, float thigh )
	{
		return Mathf.Lerp( tlow, thigh, Mathf.InverseLerp( vlow, vhigh, value ) );
	}
	
    // unclamped version of lerping for Vector3
    public static Vector3 Lerp( Vector3 from, Vector3 to, float t )
    {
        Vector3 dir = to - from;
        return from + dir*t;
    }

    // lineWrap
    //  Splits string into lines which are _charsPerLine or shorter.
    //
    //  Current word is buffered so that if _charsPerLine is reached in the middle
    //  of a word the entire word appears on the next line.
    //
    //  Note that variable width fonts are not accounted for so it is likely
    //  you will have to set _charsPerLine much shorter than desired so that lines
    //  with several capital letters or wide chars wrap correctly.
    //
    //  Also takes carriage returns (\n) already in the string into account.
    //
    //
    // Parameters:
    //  string _str      - string to process
    //  int _charPerLine - max # of characters per line.
    //  bool _forceWrap  - if set to true, a continuous string of characters with no spaces
    //            word will be forced to wrap to _charsPerLine 
    //            if set to false, word will stay intact and violate _charsPerLine
    //
    // TODO:
    //  Don't count the space at end of a line.
    //  _forceWrap can cause somewhat odd behavior as it is a very simple implementation.
    //
    //  Provided by typeRice - June 12, 2009
    public static string LineWrap( string _str, int _charsPerLine, bool _forceWrap )
    {
        if( _str.Length < _charsPerLine )
            return _str;

        // replace stringified newlines that result from using \n in a string property
        // with real newlines.
        _str = _str.Replace("\\n", System.Environment.NewLine);
        
        string  result = "",
                buf = "";                  // Stores current word

        int cursor    = 0,          // Position within _str
            charCount   = 0;               // # of chars on current line

        bool bLineEmpty = false;                // if a new line was added to the result &
                                                // buf has not been appended.

        while( cursor < _str.Length )
        {
            buf += _str[cursor];                // add next character to buffer
            charCount++;                        // increment count of chars on current line

            if( _str[cursor] == ' ' )         // if space is encountered
            {
                result += buf;          // write the buffer to the result and clear
                buf = "";
                bLineEmpty = false;    // Something added since the last carriage return
            }
            else if( _str[cursor] == '\n' )  // manual carriage return encountered
            {
                result += buf;          // write the buffer to the result and clear (buf contains the \n already)
                buf = "";
                charCount=0;                    // Start new line so reset character count.
            }

            if( charCount >= _charsPerLine )    // if charCount has reached max chars per line
            {
                if( !bLineEmpty )            // If line has something in it.
                {
                    result += '\n';    // Start a new line in the result
                    charCount = buf.Length;  // reset character count to current buf size as ths will be placed on the new line.
                    bLineEmpty = true;      // Newest line is empty
                }
                else if( _forceWrap )         // else if the line is empty and we want to force word to wrap
                {
                    result += buf + '\n';      // write the buffer to the result with a carriage return
                    buf = "";               // clear the buffer
                    bLineEmpty = true;      // Newest line is empty
                    charCount = 0;        // Start new line so reset character count.
                }
            }

            cursor++;                     // Goto next character
        }

        result += buf;              // add any remaining characters in the buffer
        return result;
    }
	
	// h: 0 - 360
	// s: 0 - 1
	// v: 0 - 1
	public static Color HSVtoRGB (float h, float s, float v, float alpha = 1f)
	{
		while ( h < 0 )
		{
			h += 360;
		}
		
		while ( h >= 360 )
		{
			h -= 360;
		}
		
		float r = 0, g = 0, b = 0;
		
		if ( v <= 0 )
		{
			r = 0;
			g = 0;
			b = 0;
		}
		else if ( s <= 0 )
		{
			r = v;
			g = v;
			b = v;
		}
		else
		{
			float hf = h / 60f;
			
			int i = (int)Mathf.Floor(hf);
			
			float f = hf - i;
			float pv = v * (1 - s);
			float qv = v * (1 - s * f);
			float tv = v * (1 - s * (1 - f));
			
			switch (i)
			{
				//red
				case 0:
					r = v;
					g = tv;
					b = pv;
					break;
				
				//green
				case 1:
					r = qv;
					g = v;
					b = pv;
					break;
				case 2:
					r = pv;
					g = v;
					b = tv;
					break;
				
				//blue
				case 3:
					r = pv;
					g = qv;
					b = v;
					break;
				case 4:
					r = tv;
					g = pv;
					b = v;
					break;
				
				//red
				case 5:
					r = v;
					g = pv;
					b = qv;
					break;
				
				//overshoot
				case 6:
					r = v;
					g = tv;
					b = pv;
					break;
				case -1:
					r = v;
					g = pv;
					b = qv;
					break;
			}
			
			r = Mathf.Clamp01(r);
			g = Mathf.Clamp01(g);
			b = Mathf.Clamp01(b);
		}
		
		return new Color( r, g, b, alpha );
	}
}
