using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIGameScreen : BaseScreen
{
    [Header("Component")]
    public MobileButton leftBtn;
    public MobileButton rightBtn;
    public MobileButton choiceBtn;

    [Header("Resource")]
    [SerializeField] private UICoinInfo coinInfo;

    private LevelSave levelSave;

    protected override void Init()
    {
        levelSave = DataManager.Save.Level;

        //coinInfo.Set();
    }

    public override void Show()
    {
        base.Show();

        //coinInfo.UpdateInfo();
    }

    public void OnSetting()
    {
        UIManager.Instance.ShowPopup<UIPopupSetting>();
    }

    public void OnReset()
    {
        MainGame.Instance.ResetLevel();
    }

}
