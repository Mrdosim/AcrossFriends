using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    public float maxRangeDestroy = 12f;
    public float minRangeDestroy = -12f;

    private float moveSpeed;
    private string poolTag;
    private ObjectPool pool;

    void Update()
    {
        float moveX = moveSpeed * Time.deltaTime;
        this.transform.Translate(moveX, 0f, 0f);

        if (this.transform.localPosition.x > maxRangeDestroy || this.transform.localPosition.x < minRangeDestroy)
        {
            pool.ReturnToPool(poolTag, this.gameObject);
        }
    }

    public void Initialize(CarDataSO carData, ObjectPool pool, string poolTag)
    {
        moveSpeed = carData.moveSpeed;
        GetComponentInChildren<Renderer>().material.color = carData.color;
        transform.localScale = carData.size;
        this.pool = pool;
        this.poolTag = poolTag;
    }
}
