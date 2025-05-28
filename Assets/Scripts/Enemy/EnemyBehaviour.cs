using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehaviour : EntityBehaviour
{
    [Header("AI")]
    [SerializeField] private float _attackDistance = 2.0f;
    [SerializeField] private float _chaseDistance = 6.25f;
    [SerializeField] private float _updateNodeDistance = 0.75f;

    [Header("Animation")]
    [SerializeField] private string _attackTriggerName = "onAttack";
    [SerializeField] private string _velocityFloatName = "velocity";

    [Header("Combat")]
    [SerializeField] private int _dmg = 10;

    [Header("Physics")]
    [SerializeField] private float _attackRayDistance = 2.0f;
    [SerializeField] private LayerMask _attackRayMask;
    [SerializeField] private float _attackSphereRadius = 0.75f;
    [SerializeField] private Transform _rayOrigin;

    private float _playerDistance = 0.0f, _nodeDistance = 0.0f;
    private string _state = "";

    private Animator _animator;
    private NavMeshAgent _agent;
    private Transform[] _aiNodes;
    private Transform _actualNode;
    private PlayerBehaviour _player;

    private Ray _attackRay;
    private RaycastHit _attackHit;

    protected override void Awake()
    {
        base.Awake();

        _agent = GetComponent<NavMeshAgent>();
    }

    protected override void Start()
    {
        base.Start();

        _animator = GetComponentInChildren<Animator>();

        _aiNodes = GameManager.Instance.AINodes;

        _actualNode = GetNewNode();

        _agent.SetDestination(_actualNode.position);

        _player = GameManager.Instance.Player;
    }

    protected override void Update()
    {
        base.Update();

        _animator.SetFloat(_velocityFloatName, _agent.velocity.magnitude);

        _playerDistance = Vector3.SqrMagnitude(transform.position - _player.transform.position);

        if(_playerDistance <= _chaseDistance * _chaseDistance)
        {
            if(_playerDistance <= _attackDistance * _attackDistance)
            {
                _state = "Attack";

                if (!_agent.isStopped)
                {
                    _agent.isStopped = true;
                }

                _animator.SetTrigger(_attackTriggerName);
            }
            else
            {
                _state = "Chase";

                if (_agent.isStopped)
                {
                    _agent.isStopped = false;
                }

                _agent.SetDestination(_player.transform.position);
            }
        }
        else
        {
            _state = "Patrol";

            if(_agent.destination != _actualNode.position)
            {
                _agent.SetDestination(_actualNode.position);
            }

            _nodeDistance = Vector3.SqrMagnitude(transform.position - _actualNode.position);

            if(_nodeDistance <= _updateNodeDistance * _updateNodeDistance)
            {
                _actualNode = GetNewNode(_actualNode);
                _agent.SetDestination(_actualNode.position);
            }
        }

        //Debug.Log($"{name}: State: {_state}. Distance to player: {Mathf.Sqrt(_playerDistance)}.");
    }

    public void Attack()
    {
        _attackRay = new Ray(_rayOrigin.position, transform.forward);

        if(Physics.SphereCast(_attackRay, _attackSphereRadius, out _attackHit, _attackRayDistance, _attackRayMask))
        {
            if(_attackHit.collider.TryGetComponent(out EntityBehaviour entity))
            {
                entity.TakeDamage(_dmg);
            }
        }
    }

    private Transform GetNewNode(Transform actualNode = null)
    {
        if (!actualNode)
        {
            return _aiNodes[Random.Range(0, _aiNodes.Length)];
        }
        else
        {
            Transform newNode;

            do
            {
                newNode = _aiNodes[Random.Range(0, _aiNodes.Length)];
            }
            while (actualNode == newNode);

            return newNode;
        }
    }

    public override void TakeDamage(int dmg)
    {
        _actualHP -= dmg;

        if(_actualHP <= 0)
        {
            Destroy(gameObject);
        }
        else
        {
            Debug.Log($"{name}: Au. :(");
        }
    }
}
