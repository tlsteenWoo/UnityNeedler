using UnityEngine;
using System.Collections;

/// <summary>
/// NeedlerBehaviour hosts the mechanics and visuals of the needler.
/// It uses the needler spawner to produce needles, and it uses
/// the animation controller to animate the needler mesh accordingly.
/// </summary>
[RequireComponent(typeof(NeedleSpawner))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AudioSource))]
public class NeedlerBehaviour : MonoBehaviour {
	public Animator muzzleFlareAC;
	protected Animator animator;
	protected NeedleSpawner spawner;
	protected AudioSource audioSource;
	//this hash is reused for needlerAC and muzzleFlareAC
	protected int shootTriggerHash;
	protected int reloadTriggerHash;
	public int reserveAmmo = 100;
	public int reserveAmmoLimit = 200;
	public AudioClip shootSound;
	public AudioClip reloadSound;

	/// <summary>
	/// Store reference to animation controller and needle spawner.
	/// </summary>
	void Start () {
		animator = GetComponent<Animator> ();
		spawner = GetComponent<NeedleSpawner> ();
		audioSource = GetComponent<AudioSource> ();
		shootTriggerHash = Animator.StringToHash ("ShootTrigger");
		reloadTriggerHash = Animator.StringToHash ("ReloadTrigger");
	}

	/// <summary>
	/// Shoot a needle and trigger animation.
	/// </summary>
	public bool Shoot()
	{
		if (spawner.Trigger ()) {
			audioSource.PlayOneShot(shootSound);
			animator.SetTrigger(shootTriggerHash);
			muzzleFlareAC.SetTrigger(shootTriggerHash);
			return true;
		}
		return false;
	}

	/// <summary>
	/// Refill the spawner when the needler completes it's reload animation.
	/// </summary>
	public bool Reload()
	{
		//Return false if we have no ammo or the spawner is full
		if (reserveAmmo <= 0 || spawner.Ammo == spawner.ammoLimit)
			return false;
		//Commence reloading via animation
		animator.SetTrigger (reloadTriggerHash);
		//play reload sound as well
		audioSource.PlayOneShot (reloadSound);
		return true;
	}

	/// <summary>
	/// Triggered when the reload animation is successfully completed.
	/// At this moment the needler should be refilled to reflect the animation.
	/// </summary>
	public void onReloadComplete()
	{
		//refill as much ammo as we can
		int ammoTaken = spawner.Refill (reserveAmmo);
		reserveAmmo -= ammoTaken;
	}
}
