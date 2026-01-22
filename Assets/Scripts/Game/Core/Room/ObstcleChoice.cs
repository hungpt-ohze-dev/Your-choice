using com.homemade.modules.audio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstcleChoice : MonoBehaviour
{
    [Header("Component")]
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject popup;

    [Header("Percent Win")]
    [SerializeField] private int percentWin = 70;
    [SerializeField] private int amount = 100;

    private bool isPlayed = false;

    private void Start()
    {
        Hide();
    }

    public void ShowChoice()
    {
        //if (isPlayed)
        //{
        //    UIManager.Extra.ShowToast("You have played this slot machine");
        //    return;
        //}

        GamePlay.Player.CanMove = false;

        popup.SetActive(true);
        SpineSlotMachine();

        isPlayed = true;
    }

    public void Hide()
    {
        popup.SetActive(false);
        GamePlay.Player.CanMove = true;
    }

    private void SpineSlotMachine()
    {
        bool win = Random.Range(0, 100) < percentWin;
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
            animator.CrossFade("SlotLose", 0f);
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

        if (DataManager.Save.Resource.Coin <= 0)
        {
            GamePlay.Instance.LoseGame();
        }
    }
}
