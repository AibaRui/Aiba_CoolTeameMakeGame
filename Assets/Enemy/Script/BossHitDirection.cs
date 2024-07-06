using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Playables;

public class BossHitDirection : MonoBehaviour
{
    private BossControl _bossControl;
    private PlayerControl _playerControl;

    [Header("Hit演出の情報")]
    [SerializeField] private List<BossHitDirectionNoamlHitInfo> _hitInfo;

    [Header("Breake演出の情報")]
    [SerializeField] private List<BossHitDirectionBreakeInfo> _brekakeInfo;

    [Header("元のレイヤー")]
    [SerializeField] private LayerMask _defultLayerMask;

    [SerializeField] private GameObject _boss2;

    private void Awake()
    {
        _bossControl = FindObjectOfType<BossControl>();
        _playerControl = FindObjectOfType<PlayerControl>();
    }

    public void BossDamageStartDirection(BossDirectionType type, bool isRandam, int num)
    {
        //対応した、UIのTimeLineを流す
        if (type == BossDirectionType.Hit)
        {
            int i = 0;
            if (isRandam || num >= _hitInfo.Count)
            {
                i = Random.Range(0, _hitInfo.Count);
            }
            else
            {
                i = num;
            }

            //マテリアル設定
            _playerControl.PlayerMaterial.ChangePlayerMaterial(_hitInfo[i].PlayerMaterialType);
            _bossControl.MaterialChange.ChangeBossMaterial(_hitInfo[i].BossMaterialType);

            //レイヤーの設定
            Camera.main.cullingMask = _hitInfo[i].SetLayer;
            //TimeLineの再生
            _hitInfo[i].UITimeLine.Play();
        }
        else
        {
            int i = 0;
            if (isRandam || num >= _brekakeInfo.Count)
            {
                i = Random.Range(0, _brekakeInfo.Count);
            }
            else
            {
                i = num;
            }           
            
            //マテリアル設定
            _playerControl.PlayerMaterial.ChangePlayerMaterial(_brekakeInfo[i].PlayerMaterialType);
            _bossControl.MaterialChange.ChangeBossMaterial(_brekakeInfo[i].BossMaterialType);
            //レイヤーの設定
            Camera.main.cullingMask = _brekakeInfo[i].SetLayer;
            //TimeLineの再生
            _brekakeInfo[i].UITimeLine.Play();
        }

        _playerControl.CameraBrain.m_IgnoreTimeScale = true;
        Time.timeScale = 0.1f;

        StartCoroutine(FF());

        _boss2.gameObject.SetActive(true);

        _playerControl.Anim.SetTrigger("Damage");
    }

    public IEnumerator FF()
    {
        yield return new WaitForSecondsRealtime(1.5f);
        _playerControl.CameraBrain.m_IgnoreTimeScale = false;
        Time.timeScale = 1f;
        _bossControl.MaterialChange.ChangeBossMaterial(ModelMaterialType.Nomal);
        _playerControl.PlayerMaterial.ChangePlayerMaterial(ModelMaterialType.Nomal);
        _boss2.gameObject.SetActive(false);
        Camera.main.cullingMask = _defultLayerMask;
    }



}

public enum BossDirectionType
{
    /// <summary>攻撃が当たった</summary>
    Hit,
    /// <summary>弱点を破壊</summary>
    Breake,
}


[System.Serializable]
public class BossHitDirectionBreakeInfo
{
    [SerializeField] private string _name;

    [Header("UIのTimeLine")]
    [SerializeField] private PlayableDirector _UItimeLine;

    [Header("演出時に、描画するレイヤー")]
    [SerializeField] private LayerMask _layer;

    [Header("Playerのマテリアル")]
    [SerializeField] private ModelMaterialType _playerMaterialType;

    [Header("ボスのマテリアル")]
    [SerializeField] private ModelMaterialType _bossMaterialType;

    public PlayableDirector UITimeLine => _UItimeLine;
    public LayerMask SetLayer => _layer;

    public ModelMaterialType PlayerMaterialType => _playerMaterialType;
    public ModelMaterialType BossMaterialType => _bossMaterialType;
}

[System.Serializable]
public class BossHitDirectionNoamlHitInfo
{
    [SerializeField] private string _name;

    [Header("UIのTimeLine")]
    [SerializeField] private PlayableDirector _UItimeLine;

    [Header("演出時に、描画するレイヤー")]
    [SerializeField] private LayerMask _layer;

    [Header("Playerのマテリアル")]
    [SerializeField] private ModelMaterialType _playerMaterialType;

    [Header("ボスのマテリアル")]
    [SerializeField] private ModelMaterialType _bossMaterialType;

    public PlayableDirector UITimeLine => _UItimeLine;
    public LayerMask SetLayer => _layer;

    public ModelMaterialType PlayerMaterialType => _playerMaterialType;
    public ModelMaterialType BossMaterialType => _bossMaterialType;
}