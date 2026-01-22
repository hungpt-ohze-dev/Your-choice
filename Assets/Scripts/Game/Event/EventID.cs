public enum EventID
{
    // Core loop
    StartGame,
    PauseGame,
    ResumGame,
    WinGame,
    LoseGame,
    ReviveGame,

    // Game play
    Ask_To_Play,
    Skip_Slot,
    Play_Slot,

    // Info
    Update_Resource,
    Update_Booster,

    // Booster
    Booster_Packed,
    Booster_Refresh,
    Booster_Time,
    Booster_FireGun,
    Using_FireGun,
    NotUsing_FireGun,

    // Package
    Buy_NoAds_Success,
    Buy_Piggy_Bank,
    Buy_Starter_Pack,

    // Piggy bank
    PiggyBank_AddCoin,

    // Gold rush
    GoldRush_Received,

    // Decor
    Build_Decor,
    Build_All_Done,
    Next_Map,

    // Special pack
    Special_Pack_All,
}
