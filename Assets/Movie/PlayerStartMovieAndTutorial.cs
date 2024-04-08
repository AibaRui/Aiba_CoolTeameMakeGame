using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Playables;

public class PlayerStartMovieAndTutorial : MonoBehaviour, IEnterTutorialGoalble
{
    [SerializeField] private PlayerControl _playerControl;

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
    public bool IsEndTutorial => _isEndTutorial;

    void Start()
    {
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
            }
            else
            {
                StartMovieEnterStartPosition();
                _isEndTutorial = true;
            }
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
        }
    }


    /// <summary>初回のムービー。プレイヤーを登場させる </summary>
    public void StartMoviePlayerActive()
    {

        transform.position = _startPos.position;

        _playerControl.Rb.isKinematic = false;
        _isStart = true;


    }

    public void ChengeCamera()
    {
        _movieCamera.Priority = 0;
        _brain.m_BlendUpdateMethod = CinemachineBrain.BrainUpdateMethod.FixedUpdate;
        _brain.m_UpdateMethod = CinemachineBrain.UpdateMethod.FixedUpdate;
    }

    public void StartMovieEnterStartPosition()
    {
        transform.position = _startPos.position;
        //チュートリアル用のアニメーション
        _playerControl.Anim.SetBool("IsTutorial", false);

        _playerControl.Rb.velocity = new Vector3(-3, -10, -1);

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
        {
            _isFirstTimeScaleDown = true;

            //アニメーション速度の変更
            _playerControl.Anim.speed = 0.5f;
            //SwingのチュートリアルUIを表示
            _swingTutorialUI.gameObject.SetActive(true);
        }

        //さらに1秒経ったら、操作可能にする
        if (_countEndMovieTimeToSwingTutorial > _showUITime && !_isEndSwingTutorial && !_playerControl.InputManager.IsCanInput)
        {

            _isEnd = true;

            //入力を可能にする
            _playerControl.InputManager.IsCanInput = true;

            //チュートリアル用のアニメーション
            _playerControl.Anim.SetBool("IsTutorial", false);

            _playerControl.Anim.speed = 0.2f;
        }

        //スロー中の速度設定
        if (_isFirstTimeScaleDown && !_isInoutSwingButtun)
        {
            float t = _countEndMovieTimeToSwingTutorial - 1 / 1;
            float d = Mathf.Lerp(_playerControl.Rb.velocity.y, -3, t);

            _playerControl.Rb.velocity = new Vector3(_playerControl.Rb.velocity.x, d, _playerControl.Rb.velocity.z);
        }


        if (!_isInoutSwingButtun)
        {
            if (_playerControl.InputManager.IsSwing == 1)
            {
                _isInoutSwingButtun = true;
                _isEndTutorial = true;
                _playerControl.Anim.speed = 1f;

                Vector3 dir = transform.forward;
                _playerControl.Rb.velocity = new Vector3(dir.x * 20, -20, dir.z * 20);

                //SwingのチュートリアルUIを表示
                // _swingTutorialUI.SetBool("On", false);
                _swingTutorialUI.gameObject.SetActive(false);
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

                if (_playerControl.transform.position.y < _stopSwingY && !_isStopSwingY)
                {
                    _isStopSwingY = true;
                    _saveStopSwingPos = _playerControl.transform.position;
                    _playerControl.Rb.velocity = Vector3.zero;
                    _playerControl.Rb.isKinematic = true;
                    _playerControl.Anim.speed = 0;
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



}

public enum TutorialType
{
    Swing,
    Move,
}