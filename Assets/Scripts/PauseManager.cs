using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    [Header("Pause���")]
    [SerializeField] private GameObject _pausePanel;

    [Header("�^�C�g����")]
    [SerializeField] private string _titleName = "Title";

    [Header("�Q�[���V�[����")]
    [SerializeField] private string _gameSceneName = "Game";


    private bool _isPause = false;


    void Start()
    {

    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _isPause = !_isPause;

            if (_isPause)
            {
                Time.timeScale = 0f;
            }
            else
            {
                Time.timeScale = 1f;
            }
            _pausePanel.SetActive(_isPause);
        }
    }

    public void GameScene()
    {
        SceneManager.LoadScene(_gameSceneName);
    }

    public void Title()
    {
        SceneManager.LoadScene(_titleName);
    }



}