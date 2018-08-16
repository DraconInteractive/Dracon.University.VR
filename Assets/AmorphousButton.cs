using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Valve.VR.InteractionSystem;

public class AmorphousButton : MonoBehaviour {
    public ParticleSystem system;
    public Color farColor, closeColor, activeColor;
    bool activated = false;

    public UnityEvent OnActivated;

    public bool primed;

    public GameObject primeSignal;
	// Update is called once per frame
	void Update () {
        if (system != null && system.particleCount > 0)
        {
            var main = system.main;
            float dist = Vector3.Distance(transform.position, Player.instance.rightHand.transform.position);

            if (!activated)
            {
                float scaledDistance = dist / 0.5f;
                main.startColor = Color.Lerp(closeColor, farColor, scaledDistance);
            }          
        }
    }

    void OnTriggerEnter (Collider col) {
        if (!activated && col.transform.tag == "Hand") {
            activated = true;
            system.Stop();
            var mainSub = system.subEmitters.GetSubEmitterSystem(0).main;
            mainSub.startColor = activeColor;
            system.TriggerSubEmitter(0);

            if (OnActivated != null) {
                OnActivated.Invoke ();
            }
            print ("active");
        }
    }

    void LateUpdate () {
        if (primeSignal != null) {
            if (primed) {
                if (!primeSignal.activeSelf) {
                    primeSignal.SetActive(true);
                }
            } else {
                if (primeSignal.activeSelf) {
                    primeSignal.SetActive(false);
                }
            }

            primed = false;
        }
       
    }
}
