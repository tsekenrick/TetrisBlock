using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleGridGen : MonoBehaviour {

    public GameObject gridSquare;
    public static bool[,] gridState;
    public const int GRID_WIDTH = 21;
    public const int GRID_HEIGHT = 17;

    // Use this for initialization
    void Start()
    {
        gridState = new bool[GRID_HEIGHT, GRID_WIDTH];
        for (int i = 0; i < GRID_HEIGHT; i++)
        {
            for (int j = 0; j < GRID_WIDTH; j++)
            {
                GameObject tempSquare = Instantiate(gridSquare, transform.position, Quaternion.identity);
                gridState[i, j] = false;
                //tempSquare.transform.parent = this.transform;
                tempSquare.transform.localPosition = new Vector3(j, i, 10);
                transform.position = new Vector2(transform.position.x + 1, transform.position.y);
            }
            transform.position = new Vector2(transform.position.x - 20, transform.position.y - 1);
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
