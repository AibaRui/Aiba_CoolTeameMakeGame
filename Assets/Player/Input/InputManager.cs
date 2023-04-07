using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    /// <summary>キーによる方向</summary>
    private Vector3 inputVector;
    public Vector3 InputVector => inputVector;

    [Tooltip("スペースを押す")]
    private bool _isJumping;
    public bool IsJumping { get => _isJumping; set => _isJumping = value; }

    [Tooltip("左クリック_押す")]
    private bool _isLeftMouseClickDown = false;
    public bool IsLeftMouseClickDown { get => _isLeftMouseClickDown; }

    [Tooltip("左クリック_離す")]
    private bool _isLeftMouseClickUp = false;
    public bool IsLeftMouseClickUp { get => _isLeftMouseClickUp; }

    [Tooltip("右クリック_押す")]
    private bool _isRightMouseClickDown = false;
    public bool IsRightMouseClickDown { get => _isRightMouseClickDown; }

    [Tooltip("左Ctrl_押す")]
    private bool _isCtrlDown;
    public bool IsCtrlDown { get => _isCtrlDown; }

    [Tooltip("左Ctrl_離す")]
    private bool _isCtrlUp;
    public bool IsCtrlUp { get => _isCtrlUp; }

    [Tooltip("攻撃")]
    private bool _isAttack;
    public bool IsAttack { get => _isAttack; }

    [Tooltip("回避")]
    private bool _isAvoid;
    public bool IsAvoid { get => _isAvoid; }

    [Tooltip("構え")]
    private float _isSetUp;
    public float IsSetUp { get => _isSetUp; }

    private float _isMouseScrol = 0;

    public float IsMouseScrol => _isMouseScrol;


    private float _isSwing;

    public float IsSwing => _isSwing;


    [Tooltip("Tab_押す")]
    private bool _isTabDown;
    public bool IsTabDown => _isTabDown;

    [Tooltip("左Shift_押す")]
    private bool _isLeftShiftDown = false;
    public bool IsLeftShiftDown => _isLeftShiftDown;

    private bool _isLeftShift = false;
    public bool IsLeftShift => _isLeftShift;

    [Tooltip("左Shift_離す")]
    private bool _isLeftShiftUp = false;
    public bool IsLeftShiftUp => _isLeftShiftUp;

    private float _horizontalInput;
    public float HorizontalInput { get => _horizontalInput; }

    private float _verticalInput;

    public float VerticalInput { get => _verticalInput; }

    private float yInput;

    public void HandleInput()
    {
        _horizontalInput = 0;
        _verticalInput = 0;


        _horizontalInput = Input.GetAxisRaw("Horizontal");
        _verticalInput = Input.GetAxisRaw("Vertical");

        //マウスの左クリック
        _isLeftMouseClickDown = Input.GetMouseButtonDown(0);
        _isLeftMouseClickUp = Input.GetMouseButtonUp(0);

        //マウス右クリック
        _isRightMouseClickDown = Input.GetMouseButtonDown(1);

        //攻撃
        _isAttack = Input.GetButtonDown("Fire3");

        _isAvoid = Input.GetButtonDown("Avoid");

        _isSetUp = Input.GetAxisRaw("SetUp");

        //Ctrlを押したか
        _isCtrlDown = Input.GetKeyDown(KeyCode.LeftControl);
        //Ctrlを離したか
        _isCtrlUp = Input.GetKeyUp(KeyCode.LeftControl);

        //Space
        _isJumping = Input.GetButtonDown("Jump");

        //Tab
        _isTabDown = Input.GetKeyDown(KeyCode.Tab);

        // Shift
        _isLeftShiftDown = Input.GetKeyDown(KeyCode.LeftShift);
        _isLeftShiftUp = Input.GetKeyUp(KeyCode.LeftShift);
        _isLeftShift = Input.GetKey(KeyCode.LeftShift);

        _isSwing = Input.GetAxisRaw("Swing");

        _isMouseScrol = Input.GetAxis("Mouse ScrollWheel");
    }

    private void Update()
    {
        HandleInput();
    }
}
