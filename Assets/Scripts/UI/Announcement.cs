using UnityEngine;
using System.Collections;
using TMPro;

public class Announcement : MonoBehaviour
{
    public TextMeshProUGUI annText;
    public float showTime = 5.0f;

    private void Start()
    {
        annText.gameObject.SetActive(false);
    }

    public void LevelUpAnn(Player player)
    {
        annText.text = player.playerName + ConstantText.levelUpAnn + player.level;
        StartCoroutine(AnnTextDisappearCoroutine());
    }

    IEnumerator AnnTextDisappearCoroutine()
    {
        annText.gameObject.SetActive(true);
        yield return new WaitForSeconds(showTime);
        annText.gameObject.SetActive(false);
    }
}
