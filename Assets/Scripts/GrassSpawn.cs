using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassSpawn : MonoBehaviour
{
    public List<Transform> envObjectList = new List<Transform>();
    public int startMinVal = -12;
    public int startMaxVal = 12;

    public int spawnRandomRate = 50;

    void GeneratorRoundBlock()
    {
        int randomIndex = 0;

        GameObject tempClone = null;
        Vector3 offSetPos = Vector3.zero;

        float posZ = this.transform.position.z;

        for (int i = startMinVal; i < startMaxVal; ++i)
        {
            if (i < -5 || i > 5)
            {
                randomIndex = Random.Range(0, envObjectList.Count);
                tempClone = Instantiate(envObjectList[randomIndex].gameObject);
                offSetPos.Set(i, 1, 0);

                tempClone.transform.SetParent(this.transform);
                tempClone.transform.localPosition = offSetPos;
            }
        }
    }

    void GeneratorBackBlock()
    {
        int randomIndex = 0;

        GameObject tempClone = null;
        Vector3 offSetPos = Vector3.zero;

        float posZ = this.transform.position.z;

        for (int i = startMinVal; i < startMaxVal; ++i)
        {
            randomIndex = Random.Range(0, envObjectList.Count);
            tempClone = Instantiate(envObjectList[randomIndex].gameObject);
            offSetPos.Set(i, 1, 0);

            tempClone.transform.SetParent(this.transform);
            tempClone.transform.localPosition = offSetPos;
        }
    }

    void GeneratorTree()
    {
        int randomIndex = 0;
        int randomVal = 0;

        GameObject tempClone = null;
        Vector3 offSetPos = Vector3.zero;

        float posZ = this.transform.position.z;

        for (int i = startMinVal; i < startMaxVal; ++i)
        {
            randomVal = Random.Range(0, 100);
            if (randomVal < spawnRandomRate)
            {
                randomIndex = Random.Range(0, envObjectList.Count);
                tempClone = Instantiate(envObjectList[randomIndex].gameObject);
                offSetPos.Set(i, 1, 0);

                tempClone.transform.SetParent(this.transform);
                tempClone.transform.localPosition = offSetPos;
            }
        }
    }

    void GeneratorEnv()
    {
        if (this.transform.position.z <= -4)
        {
            GeneratorBackBlock();
        }
        else if (this.transform.position.z <= 0)
        {
            GeneratorRoundBlock();
        }
        else
        {
            GeneratorTree();
        }
    }

    private void Start()
    {
        GeneratorEnv();
    }
}
