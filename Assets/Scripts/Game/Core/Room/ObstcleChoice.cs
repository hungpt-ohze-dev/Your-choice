using com.homemade.modules.audio;
using com.homemade.pattern.observer;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class ObstcleChoice : MonoBehaviour
{
    [Header("Info")]
    [SerializeField] private int timeSpin = 0;

    [Header("Component")]
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject popup;
    [SerializeField] private SortingGroup sorting;

    [Header("Percent Win")]
    [SerializeField] private int percentWin = 70;
    [SerializeField] private int amount = 100;

    private bool isPlayed = false;

    // Get set
    public int TimeSpin => timeSpin;

    private void Start()
    {
        Hide();
    }

    public void ShowChoice()
    {
        GamePlay.Instance.AskToPlaySlot(this);
    }

    public void Hide()
    {
        popup.SetActive(false);
        GamePlay.Player.CanMove = true;
    }

    public void SpineSlotMachine()
    {
        timeSpin++;
        isPlayed = true;
        popup.SetActive(true);

        bool win = UnityEngine.Random.Range(0, 100) < percentWin;
        StartCoroutine(SpineSlotIEnum(win));
    }

    private IEnumerator SpineSlotIEnum(bool isWin)
    {
        animator.CrossFade("Slot", 0f);
        var spinSound = AudioController.Instance.PlaySmartSound(SoundClips.spin_sound);

        yield return new WaitForSeconds(3f);
        spinSound.Stop();

        if (isWin)
        {
            animator.CrossFade("SlotWin", 0f);
            Win();
        }
        else
        {
            int randLose = UnityEngine.Random.Range(0, 3);
            animator.CrossFade($"SlotLose_{randLose}", 0f);
            Lose();
        }

        yield return new WaitForSeconds(2f);
        Hide();
    }

    private void Win()
    {
        DataManager.Save.Resource.Add(ResourceType.Coin, amount);

        UIManager.Extra.ShowToast($"You has collect {amount} coin");
        AudioController.Instance.PlaySound(SoundClips.win_coins);
    }

    private void Lose()
    {
        DataManager.Save.Resource.Subtract(ResourceType.Coin, amount);

        UIManager.Extra.ShowToast($"You has lost {amount} coin");
        AudioController.Instance.PlaySound(SoundClips.lose);

        GamePlay.Player.LostCoin();
    }

    public void PushLayer()
    {
        sorting.sortingLayerID = SortingLayer.NameToID("Extra");
    }

    public void ResetLayer()
    {
        sorting.sortingLayerID = SortingLayer.NameToID("Default");
    }
}
