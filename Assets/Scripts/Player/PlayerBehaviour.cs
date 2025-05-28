using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerBehaviour : EntityBehaviour
{
    [Header("Animator")]
    [SerializeField] private string _airBoolName = "isOnAir";
    [SerializeField] private string _atkTriggerName = "onAttack";
    [SerializeField] private string _intTriggerName = "onInteract";
    [SerializeField] private string _jumpTriggerName = "onJump";
    [SerializeField] private string _moveBoolName = "isMoving";
    [SerializeField] private string _xAxisName = "xAxis";
    [SerializeField] private string _zAxisName = "zAxis";

    [Header("Gameplay")]
    [SerializeField] private int _atkDmg = 10;

    [Header("Inputs")]
    [SerializeField] private KeyCode _atkKey = KeyCode.Mouse0;
    [SerializeField] private KeyCode _intKey = KeyCode.F;
    [SerializeField] private KeyCode _jumpKey = KeyCode.Space;
    [SerializeField] private KeyCode _quitKey = KeyCode.Escape;
    [SerializeField] private KeyCode _resetKey = KeyCode.R;

    [Header("Physics")]
    [SerializeField] private float _atkDistance = 2.0f;
    [SerializeField] private LayerMask _atkMask;
    [SerializeField] private float _atkRadius = 1.0f;
    [SerializeField] private float _intDistance = 2.0f;
    [SerializeField] private LayerMask _intMask;
    [SerializeField] private float _intRadius = 0.75f;
    [SerializeField] private float _groundRayDistance = 0.25f;
    [SerializeField] private LayerMask _groundRayMask;
    [SerializeField] private float _jumpForce = 5.0f;
    [SerializeField] private float _moveSpeed = 3.5f;
    [SerializeField] private Transform _rayOrigin;

    private bool _isGrounded = false;

    private Vector3 _dir = new(), _dirFix = new(), _camForwardFix = new(), 
                    _camRightFix = new(), _posOffset = new();

    private Animator _animator;
    private Rigidbody _rb;
    private SpringArm _arm;
    private Transform _camTransform;

    private Ray _atkRay, _intRay, _groundRay;
    private RaycastHit _intHit;
    private RaycastHit[] _atkHits;

    protected override void Awake()
    {
        GameManager.Instance.Player = this;

        base.Awake();

        _rb = GetComponent<Rigidbody>();
    }

    protected override void Start()
    {
        GameManager.Instance.ActualCheckpoint = transform.position;

        _animator = GetComponentInChildren<Animator>();

        _camTransform = Camera.main.transform;
        _arm = Camera.main.GetComponentInParent<SpringArm>();
    }

    protected override void Update()
    {
        base.Update();

        _dir.x = Input.GetAxis("Horizontal");
        _animator.SetFloat(_xAxisName, _dir.x);
        _dir.z = Input.GetAxis("Vertical");
        _animator.SetFloat(_zAxisName, _dir.z);

        _isGrounded = IsGrounded();

        _animator.SetBool(_airBoolName, !_isGrounded);
        _animator.SetBool(_moveBoolName, _dir.sqrMagnitude != 0.0f);

        if (Input.GetKeyDown(_jumpKey) && _isGrounded)
        {
            _animator.SetTrigger(_jumpTriggerName);
            Jump();
        }
        else if (Input.GetKeyDown(_intKey))
        {
            _animator.SetTrigger(_intTriggerName);
        }
        else if (Input.GetKeyDown(_atkKey))
        {
            _animator.SetTrigger(_atkTriggerName);
        }

        if (Input.GetKeyDown(_quitKey))
        {
            SceneLoadManager.Instance.LoadScene("MainMenu");
        }
        else if (Input.GetKeyDown(_resetKey))
        {
            ResetPlayer();
        }
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if(_dir.sqrMagnitude != 0.0f)
        {
            Movement(_dir);
        }
    }

    public void Attack()
    {
        _atkRay = new Ray(_rayOrigin.position, transform.forward);

        _atkHits = Physics.SphereCastAll(_atkRay, _atkRadius, _atkDistance, _atkMask);

        foreach(RaycastHit hit in _atkHits)
        {
            if(hit.collider.TryGetComponent(out EntityBehaviour entity))
            {
                entity.TakeDamage(_atkDmg);
            }
        }
    }

    public void Interact()
    {
        _intRay = new Ray(_rayOrigin.position, transform.forward);

        if (Physics.SphereCast(_intRay, _intRadius, out _intHit, _intDistance, _intMask))
        {
            Debug.Log($"Collided obj : {_intHit.collider.name}.");

            if(_intHit.collider.TryGetComponent(out IInteractable interactable))
            {
                interactable.OnInteract();
            }
        }
    }

    private bool IsGrounded()
    {
        _posOffset = new Vector3(transform.position.x,
                                 transform.position.y + 0.1f,
                                 transform.position.z);

        _groundRay = new Ray(_posOffset, -transform.up);

        return Physics.Raycast(_groundRay, _groundRayDistance, _groundRayMask);
    }

    private void Jump()
    {
        _rb.AddForce(transform.up * _jumpForce, ForceMode.Impulse);
    }

    private void Movement(Vector3 dir)
    {
        _camForwardFix = _camTransform.forward;
        _camRightFix = _camTransform.right;

        _camForwardFix.y = 0.0f;
        _camRightFix.y = 0.0f;

        Rotate(_camForwardFix);

        _dirFix = (_camRightFix * dir.x + _camForwardFix * dir.z).normalized;

        _rb.MovePosition(transform.position + _dirFix * _moveSpeed * Time.fixedDeltaTime);
    }

    public void ResetPlayer()
    {
        transform.position = GameManager.Instance.ActualCheckpoint;
    }

    private void Rotate(Vector3 dir)
    {
        transform.forward = dir;
    }

    //private void OnDrawGizmos()
    //{
    //    if (_isGrounded)
    //    {
    //        Gizmos.color = Color.green;
    //    }
    //    else
    //    {
    //        Gizmos.color= Color.red;
    //    }
    //    Gizmos.DrawLine(_groundRay.origin, _groundRay.direction * _groundRayDistance);
    //}
}
