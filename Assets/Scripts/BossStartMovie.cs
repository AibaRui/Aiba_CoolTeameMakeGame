using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class BossStartMovie : MonoBehaviour
{
    [SerializeField] private PlayerControl _playerControl;
    [SerializeField] private BossControl _bossControl;
    [SerializeField] private PlayerBossMovie _bossMovie;

    [Header("ボス登場ムービー")]
    [SerializeField] private PlayableDirector _movie;

    [Header("構えのボタン画像")]
    [SerializeField] private GameObject _setUpImage;


    [Header("構えながらのボタン画像")]
    [SerializeField] private GameObject _setUpToImage;
    [Header("攻撃のボタン画像")]
    [SerializeField] private GameObject _attackImage;



    [Header("演出用のレイヤー")]
    [SerializeField] private LayerMask _setLayerMask;
    [Header("元のレイヤー")]
    [SerializeField] private LayerMask _defultLayerMask;

    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioSource _mainBGM;

    private bool _isStop = false;

    private bool _isFirst = false;

    private bool _isAllInput = false;

    private bool _isPushSetUpButtun = false;

    public void OnGray()
    {
        Camera.main.cullingMask = _setLayerMask;
        _bossControl.MaterialChange.ChangeBossMaterial(ModelMaterialType.GrayScal);
    }

    public void OffGray()
    {
        Camera.main.cullingMask = _defultLayerMask;
        _bossControl.MaterialChange.ChangeBossMaterial(ModelMaterialType.Nomal);
    }

    public void BGMStop()
    {
        _audioSource.Stop();
    }

    public void BGMStart()
    {
        _mainBGM.Play();
    }

    public void MovieStop()
    {
        Time.timeScale = 0;
        if (!_isFirst)
        {
            _isFirst = true;
            _setUpImage.SetActive(true);
        }
        else
        {
            _isStop = true;
            _setUpToImage.SetActive(true);
        }
    }

    private void Update()
    {
        if (_bossMovie.IsPlayMovie)
        {
            if (_isAllInput) return;

            float rightTrigger = Input.GetAxisRaw("RightTrigger");
            float leftTrigger = Input.GetAxisRaw("LeftTrigger");

            if (_isFirst && !_isPushSetUpButtun)
            {
                if (leftTrigger > 0)
                {
                    Time.timeScale = 1;
                    _isPushSetUpButtun = true;
                    _setUpImage.SetActive(false);
                }
            }
            else if (_isStop)
            {
                if (leftTrigger > 0)
                {
                    _setUpToImage.SetActive(false);
                    _attackImage.SetActive(true);
                    if (rightTrigger > 0)
                    {
                        _isAllInput = true;
                        Time.timeScale = 1;
                        _attackImage.SetActive(false);
                    }
                }
                else
                {
                    _setUpToImage.SetActive(true);
                    _attackImage.SetActive(false);
                }

            }

        }
    }

}
