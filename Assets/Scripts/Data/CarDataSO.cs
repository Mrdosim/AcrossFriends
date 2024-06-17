using UnityEngine;

[CreateAssetMenu(fileName = "NewCarData", menuName = "Car Data", order = 51)]
public class CarDataSO : ScriptableObject
{
    public float moveSpeed;
    public Color color;
    public Vector3 size;
}