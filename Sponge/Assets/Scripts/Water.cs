using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    PlayerMovement player;
    public float fillSpeed = 5;

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            if (player == null)
                player = other.GetComponent<PlayerMovement>();

            player.WaterCount += fillSpeed * Time.deltaTime;
        }
    }
}
