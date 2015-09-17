using UnityEngine;
using System.Collections;

/// <summary>
/// PathingBehaviour guides a transform or character controller along a path.
/// </summary>
public class PathingBehaviour : MonoBehaviour {
	protected bool isGoingReverse;
	public int nextNode = 0;
	public float rate = 1;
	public float reachThreshold = 0.5f;
	public NodeContainer path;
	public CharacterController controller;
	public NodeContainer GetSetPath
	{
		get{ return path;}
		set{ path = value;}
	}
	
	void Update()
	{
		FollowPath ();
	}

	Vector3 getPosition()
	{
		if (controller != null)
			return controller.transform.position;
		return transform.position;
	}
	void applyChange(Vector3 direction)
	{
		if (controller != null) {
			controller.SimpleMove (direction * rate);
			controller.transform.forward = direction;
		} else {
			transform.position += direction * rate;
			transform.forward = direction;
		}
	}

	void FollowPath()
	{
		if (reachedNode ())
		{
			setNextNode();
		}
		chaseNode ();
	}

	Vector3 ToTarget()
	{
		Vector3 position = getPosition ();
		Vector3 target = path.GetNode(nextNode).transform.position;
		Vector3 toTarget = target - position;
		//flatten y since its not a factor in SimpleMove
		if (controller != null)
			toTarget.y = 0;
		return toTarget;
	}

	bool reachedNode()
	{
		Vector3 diff = ToTarget ();
		return diff.sqrMagnitude <= reachThreshold*reachThreshold;
	}

	void setNextNode()
	{
		if (isGoingReverse)
			nextNode--;
		else
			nextNode++;
		if (nextNode >= path.Count ()) {
			if (!path.isClosedLoop) {
				isGoingReverse = true;
				nextNode = path.Count() - 2;
			}
			else
			{
				nextNode = nextNode % path.Count();
			}
		}
		else if(nextNode < 0)
		{
			if(!path.isClosedLoop){
				isGoingReverse = false;
				nextNode = 1;
			}
			else
			{
				nextNode = path.Count() + nextNode;
			}
			}
	}

	void chaseNode()
	{
		Vector3 toNode = ToTarget ();
		toNode.Normalize ();
		applyChange (toNode);
	}
}
