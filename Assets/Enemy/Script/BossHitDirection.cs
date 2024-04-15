using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHitDirection : MonoBehaviour
{
    private BossControl _bossControl;
    private PlayerControl _playerControl;

    [Header("元のレイヤー")]
    [SerializeField] private LayerMask _defultLayerMask;

    [Header("描画するレイヤー")]
    [SerializeField] private LayerMask _layer;

    [SerializeField] private GameObject _ui;

    private void Awake()
    {
        _bossControl = FindObjectOfType<BossControl>();
        _playerControl = FindObjectOfType<PlayerControl>();
    }

    public void BossDamageStartDirection()
    {
        Time.timeScale = 0.4f;
        _bossControl.MaterialChange.ChangeBossMaterial(ModelMaterialType.GrayScal);
        _playerControl.PlayerMaterial.ChangePlayerMaterial(ModelMaterialType.GrayScal);
        _ui.SetActive(true);
        StartCoroutine(FF());
        Camera.main.cullingMask = _layer;
    }

    public IEnumerator FF()
    {
        yield return new WaitForSecondsRealtime(3);
        Time.timeScale = 1f;
        _bossControl.MaterialChange.ChangeBossMaterial(ModelMaterialType.Nomal);
        _playerControl.PlayerMaterial.ChangePlayerMaterial(ModelMaterialType.Nomal);
        _ui.SetActive(false);

        Camera.main.cullingMask = _defultLayerMask;
    }

}
