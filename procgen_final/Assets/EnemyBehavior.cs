using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyBehavior : MonoBehaviour {
    public GameObject manager;
    public bool movingToPoint;
    public Vector2[] criticalPath;
    public float moveSpeed = 1f;
    private int iterationCount; //used to failsafe infinite while loop below
    public bool hasPunishedRecently = false;
    public float punishTimer = 1.5f;
    private Vector2 previousTarget;
	void Start () {
        manager = GameObject.Find("Game Manager");
        movingToPoint = false;
        Vector2 source = new Vector2(Mathf.Floor(transform.position.x), Mathf.Floor(transform.position.y));
        criticalPath = findBFSPath(source, GameManager.goalPos);
        movingToPoint = true;
	}
	
	void Update () {
        //two behaviors - first call function to get first location it should move to
        //as it moves to location, do not do any pathfinding
        //once it's there, call function again

        moveSpeed = 1.4f + (GameManager.waveCounter * .1f);

        if (movingToPoint)
        {
            transform.position = Vector2.MoveTowards(transform.position, criticalPath[1], moveSpeed * Time.deltaTime);

            if(Vector2.Distance(transform.position, criticalPath[1]) < .01f)
            {
                transform.position = criticalPath[1];
                criticalPath[0] = previousTarget;
                movingToPoint = false;
            }
        }

        //makes sure timer increase punishment for blocking critical path doesn't occur more than once for the same instance
        if (hasPunishedRecently)
        {
            punishTimer -= Time.deltaTime;

            if(punishTimer <= 0)
            {
                punishTimer = 1.5f;
                hasPunishedRecently = false;
            }
        }

        while (!movingToPoint)
        {
            iterationCount++;
            Vector2 source = new Vector2(Mathf.Floor(transform.position.x), Mathf.Floor(transform.position.y));
            criticalPath = findBFSPath(source, GameManager.goalPos);
            if(criticalPath == null)
            {
                if(iterationCount > 15)
                {
                    destroyInRadius(transform.position, 10); //provides failsafe for if you block crit path such that destroying a small area can't fix it - this wipes the board.
                }
                destroyInRadius(transform.position, 3);
                if (!hasPunishedRecently)
                {
                    GameManager.timer += 20;
                    hasPunishedRecently = true;
                }
                
                continue;
            }
            else
            {
                iterationCount = 0;
                movingToPoint = true;    
            }
            
        }

        if((Vector2)transform.position == GameManager.goalPos)
        {

            if(GameManager.waveCounter > GameManager.highScore)
            {
                GameManager.highScore = GameManager.waveCounter;
            }
            
            SceneManager.LoadScene(2);
        }

        if(GameManager.timer <= 0)
        {
            manager.GetComponent<GameManager>().newWaveStart();
            Destroy(this.gameObject);
        }

	}
    public void destroyInRadius(Vector2 center, int radius)
    {
        Vector2 lowerLeftBound = new Vector2(center.x - radius, center.y - radius);
        Vector2 upperRightBound = new Vector2(center.x + radius, center.y + radius);
        foreach (Collider2D collision in Physics2D.OverlapAreaAll(lowerLeftBound, upperRightBound, LayerMask.GetMask("Wall"))){
            Destroy(collision.gameObject);
        }
    }

    //this still needs fixing
    /*private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            movingToPoint = false;
            transform.position = previousTarget;
            
        }
    }*/

    public class GraphVertex
    {
        public Vector2 gridPoint;
        public GraphVertex parent;
    }

    public bool withinBounds(int xPos, int yPos)
    {
        if (xPos < 0 || xPos > 20)
        {
            return false;
        }

        if (yPos < 0 || yPos > 16)
        {
            return false;
        }

        return true;
    }

    public Vector2[] findBFSPath(Vector2 sourcePoint, Vector2 targetPoint)
    {

        List<GraphVertex> agenda = new List<GraphVertex>();
        List<Vector2> alreadyInAgenda = new List<Vector2>();

        GraphVertex sourceVertex = new GraphVertex();
        sourceVertex.gridPoint = sourcePoint;
        sourceVertex.parent = null;

        agenda.Add(sourceVertex);
        alreadyInAgenda.Add(sourcePoint);

        while (agenda.Count > 0)
        {
            GraphVertex currentVertex = agenda[0];
            agenda.RemoveAt(0);

            if (currentVertex.gridPoint == targetPoint)
            {
                List<Vector2> path = new List<Vector2>();
                do
                {
                    path.Add(currentVertex.gridPoint);
                    currentVertex = currentVertex.parent;
                } while (currentVertex != null);
                path.Reverse();
                return path.ToArray();
            }

            // Look at all the neighbors. 

            // Up neighbor.
            Vector2 upPoint = currentVertex.gridPoint + Vector2.up;
            if (withinBounds((int)upPoint.x, (int)upPoint.y) //withinBounds checks bounds
                && !alreadyInAgenda.Contains(upPoint)
                && !Physics2D.OverlapPoint(upPoint, LayerMask.GetMask("Wall"))) //checks that the point isn't a wall
            {
                GraphVertex neighborVertex = new GraphVertex();
                neighborVertex.gridPoint = upPoint;
                neighborVertex.parent = currentVertex;
                alreadyInAgenda.Add(upPoint);
                agenda.Add(neighborVertex);
            }
            Vector2 rightPoint = currentVertex.gridPoint + Vector2.right;
            if (withinBounds((int)rightPoint.x, (int)rightPoint.y)
                && !alreadyInAgenda.Contains(rightPoint)
                && !Physics2D.OverlapPoint(rightPoint, LayerMask.GetMask("Wall")))
            {
                GraphVertex neighborVertex = new GraphVertex();
                neighborVertex.gridPoint = rightPoint;
                neighborVertex.parent = currentVertex;
                alreadyInAgenda.Add(rightPoint);
                agenda.Add(neighborVertex);

            }
            Vector2 downPoint = currentVertex.gridPoint - Vector2.up;
            if (withinBounds((int)downPoint.x, (int)downPoint.y)
                && !alreadyInAgenda.Contains(downPoint)
                && !Physics2D.OverlapPoint(downPoint, LayerMask.GetMask("Wall")))
            {
                GraphVertex neighborVertex = new GraphVertex();
                neighborVertex.gridPoint = downPoint;
                neighborVertex.parent = currentVertex;
                alreadyInAgenda.Add(downPoint);
                agenda.Add(neighborVertex);

            }
            Vector2 leftPoint = currentVertex.gridPoint - Vector2.right;
            if (withinBounds((int)leftPoint.x, (int)leftPoint.y)
                && !alreadyInAgenda.Contains(leftPoint)
                && !Physics2D.OverlapPoint(leftPoint, LayerMask.GetMask("Wall")))
            {
                GraphVertex neighborVertex = new GraphVertex();
                neighborVertex.gridPoint = leftPoint;
                neighborVertex.parent = currentVertex;
                alreadyInAgenda.Add(leftPoint);
                agenda.Add(neighborVertex);

            }
        }

        return null;

    }
}
