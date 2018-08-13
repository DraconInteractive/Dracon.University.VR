using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonReceiver : MonoBehaviour {
	public GameObject focus;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void EnableChild () {
		focus.SetActive(true);
	}
}
