using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class EyeLaser : MonoBehaviour {
    LineRenderer line;
    Hand hand;
    bool active = false;
	// Use this for initialization
	void Start () {
        line = GetComponent<LineRenderer>();
        hand = GetComponent<Hand>();
	}

    // Update is called once per frame
    void Update() {
        if (hand.GetStandardInteractionButtonDown())
        {
            line.enabled = true;
            active = true;
        }

        if (hand.GetStandardInteractionButtonUp())
        {
            line.enabled = false;
            active = false;
        }

        if (active)
        {
            line.SetPosition(0, transform.position);
            line.SetPosition(1, UIController.GetGazeEnemy().transform.position);
        }
        
	}
}
