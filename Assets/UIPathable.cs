using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPathable : MonoBehaviour {

    public GameObject graphic;

    private void Awake()
    {
        UIController.paths.Add(this);
    }


    public void SetActive(bool a)
    {
        graphic.SetActive(a);
    }
}
