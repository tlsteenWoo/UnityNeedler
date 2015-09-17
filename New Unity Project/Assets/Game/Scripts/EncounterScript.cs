using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class EncounterScript<T> : MonoBehaviour
	where T:Component
{
	public T mostRecent{ get; private set; }
	public Rigidbody mostRecentRigidBody{ get; private set; }
	public Collider mostRecentCollider{ get; private set; }
	/// <summary>
	/// Gets the most recent game object. This will be the object attached to the rigib body of the collider,
	/// or the game object attached to the collider if it has no rigid body.
	/// </summary>
	/// <value>The most recent game object.</value>
	public GameObject mostRecentGameObject{ get; private set; }
	public UnityEvent onTargetEnter;
	public UnityEvent onTargetExit;
	//extra
	public bool log;
	public bool IsEnabled
	{
		get{ return internalEnabled;}
		set{ internalEnabled = value;}
	}
	public bool internalEnabled = true;
	
	void OnTriggerEnter(Collider other)
	{
		if (!internalEnabled)
			return;
		if (!BindType (other))
			return;
		if (log)
			Debug.Log (string.Format("EncounterScript: {0} Encountered {1}", gameObject, other));
		if(onTargetEnter != null)
			onTargetEnter.Invoke ();
	}
	
	void OnTriggerExit(Collider other)
	{
		if (!internalEnabled)
			return;
		if (!BindType (other))
			return;
		if (log)
			Debug.Log (string.Format("EncounterScript: {1} Left {0}", gameObject, other));
		if(onTargetExit != null)
			onTargetExit.Invoke ();
	}

	bool BindType(Collider other)
	{
		T x = other.GetComponent<T> ();
		if (x == null) return false;
		mostRecent = x;
		mostRecentCollider = other;
		mostRecentGameObject = other.gameObject;
		mostRecentRigidBody = other.attachedRigidbody;
		if (mostRecentRigidBody != null)
			mostRecentGameObject = mostRecentRigidBody.gameObject;
		return true;
	}
	
	public void DisableRecent()
	{
		mostRecentGameObject.SetActive(false);
	}
}
