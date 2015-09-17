using UnityEngine;
using System.Collections;

public class LevelChanger : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

	public int NextLevel
	{
		get{ return (Application.loadedLevel + 1) % Application.levelCount;}
	}
	public void GotoLevel(int index)
	{
		Application.LoadLevel (index);
	}
	public void GotoLevel(string name)
	{
		Application.LoadLevel (name);
	}
	public void GotoNextLevel()
	{
		Debug.Log ("LevelChanger: Loading Next Level { " + NextLevel + " }");
		Application.LoadLevel (NextLevel);
	}
}
