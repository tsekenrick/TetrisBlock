using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkGen : MonoBehaviour {
    public int chunkIndex = 0;
    public GameObject squarePrefab;
    public int chunkRadius = 3;

    public Vector2[] neighborOffsets;
    // Use this for initialization
    void Start () {
        List<Vector2> squaresToSpawn = new List<Vector2>();
        squaresToSpawn.Add(Vector2.zero);
        List<Vector2> newSquaresToAdd = new List<Vector2>();
        List<Vector2> alreadyHasSquare = new List<Vector2>();
        for (int n = 0; n < chunkRadius; n++)
        {
            newSquaresToAdd.Clear();
            foreach (Vector2 squarePosToSpawn in squaresToSpawn)
            {
                // Before we spawn a hex, check if there's one there already!
                //replace with a 2d array that indicates where there are already squares
                /*if (Physics.Raycast(new Ray(new Vector3(transform.position.x + squarePosToSpawn.x, transform.position.y + squarePosToSpawn.y, -10), Vector3.forward)))
                {
                    continue;
                }*/

                if (alreadyHasSquare.Contains(squarePosToSpawn))
                {
                    continue;
                }

                GameObject squareObj = Instantiate(squarePrefab);
                squareObj.transform.parent = transform;
                squareObj.transform.localPosition = squarePosToSpawn;
                alreadyHasSquare.Add(new Vector2(squareObj.transform.position.x, squareObj.transform.position.y));
                // Now let's add alls it's neighbors to the list of next ones to spawn.
                GridSquareInfo squareInfo = squareObj.GetComponent<GridSquareInfo>();
                squareInfo.setChunkColor(chunkIndex);


                foreach (Vector2 neighborOffset in squareInfo.neighborOffsets)
                {
                    Vector2 neighborPos = squarePosToSpawn + neighborOffset;
                    if (!newSquaresToAdd.Contains(neighborPos))
                    {
                        newSquaresToAdd.Add(neighborPos);
                    }
                }
            }
            foreach (Vector2 newSquare in newSquaresToAdd)
            {
                if (!squaresToSpawn.Contains(newSquare))
                {
                    squaresToSpawn.Add(newSquare);
                }
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
