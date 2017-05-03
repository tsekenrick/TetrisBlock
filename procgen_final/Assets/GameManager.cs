using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{


    public GameObject enemyPrefab;
    public GameObject goalPrefab;
    public GameObject goal;
    public static float timer;
    public static bool waveInProgress;
    public static int waveCounter;
    public static int highScore;
    private static float diffCoefficient;
    private static float distanceModifier;
    public static bool canDropTiles;

    //list of possible locations for enemy to spawn and try to move to
    public static Vector2[] possibleLocations = { new Vector2(0, 16),
                                                  new Vector2(0, 8),
                                                  new Vector2(0, 0),
                                                  new Vector2(10, 0),
                                                  new Vector2(10, 16),
                                                  new Vector2(20, 16),
                                                  new Vector2(20, 8),
                                                  new Vector2(20, 0),
                                                };

    public static Vector2 spawnPos;
    public static Vector2 goalPos;
    void Start()
    {
        canDropTiles = true;
        waveCounter = 0;
        diffCoefficient = 1.2f;

        //distance modifier is the quadratic equation f(x) = 0.029(x-25.6125)^2 + 1, where x is the distance between the spawn and goal points.
        //the function has a vertex at (25.6125, 1) and a point at (8, 10). 25.6125 is the maximum distance possible between the spawn and goal,
        //while 8 is the min distance. the function therefore adds 10 to the distance factor at min distance, with the addition decreasing as
        //distance approahces max, where it adds 1 to the distance factor. this reduces the variance of the timer as a function of distance.
        distanceModifier = 0.029f * (Vector2.Distance(goalPos, spawnPos) - 25.6125f) * (Vector2.Distance(goalPos, spawnPos) - 25.6125f) + 1;
        timer = (Vector2.Distance(goalPos,spawnPos) + distanceModifier) * diffCoefficient;
        waveStart();
    }


    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(1);  
        }
        //difficulty coefficient increases as waves increase, and is multiplied with 
        //the previously described distance modifier to get the time needed to survive each wave
        if(waveCounter != 0)
        {
            diffCoefficient = waveCounter * .7f;
        }
        else
        {
            diffCoefficient = 1.2f;
        }

        Debug.Log(Vector2.Distance(spawnPos, goalPos));

        if (waveInProgress)
        {
            timer -= Time.deltaTime * 2.25f;
        }
    }

    //starts a new wave with the wave counter incremented by 1

    public void newWaveStart() //used to allow access to start wave coroutine outside of script
    {
        StartCoroutine(startWave());
    }

    public void waveStart() //used to start initial wave
    {
        clearBoard();
        waveInProgress = false;
        waveCounter++;

        int spawnPicker = 0;
        int goalPicker = 0;

        while (goalPicker == spawnPicker)
        {
            goalPicker = Random.Range(0, 8);
            spawnPicker = Random.Range(0, 8);
        }

        spawnPos = possibleLocations[spawnPicker];
        goalPos = possibleLocations[goalPicker];

        distanceModifier = 0.029f * (Vector2.Distance(goalPos, spawnPos) - 25.6125f) * (Vector2.Distance(goalPos, spawnPos) - 25.6125f) + 1;
        timer = (Vector2.Distance(goalPos, spawnPos) + distanceModifier) * diffCoefficient;

        goal = Instantiate(goalPrefab, goalPos, Quaternion.identity);
        GameObject enemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
        waveInProgress = true;

    }

    public IEnumerator startWave() //used to give 2 seconds stop time for each subsequent wave
    {
        clearBoard();
        waveInProgress = false;
        waveCounter++;   

        int spawnPicker = 0;
        int goalPicker = 0;

        while (goalPicker == spawnPicker)
        {
            goalPicker = Random.Range(0, 8);
            spawnPicker = Random.Range(0, 8);
        }

        spawnPos = possibleLocations[spawnPicker];
        goalPos = possibleLocations[goalPicker];

        distanceModifier = 0.029f * (Vector2.Distance(goalPos, spawnPos) - 25.6125f) * (Vector2.Distance(goalPos, spawnPos) - 25.6125f) + 1;
        timer = (Vector2.Distance(goalPos, spawnPos) + distanceModifier) * diffCoefficient;

        goal = Instantiate(goalPrefab, goalPos, Quaternion.identity);
        GameObject enemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);

        canDropTiles = false;
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(2);
        Time.timeScale = 1;
        canDropTiles = true;
        waveInProgress = true;

    }

    public void clearBoard()
    {
        Vector2 lowerLeftBound = new Vector2(0, 0);
        Vector2 upperRightBound = new Vector2(20, 16);
        foreach (Collider2D collision in Physics2D.OverlapAreaAll(lowerLeftBound, upperRightBound, LayerMask.GetMask("Wall")))
        {
            Destroy(collision.gameObject);
        }
        Destroy(goal);
    }
}
