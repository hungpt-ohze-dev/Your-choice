using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomGate : MonoBehaviour
{
    public LayerMask mask;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(Utils.CheckLayerMaskCollier2D(mask, other))
        {
            //GamePlay.Room.NextRoom();
        }
    }
}
