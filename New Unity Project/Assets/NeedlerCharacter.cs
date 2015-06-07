using UnityEngine;
using System.Collections;

/// <summary>
/// The needler character will have two needler holders,
/// which can shoot, reload, and holster.
/// It also carries reserve ammunition for the needle spawners.
/// </summary>
public class NeedlerCharacter : MonoBehaviour {
	public NeedlerHolder needlerHolderR, needlerHolderL;
	public int reserveAmmo = 100;
	public int reserveAmmoLimit = 200;

	// Use this for initialization
	void Start () {
	
	}

	/// <summary>
	/// Shoot the needlerHolder at the specified index.
	/// </summary>
	/// <param name="needlerHolderIndex">Needler holder index (-1 = Left, 1 = Right, 0 = Both).</param>
	public bool Shoot(int needlerHolderIndex)
	{
		bool oohShotsFired = false;
		//shoot left
		if(needlerHolderIndex == 0 || needlerHolderIndex == -1)
		{
			oohShotsFired = oohShotsFired || shootIndividual(needlerHolderL);
		}
		//shoot right
		if(needlerHolderIndex == 0 || needlerHolderIndex == -1)
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
		if(needlerHolderIndex == 0 || needlerHolderIndex == -1)
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
		reserveAmmo -= needler.beginReload (reserveAmmo);
		return reserveAmmo != 0;
	}

	/// <summary>
	/// Automatically reload empty needlers
	/// </summary>
	void Update () {
		//TODO: Remove this test
		Shoot (0);
		//TODO: Manage reserve ammo in a property
		reserveAmmo = Mathf.Clamp (reserveAmmo, 0, reserveAmmoLimit);
		AutomaticReload (needlerHolderL);
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
}
