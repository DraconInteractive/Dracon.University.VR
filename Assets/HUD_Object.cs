using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class HUD_Object : MonoBehaviour {
    Hand attachedHand;

    Vector3 oldPosition;
    Quaternion oldRotation;
    private Hand.AttachmentFlags attachmentFlags = Hand.defaultAttachmentFlags & (~Hand.AttachmentFlags.SnapOnAttach) & (~Hand.AttachmentFlags.DetachOthers);

    private void HandHoverUpdate(Hand hand)
    {
        if (hand.GetStandardInteractionButtonDown() || ((hand.controller != null) && hand.controller.GetPressDown(Valve.VR.EVRButtonId.k_EButton_Grip)))
        {
            if (hand.currentAttachedObject != gameObject)
            {
                // Save our position/rotation so that we can restore it when we detach
                oldPosition = transform.position;
                oldRotation = transform.rotation;

                // Call this to continue receiving HandHoverUpdate messages,
                // and prevent the hand from hovering over anything else
                hand.HoverLock(GetComponent<Interactable>());

                // Attach this object to the hand
                hand.AttachObject(gameObject, attachmentFlags);
            }
            else
            {
                // Detach this object from the hand
                hand.DetachObject(gameObject);

                // Call this to undo HoverLock
                hand.HoverUnlock(GetComponent<Interactable>());

                // Restore position/rotation
                //transform.position = oldPosition;
                //transform.rotation = oldRotation;

                Collider[] cols = Physics.OverlapSphere(transform.position, 0.4f);
                
                foreach (Collider col in cols)
                {
                    if (col.transform.tag == "Head")
                    {
                        print("HUD Equipped");
                        Destroy(this.gameObject);
                        UIController.controller.SetMode(UIController.Mode.Targeting);
                    }
                }
            }
        }
    }

}
