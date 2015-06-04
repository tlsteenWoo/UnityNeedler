using UnityEngine;
using System.Collections;

public class NeedleSpawner : MonoBehaviour {
	public GameObject Needle;
	public bool automatic = true;
	public bool infiniteAmmo;
	public float spreadAngleYaw = 10;
	public float spreadAnglePitch = 10;
	protected bool fireReady;
	protected int ammo = 7;
	public int ammoLimit = 7;
	public float fireRate = 1;
	protected float fireRateProgress = 1;
	/// <summary>
	/// Returns the ammo available, and sets the ammo within the range [0, ammoLimit]
	/// </summary>
	/// <value>The ammo.</value>
	public int Ammo
	{
		get{ return ammo;}
		set{ ammo = Mathf.Clamp(value,0,ammoLimit);}
	}

	// Use this for initialization
	void Start () {
		//throw an error if needle is not defined
		if (Needle == null)
			throw new UnityException ("NeedleSpawner: Needle is not defined.");
	}
	
	// Update is called once per frame
	void Update () {
		//Udate the fireRate delay
		if (fireRateProgress < fireRate) {
			fireRateProgress += Time.deltaTime;
		}
		if(fireRateProgress >= fireRate)
		{
			fireReady = true;
			//set progress to the fire rate so progress won't go over 100%
			fireRateProgress = fireRate;
		}

		//refill ammo if we have infinite
		if (infiniteAmmo)
			ammo = ammoLimit;

		//Try to automatically trigger
		if (automatic) {
			Trigger();
		}
	}

	/// <summary>
	/// Attempts to create a new needle at this position and with a direction within the cone.
	/// </summary>
	public bool Trigger()
	{
		//Check for ammo
		if (ammo == 0)
			return false;
		//Check if the spawner is fire ready
		if (!fireReady)
			return false;

		//determine the needles properties
		Vector3 needlePosition = transform.position;
		Quaternion needleDirection = transform.rotation;
		//Spawn the needle and store a reference
		GameObject newNeedle = (GameObject)Instantiate (Needle, needlePosition, needleDirection);
		//randomize the yaw(x) and pitch(y)
		//multiply by .5 to make the spreadAngle variables accurate to the range
		Vector2 randomCircle = Random.insideUnitCircle;
		float randomYaw = randomCircle.y * spreadAngleYaw * 0.5f;
		float randomPitch = randomCircle.x * spreadAnglePitch * 0.5f;
		newNeedle.transform.Rotate (randomYaw, randomPitch, 0, Space.Self);

		//now restart the fireRate delay and decrement the ammo
		fireRateProgress = 0;
		fireReady = false;
		ammo--;

		return true;
	}
}
