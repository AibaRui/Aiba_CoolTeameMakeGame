using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerControl : MonoBehaviour
{
    [SerializeField] private Animator _anim;

    [SerializeField] private Transform _playerT;

    [SerializeField] private Transform _modelT;

    [SerializeField] private Transform _hands;

    [SerializeField] private Transform _modelTop;
    [SerializeField] private Transform _modelDown;

    [SerializeField] private LineRenderer _lineRenderer;

    [Header("“–‚½‚è”»’è")]
    [SerializeField] private CapsuleCollider _collider;

    public CapsuleCollider PlayerCollider => _collider;

    [SerializeField] private Rigidbody _rb;
    [SerializeField] private InputManager _inputManager;
    [SerializeField] private CameraControl _cameraControl;
    [SerializeField] private PlayerAudioManager _playerAudioManager;
    [SerializeField] private ControllerVibrationManager _controllerVibrationManager;
    [SerializeField] private PlayerStateMachine _stateMachine = default;
    [SerializeField] private PlayerMove _playerMove;

    [SerializeField] private PlayerAnimationControl _animControl;
    [SerializeField] private GroundCheck _groundCheck;

    [SerializeField] private VelocityLimit _velocityLimit;
    [Header("=======Swing_========")]
    [SerializeField] private Swing _swing;
    [Header("=======Swing_Point’T‚µ========")]
    [SerializeField] private SearchSwingPoint _searchSwingPoint;
    [Header("=======Zip_“®‚«‚ÌÝ’è========")]
    [SerializeField] private Zip _zipMove;
    [Header("=======Zip_LineRender========")]
    [SerializeField] private ZipLineRenderer _zipLineRenderer;

    [Header("===PointZip====")]
    [SerializeField] private PointZip _pointZip;

    [SerializeField] private WallRun _wallRun;
    [SerializeField] private WallRunStepCheck _wallRunStep;
    [SerializeField] private WallRunUpZip _wallRunUpZip;
    [SerializeField] private WallRunCheck _wallRunCheck;
    [SerializeField] private Grapple _grapple;
    [SerializeField] private Attack _attack;
    [SerializeField] private Avoid _avoid;
    [SerializeField] private SetUp _setUp;
    [SerializeField] private PlayerEffectControl _effectControl;
    [SerializeField] private AimAssist _assist;

    private CinemachineBrain _CameraBrain;
    private CinemachineVirtualCamera _camera;
    private CinemachineVirtualCamera _cameraGrapple;

    private SpringJoint _joint;

    public Transform ModelTop => _modelTop;
    public Transform ModelDown => _modelDown;
    public PointZip PointZip => _pointZip;
    public ControllerVibrationManager ControllerVibrationManager => _controllerVibrationManager;

    public CameraControl CameraControl => _cameraControl;
    public CinemachineBrain CameraBrain => _CameraBrain;
    public InputManager InputManager => _inputManager;

    public PlayerAudioManager PlayerAudioManager => _playerAudioManager;
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
    public Zip ZipMove => _zipMove;
    public WallRun WallRun => _wallRun;
    public Grapple Grapple => _grapple;
    public Attack Attack => _attack;
    public WallRunCheck WallRunCheck => _wallRunCheck;
    public Avoid Avoid => _avoid;
    public PlayerEffectControl EffectControl => _effectControl;
    public WallRunUpZip WallRunUpZip => _wallRunUpZip;
    public WallRunStepCheck WallRunStep => _wallRunStep;
    public AimAssist AimAssist => _assist;
    public SetUp SetUp => _setUp;
    public ZipLineRenderer ZipLineRenderer => _zipLineRenderer;

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
        _wallRunUpZip.Init(this);
        _wallRunStep.Init(this);
        _assist.Init(this);
        _zipLineRenderer.Init(this);
        _pointZip.Init(this);
    }

    void Start()
    {

    }

    private void OnDrawGizmosSelected()
    {
        _groundCheck.OnDrawGizmos(PlayerT);

        _searchSwingPoint.OnDrawGizmos(PlayerT);

        _wallRunCheck.OnDrawGizmos(PlayerT);

        _wallRunUpZip.OnDrawGizmos(PlayerT);
        _pointZip.PointZipSearch.OnDrawGizmos(PlayerT);
    }
    private void Update()
    {
        _stateMachine.Update();
        _velocityLimit.LimitSpeed();

        _animControl.AnimSet();

        _playerAudioManager.LoopAudio.SettingLoopAudio();
        _effectControl.ConcentrationLineEffect();
        _assist.Targetting();
        _assist.AssistUISetting();
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
