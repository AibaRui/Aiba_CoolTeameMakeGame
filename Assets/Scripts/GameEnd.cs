using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using VTNConnect;

public class GameEnd : MonoBehaviour
{
    [SerializeField] private VantanConnectNexusEvent _event;

    public void GameEndProcess()
    {
        _event.EnemyEscape();
        _event.ActorEffect();
        GameEpisode epic = VantanConnect.CreateEpisode(EpisodeCode.SGEnemyLeave);
        epic.SetEpisode("異世界に繋がるゲートに入ってしまった"); // エピソードを設定する
        epic.DataPack("異世界に繋がるゲートに入ってしまった", this.transform.position); // エピソードの補足を設定する
        VantanConnect.SendEpisode(epic);
        //ゲーム終了
        VantanConnect.GameEnd(false, (VC_StatusCode status) =>
        {
            SceneManager.LoadScene("Title");
        });
    }
}
