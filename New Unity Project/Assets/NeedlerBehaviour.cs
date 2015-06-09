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
	//The needler was somehow being used before it had started
	protected bool hasStarted;
	protected bool isReloading;
	//this hash is reused for needlerAC and muzzleFlareAC
	protected int shootTriggerHash;
	protected int reloadTriggerHash;
	public int ammoInbound{ get; protected set; }
	public AudioClip shootSound;
	public AudioClip reloadSound;
	protected AudioSource audioSource;
	public Animator muzzleFlareAC;
	protected Animator animator;
	public NeedleSpawner spawner{ get; protected set; }
	
	/// <summary>
	/// Store reference to animation controller and needle spawner.
	/// </summary>
	void Awake()
	{
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
	/// Check for ammo, if needed then begin the reload
	/// </summary>
	/// <returns>The amount of ammo that will be reloaded.</returns>
	/// <param name="a_ammo">All ammo available for reloading.</param>
	public int beginReload(int a_ammo)
	{
		//return nothing if we got no ammo, or if the reload is already in action, or if the spawner is full
		if (a_ammo <= 0 || isReloading || spawner.AmmoSpace == 0)
			return 0;
		//Commence reloading via animation
		animator.SetTrigger (reloadTriggerHash);
		isReloading = true;
		//play reload sound as well
		audioSource.Play ();
		//use only as much space as we have
		ammoInbound = Mathf.Min (spawner.AmmoSpace, a_ammo);
		return ammoInbound;
	}

	/// <summary>
	/// Triggered when the reload animation is successfully completed.
	/// At this moment the needler should be refilled with the inbound ammo, reflecting the animation.
	/// Another reload becomes possible.
	/// </summary>
	public void onReloadComplete()
	{
		//refill as much ammo as we can, ammoInbound should be exactly what we need
		spawner.Refill (ammoInbound);
		isReloading = false;
	}
}
