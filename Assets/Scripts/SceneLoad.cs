using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class SceneLoad : MonoBehaviour
{
    [Header("�^�C�g����")]
    [SerializeField] private string _tgameSceneName = "Game";

    public void GameScene()
    {
        SceneManager.LoadScene(_tgameSceneName);
    }

    private void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            GameScene();
        }
    }

}