using UnityEngine;
using System.Collections;

public class NeedleHost : MonoBehaviour {
	int needleCount;
	//the explosion should blow a grunt off his feet.
	//TODO: Make the explosion global
	public int explosionDamage = 80;
	public int explosionIndex = 5;
	public float explosionResetTime = 1;
	protected float explosionResetProgress;
	public float respawnTime = 4;
	protected float respawnProgress;
	//this should not be accessed directly
	private float m_real_health;
	public float Health {
		get{ return m_real_health;}
		set {
			m_real_health = Mathf.Clamp (value, 0, maxHealth);
			if (IsDead) {
				onDeath ();
			}
		}
	}
	public bool IsDead{
		get{ return m_real_health <= 0;}
	}
	//100 pounds of flesh 0.0
	//Grunts are small but meaty
	public float maxHealth = 100;
	protected Vector3 startingPosition;
	protected NeedleBehaviour[] needles;
	public ParticleSystem explosionVfx;
	protected Animator animator;
	public AudioClip explosionSound;
	protected AudioSource audioSource;
	public SkinnedMeshRenderer representative;

	/// <summary>
	/// Start health at maxHealth and construct needle array.
	/// Store starting variables for respawn.
	/// </summary>
	void Start () {
		Health = maxHealth;
		needles = new NeedleBehaviour[explosionIndex];
		startingPosition = transform.position;
		animator = GetComponent<Animator> ();
		audioSource = GetComponent<AudioSource> ();
	}
	
	/// <summary>
	/// Checks for explosion reset and respawn.
	/// </summary>
	void Update () {
		explosionResetProgress += Time.deltaTime;
		if (explosionResetProgress >= explosionResetTime) {
			Reset();
		}

		if (IsDead) {
			respawnProgress += Time.deltaTime;
			if (respawnProgress >= respawnTime) {
				Respawn ();
			}
		}
	}

	/// <summary>
	/// Removes all needles, and resets explosion progress.
	/// </summary>
	void Reset()
	{
		for (int i = 0; i < needleCount; ++i) {
			needles[i].Expire();
		}
		needleCount = 0;
		explosionResetProgress = 0;
	}

	/// <summary>
	/// Refresh needles and reset the explosion timeout so we can wait for the explosion.
	/// </summary>
	void Refresh()
	{
		explosionResetProgress = 0;
		for(int i = 0; i < needleCount; ++i)
		{
			needles[i].Refresh();
		}
	}

	/// <summary>
	/// Trigger a large explosion, take damage, and reset.
	/// </summary>
	void Explode()
	{
		//Reset first in case the host dies
		Reset ();
		explosionVfx.Play ();
		audioSource.PlayOneShot (explosionSound, 1);
		Health -= explosionDamage;
	}

	/// <summary>
	/// When a host dies it will play an animation.
	//TODO: What do we do with the corpse?
	/// </summary>
	void onDeath()
	{
		//TODO: Play an animation
		animator.SetBool ("dead", true);
	}

	/// <summary>
	/// Refill health and reset respawn timer.
	/// Prop the host back up so they look alive.
	/// </summary>
	public void Respawn()
	{
		respawnProgress = 0;
		Health = maxHealth;
		//TODO: Navigate away from death animation
		animator.SetBool ("dead", false);
	}

	/// <summary>
	/// When hit by a needle the host will store it,
	/// increment needle count, take damage, and potentially explode.
	/// If it doesn't explode the reset progress will ...reset.
	/// Waiting for the explosion index to be reached.
	/// </summary>
	/// <param name="needle">Needle.</param>
	public void Hit(NeedleBehaviour needle)
	{
		//ignore special interactions if dead
		if (IsDead)
			return;
		needles [needleCount++] = needle;
		Health -= needle.damage;
		if (needleCount >= explosionIndex) {
			Explode ();
		} else {
			//wait for more needles
			Refresh();
		}
	}
}
