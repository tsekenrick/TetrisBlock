using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrisBagSystem : MonoBehaviour {
    public static List<GameObject> _tileBag;
    public GameObject[] possibleTiles;
    protected static GameObject _currentBlock;

	void Start () {
        _tileBag = new List<GameObject>(7);
        fillBag();
        _currentBlock = Instantiate(_tileBag[0], transform.position, Quaternion.identity);
        _tileBag.RemoveAt(0);
	}
	
	void Update () {
       
        if(_tileBag.Count <= 0)
        {
            fillBag();
        }   
        
	}

    protected void fillBag() //fills and shuffles bag
    {

        for(int i = 0; i < possibleTiles.Length; i++)
        {
            _tileBag.Add(possibleTiles[i]);
        }

        shuffleBag(_tileBag);

    }

    private static List<GameObject> shuffleBag(List<GameObject> myList)
    {

        System.Random _random = new System.Random();

        GameObject tempObj;

        int n = myList.Count;
        for (int i = 0; i < n; i++)
        {
            // NextDouble returns a random number between 0 and 1.
            int j = i + (int)(_random.NextDouble() * (n - i));
            tempObj = myList[j];
            myList[j] = myList[i];
            myList[i] = tempObj;
        }

        return myList;
    }

}
