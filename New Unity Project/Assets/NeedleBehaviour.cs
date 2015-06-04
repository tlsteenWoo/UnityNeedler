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
	public float trackStrength = 100;
	public float trackDamping = .7f;
	protected NeedleHost target;
	protected float lifeLived;
	protected float deltaTime;
	public NeedleHost Target
	{
		get{ return target;}
		set{ target = value;}
	}
	protected Rigidbody rigidBody;

	// Use this for initialization
	void Start () {
		rigidBody = GetComponent<Rigidbody> ();
		rigidBody.velocity = transform.forward * travelSpeed;
	}
	
	// Update is called once per frame
	void Update () {
		deltaTime = Time.deltaTime;
		lifeLived += deltaTime;
		if (lifeLived > lifeTime) {
			Expire ();
		} else {
			TrackTarget();
		}
	}

	/// <summary>
	/// Used purely to update the needle direction
	/// </summary>
	void LateUpdate()
	{
		if (isStuck)
			return;
		if (rigidBody.velocity.sqrMagnitude > 0) {
			transform.forward = rigidBody.velocity.normalized;
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
	/// If the target is not null, pushes the needle towards its target.
	/// </summary>
	void TrackTarget()
	{
		if (target == null || isStuck)
			return;
		//Get the vector to the target
		Vector3 toTarget = target.transform.position - transform.position;
		//store the length
		float magnitude = toTarget.magnitude;
		//normalize
		toTarget /= magnitude;
		//multiply by 1/length * scalar to diminish by distance than linearly strengthen
		Vector3 force = toTarget * (1 / magnitude) * trackStrength;
		rigidBody.AddForce(force);
		rigidBody.velocity *= trackDamping;
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
}
