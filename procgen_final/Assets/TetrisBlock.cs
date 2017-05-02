using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrisBlock : TetrisBagSystem {
    public bool isHeld;
    public Vector3 worldPos;
	// Use this for initialization
	void Start () {
        isHeld = true;
	}
	
	// Update is called once per frame
	void Update () {

        if (isHeld)
        {
            //gameObject.layer = LayerMask.NameToLayer("Base");
            worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            worldPos = new Vector3(Mathf.Floor(worldPos.x), Mathf.Floor(worldPos.y), 5);
            transform.position = worldPos;
        }

        if((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Mouse1)) && isHeld)
        {
            transform.Rotate(0, 0, 90);
        }

        if(Input.GetMouseButtonDown(0) && isHeld)
        {
            dropTile();
        }
	}

    void dropTile()
    {
        //figure out a way to make it so you can't drop tiles outside of the grid space, probably something to do with only allowing drops on Base layer
        foreach (Transform child in transform)
        {
            //may need more complex implementation later involving layers
            foreach (Collider2D foundColl in Physics2D.OverlapPointAll(child.position, LayerMask.GetMask("Wall")))
            {
                if(foundColl.transform != child)
                {
                    Debug.Log(foundColl);
                    return;
                }

            }

            bool foundBase = false;
            foreach (Collider2D foundColl in Physics2D.OverlapPointAll(child.position, LayerMask.GetMask("Base")))
            {
                if(foundColl.transform != child)
                {
                    foundBase = true;
                }
            }

            if (!foundBase)
            {
                return;
            }




        }

        isHeld = false;
        gameObject.layer = LayerMask.NameToLayer("Wall");
        foreach (Transform child in transform)
        {
            child.gameObject.layer = LayerMask.NameToLayer("Wall");
        }

        _currentBlock = Instantiate(_tileBag[0], transform.position, Quaternion.identity);
        _tileBag.RemoveAt(0);
        if (_tileBag.Count == 0)
        {
            fillBag();
        }
    }
}