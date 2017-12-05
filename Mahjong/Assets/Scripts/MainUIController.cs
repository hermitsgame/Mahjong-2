using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainUIController : MonoBehaviour {
    
    [SerializeField]
    private Camera[] cameras;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SwitchCamera(int id)
    {
        foreach (Camera c in cameras)
            c.enabled = false;
        cameras[id].enabled = true;
    }
}
