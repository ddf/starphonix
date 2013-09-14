using UnityEngine;
using System.Collections;

public class failscript : MonoBehaviour {
	
	public Transform customer;
	private Vector3 startpos;

	void Start () {
		startpos = customer.position;
	}
	
	void Update () {
		if(customer!=null)
			if(customer.position.y<-3)
				guiText.text = "Uh oh!";
		
		if(customer!=null)
			if(customer.position.y<-30)
		{
				guiText.text = "";
			customer.position = startpos;
		}
	}
}
