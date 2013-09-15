using UnityEngine;
using System.Collections;

public class CommLog : MonoBehaviour 
{
	public CommLogList SuccessList;
	public CommLogList FailureList;

	public void AddSuccess()
	{
		SuccessList.LightNext();
	}

	public void AddFailure()
	{
		FailureList.LightNext();
	}
}
