using System;
using UnityEngine;

[Serializable]
public class LevelSave : BaseDataSave
{
    public int roomId;

    public override void Init()
    {
        base.Init();

        roomId = 0;
    }

    public void FinishLevel()
    {
        roomId += 1;
        Save();
    }
}
