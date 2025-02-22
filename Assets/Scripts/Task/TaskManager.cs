using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TaskManager : MonoBehaviour
{
    [Header("巡る箇所")]
    [SerializeField] private List<RootTask> _root4Tower = new List<RootTask>();

    [Header("最後の集まる場所")]
    [SerializeField] private GameObject _lastRoot;

    [Header("塔の上を見るムービー")]
    [SerializeField] private PlayableDirector _playableDirector;

    [Header("塔の上を見るムービー")]
    [SerializeField] private PlayableDirector _bossShowMovie;

    [Header("セリフ")]
    [SerializeField] private List<GameObject> _serihu = new List<GameObject>();

    [SerializeField] private PlayerControl _player;
    [SerializeField] private BGMControl _bgmControl;

    [SerializeField] private Transform _position;

    private bool _isEndAlltask = false;

    private int _rootTaskCount = 0;



    public void TaskStart()
    {
        _root4Tower[_rootTaskCount].gameObject.SetActive(true);
    }

    public void EndMovie()
    {
        _lastRoot.gameObject.SetActive(true);
        _player.EndEvent();
        _player.transform.position = _position.position;
    }

    /// <summary>
    /// 最後の集合ポイントに来た時の処理
    /// </summary>
    public void SetLastRoot()
    {
        _playableDirector.Play();
        _player.StartEvent();
    }

    /// <summary>
    /// 巡った場所を計測
    /// </summary>
    public void EndRootTask()
    {
        if (_isEndAlltask)
        {
            _bgmControl.EndBGM();
            _bossShowMovie.Play();
        }

        if (_rootTaskCount < 3)
        {
            if (_rootTaskCount < _serihu.Count)
            {
                _serihu[_rootTaskCount].SetActive(true);
            }
        }

        if (_rootTaskCount < _root4Tower.Count)
        {
            _root4Tower[_rootTaskCount].gameObject.SetActive(false);
        }


        _rootTaskCount++;

        if (_rootTaskCount >= _root4Tower.Count)
        {
            SetLastRoot();
            _isEndAlltask = true;
        }
        else
        {
            _root4Tower[_rootTaskCount].gameObject.SetActive(true);
        }
    }

}
