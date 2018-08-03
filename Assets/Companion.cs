using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

[RequireComponent(typeof(Interactable), typeof(AI_Behaviour))]
public class Companion : MonoBehaviour {

    public Coroutine actionRoutine;

    public float moveSpeed, rotateSpeed;

    AudioSource tapSound;

    private void Awake()
    {
        tapSound = GetComponent<AudioSource>();
    }
    private void HandHoverUpdate(Hand hand)
    {
        if (hand.GetStandardInteractionButtonDown() || ((hand.controller != null) && hand.controller.GetPressDown(Valve.VR.EVRButtonId.k_EButton_Grip)))
        {
            TapEvent();
        }
    }

    public void ClearAction ()
    {
        if (actionRoutine != null)
        {
            StopCoroutine(actionRoutine);
        }
    }
    public IEnumerator MoveToPlayer ()
    {
        while (true)
        {
            Transform cam = Camera.main.transform;
            Vector3 targetPoint = cam.position + cam.transform.right * 1f + cam.transform.forward * 1;
            transform.position = Vector3.MoveTowards(transform.position, targetPoint, moveSpeed * Time.deltaTime);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(Camera.main.transform.position - transform.position).normalized, rotateSpeed);
            yield return null;
        }
        yield break;
    }

    public IEnumerator Attend ()
    {
        while (true)
        {
            Transform cam = Camera.main.transform;
            Vector3 targetPoint = transform.position;
            targetPoint.y = cam.position.y + Mathf.Sin(Time.time);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(Camera.main.transform.position - transform.position).normalized, rotateSpeed);
            transform.position = Vector3.MoveTowards(transform.position, targetPoint, moveSpeed * Time.deltaTime)
            yield return null;
        }
        yield break;
    }

    public void TapEvent ()
    {
        transform.position = Camera.main.transform.position + Random.insideUnitSphere * 10;
        tapSound.Play();
    }
}
