using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathf : MonoBehaviour
{
    private int wayDist;
    public BaseDotWay startPos;
    public BaseDotWay lastPos;
    List<BaseDotWay> PathWay = new List<BaseDotWay>();
    BaseDotWay lastRet;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (startPos && lastPos && Input.GetKeyDown(KeyCode.Space))
        {
            Pathfinder(startPos, lastPos);
        }
    }



    private List<BaseDotWay> sonsReturner(List<BaseDotWay> dotWaySp, ref List<BaseDotWay> dotWayVisited, BaseDotWay finish)
    {
        List<BaseDotWay> spToReturn = new List<BaseDotWay>();
        foreach (BaseDotWay i in dotWaySp)
        {
            foreach (BaseDotWay k in i.dotNeibors)
            {
                if (!dotWayVisited.Contains(k))
                {
                    spToReturn.Add(k);
                    dotWayVisited.Add(k);

                    if (k == finish)
                    {
                        PathWay.Add(i);
                        lastRet = i;
                    }
                }
               
            }
        }
        return spToReturn;
    }



    private int isWay(BaseDotWay start, BaseDotWay finish)
    {
        List<BaseDotWay> dotWaySp = new List<BaseDotWay>();
        List<BaseDotWay> dotWayVisited = new List<BaseDotWay>();
        dotWaySp.Add(start);
        bool whileNotFinish = true;
        int counter = 0;

        while(whileNotFinish)
        {
            foreach (BaseDotWay i in dotWaySp) {
                if (!dotWaySp.Contains(finish))
                {
                    dotWaySp = sonsReturner(dotWaySp, ref dotWayVisited, finish);
                    counter++;
                }
                else
                {
                    whileNotFinish = false; 
                }
            }
        }
        return counter;
    }

    private List<BaseDotWay> Pathfinder(BaseDotWay start, BaseDotWay finish)
    {
        lastRet = finish;
        PathWay = new List<BaseDotWay>();
        BaseDotWay fin = finish;
        while(isWay(start, fin) != 0)
        {
            fin = lastRet;
        }
        PathWay.Reverse();
        PathWay.Add(finish);
        Debug.Log(string.Join(",", PathWay));
        return PathWay;
    }


    

}
