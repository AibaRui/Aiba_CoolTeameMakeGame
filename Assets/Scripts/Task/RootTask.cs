using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootTask : MonoBehaviour
{
    [SerializeField] private TaskManager _manager;

    [Header("ラストの場所かどうか")]
    [SerializeField] private bool _isLast = false;

    // UIを表示させる対象オブジェクト
    [SerializeField] private Transform _target;

    // 表示するUI
    [SerializeField] private Transform _targetUI;

    // オブジェクト位置のオフセット
    [SerializeField] private Vector3 _worldOffset;

    private Camera _targetCamera;

    private RectTransform _parentUI;


    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag=="Player")
        {
            TaskClear();
        }
    }

    public void TaskClear()
    {
        _manager.EndRootTask();
        _targetUI.gameObject.SetActive(false);
    }

    private void Awake()
    {
        _targetCamera = Camera.main;

        // 親UIのRectTransformを保持
        _parentUI = _targetUI.parent.GetComponent<RectTransform>();
    }

    // UIの位置を毎フレーム更新
    private void Update()
    {
        OnUpdatePosition();
    }
    // UIの位置を更新する
    private void OnUpdatePosition()
    {
        var cameraTransform = _targetCamera.transform;

        // カメラの向きベクトル
        var cameraDir = cameraTransform.forward;
        // オブジェクトの位置
        var targetWorldPos = _target.position + _worldOffset;
        // カメラからターゲットへのベクトル
        var targetDir = targetWorldPos - cameraTransform.position;

        // 内積を使ってカメラ前方かどうかを判定
        var isFront = Vector3.Dot(cameraDir, targetDir) > 0;

        // カメラ前方ならUI表示、後方なら非表示
        _targetUI.gameObject.SetActive(isFront);
        if (!isFront) return;

        // オブジェクトのワールド座標→スクリーン座標変換
        var targetScreenPos = _targetCamera.WorldToScreenPoint(targetWorldPos);

        // スクリーン座標変換→UIローカル座標変換
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _parentUI,
            targetScreenPos,
            null,
            out var uiLocalPos
        );

        // RectTransformのローカル座標を更新
        _targetUI.localPosition = uiLocalPos;
    }

}
