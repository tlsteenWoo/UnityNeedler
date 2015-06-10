using UnityEngine;
using System.Collections;

/// <summary>
/// NodeHead treats all children as other nodes. Useful for pathing.
/// </summary>
[ExecuteInEditMode]
public class NodeContainer : MonoBehaviour {
	public bool addNode = false;
	public float downwardsRayLength = 10;
	public GameObject node;
	protected GameObject[] nodes;

	//Ensure addNode is false
	void Awake()
	{
		addNode = false;
	}

	/// <summary>
	/// When the user increases the number of nodes, new nodes should be created.
	/// When the user deletes a node the length should adjust.
	/// </summary>
	void Update()
	{
		//nodes include this node head + all children
		if (addNode) {
			addNode = false;
			var newNode = (GameObject)Instantiate(node, transform.position, transform.rotation);
			//use child count in anticipation of any shifts
			newNode.name = node.name + '_' + (transform.childCount);
			newNode.transform.SetParent(this.transform);
		}
		//node array should synchronize with children if it has an inccorect length
		if (nodes == null || nodes.Length != transform.childCount) {
			nodes = null;
			nodes = new GameObject[transform.childCount];
			for (int c = 0; c < transform.childCount; ++c) {
				nodes [c] = transform.GetChild(c).gameObject;
			}
		}
		//draw connections
		if (nodes.Length > 0) {
			for (int i = 0; i < nodes.Length; ++i) {
				//Draw a ray downwards to help find the ground
				Vector3 position = nodes[i].transform.position;
				Debug.DrawLine (position, position + Vector3.down * downwardsRayLength, Color.red);
				//skip the first node since we draw lines backwards
				if(i == 0) continue;
				//draw the line to the previous node
				Debug.DrawLine (position, nodes [i - 1].transform.position, Color.cyan);
			}
		}
	}
}
