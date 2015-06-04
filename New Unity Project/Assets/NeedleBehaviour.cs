using UnityEngine;
using System.Collections;

/// <summary>
/// NeedleBehaviour is a component for NeedleProjectiles.
/// It provides logic for flying and expiration.
/// </summary>
public class NeedleBehaviour : MonoBehaviour {
	public bool isStuck{ get; protected set; }
	//7 needles will kill a grunt dead
	public int damage = 10;
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
	public AudioClip impactSound;
	protected AudioSource audioSource;

	/// <summary>
	/// Store references to the physics and sound components.
	/// </summary>
	void Start () {
		rigidBody = GetComponent<Rigidbody> ();
		//Initialize the needles travel
		rigidBody.velocity = transform.forward * travelSpeed;
		audioSource = GetComponent<AudioSource> ();
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
	/// If a needle host is hit it has a special reaction.
	/// </summary>
	/// <param name="host">Host which was impacted bu the needle.</param>
	public void Impact(Collider host)
	{
		isStuck = true;
		//pull the needle out a little so it always shows
		transform.position -= rigidBody.velocity * Time.deltaTime;
		//attach it to the object
		transform.SetParent (host.gameObject.transform);
		//Rigid body is neutralized so that no more collision response occurs
		rigidBody.velocity = Vector3.zero;
		rigidBody.isKinematic = true;
		Refresh ();
		audioSource.PlayOneShot (impactSound, 1);
		var needleHost = host.gameObject.GetComponent<NeedleHost> ();
		if (needleHost == null)
			return;
		needleHost.Hit (this);
	}

	/// <summary>
	/// If the target is not null, pushes the needle towards its target.
	/// </summary>
	void TrackTarget()
	{
		if (target == null || isStuck)
			return;
		Vector3 targetPosition = target.transform.position;
		//use the representative if one is specified
		if (target.representative != null)
			targetPosition = target.representative.bounds.center;
		//Get the vector to the target
		Vector3 toTarget = targetPosition - transform.position;
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
		Destroy(this.gameObject);
	}
}
