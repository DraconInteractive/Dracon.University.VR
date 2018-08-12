using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandUI : MonoBehaviour {

	bool facingUp = false;
	public float dotToUp, dotFromCamera;

	public GameObject uiPrefab;
	GameObject prefabStore;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		bool b = GetFacingUp();
		if (facingUp != b) {
			if (b) {
				uiPrefab.SetActive(true);
			} else {
				uiPrefab.SetActive(false);				
			}
			facingUp = b;
		}
	}

	bool GetFacingUp () {
		dotToUp = Vector3.Dot(-transform.up, Vector3.up);
		if (dotToUp > 0.6f) {
			Vector3 vectorToController = transform.position - Camera.main.transform.position;
			dotFromCamera = Vector3.Dot(Camera.main.transform.forward, vectorToController.normalized);
			if (dotFromCamera > 0.9f) {
				return true;
			}
		} 

		return false;
	}
}
