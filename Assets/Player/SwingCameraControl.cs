using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingCameraControl : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //var h = _playerControl.InputManager.HorizontalInput;
        //var v = _playerControl.InputManager.VerticalInput;
        //if (h != 0 || v != 0)
        //{
        //    //追従を終了
        //    _isEndAutoFollow = false;
        //}

        //if (_isEndAutoFollow && _isDontCameraMove)
        //{
        //    if (_autoFloowCount < 0.8f)
        //    {
        //        _autoFloowCount += 0.01f;
        //    }

        //    float y = 0;
        //    if (_playerControl.PlayerT.eulerAngles.y > 180)
        //    {
        //        y = _playerControl.PlayerT.eulerAngles.y - 360;
        //    }
        //    else
        //    {
        //        y = _playerControl.PlayerT.eulerAngles.y;
        //    }

        //    float angleDiff = Mathf.DeltaAngle(y, _swingCinemachinePOV.m_HorizontalAxis.Value); // 角度差を-180度から180度の範囲に収める

        //    if (Mathf.Abs(angleDiff) > 90f)
        //    {
        //        angleDiff -= Mathf.Sign(angleDiff) * 180f;
        //    }// 角度差が90度より大きい場合は、逆方向に回転する

        //    if (angleDiff > 0f)
        //    {
        //        _swingCinemachinePOV.m_HorizontalAxis.Value -= Mathf.Min(angleDiff, _autoFloowCount);
        //    }// プレイヤーの回転角度に近づくようにValueの値を減らす
        //    else if (angleDiff < 0f)
        //    {
        //        _swingCinemachinePOV.m_HorizontalAxis.Value += Mathf.Min(-angleDiff, _autoFloowCount);
        //    }// プレイヤーの回転角度に近づくようにValueの値を増やす

        //    float dis = Mathf.Abs(_swingEndPlayerRotateY - _swingCinemachinePOV.m_HorizontalAxis.Value);

        //    if (dis < 1f)
        //    {
        //        //追従を終了
        //        _isEndAutoFollow = false;

        //        //現在のカメラの回転速度を受け継ぐ
        //        _countCameraMoveAirX = _autoFloowCount;

        //        return;
        //    }
        //}
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
