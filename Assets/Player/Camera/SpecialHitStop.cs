using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialHitStop : MonoBehaviour
{
    [Header("ヒットストップの種類")]
    [SerializeField] private List<SpecialHitStopInfo> _specialHitStopInfos = new List<SpecialHitStopInfo>();

    [Header("元のレイヤー")]
    [SerializeField] private LayerMask _defultLayerMask;


    [SerializeField] private PlayerControl _playerControl;

    /// <summary>HitStopの際、描画するレイヤー</summary>
    private LayerMask _setLayerMask;

    /// <summary>HitStopの際、描画したい画像</summary>
    private GameObject _hitStopImages;

    /// <summary>描画したい演出</summary>
    private List<GameObject> _hitStopObjects;

    /// <summary>HitStopの実行時間</summary>
    private float _finishTime = 0.7f;

    /// <summary>HitStopの時間計測 </summary>
    private float _timeCount;

    /// <summary>Hitstop実行中かどうか</summary>
    private bool _isHitStop = false;

    private bool _isDoStopTime = false;

    /// <summary>HitStopの情報を設定</summary>
    /// <param name="i"></param>
    public void SetHitStopInfo(int i, bool isDoStopTime)
    {
        _isDoStopTime = isDoStopTime;

        foreach (var a in _specialHitStopInfos)
        {
            if (a.Number == (i + 1))
            {
                _finishTime = a.HitStopTime;
                _setLayerMask = a.Layer;
                _hitStopImages = a.UI;
                _hitStopObjects = a.Objects;
                return;
            }
        }
        Debug.LogError("SpecialHitStopの情報が登録されていません");
    }

    /// <summary>HitStoo開始</summary>
    public void StartHitStop()
    {
        if (_isDoStopTime)
        {
            Time.timeScale = 0f;  
            _isHitStop = true;
        }

        Camera.main.cullingMask = _setLayerMask;
        _hitStopImages.SetActive(true);

        foreach (var a in _hitStopObjects)
        {
            a.SetActive(true);
        }

        _playerControl.PlayerMaterial.ChangePlayerMaterial(ModelMaterialType.GrayScal);
    }

    public void EndHitStop()
    {
        Camera.main.cullingMask = _defultLayerMask;
        _hitStopImages.SetActive(false);
        foreach (var a in _hitStopObjects)
        {
            a.SetActive(false);
        }
        Time.timeScale = 1f;
        _playerControl.PlayerMaterial.ChangePlayerMaterial(ModelMaterialType.Nomal);
        _timeCount = 0;
    }

    void Update()
    {
        if (!_isHitStop) return;


        _timeCount += Time.unscaledDeltaTime;

        if (_timeCount > _finishTime)
        {
            Camera.main.cullingMask = _defultLayerMask;
            _hitStopImages.SetActive(false);
            foreach (var a in _hitStopObjects)
            {
                a.SetActive(false);
            }
            Time.timeScale = 1f;
            _playerControl.PlayerMaterial.ChangePlayerMaterial(ModelMaterialType.Nomal);
            _timeCount = 0;
            _isHitStop = false;
        }
    }
}

[System.Serializable]
public class SpecialHitStopInfo
{
    [SerializeField] private string _numbers;

    [Header("識別番号")]
    [SerializeField] private int _number;

    [Header("描画するレイヤー")]
    [SerializeField] private LayerMask _layer;

    [Header("描画するUI")]
    [SerializeField] private GameObject _UI;

    [Header("表示したい演出オブジェクト")]
    [SerializeField] private List<GameObject> _objects;

    [Header("ヒットストップの実行時間")]
    [SerializeField] private float _hitStopTime = 0.7f;

    public int Number => _number;
    public LayerMask Layer => _layer;
    public GameObject UI => _UI;

    public List<GameObject> Objects => _objects;
    public float HitStopTime => _hitStopTime;
}