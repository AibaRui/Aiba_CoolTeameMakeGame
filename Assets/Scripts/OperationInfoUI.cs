using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OperationInfoUI : MonoBehaviour
{
    [Header("UIの一番上の親")]
    [SerializeField] private GameObject _uiParent;

    [Header("押して表示")]
    [SerializeField] private GameObject _opeUI_Open;
    [Header("押して閉じる")]
    [SerializeField] private GameObject _opeUI_Close;

    [Header("操作説明画像_通常")]
    [SerializeField] private GameObject _opeUI_Nomal;
    [Header("操作説明画像_ボス戦")]
    [SerializeField] private GameObject _opeUI_Boss;

    [SerializeField] private PlayerControl _playerControl;

    /// <summary>操作説明のUIが開いているかどうか</summary>
    private bool _isOpneUI = false;

    /// <summary>UIを表示可能かどうか</summary>
    private bool _isCanShowUI = false;


    private void Awake()
    {

    }

    /// <summary>ムービーなどで表示したくない場合に使う</summary>
    public void UISetOnOff(bool isSet)
    {
        _isCanShowUI = isSet;
        _uiParent.SetActive(isSet);
    }

    public void CheckUI()
    {
        _isOpneUI = !_isOpneUI;

        if (!_playerControl.IsBossButtle)
        {
            _opeUI_Nomal.SetActive(_isOpneUI);
        }
        else
        {
            _opeUI_Boss.SetActive(_isOpneUI);
        }   //各操作説明の画像を表示


        if (_isOpneUI)
        {
            _opeUI_Open.SetActive(false);
            _opeUI_Close.SetActive(true);
        }
        else
        {
            _opeUI_Open.SetActive(true);
            _opeUI_Close.SetActive(false);
        }
    }

    void Update()
    {
        if (!_isCanShowUI) return;

        if (Input.GetButtonDown("Menu"))
        {
            CheckUI();
        }
    }



}
