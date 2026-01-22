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

    [Header("Group")]
    [SerializeField] private GameObject mainGroup;
    [SerializeField] private GameObject slotMachineGroup;

    private LevelSave levelSave;

    protected override void Init()
    {
        levelSave = DataManager.Save.Level;

        coinInfo.Set();
    }

    public override void Show()
    {
        base.Show();

        coinInfo.UpdateInfo();

        ShowMain();
    }

    public void OnSetting()
    {
        UIManager.Instance.ShowPopup<UIPopupSetting>();
    }

    public void OnReset()
    {
        MainGame.Instance.ResetLevel();
    }

    public void ShowSlotMachine()
    {
        mainGroup.SetActive(false);
        slotMachineGroup.SetActive(true);
    }

    public void ShowMain()
    {
        slotMachineGroup.SetActive(false);
        mainGroup.SetActive(true);
    }
}
