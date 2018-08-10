using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GazeController : MonoBehaviour {

	// Update is called once per frame
	void Update () {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 20))
        {
            Companion c = hit.transform.GetComponent<Companion>();
            if (c != null)
            {
                c.staredAt = true;
            }
        }
	}
}
