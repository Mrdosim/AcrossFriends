using UnityEngine;

[CreateAssetMenu(fileName = "NewCarData", menuName = "Raft Data", order = 52)]
public class RaftDataSO : ScriptableObject
{
    public float moveSpeed;
    public Vector3 size;
}