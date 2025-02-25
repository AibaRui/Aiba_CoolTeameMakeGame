using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Playables;

public class PlayerStartMovieAndTutorial : MonoBehaviour, IEnterTutorialGoalble
{
    [SerializeField] private PlayerControl _playerControl;
    [SerializeField] private CameraControl _cameraControl;

    [SerializeField] private PlayableDirector _movie;

    [Header("チュートリアルをするかどうか")]
    [SerializeField] private bool _isTutorial = true;

    [Header("ムービーを流すかどうか")]
    [SerializeField] private bool _isPlayMovie = false;

    [Header("初回のムービーのカメラ")]
    [SerializeField] private CinemachineVirtualCamera _movieCamera;

    [SerializeField] private CinemachineBrain _brain;

    [Header("ムービー移動開始位置")]
    [SerializeField] private Transform _startPos;

    [Header("着地防止の高さ")]
    [SerializeField] private float _stopSwingY = 20;

    [Header("スウィングチュートリアルのUI")]
    [SerializeField] private Animator _swingTutorialUI;

    [Header("スウィング前、開始のUI")]
    [SerializeField] private GameObject _swingStartUI;

    [Header("スウィング中、中断のUI")]
    [SerializeField] private GameObject _swingEndUI;

    [Header("ムービー終了からスウィングチュートリアルのUIを出すまでの時間")]
    [SerializeField] private float _showUITime = 2f;

    [Header("移動、カメラのチュートリアルUI")]
    [SerializeField] private GameObject _moveCameraTutorialUI;


    [SerializeField] private GameObject _opeParentUI;
    [SerializeField] private GameObject _swingPress;
    [SerializeField] private GameObject _swingRerese;
    [SerializeField] private GameObject _zipInfo;
    [SerializeField] private GameObject _groundInfo;


    [SerializeField] private OperationInfoUI _operationInfoUI;
    [SerializeField] private TaskManager _taskManager;


    private bool _isFirstTimeScaleDown = false;
    private float _countEndMovieTimeToSwingTutorial = 0;

    private bool _isInoutSwingButtun = false;
    /// <summary>Swingのチュートリアルが終わったかどうか</summary>
    private bool _isEndSwingTutorial = false;
    private bool _isEnterSwingTutorialZone = false;

    private bool _isEndZupTutorial = false;
    private bool _isEnterZipTutorialZone = false;

    /// <summary></summary>
    public bool IsEndSwingTutorial => _isEndSwingTutorial;

    public bool IsEndMovie => _isEnd;

    private bool _isStart = false;

    private bool _isEnd = false;

    /// <summary>Swingのチュートリアル中、地面に近くなっているかどうか</summary>
    private bool _isStopSwingY = false;

    private Vector3 _saveStopSwingPos;

    private bool _isEndTutorial = false;

    private bool _isChangeZip = false;

    private bool _isCanUI = false;


    public bool IsEndTutorial => _isEndTutorial;

    void Start()
    {
        if (_playerControl.IsBossButtle)
        {
            _isEndTutorial = true;
            _isEnd = true;

            _isEndSwingTutorial = true;


            _playerControl.InputManager.IsCanInput = true;

            //チュートリアル用のアニメーション
            _playerControl.Anim.SetBool("IsTutorial", false);
            return;
        }


        if (_isTutorial)
        {
            _movie.Play();
            //チュートリアル用のアニメーション
            _playerControl.Anim.SetBool("IsTutorial", true);

            if (_isPlayMovie)
            {
                //プレイヤーが動かないようにする
                _playerControl.Rb.isKinematic = true;
                _playerControl.InputManager.IsCanInput = false;
                IsCanUI(false);
            }
            else
            {
                StartMovieEnterStartPosition();
                _isEndTutorial = true;
            }

            _cameraControl.IsTutorial = true;
        }
        else
        {
            _isEndTutorial = true;
            _isEnd = true;

            _isEndSwingTutorial = true;

            _playerControl.InputManager.IsCanInput = true;

            //チュートリアル用のアニメーション
            _playerControl.Anim.SetBool("IsTutorial", false);
            _brain.m_BlendUpdateMethod = CinemachineBrain.BrainUpdateMethod.FixedUpdate;
            _brain.m_UpdateMethod = CinemachineBrain.UpdateMethod.FixedUpdate;

            //タスク開始
            _taskManager.TaskStart();

            //操作説明のUIを表示
            _operationInfoUI.UISetOnOff(true);

            IsCanUI(true);
        }
    }


    /// <summary>初回のムービー。プレイヤーを登場させる </summary>
    public void StartMoviePlayerActive()
    {
        transform.position = _startPos.position;
        _playerControl.gameObject.transform.rotation = Quaternion.Euler(0, 270, 0);

        _playerControl.Rb.isKinematic = false;
        _isStart = true;
    }

    public void ChengeCamera()
    {
        _movieCamera.Priority = 0;
        _brain.m_BlendUpdateMethod = CinemachineBrain.BrainUpdateMethod.FixedUpdate;
        _brain.m_UpdateMethod = CinemachineBrain.UpdateMethod.FixedUpdate;
    }

    /// <summary>ムービー再生終了後、プレイヤーを開始地点に移動させる</summary>
    public void StartMovieEnterStartPosition()
    {
        //操作説明のUIを表示
        _operationInfoUI.UISetOnOff(true);
        IsCanUI(true);

        transform.position = _startPos.position;
        //チュートリアル用のアニメーション
        _playerControl.Anim.SetBool("IsTutorial", false);

        _playerControl.Rb.velocity = new Vector3(-20, -10, -10);

        _isEnd = true;

        _brain.m_BlendUpdateMethod = CinemachineBrain.BrainUpdateMethod.FixedUpdate;
        _brain.m_UpdateMethod = CinemachineBrain.UpdateMethod.FixedUpdate;
    }

    /// <summary>ムービーから登場してスウィングのチュートリアルを出すまでの時間を計測</summary>
    public void CountEndMovieToSwingTutorial()
    {
        if (!_isTutorial || !_isStart) return;


        _countEndMovieTimeToSwingTutorial += Time.deltaTime;

        //登場から1秒経ったら、操作説明のUIを表示する
        if (_countEndMovieTimeToSwingTutorial > 1 && !_isFirstTimeScaleDown)
        {        //操作説明のUIを表示
            _operationInfoUI.UISetOnOff(true);
            IsCanUI(true);
            _isFirstTimeScaleDown = true;

            //アニメーション速度の変更
            _playerControl.Anim.speed = 0.5f;
            //SwingのチュートリアルUIを表示
            _swingTutorialUI.gameObject.SetActive(true);
        }

        //スロー中の速度設定
        if (_isFirstTimeScaleDown && !_isInoutSwingButtun)
        {
            float t = _countEndMovieTimeToSwingTutorial - 1 / 1;
            float d = Mathf.Lerp(_playerControl.Rb.velocity.y, -3, t);

            _playerControl.Rb.velocity = new Vector3(_playerControl.Rb.velocity.x, d, _playerControl.Rb.velocity.z);
        }


        //さらに1秒経ったら、操作可能にする
        if (_countEndMovieTimeToSwingTutorial > _showUITime && !_isEndSwingTutorial && !_playerControl.InputManager.IsCanInput)
        {
            _playerControl.Rb.isKinematic = true;
            _playerControl.Rb.velocity = Vector3.zero;
            _isEnd = true;

            //入力を可能にする
            _playerControl.InputManager.IsCanInput = true;

            //チュートリアル用のアニメーション
            _playerControl.Anim.SetBool("IsTutorial", false);

            _playerControl.Anim.speed = 0f;
        }



        //初めて、スウィングのボタンを押した際の処理
        if (!_isInoutSwingButtun)
        {
            if (_playerControl.InputManager.IsSwing == 1)
            {
                //カメラを動かせるようにする
                _cameraControl.IsTutorial = false;

                _isInoutSwingButtun = true;
                _isEndTutorial = true;
                _isEndSwingTutorial = true;
                _playerControl.Anim.speed = 1f;

                _playerControl.Rb.isKinematic = false;
                Vector3 dir = transform.forward;
                _playerControl.Rb.velocity = new Vector3(dir.x * 20, -10, dir.z * 20);

                //SwingのチュートリアルUIを表示
                // _swingTutorialUI.SetBool("On", false);
                _swingTutorialUI.gameObject.SetActive(false);

                //タスク開始
                _taskManager.TaskStart();
            }
        }
        else
        {
            if (_playerControl.InputManager.IsSwing == 1)
            {
                _swingStartUI.SetActive(false);

                if (!_swingEndUI.activeSelf)
                {
                    if (_playerControl.Rb.velocity.y > 10)
                    {
                        _swingEndUI.SetActive(true);
                    }
                }

                if (_isStopSwingY)
                {
                    _isStopSwingY = false;
                    _playerControl.Anim.speed = 1;
                    _playerControl.Rb.isKinematic = false;
                    Vector3 dir = transform.forward;
                    _playerControl.Rb.velocity = new Vector3(dir.x * 20, -20, dir.z * 20);
                }
            }
            else
            {
                _swingEndUI.SetActive(false);


                if (_isEnterSwingTutorialZone && _playerControl.Rb.velocity.y < 0)
                {
                    EndSwingTutorial();
                    return;
                }

                if (!_swingStartUI.activeSelf)
                {
                    if (_playerControl.Rb.velocity.y < -10)
                    {
                        _swingStartUI.SetActive(true);
                    }
                }
            }

        }
    }

    private void EndSwingTutorial()
    {
        _isEndSwingTutorial = true;
        _playerControl.Anim.speed = 1f;
        _playerControl.Rb.isKinematic = false;
        _swingEndUI.SetActive(false);
        _swingStartUI.SetActive(false);
        _swingTutorialUI.gameObject.SetActive(false);
    }


    public void StartMove()
    {

    }


    void Update()
    {
        if (!_isEndSwingTutorial)
        {
            CountEndMovieToSwingTutorial();
        }
    }




    /// <summary>チュートリアルのゴールに入ったとき</summary>
    public void EnterZone(TutorialType type)
    {
        if (type == TutorialType.Swing)
        {
            _isEnterSwingTutorialZone = true;

            //入力を可能にする
            _playerControl.InputManager.IsCanInput = false;
        }
    }

    public void EndFirstTask()
    {
        _isChangeZip = true;
    }

    public void IsCanUI(bool isOn)
    {
        _isCanUI = isOn;
        _opeParentUI.gameObject.SetActive(isOn);
        if (isOn == false)
        {
            _swingPress.SetActive(false);
            _swingRerese.SetActive(false);
            _zipInfo.SetActive(false);
            _groundInfo.SetActive(false);
        }

    }

    /// <summary>Swingについて説明する_押す</summary>
    public void ShowSwingInfoPress(bool isOn)
    {
        if (_playerControl.IsBossButtle) return;

        if (isOn == true)
        {
            if (_isCanUI == false || _isChangeZip) return;
            _swingPress.SetActive(true);
        }
        else
        {
            _swingPress.SetActive(false);
        }


        _swingPress.SetActive(isOn);
    }

    /// <summary>Swingについて説明する_離す</summary>
    public void ShowSwingInfoRelese(bool isOn)
    {
        if (_playerControl.IsBossButtle) return;

        if (isOn == true)
        {
            if (_isCanUI == false || _isChangeZip) return;
            _swingRerese.SetActive(true);
        }
        else
        {
            _swingRerese.SetActive(false);
        }
    }

    public void ZipInfo(bool isOn)
    {
        if (_playerControl.IsBossButtle) return;

        if (isOn)
        {
            if (_isCanUI == false || !_playerControl.ZipMove.IsCanZip || !_isChangeZip) return;
            _zipInfo.SetActive(true);
        }
        else
        {
            _zipInfo.SetActive(false);
        }
    }


    /// <summary>地面についたら</summary>
    public void GroundJumpInfo(bool isOn)
    {
        if (_playerControl.IsBossButtle) return;

        if (isOn)
        {
            if (_isCanUI == false) return;
            _groundInfo.SetActive(true);
        }
        else
        {
            _groundInfo.SetActive(false);
        }
    }

}

public enum TutorialType
{
    Swing,
    Move,
}