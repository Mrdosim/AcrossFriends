using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    private static MapManager _instance;

    public static MapManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<MapManager>();

                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject();
                    _instance = singletonObject.AddComponent<MapManager>();
                    singletonObject.name = typeof(MapManager).ToString() + " (Singleton)";
                }
            }

            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }

    public enum E_EnvironmentType
    {
        Grass = 0,
        Road,
        Water,
        Max
    }

    public enum E_LastRoadType
    {
        Grass = 0,
        Water,
        Road,
        Max
    }

    [Header("º¹Á¦¿ë±æ")]
    public DriveWay defaultRoad = null;
    public Water waterRoad = null;
    public GrassSpawn grassRoad = null;
    public Transform fireworkPrefab; // ÆøÁ× ÇÁ¸®ÆÕ Ãß°¡

    public Transform parentTransform = null;

    public int minPosZ = -20;
    public int maxPosZ = 20;

    public int frontOffSetPosZ = 10;
    public int backOffSetPosZ = 10;

    public int fireworkSpawnRate = 10; // ÆøÁ× »ý¼º È®·ü Ãß°¡

    private E_LastRoadType lastRoadType = E_LastRoadType.Max;
    public List<Transform> lineMapList = new List<Transform>();
    public Dictionary<int, Transform> lineMapDic = new Dictionary<int, Transform>();
    public Dictionary<int, Transform> fireworkMapDic = new Dictionary<int, Transform>(); // ÆøÁ×À» ÀúÀåÇÏ´Â Dictionary Ãß°¡
    public int lastLinePos = 0;
    public int minLine = 0;
    public int deleteLine = 10;
    public int backOffSetLineCount = 20;

    private void Start()
    {
        InitializeMap();
    }

    public void InitializeMap()
    {
        // ±âÁ¸ ¸Ê Å¬¸®¾î
        foreach (Transform line in lineMapList)
        {
            Destroy(line.gameObject);
        }

        lineMapList.Clear();
        lineMapDic.Clear();
        fireworkMapDic.Clear(); // ÆøÁ× ¸Ê ÃÊ±âÈ­
        lastLinePos = 0;
        minLine = minPosZ;

        lastRoadType = E_LastRoadType.Grass;
        GenerateInitialMap();
    }

    private void GenerateInitialMap()
    {
        for (int i = minPosZ; i < maxPosZ; ++i)
        {
            int offSetVal = 0;
            if (i < 0)
            {
                GenerateGrassLine(i);
            }
            else
            {
                if (lastRoadType == E_LastRoadType.Grass)
                {
                    int randomVal = Random.Range(0, 2);
                    if (randomVal == 0)
                    {
                        offSetVal = GroupRandomWaterLine(i);
                        lastRoadType = E_LastRoadType.Water;
                    }
                    else
                    {
                        offSetVal = GroupRandomRoadLine(i);
                        lastRoadType = E_LastRoadType.Road;
                    }
                }
                else
                {
                    offSetVal = GroupRandomGrassLine(i);
                    lastRoadType = E_LastRoadType.Grass;
                }
                i += offSetVal - 1;
            }
        }
    }

    public int GroupRandomRoadLine(int posZ)
    {
        int randomCount = Random.Range(1, 4);

        for (int i = 0; i < randomCount; ++i)
        {
            GenerateRoadLine(posZ + i);
        }

        return randomCount;
    }

    public int GroupRandomWaterLine(int posZ)
    {
        int randomCount = Random.Range(1, 4);

        for (int i = 0; i < randomCount; ++i)
        {
            GenerateWaterLine(posZ + i);
        }

        return randomCount;
    }

    public int GroupRandomGrassLine(int posZ)
    {
        int randomCount = Random.Range(1, 3);

        for (int i = 0; i < randomCount; ++i)
        {
            GenerateGrassLine(posZ);
            posZ++;
        }

        return randomCount;
    }

    public void GenerateWaterLine(int posZ)
    {
        if (lineMapDic.ContainsKey(posZ))
        {
            return;
        }

        GameObject cloneObj = Instantiate(waterRoad.gameObject);

        Vector3 offSetPos = Vector3.zero;
        offSetPos.z = posZ;
        cloneObj.transform.parent = parentTransform;
        cloneObj.transform.position = offSetPos;

        int randomRot = Random.Range(0, 2);
        if (randomRot == 1)
        {
            cloneObj.transform.rotation = Quaternion.Euler(0, 180f, 0);
        }

        lineMapList.Add(cloneObj.transform);
        lineMapDic.Add(posZ, cloneObj.transform);
    }

    public void GenerateRoadLine(int posZ)
    {
        if (lineMapDic.ContainsKey(posZ))
        {
            return;
        }

        GameObject cloneObj = Instantiate(defaultRoad.gameObject);

        Vector3 offSetPos = Vector3.zero;
        offSetPos.z = posZ;
        cloneObj.transform.parent = parentTransform;
        cloneObj.transform.position = offSetPos;

        int randomRot = Random.Range(0, 2);
        if (randomRot == 1)
        {
            cloneObj.transform.rotation = Quaternion.Euler(0, 180f, 0);
        }

        lineMapList.Add(cloneObj.transform);
        lineMapDic.Add(posZ, cloneObj.transform);
    }

    public void GenerateGrassLine(int posZ)
    {
        if (lineMapDic.ContainsKey(posZ))
        {
            return;
        }

        GameObject cloneObj = Instantiate(grassRoad.gameObject);

        Vector3 offSetPos = Vector3.zero;
        offSetPos.z = posZ;
        cloneObj.transform.parent = parentTransform;
        cloneObj.transform.position = offSetPos;

        lineMapList.Add(cloneObj.transform);
        lineMapDic.Add(posZ, cloneObj.transform);

        // È®·üÀûÀ¸·Î ÆøÁ×À» »ý¼º
        if (Random.Range(0, 100) < fireworkSpawnRate)
        {
            GameObject fireworkClone = Instantiate(fireworkPrefab.gameObject);
            fireworkClone.transform.SetParent(cloneObj.transform);
            fireworkClone.transform.localPosition = Vector3.zero;
            fireworkClone.SetActive(false); // Ã³À½¿¡´Â ºñÈ°¼ºÈ­ »óÅÂ·Î Ãß°¡
            fireworkMapDic.Add(posZ, fireworkClone.transform); // ÆøÁ×À» Dictionary¿¡ Ãß°¡
        }
    }

    public void UpdateForwardNBAckMove(int posZ)
    {
        if (lineMapList.Count <= 0)
        {
            lastRoadType = E_LastRoadType.Grass;
            minLine = minPosZ;
            int i = 0;
            for (i = minPosZ; i < maxPosZ; ++i)
            {
                int offSetVal = 0;
                if (i < 0)
                {
                    GenerateGrassLine(i);
                }
                else
                {
                    if (lastRoadType == E_LastRoadType.Grass)
                    {
                        int randomVal = Random.Range(0, 2);
                        if (randomVal == 0)
                        {
                            offSetVal = GroupRandomWaterLine(i);
                            lastRoadType = E_LastRoadType.Water;
                        }
                        else
                        {
                            offSetVal = GroupRandomRoadLine(i);
                            lastRoadType = E_LastRoadType.Road;
                        }
                    }
                    else
                    {
                        offSetVal = GroupRandomGrassLine(i);
                        lastRoadType = E_LastRoadType.Grass;
                    }
                    i += offSetVal - 1;
                }
            }
            lastLinePos = i;
        }

        // »õ·Ó°Ô »ý¼º
        if (lastLinePos < posZ + frontOffSetPosZ)
        {
            int offSetVal = 0;
            if (lastRoadType == E_LastRoadType.Grass)
            {
                int randomVal = Random.Range(0, 2);
                if (randomVal == 0)
                {
                    offSetVal = GroupRandomWaterLine(lastLinePos);
                    lastRoadType = E_LastRoadType.Water;
                }
                else
                {
                    offSetVal = GroupRandomRoadLine(lastLinePos);
                    lastRoadType = E_LastRoadType.Road;
                }
            }
            else
            {
                offSetVal = GroupRandomGrassLine(lastLinePos);
                lastRoadType = E_LastRoadType.Grass;
            }
            lastLinePos += offSetVal;
        }

        // Áö¿ì±â
        if (posZ - backOffSetLineCount > minLine - deleteLine)
        {
            int count = minLine + deleteLine;
            for (int i = minLine; i < count; ++i)
            {
                RemoveLine(i);
            }
            minLine += deleteLine;
        }
    }

    void RemoveLine(int posZ)
    {
        if (lineMapDic.ContainsKey(posZ))
        {
            Transform transObj = lineMapDic[posZ];
            lineMapList.Remove(transObj);
            lineMapDic.Remove(posZ);

            if (fireworkMapDic.ContainsKey(posZ))
            {
                fireworkMapDic.Remove(posZ);
            }

            Destroy(transObj.gameObject);
        }
        else
        {
            Debug.LogErrorFormat("RemoveLine ¿¡·¯");
        }
    }
}
