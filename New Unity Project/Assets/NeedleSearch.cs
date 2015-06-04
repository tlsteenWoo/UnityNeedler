using UnityEngine;
using System.Collections;

/// <summary>
/// NeedleSearch is responsible for finding targets for a NeedleBehaviour.
/// </summary>
public class NeedleSearch : MonoBehaviour {
	public NeedleBehaviour needleBehaviour;

	/// <summary>
	/// Check for targets for the NeedleBehaviour.
	/// If it already has one then exit
	/// </summary>
	/// <param name="other">Other.</param>
	public void OnTriggerEnter(Collider other)
	{
		if (needleBehaviour.Target != null)
			return;
		var needleHost = other.gameObject.GetComponent<NeedleHost> ();
		if (needleHost == null)
			return;
		needleBehaviour.Target = needleHost;
	}
}
