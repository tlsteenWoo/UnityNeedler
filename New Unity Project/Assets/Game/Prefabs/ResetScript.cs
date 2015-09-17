using UnityEngine;
using System.Collections;

public class ResetScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	if(Input.GetKeyDown(KeyCode.BackQuote))
		{
			Reset();
		}
	}

	public void Reset()
	{
		Application.LoadLevelAsync(Application.loadedLevelName);
	}
}
