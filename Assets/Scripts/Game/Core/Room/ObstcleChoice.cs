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

    private void Start()
    {
        Hide();
    }

    public void ShowChoice()
    {
        popup.SetActive(true);
        SpineSlotMachine();
    }

    public void Hide()
    {
        popup.SetActive(false);
    }

    private void SpineSlotMachine()
    {
        bool win = Random.Range(0, 100) < percentWin;
        StartCoroutine(SpineSlotIEnum(win));
    }

    private IEnumerator SpineSlotIEnum(bool isWin)
    {
        animator.CrossFade("Slot", 0f);
        yield return new WaitForSeconds(3f);
        if(isWin)
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
    }

    private void Lose()
    {
        DataManager.Save.Resource.Subtract(ResourceType.Coin, amount);
    }
}
