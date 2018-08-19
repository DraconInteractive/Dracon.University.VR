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

	public UnityEvent TriggerDownAction, TriggerUpAction;

	public float thumbstickYPrevious, thumbstickYCurrent;

	public LineRenderer pointer;
	public GameObject pointerTarget = null, pointerMarker;

	public LayerMask pointerMask;

	void Update () {
		
		if (equippedHand != null) {

			if (equippedHand.controller != null && equippedHand.controller.GetPressDown(Valve.VR.EVRButtonId.k_EButton_ApplicationMenu)) {
				UIContainer.SetActive(!UIContainer.activeSelf);
			}

			if (equippedHand.controller != null) {
				if (equippedHand.controller.GetPressDown(Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger)) {
					if (TriggerDownAction != null) {
						TriggerDownAction.Invoke();
					}
				}

				if (equippedHand.controller.GetPressUp(Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger)) {
					if (TriggerDownAction != null) {
						TriggerUpAction.Invoke();
					}
				}
				
				if (equippedHand.controller.GetPressDown(Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad)) {
					Vector2 trackPadVector = equippedHand.controller.GetAxis(Valve.VR.EVRButtonId.k_EButton_Axis0);
					CycleActiveUI(trackPadVector.x);
				}
			}

			if (pointer.enabled && equippedHand.AttachedObjects.Count <= 2) {
				Ray ray = new Ray (pointer.transform.position, pointer.transform.forward);
				RaycastHit hit;

				if (Physics.Raycast(ray, out hit, 100, pointerMask)) {
					pointerTarget = hit.transform.gameObject;
					pointer.SetPosition(1, new Vector3(0,0, hit.distance * 10));
					pointer.startColor = Color.green;
					pointer.endColor = Color.green;
					pointerMarker.transform.position = hit.point;
					
				} else {
					pointerTarget = null;
					pointer.SetPosition(1, new Vector3(0,0,100));
					pointer.startColor = Color.red;
					pointer.endColor = Color.red;
					pointerMarker.transform.localPosition = new Vector3(0,0,100);
				}

				if (pointerTarget != null) {
					Companion c = GetComponent<Companion>();
					if (c != null) {
						c.staredAt = true;
					}
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

	public void ShowActionPointer () {
		if (equippedHand.AttachedObjects.Count <= 2) {
			pointer.enabled = true;
		}
	}

	public void HideActionPointer () {
		pointer.enabled = false;
	}

	public void ActOnPointer () {
		if (pointerTarget != null) {
			
		}
	}
}
