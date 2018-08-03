using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour {

    public static UIController controller;
    public static List<UITarget> targets = new List<UITarget>();

    public enum Mode
    {
        Default,
        Targeting
    }

    Mode activeMode;
	// Use this for initialization
	void Awake () {
        controller = this;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetMode (Mode m)
    {
        print("Target Count: " + targets.Count);
        activeMode = m;
        switch (m)
        {
            case Mode.Default:
                foreach (UITarget target in targets)
                {
                    target.SetActive(false);
                }
                break;
            case Mode.Targeting:
                foreach (UITarget target in targets)
                {
                    target.SetActive(true);
                }
                break;
        }
    }
}
