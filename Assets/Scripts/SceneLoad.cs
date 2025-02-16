using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class SceneLoad : MonoBehaviour
{
    [Header("ÉVÅ[Éìñº")]
    [SerializeField] private string _tgameSceneName = "";

    public void LodeScene()
    {
        SceneManager.LoadScene(_tgameSceneName);
    }

    private void Update()
    {

    }

}
