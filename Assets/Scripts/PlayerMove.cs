using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class PlayerMove_UnityEvents : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Vector2 moveInput;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        UpdateRaft();
    }


    public enum E_DirectionType
    {
        Up = 0,
        Down,
        Left,
        Right
    }

    protected E_DirectionType m_DirectionType = E_DirectionType.Up;

    protected void SetPlayerMove(E_DirectionType p_MoveType)
    {
        Vector3 offSetPos = Vector3.zero;

        switch (p_MoveType)
        {
            case E_DirectionType.Up:
                offSetPos = Vector3.forward;
                break;
            case E_DirectionType.Down:
                offSetPos = Vector3.back;
                break;
            case E_DirectionType.Left:
                offSetPos = Vector3.left;
                break;
            case E_DirectionType.Right:
                offSetPos = Vector3.right;
                break;
            default:
                Debug.LogErrorFormat("SetPlayerMove Error: {0}", p_MoveType);
                break;
        }

        this.transform.position += offSetPos;
        raftOffsetPos += offSetPos;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            moveInput = context.ReadValue<Vector2>();

            if (moveInput.y > 0)
            {
                SetPlayerMove(E_DirectionType.Up);
            }
            else if (moveInput.y < 0)
            {
                SetPlayerMove(E_DirectionType.Down);
            }
            else if (moveInput.x < 0)
            {
                SetPlayerMove(E_DirectionType.Left);
            }
            else if (moveInput.x > 0)
            {
                SetPlayerMove(E_DirectionType.Right);
            }
        }
    }
    Vector3 raftOffsetPos = Vector3.zero;
    protected void UpdateRaft()
    {
        if (raftObject == null)
        {
            return;
        }

        Vector3 actorPos = raftObject.transform.position + raftOffsetPos;
        this.transform.position = actorPos;
    }

    protected Raft raftObject = null;
    protected Transform raftCompareObj = null;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Raft"))
        {
            raftObject = other.transform.parent.GetComponent<Raft>();
            if (raftObject != null)
            {
                raftCompareObj = raftObject.transform;
                raftOffsetPos = this.transform.position - raftObject.transform.position;
            }
            return;
        }
        if (other.CompareTag("Crash"))
        {
            Debug.Log("ºÎ‹HÇû´Ù");
            return;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Raft"))
        {
            if (raftCompareObj == other.transform.parent)
            {
                raftCompareObj = null;
                raftObject = null;
                raftOffsetPos = Vector3.zero;
            }
            return;
        }
    }
}