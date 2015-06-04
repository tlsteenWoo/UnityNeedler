using UnityEngine;
using System.Collections;

/// <summary>
/// NeedleSearch is responsible for finding targets for a NeedleBehaviour.
/// </summary>
public class NeedleSearch : MonoBehaviour {
	public NeedleBehaviour needleBehaviour;

	/// <summary>
	/// Check for targets for the NeedleBehaviour.
	/// If it already has a target or the host is dead, return.
	/// </summary>
	/// <param name="other">Other.</param>
	public void OnTriggerEnter(Collider other)
	{
		if (needleBehaviour.Target != null)
			return;
		var needleHost = other.gameObject.GetComponent<NeedleHost> ();
		if (needleHost == null)
			return;
		if (needleHost.IsDead)
			return;
		needleBehaviour.Target = needleHost;
	}
}
