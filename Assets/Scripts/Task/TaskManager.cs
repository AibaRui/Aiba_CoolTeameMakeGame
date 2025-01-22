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

    [Header("セリフ")]
    [SerializeField] private List<GameObject> _serihu = new List<GameObject>();


    private int _rootTaskCount = 0;


    public void EndMovie()
    {
        _lastRoot.gameObject.SetActive(true);
    }

    /// <summary>
    /// 最後の集合ポイント
    /// </summary>
    public void SetLastRoot()
    {
        _playableDirector.Play();
    }

    /// <summary>
    /// 巡った場所を計測
    /// </summary>
    public void EndRootTask()
    {
        if (_rootTaskCount < 3)
        {
            _serihu[_rootTaskCount].SetActive(true);
        }
        _rootTaskCount++;

        if (_rootTaskCount == _root4Tower.Count)
        {
            SetLastRoot();
        }
    }

}
