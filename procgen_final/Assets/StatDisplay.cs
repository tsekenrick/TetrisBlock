using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatDisplay : MonoBehaviour {
    public Text timerAndWave;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        timerAndWave.text = "Timer: " + string.Format("{0:0.00}", GameManager.timer) + "\nWaves Survived: " + GameManager.waveCounter;
	}
}
