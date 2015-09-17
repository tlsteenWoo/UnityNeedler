using UnityEngine;
using UnityEngine.Events;
using System.Collections;
/// <summary>
/// Determines whether a solid object is entirely in the air or touching some surface.
/// </summary>
public class InAirScript : MonoBehaviour {
	int m_collisionCount = 0;
	public UnityEvent onEnterAir;
	public UnityEvent onExitAir;
	public UnityEvent onGainCollision;
	public UnityEvent onLoseCollision;
	bool oldCharColliderHit;
	bool charColliderHit;
	float lastTimeCharColliderHit;
	public float characterColliderBuffer = 0.05f;
	public bool checkCollision = true;
	public bool checkCharacterController = false;

	// Use this for initialization
	void Start () {
		//Assume the character starts in the air and if they don't a collision event should fire soon.
		m_collisionCount = 0;
		if(onEnterAir != null)
			onEnterAir.Invoke();
	}
	
	// Update is called once per frame
	void Update () {
	}
	void LateUpdate()
	{
		if (checkCharacterController) {
			if (oldCharColliderHit != charColliderHit) {
				if (charColliderHit == true) {
					exitAir ();
				} else {
					enterAir ();
				}
			}
			oldCharColliderHit = charColliderHit;
			if (Time.time - lastTimeCharColliderHit > characterColliderBuffer) {
				charColliderHit = false;
				lastTimeCharColliderHit = Time.time;
			}
		}
	}

	void OnCollisionEnter(Collision collision)
	{
		if (checkCollision) {
			increment ();
		}
	}

	void OnCollisionExit(Collision collision)
	{
		if (checkCollision) {
			decrement ();
		}
	}

	void OnControllerColliderHit(ControllerColliderHit hit)
	{
		if (!checkCharacterController)
			return;
		charColliderHit = true;
		lastTimeCharColliderHit = Time.time;
	}

	void increment()
	{
		m_collisionCount++;
		if(onGainCollision != null)
			onGainCollision.Invoke();
		if (m_collisionCount == 1){
			exitAir();
		}
	}
	void decrement()
	{
		m_collisionCount--;
		if (m_collisionCount < 0)
			Debug.LogError ("CharacterEvents: Somehow this character has left the air while in it");
		if(onLoseCollision != null)
			onLoseCollision.Invoke ();
		if (m_collisionCount == 0) {
			enterAir();
		}
	}
	void enterAir()
	{
		if(onEnterAir != null)
		{
			onEnterAir.Invoke ();
		}
	}
	void exitAir()
	{
		if(onExitAir != null) {
			onExitAir.Invoke ();
		}
	}
}
