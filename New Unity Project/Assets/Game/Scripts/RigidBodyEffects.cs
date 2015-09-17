using UnityEngine;
using System.Collections;
using AssemblyCSharp;

[RequireComponent(typeof(Rigidbody))]
public class RigidBodyEffects : MonoBehaviour {
	Rigidbody rigidBody;
	public RigidBodyEffect[] effects;

	// Use this for initialization
	void Start () {
		rigidBody = GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void UseEffect(int i)
	{
		ApplyEffect (i, false);
	}

	public void SubtractEffect(int i)
	{
		ApplyEffect (i, true);
	}

	void ApplyEffect(int i, bool negative)
	{
		RigidBodyEffect effect = effects[i];
		
		Quaternion rotation = effect.isLocal ? transform.rotation : Quaternion.identity;
		rigidBody.velocity = rotation * effect.setVelocity;
		rigidBody.angularVelocity = rotation * effect.setAngularVelocity;
		transform.Translate(rotation * effect.addPosition * (negative ? -1 : 1));
	}
}
