using UnityEngine;
using System.Collections;

/// <summary>
/// NeedlerPlayerController, provides user input for a NeedlerCharacter.
/// If there is a character controller it will provide input for that as well.
/// </summary>
[RequireComponent(typeof(NeedlerCharacter))]
public class NeedlerPlayerController : MonoBehaviour {
	NeedlerCharacter needlerChar;

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
	}
}
