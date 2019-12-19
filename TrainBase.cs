using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainBase : MonoBehaviour
{
    public float trainSpeed = 0.5f;
    public BaseDotWay dotDest;
    public BaseDotWay nearestDest;
    public Pathf pathFunc;
    public List<BaseDotWay> wayPoints;
    public Camera camera;
    public TrainBase forward;
    public TrainBase backward;
    public BaseDotWay lastPose;
    public bool isFollower;

    // Start is called before the first frame update
    void Start()
    {
        lastPose = backward.nearestDest;
        wayPoints = new List<BaseDotWay>();
        wayPoints.Add(nearestDest);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isFollower)
        {
            RaycastHit hit;
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            //пускаем луч
            if (Input.GetButtonDown("Fire1"))
            {
                Physics.Raycast(ray, out hit);

                if (hit.collider != null) ;
                {
                    //если луч не попал в цель
                    if (hit.collider.gameObject.tag == "dot")
                    {
                        Debug.Log(hit.collider.gameObject.name);
                        if (pathFunc.isWayCheck(nearestDest, hit.collider.gameObject.GetComponent<BaseDotWay>()))
                        {
                            dotDest = hit.collider.gameObject.GetComponent<BaseDotWay>();
                            wayPoints = pathFunc.Pathfinder(nearestDest, dotDest);
                        }


                    }
                }
            }
        }
        else
        {
            Debug.Log(string.Join(",", pathFunc.Pathfinder(nearestDest, forward.lastPose)));
            dotDest = forward.lastPose;
            wayPoints = pathFunc.Pathfinder(nearestDest, dotDest);

        }
        

        moveToDot();
       
    }

    void moveToDot()
    {
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(nearestDest.dotPos.x, 0.4f, nearestDest.dotPos.z), trainSpeed * Time.deltaTime);

        if(transform.position.x == nearestDest.dotPos.x && transform.position.z == nearestDest.dotPos.z)
        {
            dotChanger();
        }
    }

    void dotChanger()
    {
        if (wayPoints.Count > 1)
        {
            wayPoints.RemoveAt(0);
            lastPose = nearestDest;
            nearestDest = wayPoints[0];
        }
    }

    //public TrainBase forTrainReturner()
    //{
    //    if (forward)
    //    {
    //        return forTrainReturner();
    //    }
    //    else
    //    {
    //        return gameObject.GetComponent<TrainBase>();
    //    }
    //}

    //public TrainBase forwardReturner()
    //{
    //    if (forward)
    //    {
    //        return forward;
    //    }
    //    else
    //    {

    //    }
    //}

    //public TrainBase backTrainReturner()
    //{
    //    if (backward)
    //    {
    //        return backTrainReturner();
    //    }
    //    else
    //    {
    //        return gameObject.GetComponent<TrainBase>();
    //    }
    //}
}
