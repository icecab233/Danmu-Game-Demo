using UnityEngine;
using TMPro;
using System.Collections;

public class Player : MonoBehaviour
{
    [SerializeField]
    private string playerName;
    public int level;
    public int hp;
    public int hpMax;

    [SerializeField]
    private TextMeshProUGUI nameText;

    public float attackCoolDownTime = 2.0f;

    private PlayerBow playerBow;

    public Player()
    {
        level = 1;
        hp = 100;
        hpMax = 100;
    }

    private void Start()
    {
        playerBow = GetComponent<PlayerBow>();
        StartCoroutine(autoBowAttackCoroutine(attackCoolDownTime));
    }

    public void changeName(string _name)
    {
        playerName = _name;

        // refresh UI name text display
        nameText.text = playerName;
    }

    public string getName()
    {
        return playerName;
    }

    IEnumerator autoBowAttackCoroutine(float coolDownTime)
    {
        while (true)
        {
            yield return new WaitForSeconds(coolDownTime);
            playerBow.BowAttack();
        }
    }
}
