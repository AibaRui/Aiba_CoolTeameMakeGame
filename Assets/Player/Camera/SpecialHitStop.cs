using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialHitStop : MonoBehaviour
{
    [Header("�q�b�g�X�g�b�v�̎��")]
    [SerializeField] private List<SpecialHitStopInfo> _specialHitStopInfos = new List<SpecialHitStopInfo>();

    [Header("���̃��C���[")]
    [SerializeField] private LayerMask _defultLayerMask;


    [SerializeField] private PlayerControl _playerControl;

    /// <summary>HitStop�̍ہA�`�悷�郌�C���[</summary>
    private LayerMask _setLayerMask;

    /// <summary>HitStop�̍ہA�`�悵�����摜</summary>
    private GameObject _hitStopImages;

    /// <summary>HitStop�̎��s����</summary>
    private float _finishTime = 0.7f;

    /// <summary>HitStop�̎��Ԍv�� </summary>
    private float _timeCount;

    /// <summary>Hitstop���s�����ǂ���</summary>
    private bool _isHitStop = false;

    /// <summary>HitStop�̏���ݒ�</summary>
    /// <param name="i"></param>
    public void SetHitStopInfo(int i)
    {
        foreach (var a in _specialHitStopInfos)
        {
            if (a.Number == (i + 1))
            {
                _finishTime = a.HitStopTime;
                _setLayerMask = a.Layer;
                _hitStopImages = a.UI;
                return;
            }
        }
        Debug.LogError("SpecialHitStop�̏�񂪓o�^����Ă��܂���");
    }

    /// <summary>HitStoo�J�n</summary>
    public void StartHitStop()
    {
        Time.timeScale = 0f;

        _isHitStop = true;

        Camera.main.cullingMask = _setLayerMask;
        _hitStopImages.SetActive(true);
        _playerControl.PlayerMaterial.ChangePlayerMaterial(ModelMaterialType.GrayScal);
    }

    void Update()
    {
        if (!_isHitStop) return;


        _timeCount += Time.unscaledDeltaTime;

        if (_timeCount > _finishTime)
        {
            Camera.main.cullingMask = _defultLayerMask;
            _hitStopImages.SetActive(false);
            Time.timeScale = 1f;
            _playerControl.PlayerMaterial.ChangePlayerMaterial(ModelMaterialType.Nomal);
            _timeCount = 0;

        }
    }
}

[System.Serializable]
public class SpecialHitStopInfo
{
    [SerializeField] private string _numbers;

    [Header("���ʔԍ�")]
    [SerializeField] private int _number;

    [Header("�`�悷�郌�C���[")]
    [SerializeField] private LayerMask _layer;

    [Header("�`�悷��UI")]
    [SerializeField] private GameObject _UI;

    [Header("�q�b�g�X�g�b�v�̎��s����")]
    [SerializeField] private float _hitStopTime = 0.7f;

    public int Number => _number;
    public LayerMask Layer => _layer;
    public GameObject UI => _UI;
    public float HitStopTime => _hitStopTime;
}