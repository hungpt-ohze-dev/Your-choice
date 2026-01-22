using UnityEngine;

public class SlotAskPopup : MonoBehaviour
{
    [Header("Button")]
    [SerializeField] private GameObject skipBtn;
    [SerializeField] private GameObject playBtn;

    public void ShowButton(int timeIndex)
    {
        playBtn.SetActive(true);

        bool showSkip = timeIndex == 0 ? false : true;
        skipBtn.SetActive(showSkip);
    }

    public void Show(bool isShow)
    {
        gameObject.SetActive(isShow);
    }
}
