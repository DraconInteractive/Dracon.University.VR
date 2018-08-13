using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class HUD_Object : MonoBehaviour {
    bool onHead = false;
    private Hand.AttachmentFlags attachmentFlags = Hand.defaultAttachmentFlags & (~Hand.AttachmentFlags.SnapOnAttach) & (~Hand.AttachmentFlags.DetachOthers);

    private void HandHoverUpdate(Hand hand)
    {
        if (hand.GetStandardInteractionButtonDown() || ((hand.controller != null) && hand.controller.GetPressDown(Valve.VR.EVRButtonId.k_EButton_Grip)))
        {
            if (hand.currentAttachedObject != gameObject)
            {
                if (onHead)
                {
                    //Cycle UI Mode
                    UIController.controller.CycleMode();
                    UIController.controller.ToggleModeUI(true);
                }
                else
                {
                    // Call this to continue receiving HandHoverUpdate messages,
                    // and prevent the hand from hovering over anything else
                    hand.HoverLock(GetComponent<Interactable>());

                    // Attach this object to the hand
                    hand.AttachObject(gameObject, attachmentFlags);
                }
                

                
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
                        //print("HUD Equipped");
                        onHead = true;
                        foreach (Renderer r in GetComponentsInChildren<Renderer>())
                        {
                            r.enabled = false;
                        }
                        UIController.controller.SetMode(UIController.Mode.Targeting);
                        transform.parent = col.transform;
                        transform.localPosition = Vector3.zero;
                    }
                }
            }
        }
    }

    private void OnHandHoverBegin(Hand hand)
    {
        //print("Hand hover enter");
        if (onHead)
        {
            UIController.controller.ToggleModeUI(true);
        }
    }


    private void OnHandHoverEnd(Hand hand)
    {
        //print("Hand hover exit");
        UIController.controller.ToggleModeUI(false);
    }
}
