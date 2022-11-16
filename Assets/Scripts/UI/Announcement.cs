using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.UI;

public class Announcement : MonoBehaviour
{
    public TextMeshProUGUI annText;
    public float showTime = 5.0f;
    public GameObject announcer;
    public Color[] announcerColors;
    public Image annoucerImage;
    private void Start()
    {
        announcer.gameObject.SetActive(false);
    }

    public void LevelUpAnn(Player player)
    {
        annoucerImage.color = announcerColors[player.level];
        annText.text = player.playerName + ConstantText.levelUpAnn + player.level;
        StartCoroutine(AnnTextDisappearCoroutine());
    }

    IEnumerator AnnTextDisappearCoroutine()
    {
        announcer.gameObject.SetActive(true);
        yield return new WaitForSeconds(showTime);
        announcer.gameObject.SetActive(false);
    }
}
