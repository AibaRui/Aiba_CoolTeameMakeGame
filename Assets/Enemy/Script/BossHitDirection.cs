using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class BossHitDirection : MonoBehaviour
{
    private BossControl _bossControl;
    private PlayerControl _playerControl;

    [Header("���̃��C���[")]
    [SerializeField] private LayerMask _defultLayerMask;

    [Header("�`�悷�郌�C���[")]
    [SerializeField] private LayerMask _layer;

    [SerializeField] private GameObject _ui;

    [SerializeField] private GameObject _boss2;

    [SerializeField] private CinemachineVirtualCamera _camera;

    private void Awake()
    {
        _bossControl = FindObjectOfType<BossControl>();
        _playerControl = FindObjectOfType<PlayerControl>();
    }

    public void BossDamageStartDirection()
    {
        Time.timeScale = 0.1f;
        _bossControl.MaterialChange.ChangeBossMaterial(ModelMaterialType.GrayScal);
        _playerControl.PlayerMaterial.ChangePlayerMaterial(ModelMaterialType.GrayScal);
        _ui.SetActive(true);
        StartCoroutine(FF());
        Camera.main.cullingMask = _layer;
        _boss2.gameObject.SetActive(true);
    }

    public IEnumerator FF()
    {
        yield return new WaitForSecondsRealtime(1.5f);
        Time.timeScale = 1f;
        _bossControl.MaterialChange.ChangeBossMaterial(ModelMaterialType.Nomal);
        _playerControl.PlayerMaterial.ChangePlayerMaterial(ModelMaterialType.Nomal);
        _ui.SetActive(false);
        _boss2.gameObject.SetActive(false);
        Camera.main.cullingMask = _defultLayerMask;
    }

}