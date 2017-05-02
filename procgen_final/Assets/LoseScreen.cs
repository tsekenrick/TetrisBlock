using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class LoseScreen : MonoBehaviour {

    public Text loseScreen;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        loseScreen.text = "You lost! D: \nPress R to Restart.\nHigh Score: " + GameManager.highScore + " waves.";

        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(1);
        }
	}
}
