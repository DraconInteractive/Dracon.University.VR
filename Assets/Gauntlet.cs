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
	void Update () {
		
		if (equippedHand != null) {
			if (equippedHand.controller != null && equippedHand.controller.GetPressDown(Valve.VR.EVRButtonId.k_EButton_ApplicationMenu)) {
				UIContainer.SetActive(!UIContainer.activeSelf);
			}

			if (equippedHand.controller != null && equippedHand.controller.GetPressDown(Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger)) {
				if (GauntletAction != null) {
					GauntletAction.Invoke();
				}
			}
		}
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

	}

	public void CycleActiveUI () {
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
