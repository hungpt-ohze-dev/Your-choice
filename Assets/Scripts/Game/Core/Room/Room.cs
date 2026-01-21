using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public Transform startGateTrans;

    public void Setup()
    {
        GamePlay.Player.transform.position = startGateTrans.position;
    }
}
