using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class Gauntlet : MonoBehaviour {
	private Hand.AttachmentFlags attachmentFlags = Hand.defaultAttachmentFlags & (Hand.AttachmentFlags.ParentToHand) & (~Hand.AttachmentFlags.DetachOthers);
	public Vector3 handOffset, rotationOffset;

	public GameObject UIContainer;

	public GameObject newHandPrefab;
	public GameObject newHand;

	Hand equippedHand = null;
	void Update () {
		if (equippedHand != null && equippedHand.controller != null && equippedHand.controller.GetPressDown(Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad)) {
			UIContainer.SetActive(!UIContainer.activeSelf);
		} 
		
	}
	private void HandHoverUpdate(Hand hand)
	{
		if (hand.GetStandardInteractionButtonDown() || ((hand.controller != null) && hand.controller.GetPressDown(Valve.VR.EVRButtonId.k_EButton_Grip)))
		{
			if (hand.currentAttachedObject != gameObject)
			{
				//hand.HoverLock(GetComponent<Interactable>());

				//hand.AttachObject(gameObject, attachmentFlags);
				OnInteract(hand);
				//Do not put attachy stuff here, put it in on attached. 
			}
			else
			{
				hand.DetachObject(gameObject);

				//hand.HoverUnlock(GetComponent<Interactable>());

				//Do not put detachy stuff here, put it in on detached. 
				
			}
		}	
	}

	private void OnInteract( Hand hand )
	{
		GetComponent<Collider>().enabled = false;

		transform.parent = hand.transform;
		transform.localPosition = handOffset;
		transform.localRotation = Quaternion.Euler(rotationOffset);

		GetComponent<Interactable>().enabled = false;

		UIContainer.SetActive(true);
		equippedHand = hand;

		hand.DetachObject(this.gameObject);
		StartCoroutine(AttachNewController(hand));
	}

	IEnumerator AttachNewController (Hand hand) {
		yield return new WaitForSeconds(1);
		GameObject g = Instantiate(newHandPrefab);
		//hand.AttachObject(g, attachmentFlags);
		g.transform.parent = hand.transform;
		g.transform.localPosition = Vector3.zero;
		g.transform.localRotation = Quaternion.Euler(Vector3.zero);
		newHand = g;
		yield break;
	}

	private void OnDetachedFromHand( Hand hand )
	{

	}
}
