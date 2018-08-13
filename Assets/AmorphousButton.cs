using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Valve.VR.InteractionSystem;

public class AmorphousButton : MonoBehaviour {
    public ParticleSystem system;
    public Color farColor, closeColor, activeColor;
    bool activated = false;

    public UnityEvent testEvent;
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
            print ("active");
        }
    }
}
