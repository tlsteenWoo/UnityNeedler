using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DebugWindow : MonoBehaviour {
	public Text textWindow;
	string freshInput;

	// Use this for initialization
	void Start () {
		if (textWindow == null)
			throw new UnityException ("Debug Window has no text box to output to.");
	}
	
	// Update is called once per frame
	void Update () {
		textWindow.text = "Debug:\n" + freshInput;
		freshInput = "";
	}

	public void LOG(string Text)
	{
		freshInput += Text + '\n';
	}
}
