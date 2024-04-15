using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PointZipUI
{
    [Header("LockOnのUI")]
    [SerializeField] private GameObject _pointZipUI;

    [Header("Canvas")]
    [SerializeField] private RectTransform _parentUI;

    private PlayerControl _playerControl = null;

    /// <summary>StateMacineをセットする関数</summary>
    /// <param name="stateMachine"></param>
    public void Init(PlayerControl playerControl)
    {
        _playerControl = playerControl;
    }

    public void SetPointZipUI(bool isLockOn)
    {
        _pointZipUI.SetActive(isLockOn);
    }



    // UIの位置を更新する
    public void UpdatePointZipUIPosition()
    {
        if (!_playerControl.InputManager.LeftTrigger) return;

        if (_playerControl.PointZip.IsHitSearch)
        {
            _pointZipUI.SetActive(true);
        }
        else
        {
            _pointZipUI.SetActive(false);
            return;
        }

        var cameraTransform = Camera.main.transform;

        // カメラの向きベクトル
        var cameraDir = cameraTransform.forward;

        // オブジェクトの位置
        var targetWorldPos = _playerControl.PointZip.PointZipSearch.RayHitPoint;

        // カメラからターゲットへのベクトル
        var targetDir = targetWorldPos - _playerControl.PlayerT.position;

        // 内積を使ってカメラ前方かどうかを判定
        var isFront = Vector3.Dot(cameraDir, targetDir) > 0;

        // カメラ前方ならUI表示、後方なら非表示
        _pointZipUI.gameObject.SetActive(isFront);
        if (!isFront) return;

        // オブジェクトのワールド座標→スクリーン座標変換
        var targetScreenPos = Camera.main.WorldToScreenPoint(targetWorldPos);

        // スクリーン座標変換→UIローカル座標変換
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _parentUI,
            targetScreenPos,
            null,
            out var uiLocalPos
        );

        // RectTransformのローカル座標を更新
        _pointZipUI.transform.localPosition = uiLocalPos;
    }


}
