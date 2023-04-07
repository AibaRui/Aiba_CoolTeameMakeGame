using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyWeakPointHp : MonoBehaviour, IDamageble
{
    [Header("弱点の最大Hp")]
    [SerializeField] private float _maxHp = 3;

    [Header("弱点用のスライダー")]
    [SerializeField] private Slider _hpSlider = default;

    [Header("弱点用のスライダーのUI")]
    [SerializeField] RectTransform _enemyHpUI;

    [Header("弱点の場所")]
    [SerializeField] Transform _enemyTarget;

    private RectTransform _parentUI;

    private EnemyControl _enemyControl;

    private EnemyHpControl _enemyHpControl;

    private float _nowHp;


    /// <summary>初期設定</summary>
    public void Init(EnemyControl enemyControl, EnemyHpControl enemyHpControl)
    {
        _enemyControl = enemyControl;
        _enemyHpControl = enemyHpControl;

        _parentUI = _enemyHpUI.parent.GetComponent<RectTransform>();

        //最大Hpを設定
        _nowHp = _maxHp;

        //Sliderの最大体力を設定
        _hpSlider.maxValue = _maxHp;
        _hpSlider.minValue = 0;
        _hpSlider.value = _nowHp;
    }

    public void Update()
    {
        UIPositionSetting();
    }

    /// <summary>ダメージ処理</summary>
    void IDamageble.Damage()
    {
        //Hpを減らす
        _nowHp--;

        //スライダーの値の更新
        _hpSlider.value = _nowHp;

        //ダメージのアニメーションを再生
        _enemyControl.EnemyAnimator.Play("Damage");

        //Hpが0になったら部位破壊。このオブジェクトを消して
        //Hpコントローラーに通知を送る
        if (_nowHp <= 0)
        {
            _enemyHpControl.DeadWealkPoint();
        }
    }

    /// <summary>弱点用のスライダーの位置を更新</summary>
    public void UIPositionSetting()
    {
        var cameraTransform = Camera.main.transform;

        // カメラの向きベクトル
        var cameraDir = cameraTransform.forward;
        // オブジェクトの位置
        var targetWorldPos = _enemyTarget.position + Vector3.up;
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


}
