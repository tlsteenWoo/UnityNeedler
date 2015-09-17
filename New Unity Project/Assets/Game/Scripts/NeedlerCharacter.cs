using UnityEngine;
using System.Collections;

/// <summary>
/// The needler character will have two needler holders,
/// which can shoot, reload, and holster.
/// It also carries reserve ammunition for the needle spawners.
/// </summary>
public class NeedlerCharacter : MonoBehaviour {
	public bool isPlanar = true;
	public int reserveAmmo = 100;
	public int reserveAmmoLimit = 200;
	public float turnRateDeg = 1;
	public float lookRateDeg = 1;
	//This must be less than 180
	public float lookLimitDeg = 160;
	public GameObject shoulders;
	public NeedlerHolder needlerHolderR;
	public NeedlerHolder needlerHolderL;
	public NeedleHost target;
	public float turnDegrees;
	public float lookDegrees;

	// Use this for initialization
	void Awake () {
	}

	/// <summary>
	/// Shoot the needlerHolder at the specified index.
	/// </summary>
	/// <param name="needlerHolderIndex">Needler holder index (-1 = Left, 1 = Right, 0 = Both).</param>
	public bool Shoot(int needlerHolderIndex)
	{
		bool oohShotsFired = false;
		//shoot left
		if(needlerHolderL && (needlerHolderIndex == 0 || needlerHolderIndex == -1))
		{
			oohShotsFired = oohShotsFired || shootIndividual(needlerHolderL);
		}
		//shoot right
		if(needlerHolderR && (needlerHolderIndex == 0 || needlerHolderIndex == 1))
		{
			oohShotsFired = oohShotsFired || shootIndividual(needlerHolderR);
		}
		return oohShotsFired;
	}

	/// <summary>
	/// Fires the needler attached to the specified needlerHolder.
	/// </summary>
	/// <returns>Successful shot.</returns>
	/// <param name="a_holder">Any needler holder.</param>
	bool shootIndividual(NeedlerHolder a_holder)
	{
		if (!a_holder.IsNeedlerReady)
			return false;
		var needler = a_holder.GetOrFindNeedlerBehaviour ();
		if (!needler)
			return false;
		return needler.Shoot ();
	}
	
	/// <summary>
	/// Reload the needlerHolder at the specified index.
	/// </summary>
	/// <param name="needlerHolderIndex">Needler holder index (-1 = Left, 1 = Right, 0 = Both).</param>
	public bool Reload(int needlerHolderIndex)
	{
		bool reloadFlag = false;
		//shoot left
		if(needlerHolderIndex == 0 || needlerHolderIndex == -1)
		{
			reloadFlag = reloadFlag || reloadIndividual(needlerHolderL);
		}
		//shoot right
		if(needlerHolderIndex == 0 || needlerHolderIndex == 1)
		{
			reloadFlag = reloadFlag || reloadIndividual(needlerHolderR);
		}
		return reloadFlag;
	}
	
	/// <summary>
	/// Reloads the needler attached to the specified needlerHolder. Decrements reserve ammo if successful.
	/// </summary>
	/// <returns>Successful reload.</returns>
	/// <param name="a_holder">Any needler holder.</param>
	bool reloadIndividual(NeedlerHolder a_holder)
	{
		if (!a_holder.IsNeedlerReady)
			return false;
		var needler = a_holder.GetOrFindNeedlerBehaviour ();
		if (!needler)
			return false;
		int ammoTaken = needler.beginReload (reserveAmmo);
		reserveAmmo -= ammoTaken;
		//if ammo taken is equal to 0 then the reload didnt occur
		return ammoTaken != 0;
	}

	/// <summary>
	/// Rotates the character on the Y axis
	/// </summary>
	/// <param name="direction">Direction, any number.</param>
	public void Turn(float direction)
	{
		turnDegrees += turnRateDeg * direction;
	}

	/// <summary>
	/// Rotates the shoulders on the X axis
	/// </summary>
	/// <param name="direction">Direction, any number.</param>
	public void Look(float direction)
	{
		lookDegrees += lookRateDeg * direction;
		//split in half so we can accurately clamp the range around the center
		float lookLimitHalf = (lookLimitDeg / 2);
		//clamp the shoulder x rotation in the range [-lookLimit/2 , lookLimit/2 ]
		lookDegrees = Mathf.Clamp(lookDegrees, -lookLimitHalf, lookLimitHalf);
	}

	/// <summary>
	/// Causes the shoulders to point towards a point, using look and turn.
	/// </summary>
	/// <param name="target">Target point.</param>
	public void LookAt(Vector3 target)
	{
		Vector3 toTarget = target - shoulders.transform.position;
		Vector3 directionToTarget = toTarget.normalized;
		
		//Tilt the character to find targets above and below
		//Get the dot product on the vertical axis to determine eye level
		float upDifference = Vector3.Dot (shoulders.transform.up, directionToTarget);
		//not sure why negation is neccessary but it is
		float eyeLevel = Mathf.Clamp (-upDifference, -1, 1);
		if (eyeLevel > 0)
			Look (eyeLevel);
		else if(eyeLevel < 0)
			Look (eyeLevel);

		//Turn the character to find targets right and left
		//Flatten the two directions, right and toTarget, so they lie on the ground plane
		Vector2 planarTargetDirection = new Vector2 (directionToTarget.x, directionToTarget.z);
		Vector2 planarRight = new Vector2 (shoulders.transform.right.x, shoulders.transform.right.z);
		//check whether the target direction points to the right of the shoulders
		float horizontal = Vector2.Dot (planarRight, planarTargetDirection);
		if (horizontal < 0) {
			Turn (horizontal);
		} else if (horizontal > 0) {
			Turn (horizontal);
		}
	}

	/// <summary>
	/// Automatically reload empty needlers
	/// </summary>
	void Update () {
		if (target != null) {
			Vector3 targetPos = target.transform.position;
			if(target.representative)
			{
				targetPos = target.representative.bounds.center;
			}
			LookAt (targetPos);
		}
		UpdateOrientation ();
		//TODO: Manage reserve ammo in a property
		reserveAmmo = Mathf.Clamp (reserveAmmo, 0, reserveAmmoLimit);
		if(needlerHolderL)
			AutomaticReload (needlerHolderL);
		if(needlerHolderR)
			AutomaticReload (needlerHolderR);
	}
	/// <summary>
	/// Automatically reloads a needlerHolder if it is empty.
	/// </summary>
	/// <param name="a_holder">The holder to check for emptiness.</param>
	void AutomaticReload(NeedlerHolder a_holder)
	{
		if (!a_holder.IsNeedlerReady)
			return;
		if (a_holder.GetOrFindNeedlerBehaviour ().spawner.Ammo == 0)
			reloadIndividual (a_holder);
	}
	/// <summary>
	/// Keeps the character planar and limits the shoulders up/down
	/// </summary>
	void UpdateOrientation()
	{
		//operate on character rotation
		var charEuler = transform.eulerAngles;
		if (isPlanar) {
			charEuler.z = 0;
			charEuler.x = 0;
		}
		charEuler.y = turnDegrees;
		transform.eulerAngles = charEuler;
		//operate on shoulder rotation
		var shoulderEuler = shoulders.transform.localEulerAngles;
		shoulderEuler.x = lookDegrees;
		//set these to 0 to ensure proper targeting
		shoulderEuler.y = 0;
		shoulderEuler.z = 0;
		shoulders.transform.localEulerAngles = shoulderEuler;
	}
}
