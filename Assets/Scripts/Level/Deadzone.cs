using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deadzone : MonoBehaviour
{
    public LevelManager levelManager;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<MonsterBehavior>() != null)
        {
            levelManager.GameFail();
        }
    }
}
