using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public float rangeDestroy = 12f;

    void Start()
    {
        
    }

    void Update()
    {
        float moveX = moveSpeed * Time.deltaTime;
        this.transform.Translate(moveX, 0f, 0f);    

        if (this.transform.localPosition.x > rangeDestroy)
        {
            GameObject.Destroy(gameObject);
        }
    }
}
