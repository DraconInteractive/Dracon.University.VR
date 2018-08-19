using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class TwinSword_OBJ : MonoBehaviour {

	public Animation statueAnimation;
	private Hand.AttachmentFlags attachmentFlags = Hand.defaultAttachmentFlags & (Hand.AttachmentFlags.SnapOnAttach ) & ( ~Hand.AttachmentFlags.DetachOthers );

	public TrailRenderer[] trails;

	bool active;
	float activeTimer = 0;

	public Material runeMaterial;
	public Color runeColor;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

		private void HandHoverUpdate( Hand hand )
		{
			if (( ( hand.controller != null ) && hand.controller.GetPressDown( Valve.VR.EVRButtonId.k_EButton_Grip ) ) )
			{
				if ( hand.currentAttachedObject != gameObject )
				{

					// Call this to continue receiving HandHoverUpdate messages,
					// and prevent the hand from hovering over anything else
					hand.HoverLock( GetComponent<Interactable>() );
					
					// Attach this object to the hand
					hand.AttachObject( gameObject, attachmentFlags);

					transform.position = hand.transform.position;
					transform.rotation = hand.transform.rotation;
					activeTimer = 0;
					statueAnimation.Play();
				}
				else
				{
					// Detach this object from the hand
					hand.DetachObject( gameObject );

					// Call this to undo HoverLock
					hand.HoverUnlock( GetComponent<Interactable>() );

				}
			}
		}

		private void HandAttachedUpdate( Hand hand )
		{
			transform.position = Vector3.MoveTowards(transform.position, hand.transform.position, 12 * Time.deltaTime);
			transform.rotation = Quaternion.RotateTowards(transform.rotation, hand.transform.rotation, 450 * Time.deltaTime);

			if (hand.controller != null && hand.controller.GetPressDown(Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger)) {
				foreach (TrailRenderer r in trails) {
					r.enabled = true;
				}
				active = true;
			}

			if (hand.controller != null && hand.controller.GetPressUp(Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger)) {
				foreach (TrailRenderer r in trails) {
					r.Clear();
					r.enabled = false;
				}
				active = false;
			}

			if (active) {
				activeTimer += Time.deltaTime;
			} else {
				activeTimer -= Time.deltaTime;
			}

			activeTimer = Mathf.Clamp(activeTimer, 0, 1);

			runeMaterial.EnableKeyword("_EMISSION");
			runeMaterial.SetColor("_EmissionColor", Color.Lerp(Color.black, runeColor * 3, activeTimer));
		}

}
