using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class AmorphousButton : MonoBehaviour {
    public ParticleSystem system;
    public Color farColor, closeColor;
    bool activated = false;
	// Update is called once per frame
	void Update () {
        if (system != null && system.particleCount > 0)
        {
            var main = system.main;
            float dist = Vector3.Distance(transform.position, Player.instance.rightHand.transform.position);

            if (!activated && dist < 0.2f)
            {
                activated = true;
                main.startColor = Color.red;
            }
            else
            {
                float scaledDistance = dist / 0.5f;
                main.startColor = Color.Lerp(closeColor, farColor, scaledDistance);
            }
            

        }

    }
}
