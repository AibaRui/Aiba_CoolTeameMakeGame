using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerControl : MonoBehaviour, IDamageble, IReplaceble
{
    // [Header("ムービー再生中かどうか")]
    //  [SerializeField] private bool _isMovie = false;

    [Header("ボス戦かどうか")]
    [SerializeField] private bool _isBossButtle = false;

    [SerializeField] private Animator _anim;

    [SerializeField] private Transform _playerT;

    [SerializeField] private Transform _modelT;

    [SerializeField] private Transform _hands;

    [SerializeField] private Transform _modelTop;
    [SerializeField] private Transform _modelDown;

    [SerializeField] private LineRenderer _lineRenderer;

    [Header("当たり判定")]
    [SerializeField] private CapsuleCollider _collider;


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

    [Header("回転設定")]
    [SerializeField] private PlayerModelRotation _playerModelRotation;
    [Header("=======Swing_========")]
    [SerializeField] private Swing _swing;
    [Header("=======Swing_Point探し========")]
    [SerializeField] private SearchSwingPoint _searchSwingPoint;
    [Header("=======Zip_動きの設定========")]
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
    [SerializeField] private PlayerDamage _damage;

    [Header("マテリアル変更")]
    [SerializeField] private PlayerMaterial _materialChange;


    [SerializeField] private PlayerBossHit _bossHit;
    [SerializeField] private SpecialHitStop _specialHitStop;
    [SerializeField] private PlayerStartMovieAndTutorial _tutorial;

    [SerializeField] private PlayerPostEffectSetting _playerPostEffectSetting;
    [SerializeField] private PlayerReplace _playerReplace;
    [SerializeField] private PlayerBossMovie _bossMovie;





    [SerializeField] private CinemachineBrain _CameraBrain;
    [SerializeField] private CinemachineVirtualCamera _cameraGrapple;

    private CinemachineVirtualCamera _camera;
    private SpringJoint _joint;


    /// <summary>ボス戦のイベントタイプ</summary>
    private PlayerBossEventType _eventType = PlayerBossEventType.BossMovie;


    public bool IsBossButtle => _isBossButtle;
    public CapsuleCollider PlayerCollider => _collider;
    public PlayerBossMovie BossMovie => _bossMovie;
    public PlayerBossHit PlayerBossHit => _bossHit;
    public PlayerReplace PlayerReplace => _playerReplace;
    public PlayerPostEffectSetting PlayerPostEffectSetting => _playerPostEffectSetting;
    public PlayerBossEventType EventType => _eventType;
    public SpecialHitStop SpecialHitStop => _specialHitStop;
    public PlayerStartMovieAndTutorial Tutorial => _tutorial;
    public PlayerMaterial PlayerMaterial => _materialChange;
    public PlayerModelRotation PlayerModelRotation => _playerModelRotation;
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
    public PlayerDamage PlayerDamage => _damage;
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
        _playerModelRotation.Init(this);
        _damage.Init(this);
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

    public void Damage(DamageType type)
    {
        _damage.Damage(type);
    }

    public void EnterReplaceZone(ReplceType replceType)
    {
        _playerReplace.EnterReplaceZone(replceType);
        SetEventType(PlayerBossEventType.BossStage_Replace);
    }

    public void ExitReplaceZone(ReplceType replceType)
    {
        _playerReplace.ExitReplaceZone();
    }

    public void SetEventType(PlayerBossEventType eventType)
    {
        _eventType = eventType;
    }

}

public enum PlayerBossEventType
{
    /// <summary>ボス登場ムービー再生</summary>
    BossMovie,

    /// <summary>Boss戦時に、一定の高さに戻る</summary>
    BossStage_Replace,

    /// <summary>ボスの攻撃に辺り</summary>
    BossStage_HitBoss


}