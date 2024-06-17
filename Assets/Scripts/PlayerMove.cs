using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class PlayerMove : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Vector2 moveInput;
    private Rigidbody rb;
    public Animator animator;
    public AudioClip musicClip;
    private Vector3 initialPosition; // 초기 위치를 저장하는 변수

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        string[] tempLayer = new string[] { "Environment" };
        treeLayerMask = LayerMask.GetMask(tempLayer);

        MapManager.Instance.UpdateForwardNBAckMove((int)this.transform.position.z);

        initialPosition = transform.position; // 초기 위치 저장
    }

    private void Update()
    {
        UpdateRaft();

        // 캐릭터의 Y 좌표가 0 아래로 내려가면 게임 오버 처리
        if (transform.position.y < 0)
        {
            animator.SetTrigger("Hit");
            GameManager.Instance.GameOver();
        }
    }

    public enum E_DirectionType
    {
        Up = 0,
        Down,
        Left,
        Right
    }

    protected E_DirectionType m_DirectionType = E_DirectionType.Up;
    protected int treeLayerMask = -1;

    protected bool isCheckDirectionViewMove(E_DirectionType p_MoveType)
    {
        Vector3 direction = Vector3.zero;

        switch (p_MoveType)
        {
            case E_DirectionType.Up:
                direction = Vector3.forward;
                break;
            case E_DirectionType.Down:
                direction = Vector3.back;
                break;
            case E_DirectionType.Left:
                direction = Vector3.left;
                break;
            case E_DirectionType.Right:
                direction = Vector3.right;
                break;
            default:
                Debug.LogErrorFormat("SetPlayerMove Error: {0}", p_MoveType);
                break;
        }

        RaycastHit hitObj;
        if (Physics.Raycast(this.transform.position, direction, out hitObj, 1f, treeLayerMask))
        {
            return false;
        }
        return true;
    }

    protected void SetPlayerMove(E_DirectionType p_MoveType)
    {
        if (GameManager.Instance.IsGameOver())
        {
            return;
        }

        if (!isCheckDirectionViewMove(p_MoveType))
        {
            return;
        }
        Vector3 offSetPos = Vector3.zero;

        switch (p_MoveType)
        {
            case E_DirectionType.Up:
                offSetPos = Vector3.forward;
                this.transform.rotation = Quaternion.Euler(0, 0, 0);
                break;
            case E_DirectionType.Down:
                offSetPos = Vector3.back;
                this.transform.rotation = Quaternion.Euler(0, 180, 0);
                break;
            case E_DirectionType.Left:
                offSetPos = Vector3.left;
                this.transform.rotation = Quaternion.Euler(0, -90, 0);
                break;
            case E_DirectionType.Right:
                offSetPos = Vector3.right;
                this.transform.rotation = Quaternion.Euler(0, 90, 0);
                break;
            default:
                Debug.LogErrorFormat("SetPlayerMove Error: {0}", p_MoveType);
                break;
        }

        this.transform.position += offSetPos;
        raftOffsetPos += offSetPos;

        MapManager.Instance.UpdateForwardNBAckMove((int)this.transform.position.z);

        // 점수 갱신
        GameManager.Instance.UpdateScore(this.transform.position.z);

        // 폭죽 파티클 활성화
        ActivateFireworksIfPresent();
    }

    private void ActivateFireworksIfPresent()
    {
        int posZ = Mathf.RoundToInt(this.transform.position.z);
        if (MapManager.Instance.fireworkMapDic.TryGetValue(posZ, out Transform fireworkTransform))
        {
            fireworkTransform.gameObject.SetActive(true);
            ParticleSystem ps = fireworkTransform.GetComponent<ParticleSystem>();
            if (ps != null)
            {
                ps.Play();
            }
        }
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
            animator.SetTrigger("Hit");
            GameManager.Instance.GameOver();
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

    public void ResetPosition()
    {
        transform.position = initialPosition; // 플레이어의 위치를 초기 위치로 리셋
    }
}
