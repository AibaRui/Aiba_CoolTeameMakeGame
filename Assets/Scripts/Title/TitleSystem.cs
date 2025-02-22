using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using VTNConnect;

public class TitleSystem : MonoBehaviour
{
    [Header("タイトル名")]
    [SerializeField] private string _tgameSceneName = "Game";

    [Header("Movie")]
    [SerializeField] private PlayableDirector _movie;

    [Header("Skipまでの時間")]
    [SerializeField] private float _skipTime = 3;

    private float _time;

    private bool _isMovieStart = false;


    private void Start()
    {
        //タイトルに戻ってきたとき、過去の情報がリセットされる仕組み
        VantanConnect.SystemReset();
    }

    private void Update()
    {
        MovieSkip();
    }

    public void MovieSkip()
    {
        if (!_isMovieStart) return;

        if (Input.GetButton("Jump"))
        {
            _time += Time.deltaTime;

            if (_time > _skipTime)
            {
                GameStart();
            }
        }

        if (Input.GetButtonUp("Jump"))
        {
            _time = 0;
        }

    }

    public void GameStart()
    {
        //ゲームスタート時に通信する
        VantanConnect.GameStart((VC_StatusCode code) =>
        {
            SceneManager.LoadScene(_tgameSceneName);
            _time = 0;
        });
    }

    /// <summary>ムービーを再生</summary>
    public void MovieStart()
    {
        Debug.Log("P");
        _movie.Play();
        _isMovieStart = true;
    }
}
