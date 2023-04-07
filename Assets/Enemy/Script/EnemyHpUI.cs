using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHpUI : MonoBehaviour
{
    // オブジェクト
    [SerializeField] Transform _enemyTarget;

    [SerializeField] RectTransform _enemyHpUI;

    private RectTransform _parentUI;


    public void U()
    {
        var cameraTransform = Camera.main.transform;

        // カメラの向きベクトル
        var cameraDir = cameraTransform.forward;
        // オブジェクトの位置
        var targetWorldPos = _enemyTarget.position +Vector3.up;
        // カメラからターゲットへのベクトル
        var targetDir = targetWorldPos - cameraTransform.position;

        // 内積を使ってカメラ前方かどうかを判定
        var isFront = Vector3.Dot(cameraDir, targetDir) > 0;

        // カメラ前方ならUI表示、後方なら非表示
       _enemyHpUI.gameObject.SetActive(isFront);
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
        _enemyHpUI.localPosition = uiLocalPos;
    }

    void Start()
    {
        _parentUI = _enemyHpUI.parent.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
      //  U();
    }
}
