using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainUIController : MonoBehaviour {

    GameController gameController;

	// Use this for initialization
	void Start () {
        this.gameController = GameObject.Find("GameController").GetComponent<GameController>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SwitchCamera(int id)
    {
        this.gameController.SwitchCamera(id);
    }
}
