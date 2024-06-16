using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Road : MonoBehaviour
{
    public Transform cloneTarget = null;
    public Transform generationPos = null;

    public int generationRate = 60;

    public float cloneDelaySec = 1f;

    protected float nextSecToClone = 0f;
    void Start()
    {

    }

    void Update()
    {
        float curSec = Time.time;
        if (nextSecToClone <= curSec)
        {
            int randomVal = Random.Range(0, 100);
            if (randomVal < generationRate)
            {
                CloneCar();
            }
            nextSecToClone = curSec + cloneDelaySec;
        }
    }

    void CloneCar()
    {
        Transform clonepos = generationPos;
        Vector3 offsetPos = clonepos.position;

        GameObject cloneObj = GameObject.Instantiate(cloneTarget.gameObject
            , offsetPos
            , generationPos.rotation
            , this.transform);
        cloneObj.SetActive(true);
    }
}
