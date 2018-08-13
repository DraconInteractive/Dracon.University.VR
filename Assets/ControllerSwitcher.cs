using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerSwitcher : MonoBehaviour {
	public static ControllerSwitcher instance;

	public static GameObject initRightControl, initLeftControl;
	public static GameObject leftHandController, rightHandController;
	void Awake () {
		instance = this;
	}
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public enum Side {
		Left,
		Right
	};
	public static void SwitchController (GameObject replacement, Side side) {
		if (side == Side.Left) {
			if (replacement == null) {
				//destroy existing controller and enable default
				if (leftHandController != initLeftControl) {
					Destroy(leftHandController);
				}
				initLeftControl.SetActive(true);
			} else {
				//disable or destroy existing controller, and enable replacement. 
				if (leftHandController != initLeftControl) {
					Destroy(leftHandController);
				} else {
					initLeftControl.SetActive(false);
				}
				leftHandController = replacement;
			}
		} else if (side == Side.Right) {
			if (replacement == null) {
				//destroy existing controller and enable default
				if (rightHandController != initRightControl) {
					Destroy(rightHandController);
				}
				initRightControl.SetActive(true);
			} else {
				//disable or destroy existing controller, and enable replacement. 
				if (rightHandController != initRightControl) {
					Destroy(rightHandController);
				} else {
					initRightControl.SetActive(false);
				}
				rightHandController = replacement;
			}
		}
	}
}
