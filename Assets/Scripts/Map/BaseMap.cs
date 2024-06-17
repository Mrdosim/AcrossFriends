using DG.Tweening.Core.Easing;
using System.Collections;
using UnityEngine;

public abstract class BaseMap : MonoBehaviour
{
    public Transform cloneTarget = null;
    public Transform generationPos = null;

    public int generationRate = 60;

    public float cloneDelaySec = 1f;

    protected float nextSecToClone = 0f;

    void Update()
    {
        float curSec = Time.time;
        if (nextSecToClone <= curSec)
        {
            int randomVal = Random.Range(0, 100);
            if (randomVal < generationRate)
            {
                CloneMoveObject();
            }
            nextSecToClone = curSec + cloneDelaySec;
        }
    }

    protected abstract void CloneMoveObject();
}
