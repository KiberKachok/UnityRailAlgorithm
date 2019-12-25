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

    public TrainBase intforward;
    public TrainBase intbackward;

    public BaseDotWay lastPose;
    public bool isFollower;
    public bool imustrel = false;
    public bool imustpere = false;

    public TrainController trainCont;
    public bool hasMoved;
    public LateFunk lateToo;
    public BaseDotWay dotTotalus;
    public bool iNeedThat = false;
    // Start is called before the first frame update

    //private void Awake()
    //{
    //    dotTotalus = nearestDest;
    //}

    void Start()
    {
        Debug.Log("TrainBase");
        nearestDest.dotState = 1;
        reloadGuys();
        dotDest = nearestDest;
        dotTotalus = nearestDest;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (!isFollower)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                RaycastHit hit;
                Ray ray = camera.ScreenPointToRay(Input.mousePosition);
                Physics.Raycast(ray, out hit);

                if (hit.collider != null) ;
                {
                    if (hit.collider.gameObject.tag == "dot")
                    {
                        //Debug.Log(hit.collider.gameObject.name);
                        if (hit.collider.gameObject.GetComponent<BaseDotWay>().dotState == 0 && pathFunc.isWayCheck(nearestDest, hit.collider.gameObject.GetComponent<BaseDotWay>()))
                        {

                            BaseDotWay interDot = hit.collider.gameObject.GetComponent<BaseDotWay>();

                            if (pathFunc.isWay(forTrainReturner().nearestDest, interDot) > pathFunc.isWay(backTrainReturner().nearestDest, interDot))
                            {
                                //trainCont.nextFrameyouNeed = true;
                                //Debug.Log("Есть Короткий Путь");
                                //Не стирать, престанет работать
                                lateToo.dotDot = interDot;
                                trainCont.trainWayDot = interDot;
                            }



                            dotDest = interDot;
                            wayPoints = pathFunc.Pathfinder(nearestDest, dotDest);


                        }
                    }
                }
            }
            

        }
        else
        {
            if (forward)
            {
                dotDest = forward.lastPose;
                wayPoints = pathFunc.Pathfinder(nearestDest, dotDest);
            }


        }


        moveToDot();

        
    }


    private void LateUpdate()
    {
        if (trainCont.theyNeedToTurn)
        {
            //reloadGuys();
            imustrel = true;
            imustpere = false;
        }
        if (imustrel)
        {
            swapper();
            reloadGuys();
            imustrel = false;
            if (!isFollower)
            {
                //wayPoints = pathFunc.Pathfinder(nearestDest, trainCont.trainWayDot);
                iNeedThat = false;
            }
        }
        
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
            lastPose.dotState = 0;
            nearestDest = wayPoints[0];
            nearestDest.dotState = 1;
        }
    }

    public TrainBase backTrainReturner()
    {
        if (backward != null)
        {
            return backward.backTrainReturner();
        }
        else
        {
            return gameObject.GetComponent<TrainBase>();
        }
    }

    public TrainBase forTrainReturner()
    {
        if (forward != null)
        {
            return forward.forTrainReturner();
        }
        else
        {
            return gameObject.GetComponent<TrainBase>();
        }
    }


    public List<TrainBase> trainReturner()
    {
        List<TrainBase> trainVagons = new List<TrainBase>();
        TrainBase trainInter = gameObject.GetComponent<TrainBase>();
        while(trainInter != null)
        {
            trainVagons.Add(trainInter);
            trainInter = trainInter.backward;
        }
        return trainVagons;
    }

    public void reloadGuys()
    {


        if (backward)
        {
            lastPose = backward.nearestDest;
        }
        
        //wayPoints = new List<BaseDotWay>();
        //wayPoints.Add(nearestDest);
    }

    public void swapper()
    {
        reloadGuys();
        //Debug.Log("SWAP" + gameObject.name);
        if (isFollower && backward == null)
        {
            isFollower = false;
        }
        if (!isFollower && forward == null)
        {
            isFollower = true;
        }

        intforward = forward;
        intbackward = backward;

        backward = intforward;
        forward = intbackward;


    }
}
