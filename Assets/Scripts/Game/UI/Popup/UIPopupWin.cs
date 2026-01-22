using AssetKits.ParticleImage;
using com.homemade.modules.audio;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIPopupWin : BasePopup
{
    [Header("Component")]
    [SerializeField] private TMP_Text rewardTxt;
    [SerializeField] private RawImage preview;

    public override void Open(object obj = null)
    {
        base.Open(obj);

        AudioController.Instance.PlaySound(SoundClips.win);
    }

    public void OnNext()
    {
        MainGame.Instance.NextLevel();
        Close();
    }

    public void OnHome()
    {
        MainGame.Instance.ReturnHome();
        Close();
    }

}
