using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerControl : MonoBehaviour
{
    [SerializeField] private CinemachineBrain _CameraBrain;

    [SerializeField] private CinemachineVirtualCamera _camera;

    [SerializeField] private CinemachineVirtualCamera _cameraGrapple;

    [SerializeField] private Animator _anim;

    [SerializeField] private Transform _playerT;

    [SerializeField] private Transform _modelT;

    [SerializeField] private Transform _hands;

    [SerializeField] private LineRenderer _lineRenderer;

    [SerializeField] private Rigidbody _rb;

    [SerializeField] private CameraControl _cameraControl;

    [SerializeField] private PlayerStateMachine _stateMachine = default;
    [SerializeField] private PlayerMove _playerMove;
    [SerializeField] private InputManager _inputManager;
    [SerializeField] private PlayerAnimationControl _animControl;
    [SerializeField] private GroundCheck _groundCheck;
    [SerializeField] private Swing _swing;
    [SerializeField] private VelocityLimit _velocityLimit;
    [SerializeField] private SearchSwingPoint _searchSwingPoint;
    [SerializeField] private ZipMove _zipMove;
    [SerializeField] private WallRun _wallRun;
    [SerializeField] private WallRunCheck _wallRunCheck;
    [SerializeField] private Grapple _grapple;
    [SerializeField] private Attack _attack;
    [SerializeField] private Avoid _avoid;
    [SerializeField] private SetUp _setUp;
    [SerializeField] private PlayerEffectControl _effectControl;


    private SpringJoint _joint;


    public CameraControl CameraControl => _cameraControl;
    public CinemachineBrain CameraBrain => _CameraBrain;
    public InputManager InputManager => _inputManager;

    public PlayerMove Move => _playerMove;
    public Swing Swing => _swing;
    public CinemachineVirtualCamera Camera => _camera;
    public CinemachineVirtualCamera CameraGrapple => _cameraGrapple;
    public Transform ModelT => _modelT;
    public Transform Hads => _hands;
    public Animator Anim => _anim;
    public Rigidbody Rb => _rb;
    public Transform PlayerT => _playerT;
    public PlayerAnimationControl AnimControl => _animControl;
    public SpringJoint Joint { get => _joint; set => _joint = value; }
    public GroundCheck GroundCheck => _groundCheck;
    public LineRenderer LineRenderer => _lineRenderer;
    public VelocityLimit VelocityLimit { get => _velocityLimit; set => _velocityLimit = value; }
    public SearchSwingPoint SearchSwingPoint => _searchSwingPoint;
    public ZipMove ZipMove => _zipMove;
    public WallRun WallRun => _wallRun;
    public Grapple Grapple => _grapple;
    public Attack Attack => _attack;
    public WallRunCheck WallRunCheck => _wallRunCheck;
    public Avoid Avoid => _avoid;
    public PlayerEffectControl EffectControl => _effectControl;


    public SetUp SetUp => _setUp;

    float h = 0;
    float v = 0;

    Vector3 velo;

    private void Awake()
    {
        _stateMachine.Init(this);
        _playerMove.Init(this);
        _animControl.Init(this);
        _groundCheck.Init(this);
        _swing.Init(this);
        _velocityLimit.Init(this);
        _searchSwingPoint.Init(this);
        _zipMove.Init(this);
        _wallRunCheck.Init(this);
        _wallRun.Init(this);
        _grapple.Init(this);
        _attack.Init(this);
        _avoid.Init(this);
        _setUp.Init(this);
        _effectControl.Init(this);
    }

    void Start()
    {

    }

    private void OnDrawGizmosSelected()
    {
        _groundCheck.OnDrawGizmos(PlayerT);

        _searchSwingPoint.OnDrawGizmos(PlayerT);

        _wallRunCheck.OnDrawGizmos(PlayerT);
    }
    private void Update()
    {
        _stateMachine.Update();
        _velocityLimit.LimitSpeed();

        _animControl.AnimSet();


        _effectControl.ConcentrationLineEffect();
    }

    private void FixedUpdate()
    {
        _stateMachine.FixedUpdate();
    }

    private void LateUpdate()
    {
        _stateMachine.LateUpdate();
    }

    public void DestroyJoint()
    {
        Destroy(_joint);
    }

    public GameObject InstantiateObject(GameObject obj)
    {
        return Instantiate(obj);
    }

    public void CoolTimes()
    {
        _attack.AttackCoolTime();
        _swing.CountCoolTime();
        _avoid.CountCoolTimeAvoid();
    }

}
