using UnityEngine;
using System.Collections;

/// <summary>
/// NeedleBehaviour is a component for NeedleProjectiles.
/// It provides logic for flying and expiration.
/// </summary>
public class NeedleBehaviour : MonoBehaviour {
	protected bool isStuck;
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
	/// When a NeedleProjectile Impacts, make it stick into object it hit, and refresh its life.
	/// Play an impact sound to confirm the hit.
	/// </summary>
	/// <param name="host">Host which was impacted bu the needle.</param>
	public void Impact(Collider host)
	{
		isStuck = true;
		//attach it to the object
		transform.SetParent (host.gameObject.transform);
		//Rigid body is destroyed so that no more collisions or forces occur.
		var rigidBody = GetComponent<Rigidbody> ();
		Destroy (rigidBody);
		Refresh ();
		//TODO: Play impact sound
	}

	/// <summary>
	/// Restart this needles life, preventing expiration
	/// </summary>
	public void Refresh()
	{
		lifeLived = 0;
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
		//do not move a needle which is stuck
		if (isStuck)
			return;
		//Advance the needle forward based on direction, speed, and change in time
		transform.position += transform.forward * travelSpeed * deltaTime;
	}

}
