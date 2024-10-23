using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class EnemyMover : MonoBehaviour
{
    [SerializeField] List<WayPoint> path = new List<WayPoint>();
    //[SerializeField] float waitTime = 1f;
    [SerializeField][Range(0f, 5f)] float speed = 1f;

    Enemy enemy;

    void OnEnable()
    {
        FindPath();
        ReturnToStart();
        //PrintWayPointName();
        //InvokeRepeating("PrintWayPointName",0,1f);
        //Debug.Log("Start here");
        StartCoroutine(followPath());
        //Debug.Log("Finishing start");

    }

    void Start()
    {
        enemy = GetComponent<Enemy>();
    }

    void FindPath()
    {
        path.Clear();

        //GameObject[] waypoints = GameObject.FindGameObjectsWithTag("Path");
        GameObject parent = GameObject.FindGameObjectWithTag("Path");

        // foreach (GameObject waypoint in waypoints)
        foreach (Transform child in parent.transform)
        {
            //path.Add(waypoint.GetComponent<WayPoint>());
            WayPoint wayPoint = child.GetComponent<WayPoint>();
            if (wayPoint != null)
            {
                path.Add(wayPoint);
            }

        }
    }

    void ReturnToStart()
    {
        transform.position = path[0].transform.position;
    }

    void FinishPath()
    {
        enemy.StealGold();
        gameObject.SetActive(false);
    }

    IEnumerator followPath()
    {
        foreach (WayPoint wayPoint in path)
        {
            //Debug.Log(wayPoint.name);

            Vector3 startPosition = transform.position;
            Vector3 endPosition = wayPoint.transform.position;
            float travelPercent = 0f;

            transform.LookAt(endPosition);

            //transform.position = wayPoint.transform.position;
            //yield return new WaitForSeconds(waitTime);
            while (travelPercent < 1f)
            {
                travelPercent += Time.deltaTime * speed;
                transform.position = Vector3.Lerp(startPosition, endPosition, travelPercent);
                yield return new WaitForEndOfFrame();
            }
        }

        //enemy.StealGold();
        //Destroy(gameObject);
        //gameObject.SetActive(false);
        FinishPath();
    }
}
