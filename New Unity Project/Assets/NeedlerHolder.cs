using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class NeedlerHolder : MonoBehaviour {
	public GameObject needlerPrefabAsset;
	//Use GetNeedler to get, and set this variable directly if you must... its dirty
	protected GameObject m_needler_dirty_ref;
	protected NeedlerBehaviour m_needlerBehaviour_dirty_ref;
	public bool needlerActive;


	// Use this for initialization
	void Start () {
		updateNeedler ();
	}
	
	// Update is called once per frame
	void Update () {
		updateNeedler ();
	}

	/// <summary>
	/// Tries to find a instance of needler as a child
	/// </summary>
	public GameObject GetOrFindNeedler()
	{
		//Check if we do not have an internal needler reference
		if (m_needler_dirty_ref == null) {
			//search for a needler child and update the instance if there is one
			var needleChild = transform.FindChild (needlerPrefabAsset.name);
			//search for a clone as well
			if(needleChild == null)
				needleChild = transform.FindChild(needlerPrefabAsset.name+"(Clone)");
			//did we find a needler child?
			if (needleChild != null) {
				m_needler_dirty_ref = needleChild.gameObject;
				m_needlerBehaviour_dirty_ref = m_needler_dirty_ref.GetComponent<NeedlerBehaviour>();
			}		
		}
		return m_needler_dirty_ref;
	}

	/// <summary>
	/// Returns the needlerBehaviour reference, and tries to find one if it is null.
	/// </summary>
	/// <returns>The needler behaviour reference.</returns>
	public NeedlerBehaviour GetOrFindNeedlerBehaviour()
	{
		if (!m_needlerBehaviour_dirty_ref)
			GetOrFindNeedler ();
		return m_needlerBehaviour_dirty_ref;
	}

	/// <summary>
	/// Creates a needler if the needlerInstance is null
	/// </summary>
	void tryCreateNeedler()
	{
		//Check if we do not have a needler
		if (!GetOrFindNeedler ()) {
			//no needler exists so create one, and store the reference to the GameObject and behaviour
			m_needler_dirty_ref = (GameObject)Instantiate (needlerPrefabAsset, transform.position, transform.rotation);
			m_needlerBehaviour_dirty_ref = m_needler_dirty_ref.GetComponent<NeedlerBehaviour>();
			//make the needler a child
			m_needler_dirty_ref.transform.SetParent(this.transform);
		}
	}

	/// <summary>
	/// Destroys our NeedlerInstance if it is not null.
	/// </summary>
	void tryDestroyNeedler()
	{
		//Check if we do have a needler
		if (GetOrFindNeedler ()) {
			DestroyImmediate(m_needler_dirty_ref);
		}
	}
	/// <summary>
	/// Creates or destroys needler children based on needlerActive
	/// </summary>
	void updateNeedler()
	{
		if (needlerActive) {
			tryCreateNeedler();
		} else {
			tryDestroyNeedler();
		}
	}
}
