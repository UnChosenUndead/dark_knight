using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Animator _animator;

    private CharacterController _characterController;

    private CollisionFlags _collisionFlags = CollisionFlags.None;

    private float _moveSpeed = 5f;

    private bool _canMove;

    private bool _finishedMovement = true;

    public bool FinishedMovement
    {
        get => _finishedMovement;
        set => _finishedMovement = value;
    }

    private Vector3 targetPose = Vector3.zero;

    public Vector3 TargetPose
    {
        get => targetPose;
        set => targetPose = value;
    }

    private Vector3 playerMove = Vector3.zero;

    private float _playerToPointDistance;
    
    private float gravity = 9.8f;
    
    private float height;

    // Start is called before the first frame update
    void Awake()
    {
        _animator = GetComponent<Animator>();
        _characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        CalculateHeight();
        CheckIfFinishedMovement();
    }

    bool isGroundead()
    {
        return _collisionFlags == CollisionFlags.CollidedBelow ? true : false;
    }

    void CalculateHeight()
    {
        if (isGroundead())
        {
            height = 0f;
        }
        else
        {
            height -= gravity * Time.deltaTime;
        }
    }

    void CheckIfFinishedMovement()
    {
        if (!_finishedMovement)
        {
            if (!_animator.IsInTransition(0) && !_animator.GetCurrentAnimatorStateInfo(0).IsName("Stand")
                                             && _animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.8f)
            {
                _finishedMovement = true;
            }
        }
        else
        {
          MoveThePlayer();
          playerMove.y = height * Time.deltaTime;
          _collisionFlags = _characterController.Move(playerMove);
        }
    }

    void MoveThePlayer()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (Camera.main != null)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit raycastHit;
                
                if (Physics.Raycast(ray, out raycastHit))
                {
                    if (raycastHit.collider is TerrainCollider)
                    {
                        _playerToPointDistance = Vector3.Distance(transform.position, raycastHit.point);

                        if (_playerToPointDistance >= 1.0f)
                        {
                            _canMove = true;
                            targetPose = raycastHit.point;
                        }
                    }
                }
            }
        }
        if (_canMove)
        {
            _animator.SetFloat("Walk", 1.0f);
            Vector3 targetTemp = new Vector3(targetPose.x, transform.position.y, targetPose.z);
            transform.rotation = Quaternion.Slerp(transform.rotation,
                Quaternion.LookRotation(targetTemp - transform.position), 
                15.0f * Time.deltaTime);
            playerMove = transform.forward * _moveSpeed * Time.deltaTime;
            if (Vector3.Distance(transform.position, targetPose) <= 0.1f)
            {
                _canMove = false;
            }
        }
        else
        {
            playerMove.Set(0f, 0f, 0f);
            _animator.SetFloat("Walk", 0f);
        }
    }
}
