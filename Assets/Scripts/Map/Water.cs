using System.Collections.Generic;
using UnityEngine;

public class Water : BaseMap
{
    public List<RaftDataSO> raftDataList;

    protected override void CloneMoveObject()
    {
        Vector3 offsetPos = generationPos.position;

        GameObject cloneObj = ObjectPool.Instance.Get("Raft");
        if (cloneObj != null)
        {
            cloneObj.transform.position = offsetPos;
            cloneObj.transform.rotation = generationPos.rotation;

            Raft raft = cloneObj.GetComponent<Raft>();
            if (raft != null)
            {
                RaftDataSO selectedRaftData = raftDataList[Random.Range(0, raftDataList.Count)];
                raft.Initialize(selectedRaftData, ObjectPool.Instance, "Raft");
            }
        }
        else
        {
            Debug.LogWarning("Failed to get object from pool with tag 'Raft'");
        }
    }
}
