using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneControl : MonoBehaviour
{
    [Header("タイトルのSceneの名前")]
    [SerializeField] private string _titleSceneName;


    public void GoTitleScene()
    {
        SceneManager.LoadScene(_titleSceneName);
    }
}
