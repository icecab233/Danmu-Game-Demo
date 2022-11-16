using DanmuGame.events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deadzone : MonoBehaviour
{
    public VoidEvent GameFailEvent;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<MonsterBehavior>() != null)
        {
            GameFailEvent.Raise();
        }
    }
}
