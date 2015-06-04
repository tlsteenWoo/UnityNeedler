using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NeedleHostUI : MonoBehaviour {
	public NeedleHost needleHost;
	public Text healthText;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		string formatHealth = string.Format("{0:D3}",(int)needleHost.Health);
		Debug.Log ("IsDead = " + needleHost.IsDead + ", Health(float) = " + needleHost.Health + ", health(int) = " + (int)needleHost.Health + ", and Format Health = " + formatHealth);
		healthText.text = formatHealth;
	}
}
