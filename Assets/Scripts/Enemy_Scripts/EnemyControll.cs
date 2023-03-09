using UnityEngine;
using UnityEngine.AI;
using UnityEngine.PlayerLoop;
using Quaternion = UnityEngine.Quaternion;

public enum EnemyState
{
    Idle,
    Walk,
    Run,
    Pause,
    GoBack,
    Attack,
    Dead
}
public class EnemyControll : MonoBehaviour
{

    private float _distanceAttack = 1.5f;

    private float _distanceAlertAttack = 8f;

    private float _followDistance = 15f;

    private float _enemyToPlayerDistance;
    
    public EnemyState enemyCurrentState = EnemyState.Idle;
    private EnemyState _enemyLastState = EnemyState.Idle;

    private Transform _target;

    private Vector3 _initialPosition;

    private float _walkSpeed = 1f;
    private float _moveSpeed = 2f;
    
    private CharacterController _characterController;
    private Vector3 _whereToMove = Vector3.zero;
    private float _currentAttackTime;
    private float _waitAttackTime = 1f;

    private Animator _animation;
    private bool _finishedAnimation = true;
    private bool _finishedMovment = true;

    private NavMeshAgent _agent;
    private Vector3 _whereToNavigate;
    
    

    // Start is called before the first frame update
    void Awake()
    {
        _target = GameObject.FindGameObjectWithTag("Player").transform;
        _agent = GetComponent<NavMeshAgent>();
        _characterController = GetComponent<CharacterController>();
        _animation = GetComponent<Animator>();
        _initialPosition = transform.position;
        _whereToNavigate = transform.position;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyCurrentState != EnemyState.Dead)
        {
            enemyCurrentState = SetEnemyState(enemyCurrentState, _enemyLastState, _enemyToPlayerDistance);
            if (_finishedMovment)
            {
               GetStateControl(enemyCurrentState);
            }
            else
            {
                if (!_animation.IsInTransition(0) 
                    && _animation.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
                {
                    _finishedMovment = true;
                }
                else if(!_animation.IsInTransition(0)
                        && _animation.GetCurrentAnimatorStateInfo(0).IsTag("Atk1")
                        || _animation.GetCurrentAnimatorStateInfo(0).IsTag("Atk2"))

                {
                    _animation.SetInteger("Atk",0);
                }
            }
        }
        else
        {
            _animation.SetBool("Death", true);
            _characterController.enabled = false;
            _agent.enabled = false;
            if (!_animation.IsInTransition(0)
                && _animation.GetCurrentAnimatorStateInfo(0).IsName("Death")
                && _animation.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.95f)
            {
                Destroy(gameObject, 2f);
            }
        }
    }

    EnemyState SetEnemyState(EnemyState currentEnemyState, EnemyState lastEnemyState, float enemyToPlayerDis)
    {
        enemyToPlayerDis = Vector3.Distance(transform.position, _target.position);
        float initialDistance = Vector3.Distance(_initialPosition, transform.position);

        if (initialDistance > _followDistance)
        {
            lastEnemyState = currentEnemyState;
            currentEnemyState = EnemyState.Walk;
        } 
        else if (enemyToPlayerDis <= _distanceAttack)
        {
            lastEnemyState = currentEnemyState;
            currentEnemyState = EnemyState.Attack;
        } 
        else if (enemyToPlayerDis >= _distanceAlertAttack
                   && lastEnemyState == EnemyState.Pause
                   || lastEnemyState == EnemyState.Attack)
        {
            lastEnemyState = currentEnemyState;
            currentEnemyState = EnemyState.Pause;
        } 
        else if (enemyToPlayerDis <= _distanceAlertAttack && enemyToPlayerDis > _distanceAttack)
        {
            if (currentEnemyState != EnemyState.GoBack || lastEnemyState == EnemyState.Walk)
            {
                lastEnemyState = currentEnemyState;
                currentEnemyState = EnemyState.Pause;
            }
        } 
        else if (enemyToPlayerDis <= _distanceAlertAttack 
                   && enemyToPlayerDis > _distanceAttack)
        {
            if (currentEnemyState != EnemyState.GoBack 
                || lastEnemyState == EnemyState.Walk)
            {
                lastEnemyState = currentEnemyState;
                currentEnemyState = EnemyState.Pause;
            }
        } 
        else if (enemyToPlayerDis > _distanceAlertAttack
                   && lastEnemyState != EnemyState.GoBack
                   && lastEnemyState != EnemyState.Pause)
        {
            lastEnemyState = currentEnemyState;
            currentEnemyState = EnemyState.Walk;
        }

        return currentEnemyState;
    }

    void GetStateControl(EnemyState curEnemyState)
    {
        if (curEnemyState == EnemyState.Run 
            || curEnemyState == EnemyState.Pause)
        {
            if (curEnemyState != EnemyState.Attack)
            {
                Vector3 targetPosition = new Vector3(_target.position.x,
                    transform.position.y,
                    _target.position.z);


                if (Vector3.Distance(transform.position, targetPosition) >= 2.1f)
                {
                    _animation.SetBool("Walk", false);
                    _animation.SetBool("Run", true);

                    _agent.SetDestination(targetPosition);
                }
            }
        }
        else if (curEnemyState == EnemyState.Attack)
        {
            _animation.SetBool("Run", false);
            _whereToMove.Set(0f, 0f,0f);
            _agent.SetDestination(transform.position);

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                Quaternion.LookRotation(_target.position - transform.position),
                5f * Time.deltaTime);
            if (_currentAttackTime >= _waitAttackTime)
            {
                int AttackRange = Random.Range(1, 3);
                _animation.SetInteger("Atk", AttackRange);
                _finishedAnimation = false;
                _currentAttackTime = 0f;
            }
            else
            {
                _animation.SetInteger("Atk", 0);
                _currentAttackTime += Time.deltaTime;
            }
        } 
        else if (curEnemyState == EnemyState.GoBack)
        {
            _animation.SetBool("Run", true);
            Vector3 targetPosition = new Vector3(_initialPosition.x, 
                transform.position.y,
                _initialPosition.z);
            _agent.SetDestination(targetPosition);

            if (Vector3.Distance(targetPosition, _initialPosition) <= 3.5)
            {
                _enemyLastState = curEnemyState;
                curEnemyState = EnemyState.Walk;
            }
        } 
        else if (curEnemyState == EnemyState.Walk)
        {
            _animation.SetBool("Run", false);
            _animation.SetBool("Walk", true);

            if (Vector3.Distance(transform.position, _whereToNavigate) <= 2f)
            {
                _whereToNavigate.x = Random.Range(_initialPosition.x - 5f, _initialPosition.x + 5f);
                _whereToNavigate.z = Random.Range(_initialPosition.z - 5f, _initialPosition.z + 5f);
            }
            else
            {
                _agent.SetDestination(_whereToNavigate);
            }
        }
        else
        {
            _animation.SetBool("Run", false);
            _animation.SetBool("Walk", false);
            _whereToMove.Set(0f, 0f,0f);
            _agent.isStopped = true;
        }
    }
    
}
