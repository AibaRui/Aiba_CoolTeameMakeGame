using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using VTNConnect;

public class GameEnd : MonoBehaviour
{
    [SerializeField] private VantanConnectNexusEvent _event;

    public void GameEndProcess()
    {
        _event.EnemyEscape();
        _event.ActorEffect();

        //ƒQ[ƒ€I—¹
        VantanConnect.GameEnd(false, (VC_StatusCode status) =>
        {
            SceneManager.LoadScene("Title");
        });
    }
}
