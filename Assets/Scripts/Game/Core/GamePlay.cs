using com.homemade.modules.audio;
using com.homemade.pattern.observer;
using com.homemade.pattern.singleton;
using Cysharp.Threading.Tasks;
using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GamePlay : MonoSingleton<GamePlay>
{
    [Header("Info")]
    [SerializeField] private PlatformEnum platformID;
    [SerializeField] private bool canTouch;

    [Header("Player")]
    [SerializeField] private PlayerController player;

    [Header("UI")]
    [SerializeField] private SlotAskPopup askPopup;

    // Get static
    public static bool LevelSetupDone = false;
    public static bool CanTouch
    {
        get => Instance.canTouch;
        set => Instance.canTouch = value;
    }
    public static PlatformEnum TargetPlatform => Instance.platformID;
    public static PlayerController Player => Instance.player;

    // Private variable
    private LevelSave levelSave;
    private SettingSave settingSave;
    private ResourceSave resourceSave;

    // Audio
    private AudioCase gameMusic;

    private UIGameScreen gameScreen;
    private ObstcleChoice slotMachine;

    protected override void OnInit()
    {
        Application.targetFrameRate = 60;
        canTouch = true;

        levelSave = DataManager.Save.Level;
        settingSave = DataManager.Save.Setting;
        resourceSave = DataManager.Save.Resource;

        UIManager.Instance.PopupShowAction += OnPopupShow;
        UIManager.Instance.PopupHideAllAction += OnPopupHideAll;
    }

    protected override void OnApplicationQuit()
    {
        applicationIsQuitting = true;
    }

    protected override void OnDestroy()
    {
        if (applicationIsQuitting) return;
        UIManager.Instance.PopupShowAction -= OnPopupShow;
        UIManager.Instance.PopupHideAllAction -= OnPopupHideAll;
    }

    protected override void Start()
    {
#if UNITY_EDITOR
        if (IsSimulatorView())
        {
            platformID = PlatformEnum.Mobile;
        }
        else
        {
            platformID = PlatformEnum.PC;
        }
#elif UNITY_ANDROID || UNITY_IOS
        platformID = PlatformEnum.Mobile;
#endif

        Init().Forget();
    }

    #region Editor
    private bool IsSimulatorView()
    {
#if UNITY_EDITOR
        var gameViews = Resources.FindObjectsOfTypeAll<EditorWindow>();
        foreach (var view in gameViews)
        {
            string title = view.titleContent.text;
            if (title == "Simulator")
                return true;
        }

#endif
        return false;
    }
    #endregion

    private async UniTaskVoid Init()
    {
        canTouch = false;

        gameScreen = await UIManager.Instance.ShowScreen<UIGameScreen>();
        UIManager.Extra.HideTransition();
        canTouch = true;

        // Release all sound
        await UniTask.DelayFrame(1);

        player.Init();

        await UniTask.DelayFrame(1);

        AudioController.Instance.ReleaseSounds();

        // Music background
        await UniTask.DelayFrame(1);
        gameMusic = AudioController.Instance.PlaySmartMusic(MusicClips.Background);
        gameMusic.source.mute = !settingSave.music;
    }

    #region Event

    public async void WinGame()
    {
        canTouch = false;
        levelSave.FinishLevel();
        player.CanMove = false;

        Debug.Log("Win");
        gameMusic.Stop();

        this.PostEvent(EventID.WinGame);

        await UniTask.WaitForSeconds(2f);
        await UIManager.Instance.ShowPopup<UIPopupWin>();
    }

    public async void LoseGame()
    {
        canTouch = false;
        player.CanMove = false;
        Debug.Log("Lose");
        gameMusic.Stop();

        this.PostEvent(EventID.LoseGame);

        await UniTask.WaitForSeconds(1f);
        await UIManager.Instance.ShowPopup<UIPopupLose>();
    }

    public void OnPopupShow()
    {
        canTouch = false;
    }

    public void OnPopupHideAll()
    {
        canTouch = true;
    }

    #endregion

    // Load slot scene
    public void LoadSlotScene()
    {
        StartCoroutine(LoadAsync("1_Slot_3X3_FruitHold"));
    }

    IEnumerator LoadAsync(string sceneName)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

        while (!operation.isDone)
        {
            //Debug.Log("Loading: " + (operation.progress * 100) + "%");
            yield return null;
        }

        MonoScene.Instance.SetActiveScene(sceneName);

        gameScreen.ShowSlotMachine();
    }

    public void RemoveSlotScene()
    {
        SceneManager.UnloadSceneAsync("1_Slot_3X3_FruitHold");
        gameScreen.ShowMain();
    }    

    public void AskToPlaySlot(ObstcleChoice slot)
    {
        this.slotMachine = slot;
        askPopup.Show(true);
        askPopup.ShowButton(slot.Id);

        player.CanMove = false;
        player.PushLayer();
        slotMachine.PushLayer();

        this.PostEvent(EventID.Ask_To_Play);
    }

    public void SkipSlot()
    {
        askPopup.Show(false);
        player.ResetLayer();
        player.CanMove = true;
        slotMachine.ResetLayer();

        this.PostEvent(EventID.Skip_Slot);
    }

    public void PlaySlot()
    {
        askPopup.Show(false);
        player.ResetLayer();
        player.CanMove = true;
        slotMachine.ResetLayer();
        slotMachine.SpineSlotMachine();

        this.PostEvent(EventID.Play_Slot);
    }

}
