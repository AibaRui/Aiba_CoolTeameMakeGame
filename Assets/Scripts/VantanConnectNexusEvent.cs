using GameLoopTest;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using VTNConnect;

public class VantanConnectNexusEvent : MonoBehaviour, IVantanConnectEventReceiver
{

    public bool IsActive => true;

    void Awake()
    {
        //イベントを受け取るためには登録が必要
        VantanConnect.RegisterEventReceiver(this);
    }



    public void OnEventCall(EventData data)
    {
        switch (data.EventCode)
        {
            //おうえんイベント
            case EventDefine.Cheer:
                {
                    CheerEvent();
                }
                break;


            case EventDefine.BonusCoin:
                {
                    BonusCoin();
                }
                break;

            case EventDefine.Levelup:
                {
                    LevelUp();
                }
                break;

            case EventDefine.GetArtifact:
                {
                    GetArtifact();
                }
                break;

            //case EventDefine.Ga:
            //    {
            //        GameStart();
            //    }
            //    break;

            //case EventDefine.GameEnd:
            //    {
            //        GameEnd();
            //    }
            //    break;

       
        }
    }



    /// <summary>アクターにランダム効果</summary>
    /// 条件_プレイヤーがゲートに落ちた(ゲーム終了ムービー時)
    public void ActorEffect()
    {
        //イベントを送信する
        //ActorEffect = 105,  //アクターにランダム効果 (すべてのゲーム -> Confront、GameOnly)
        EventData data = new EventData(EventDefine.ActorEffect);
        VantanConnect.SendEvent(data);
    }

    /// <summary>送信_部屋が暗くなる</summary>
    /// 条件_ボスステージに移行したとき
    public void DarkRoom()
    {
        //イベントを送信する
        //DarkRoom = 107,  //照明の光度が一定時間低下する。 (すべてのゲーム -> Confront、GameOnly)
        EventData data = new EventData(EventDefine.DarkRoom);
        VantanConnect.SendEvent(data);
    }

    /// <summary>送信_敵を召喚</summary>
    /// 条件_ボスが登場した時
    public void SummonEnemy()
    {
        //イベントを送信する
        // SummonEnemy = 109,  //プレイヤーの頭上から雑魚敵が降ってくる (すべてのゲーム -> Confront、GameOnly)
        EventData data = new EventData(EventDefine.SummonEnemy);
        VantanConnect.SendEvent(data);
    }

    /// <summary>送信_窓をたたく音</summary>
    /// 条件_プレイヤーがダメージを受けたとき
    public void KnockWindow()
    {
        //イベントを送信する
        //  KnockWindow = 116,  //窓をたたく音を出す (すべてのゲーム -> ToyBox、GameOnly)
        EventData data = new EventData(EventDefine.KnockWindow);
        VantanConnect.SendEvent(data);
    }

    /// <summary>受信_敵が逃げた</summary>
    /// 条件_ボスが倒されたとき
    public void EnemyEscape()
    {

    }

    /// <summary>受信_応援</summary>
    public void CheerEvent()
    {

    }


    /// <summary>コインゲット</summary>
    public void BonusCoin()
    {

    }


    /// <summary>受信_レベルアップ</summary>
    public void LevelUp()
    {

    }

    /// <summary>受信_レアアイテムゲット</summary>
    public void GetArtifact()
    {

    }

    /// <summary>受信_冒険が始まった</summary>
    public void GameStart()
    {

    }

    /// <summary>受信_冒険が終わった</summary>
    public void GameEnd()
    {

    }

}
