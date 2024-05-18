using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class AimAssist
{
    [Header("弱点を探知する円の半径")]
    [SerializeField] private float _targettingRange;

    [Header("弱点をターゲットするのにかかる時間")]
    [SerializeField] private float _targettingTime;

    [Header("敵の弱点のレイヤー")]
    [SerializeField] private LayerMask _enemyLayer;

    [Header("======UI=====")]
    [Header("射程圏内に無し、を示すパネル")]
    [SerializeField] private GameObject _noTargetInAreaPanel;

    [Header("ロックオンをするパネル")]
    [SerializeField] private GameObject _lockOnPanel;

    [Header("ロックオン完了時のパネル")]
    [SerializeField] private GameObject _lockOnSuccsecPanel;

    [Header("Canvus")]
    [SerializeField] private RectTransform _parentUI;

    /// <summary>敵の弱点を探知できているかどうか</summary>
    private bool _isTargetting;

    private bool _isSuccsesTarget;

    /// <summary>現在探知している敵の弱点 </summary>
    private GameObject _targettingObj;

    /// <summary>1f前の探知していた敵の弱点</summary>
    private GameObject _targettedObj;

    /// <summary>ターゲットするまでの時間を計測</summary>
    private float _countTargettingTime = 0;




    private PlayerControl _playerControl;

    public void Init(PlayerControl playerControl)
    {
        _playerControl = playerControl;
    }

    public void Targetting()
    {
        if (_playerControl.InputManager.IsSetUp <= 0)
        {
            _isSuccsesTarget = false;
            _isTargetting = false;
            return;
        }   //左トリガーを押していないなら何もしない

        //弱点を探知
        if (SearchWeakPoints())
        {
            if (_targettingObj != _targettedObj)
            {
                if (_lockOnPanel.activeSelf) _lockOnPanel.SetActive(false);
                if (_lockOnSuccsecPanel.activeSelf) _lockOnSuccsecPanel.SetActive(false);
                _isSuccsesTarget = false;
                _countTargettingTime = 0;
            }
            Debug.Log("敵を確認");
            if (_noTargetInAreaPanel.activeSelf) _noTargetInAreaPanel.SetActive(false);
            CountTargettingTime();
            _targettedObj = _targettingObj;
        }
        else
        {
           // Debug.Log("敵がいない");
            _isTargetting = false;
            _targettedObj = null;
        }


    }

    private void CountTargettingTime()
    {
        _countTargettingTime += Time.deltaTime;

        if (_countTargettingTime > _targettingTime && !_isSuccsesTarget)
        {
            _isSuccsesTarget = true;
            _lockOnSuccsecPanel.SetActive(true);
        }
    }

    public GameObject GetLockOnEnemy()
    {
        if (_isSuccsesTarget)
        {
            return _targettingObj;
        }
        return null;
    }

    /// <summary>
    /// 画面内に映っている一番近い弱点を探知する
    /// </summary>
    private bool SearchWeakPoints()
    {
        Collider[] hits = Physics.OverlapSphere(_playerControl.PlayerT.position, _targettingRange, _enemyLayer);

        if (hits.Length > 0)
        {
            List<GameObject> checkObj = new List<GameObject>();
            GameObject setObj = null;

            foreach (var a in hits)
            {
                if (IsObjectInViewport(a.gameObject))
                {
                    checkObj.Add(a.gameObject);
                }
            }   //探知した弱点から画面内に移っているものを選出

            if (checkObj.Count == 0)
            {
                return false;
            }   //画面内に映っていなかったらロックオン不可
            else if (checkObj.Count == 1 && checkObj[0] == _targettedObj)
            {
                _isTargetting = true;
                return true;
            }   //探知した数が1つで、探知してたものと同じだったらここで終わりにできる

            foreach (var a in checkObj)
            {
                if (setObj == null)
                {
                    setObj = a;
                }
                else
                {
                    float disA = Vector3.Distance(_playerControl.PlayerT.position, a.gameObject.transform.position);
                    float disB = Vector3.Distance(_playerControl.PlayerT.position, setObj.transform.position);

                    if (disA < disB)
                    {
                        setObj = a.gameObject;
                    }
                }
            }   //一番距離の近い弱点を決める

            _isTargetting = true;
            _targettingObj = setObj;
            return true;
        }
        else
        {
            _isTargetting = false;
            _targettingObj = null;
            return false;
        }
    }

    // オブジェクトが画面内にあるかどうかを判断する関数
    bool IsObjectInViewport(GameObject obj)
    {
        // オブジェクトの位置をワールド座標からビューポート座標に変換
        Vector3 viewportPos = Camera.main.WorldToViewportPoint(obj.transform.position);

        // ビューポート座標が0から1の範囲内かどうかを判断
        if (viewportPos.x >= 0 && viewportPos.x <= 1 && viewportPos.y >= 0 && viewportPos.y <= 1 && viewportPos.z > 0)
        {
            // 画面内にある
            return true;
        }
        else
        {
            // 画面外にある
            return false;
        }
    }

    /// <summary>
    /// アシストのUIの描画設定
    /// </summary>
    public void AssistUISetting()
    {
        if (_playerControl.InputManager.IsSetUp <= 0)
        {
            if (_lockOnPanel.activeSelf) _lockOnPanel.SetActive(false);
            if (_lockOnSuccsecPanel.activeSelf) _lockOnSuccsecPanel.SetActive(false);
            if (_noTargetInAreaPanel.activeSelf) _noTargetInAreaPanel.SetActive(false);
            return;
        }   //アシスト機能を使っていないを時は全てのパネルを非表示にする


        if (!_isTargetting)
        {
            if (_lockOnPanel.activeSelf) _lockOnPanel.SetActive(false);
            if (_lockOnSuccsecPanel.activeSelf) _lockOnSuccsecPanel.SetActive(false);
            if (!_noTargetInAreaPanel.activeSelf) _noTargetInAreaPanel.SetActive(true);
            return;
        }   //ターゲットを探知できていない

        //マーカーの位置をスクリーン画面に変換して表示する
        var targetWorldPos = _targettingObj.transform.position;
        var targetScreenPos = Camera.main.WorldToScreenPoint(targetWorldPos);

        _lockOnPanel.transform.position = targetScreenPos;

        var cameraDir = Camera.main.transform.forward;
        var targetDir = targetWorldPos - Camera.main.transform.position;

        var isFront = Vector3.Dot(targetDir, cameraDir) > 0;
        _lockOnPanel.SetActive(isFront);

        //var cameraTransform = Camera.main.transform;

        //// カメラの向きベクトル
        //var cameraDir = cameraTransform.forward;
        //// オブジェクトの位置
        //var targetWorldPos = _targettingObj.transform.position + Vector3.up;
        //// カメラからターゲットへのベクトル
        //var targetDir = targetWorldPos - cameraTransform.position;

        //// 内積を使ってカメラ前方かどうかを判定
        //var isFront = Vector3.Dot(cameraDir, targetDir) > 0;

        //// カメラ前方ならUI表示、後方なら非表示
        //_lockOnPanel.gameObject.SetActive(isFront);
        //if (!isFront)
        //{
        //    _countTargettingTime = 0;
        //    _isSuccsesTarget = false;
        //    return;
        //}
        //// オブジェクトのワールド座標→スクリーン座標変換
        //var targetScreenPos = Camera.main.WorldToScreenPoint(targetWorldPos);

        //// スクリーン座標変換→UIローカル座標変換
        //RectTransformUtility.ScreenPointToLocalPointInRectangle(
        //    _parentUI,
        //    targetScreenPos,
        //    null,
        //    out var uiLocalPos
        //);

        //Debug.Log(uiLocalPos);
        //// RectTransformのローカル座標を更新
        //_lockOnPanel.transform.localPosition = uiLocalPos;
    }
}
