using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour {

    public static UIController controller;
    public static List<UITarget> targets = new List<UITarget>();
    public static List<UIPathable> paths = new List<UIPathable>();
    public enum Mode
    {
        Default,
        Targeting,
        PathFinding
    }

    Mode activeMode;

    public AudioSource uiAudio;
    public AudioClip clickSound;
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
        uiAudio.PlayOneShot(clickSound);
        switch (m)
        {
            case Mode.Default:
                foreach (UITarget target in targets)
                {
                    target.SetActive(false);
                }

                foreach (UIPathable target in paths)
                {
                    target.SetActive(false);
                }
                break;
            case Mode.Targeting:
                foreach (UITarget target in targets)
                {
                    target.SetActive(true);
                }
                foreach (UIPathable target in paths)
                {
                    target.SetActive(false);
                }
                break;
            case Mode.PathFinding:
                foreach (UITarget target in targets)
                {
                    target.SetActive(false);
                }
                foreach (UIPathable target in paths)
                {
                    target.SetActive(true);
                }
                break;
        }
    }

    public void CycleMode ()
    {
        switch (activeMode)
        {
            case Mode.Default:
                SetMode(Mode.Targeting);
                break;
            case Mode.Targeting:
                SetMode(Mode.PathFinding);
                break;
            case Mode.PathFinding:
                SetMode(Mode.Default);
                break;
        }
    }
}
