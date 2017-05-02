using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Instructions : MonoBehaviour {
    public Text instructions;
    public string[] script;
    public int currentLine;
    public int endLine;
	// Use this for initialization
	void Start () {
        endLine = script.Length;
        currentLine = 0;
	}
	
	// Update is called once per frame
	void Update () {
        instructions.text = script[currentLine];

        if(Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
        {
            currentLine++;
        }

        if(currentLine == endLine)
        {
            SceneManager.LoadScene(1);
        }
	}
}
