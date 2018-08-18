using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Valve.VR.InteractionSystem;

public class Gauntlet : MonoBehaviour {
	private Hand.AttachmentFlags attachmentFlags = Hand.defaultAttachmentFlags & (~Hand.AttachmentFlags.DetachOthers);
	public Vector3 handOffset, rotationOffset;

	public GameObject UIContainer;
	public GameObject UIOne, UITwo;

	public GameObject newHandPrefab;
	public GameObject newHand;

	Hand equippedHand = null;

	public float followSpeed, rotateSpeed;

	public UnityEvent GauntletAction;

	public float thumbstickYPrevious, thumbstickYCurrent;
	void Update () {
		
		if (equippedHand != null) {
			if (equippedHand.controller != null && equippedHand.controller.GetPressDown(Valve.VR.EVRButtonId.k_EButton_ApplicationMenu)) {
				UIContainer.SetActive(!UIContainer.activeSelf);
			}

			if (equippedHand.controller != null) {
				if (equippedHand.controller.GetPressDown(Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger)) {
					if (GauntletAction != null) {
						GauntletAction.Invoke();
					}
				}
				
				if (equippedHand.controller.GetPressDown(Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad)) {
					Vector2 trackPadVector = equippedHand.controller.GetAxis(Valve.VR.EVRButtonId.k_EButton_Axis0);
					//Renderer r = GetComponent<Renderer>();
					//r.material.EnableKeyword("_EMISSION");
					//.material.SetColor("_EmissionColor", Color.Lerp (Color.blue, Color.red, trackPadVector.x));
					CycleActiveUI(trackPadVector.x);
				}
				
			}
		}
	}

	void ManageThumbstickUpRelease () {
		
	}
	private void HandHoverUpdate(Hand hand)
	{
		if (hand.GetStandardInteractionButtonDown() || ((hand.controller != null) && hand.controller.GetPressDown(Valve.VR.EVRButtonId.k_EButton_Grip)))
		{
			OnInteract(hand);
		}	
	}

	private void OnInteract( Hand hand )
	{
		GetComponent<Collider>().enabled = false;


		GetComponent<Interactable>().enabled = false;

		UIContainer.SetActive(true);
		equippedHand = hand;
		
		//Spawn new controller
		GameObject g = Instantiate(newHandPrefab);
		hand.AttachObject(g, attachmentFlags);

		newHand = g;

		this.transform.SetParent(g.transform);
		transform.localPosition = handOffset;
		transform.localRotation = Quaternion.Euler(rotationOffset);

		hand.teleportButton = Hand.TeleportButton.Thumbstick;
	}

	public void CycleActiveUI (float direction) {

		if (direction > 0) 
		{
			if (UIContainer.activeSelf) {
				if (UIOne.activeSelf) {
					UIOne.SetActive(false);
					UITwo.SetActive(true);
				} else {
					UIOne.SetActive(true);
					UITwo.SetActive(false);
				}
			}
		}
		else 
		{
			if (UIContainer.activeSelf) {
				if (UIOne.activeSelf) {
					UIOne.SetActive(false);
					UITwo.SetActive(true);
				} else {
					UIOne.SetActive(true);
					UITwo.SetActive(false);
				}
			}
		}
		
	}

}
