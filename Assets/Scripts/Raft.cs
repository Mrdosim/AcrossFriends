using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raft : MonoBehaviour
{
    public float rangeDestroy = 12f;

    private float moveSpeed;
    private string poolTag;
    private ObjectPool pool;

    void Update()
    {

        float moveX = moveSpeed * Time.deltaTime;
        this.transform.Translate(moveX, 0f, 0f);

        if (this.transform.localPosition.x > rangeDestroy)
        {
            pool.ReturnToPool(poolTag, this.gameObject);
        }
    }

    public void Initialize(RaftDataSO raftData, ObjectPool pool, string poolTag)
    {
        moveSpeed = raftData.moveSpeed;
        transform.localScale = raftData.size;
        this.pool = pool;
        this.poolTag = poolTag;
    }
}
