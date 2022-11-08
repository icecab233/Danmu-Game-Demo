using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerZone : MonoBehaviour
{
    public GameObject playerZone1;
    public GameObject playerZone2;
    public int posId;

    void Update()
    {
        if (PlayerManager.posOccupied[posId])
        {
            playerZone1.SetActive(true);
            playerZone2.SetActive(false);
        } else
        {
            playerZone1.SetActive(false);
            playerZone2.SetActive(true);
        }
    }
}
