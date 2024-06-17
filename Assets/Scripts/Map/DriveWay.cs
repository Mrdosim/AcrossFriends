using System.Collections.Generic;
using UnityEngine;

public class DriveWay : BaseMap
{
    public List<CarDataSO> carDataList;

    protected override void CloneMoveObject()
    {
        Vector3 offsetPos = generationPos.position;

        GameObject cloneObj = ObjectPool.Instance.Get("Car");
        if (cloneObj != null)
        {
            cloneObj.transform.position = offsetPos;
            cloneObj.transform.rotation = generationPos.rotation;

            Car car = cloneObj.GetComponent<Car>();
            if (car != null)
            {
                CarDataSO selectedCarData = carDataList[Random.Range(0, carDataList.Count)];
                car.Initialize(selectedCarData, ObjectPool.Instance, "Car");
            }
        }
        else
        {
            Debug.LogWarning("Failed to get object from pool with tag 'Car'");
        }
    }
}
