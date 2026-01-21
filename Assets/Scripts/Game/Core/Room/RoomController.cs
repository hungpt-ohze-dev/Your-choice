using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomController : MonoBehaviour
{
    private LevelSave levelSave;

    public void Init()
    {
        levelSave = DataManager.Save.Level;
    }

    private Room currentRoom;

    public void LoadRoom(int id)
    {
        var prefab = Resources.Load<Room>($"Room/Room {id}");
        currentRoom = Instantiate<Room>(prefab);
        currentRoom.Setup();
    }

    public void NextRoom()
    {
        if(currentRoom != null)
        {
            Destroy(currentRoom.gameObject);
        }

        LoadRoom(levelSave.roomId);
    }
}
