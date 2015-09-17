using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// NeedlerPlayerController, provides user input for a NeedlerCharacter.
/// If there is a character controller it will provide input for that as well.
/// </summary>
[RequireComponent(typeof(NeedlerCharacter))]
public class NeedlerPlayerController : MonoBehaviour {
	protected NeedlerCharacter needlerChar;
	public CharacterController charController;
	public float speed = 1;

	/// <summary>
	/// Obtain the needler character component.
	/// </summary>
	void Awake()
	{
		needlerChar = GetComponent<NeedlerCharacter> ();
	}

	/// <summary>
	/// Constantly allow the user to control the NeedlerCharacter and CharacterController
	/// </summary>
	void Update()
	{
		ControlNeedlerCharacter ();
		ControlCharacterController();
	}

	/// <summary>
	/// Acquires user input to control the needler character.
	/// </summary>
	void ControlNeedlerCharacter()
	{
		if (Input.GetButton ("Fire1"))
			needlerChar.Shoot (-1);
		if (Input.GetButton ("Fire2"))
			needlerChar.Shoot (1);
		if (Input.GetButton ("Reload"))
			needlerChar.Reload (0);
		needlerChar.Turn (Input.GetAxis ("Mouse X"));
		needlerChar.Look (Input.GetAxis ("Mouse Y"));
	}

	/// <summary>
	/// Acquires user input to control a specified character controller.
	/// </summary>
	void ControlCharacterController()
	{
		if (charController == null)
			return;
		float horizontal = Input.GetAxis ("Horizontal");
		float vertical = Input.GetAxis ("Vertical");
		//TODO: Make a debug option
		string debug = string.Format ("Strafe: {0:F1}, Walk: {1:F1}", horizontal * speed, vertical * speed);
		var dbg = GameObject.FindObjectOfType<DebugWindow> ();
		if (dbg)
			dbg.LOG (debug);
		Vector3 sideways = charController.gameObject.transform.right * speed * horizontal;
		Vector3 frontways = charController.gameObject.transform.forward * speed * vertical;
		Vector3 movement = sideways + frontways;
		charController.SimpleMove (movement); 
	}
}
