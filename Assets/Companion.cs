﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

[RequireComponent(typeof(Interactable), typeof(AI_Behaviour))]
public class Companion : MonoBehaviour {

    public Coroutine actionRoutine;

    public float moveSpeed, rotateSpeed, hoverFrequency, hoverMagnitude;

    public AudioSource teleportSound, talkSource;
    public AudioClip[] talkThreads;
    public GameObject uiContainer;

    public bool stay, trail;
    private void Awake()
    {
        teleportSound = GetComponent<AudioSource>();
    }
    private void HandHoverUpdate(Hand hand)
    {
        if ((hand.controller != null) && hand.controller.GetPressDown(Valve.VR.EVRButtonId.k_EButton_Grip))
        {
            GripEvent();
        }
        if ((hand.controller != null) && hand.controller.GetPressDown(Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger))
        {
            TriggerEvent();
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
            targetPoint.y = cam.position.y + Mathf.Sin(Time.time * hoverFrequency) * hoverMagnitude;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(Camera.main.transform.position - transform.position).normalized, rotateSpeed);
            transform.position = Vector3.MoveTowards(transform.position, targetPoint, moveSpeed * Time.deltaTime);
            yield return null;
        }
        yield break;
    }

    public void GripEvent ()
    {
        //transform.position = Camera.main.transform.position + Random.insideUnitSphere * 10;
        StartCoroutine(GripRoutine());
    }

    IEnumerator GripRoutine ()
    {
        GetComponent<AI_Behaviour>().disabled = true;
        ClearAction();
        teleportSound.Play();

        float original = transform.localScale.x;
        for (float f = 1; f > 0; f -= Time.deltaTime * 2)
        {
            transform.localScale = Vector3.one * original * f;
            yield return null;
        }

        transform.position = Camera.main.transform.position + Camera.main.transform.forward * -2;

        transform.localScale = Vector3.one * original;

        GetComponent<AI_Behaviour>().disabled = false;
        yield break;
    }

    public void TriggerEvent ()
    {
        ToggleUI();
    }

    public void ToggleUI()
    {
        uiContainer.SetActive(!uiContainer.activeSelf);
    }

    public void ToggleUI (bool on)
    {
        uiContainer.SetActive(on);
    }

    public void ToggleStay ()
    {
        stay = !stay;
    }

    public void ToggleTrail ()
    {
        trail = !trail;
    }

    public void Talk ()
    {
        talkSource.clip = talkThreads[Random.Range(0, talkThreads.Length)];
        talkSource.Play();
    }
}
