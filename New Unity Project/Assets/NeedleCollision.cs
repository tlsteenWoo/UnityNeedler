using UnityEngine;
using System.Collections;

/// <summary>
/// This component is used to separate needle impact triggers from needle search triggers.
/// </summary>
public class NeedleCollision : MonoBehaviour {
	public NeedleBehaviour needleBehaviour;

	/// <summary>
	/// If this trigger encounters a non-trigger then trigger needle impact.
	/// </summary>
	/// <param name="other">The non-trigger impact collider.</param>
	public void OnTriggerEnter(Collider other)
	{
		//If the needle is already stuck then dont impact again
		if (needleBehaviour.isStuck)
			return;
		if (other.isTrigger)
			return;
		needleBehaviour.Impact (other);
	}
}
