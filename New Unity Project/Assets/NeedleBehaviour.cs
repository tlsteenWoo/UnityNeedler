using UnityEngine;
using System.Collections;

/// <summary>
/// NeedleBehaviour is a component for NeedleProjectiles.
/// It provides logic for flying and expiration.
/// </summary>
public class NeedleBehaviour : MonoBehaviour {
	public float travelSpeed = 1;
	public float lifeTime = 3;
	protected float lifeLived;
	protected float deltaTime;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		deltaTime = Time.deltaTime;
		lifeLived += deltaTime;
		if (lifeLived > lifeTime) {
			Expire ();
		} else {
			Fly ();
		}
	}

	/// <summary>
	/// Destroys the NeedleProjectile.
	/// </summary>
	public void Expire()
	{
		//destroy the NeedleProjectile gameObject this is attached to
		DestroyImmediate (this.gameObject);
	}

	/// <summary>
	/// The needle will fly forward, based on travel speed and change in time.
	/// </summary>
	void Fly()
	{
		//Advance the needle forward based on direction, speed, and change in time
		transform.position += transform.forward * travelSpeed * deltaTime;
	}

}
