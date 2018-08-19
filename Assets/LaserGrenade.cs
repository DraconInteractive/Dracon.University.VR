using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class LaserGrenade : MonoBehaviour {
    private Hand.AttachmentFlags attachmentFlags = Hand.defaultAttachmentFlags & (~Hand.AttachmentFlags.SnapOnAttach) & (~Hand.AttachmentFlags.DetachOthers);
	Rigidbody rb;
	public LayerMask groundMask;
	public float alignSpeed;
	public Material laserMaterial;
	public float throwForce;
	void Start () {
		rb = GetComponent<Rigidbody>();
	}
	private void HandHoverUpdate( Hand hand )
		{
			if ((hand.controller != null) && hand.controller.GetPressDown( Valve.VR.EVRButtonId.k_EButton_Grip ))
			{
				bool b = false;
				foreach (Hand.AttachedObject obj in hand.AttachedObjects) {
					if (obj.attachedObject == gameObject) {
						b = true;
						break;
					}
				}
				if (!b)
				{

					hand.HoverLock( GetComponent<Interactable>() );

					hand.AttachObject( gameObject, attachmentFlags );
				}
				else
				{

					//hand.DetachObject( gameObject );

					//hand.HoverUnlock( GetComponent<Interactable>() );

				}
			}

			if ((hand.controller != null) && hand.controller.GetPressUp (Valve.VR.EVRButtonId.k_EButton_Grip)) {
				bool b = false;
				foreach (Hand.AttachedObject obj in hand.AttachedObjects) {
					if (obj.attachedObject == gameObject) {
						b = true;
						break;
					}
				}

				if (b) {
					hand.DetachObject(gameObject);

					hand.HoverUnlock(GetComponent<Interactable>());
				}
			}
		}

		private void OnAttachedToHand( Hand hand )
		{
			rb.isKinematic = true;
			foreach (Transform child in transform) {
				if (child.name == "laser") {
					Destroy(child.gameObject);
				}
			}
		}

		private void OnDetachedFromHand( Hand hand )
		{
			rb.isKinematic = false;
			rb.velocity = hand.GetTrackedObjectVelocity() * throwForce;
			StartCoroutine(GrenadeRelease());
		}

		IEnumerator GrenadeRelease () {
			yield return new WaitForSeconds(0.75f);

			Ray ray = new Ray (transform.position, Vector3.down);
			RaycastHit hit;
			Vector3 target = transform.position;
			if (Physics.Raycast(ray, out hit, 10, groundMask)) {
				target = hit.point + Vector3.up * 2;
			}
			Vector3 cPos = transform.position;

			rb.useGravity = false;
			rb.velocity = Vector3.zero;

			for (float f = 0; f < 1; f += Time.deltaTime / alignSpeed) {
				rb.MovePosition(Vector3.Lerp(cPos, target, f));
				yield return null;
			}

			rb.isKinematic = true;

			foreach (UITarget t in UIController.targets) {
				
				if (t.type == UITarget.TargetType.Enemy) {
					GameObject g = new GameObject("laser");
					g.transform.parent = this.transform;

					LineRenderer l = g.AddComponent<LineRenderer>();
					l.startWidth = 0.025f;
					l.endWidth = 0.025f;
					l.material = laserMaterial;
					l.positionCount = 2;

					l.SetPosition(0, transform.position);
					l.SetPosition(1, t.transform.position);
				}
				
			}
			yield break;
		}
}
