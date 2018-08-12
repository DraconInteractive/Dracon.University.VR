using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class UITarget : MonoBehaviour {

    public GameObject graphic;
    bool active;

    public enum TargetType
    {
        Enemy,
        Ally
    };

    public TargetType type;
    private void Awake ()
    {
        UIController.targets.Add(this);
        //graphic.SetActive(false);
    }

    private void Update()
    {
        if (active)
        {
            graphic.transform.LookAt(Player.instance.hmdTransform);
        }
    } 

    public void SetActive (bool a)
    {
        print("target mode set to " + a);
        active = a;
        graphic.SetActive(a);
    }
}
