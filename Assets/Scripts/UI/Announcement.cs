using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.UI;

public class Announcement : MonoBehaviour
{
    public TextMeshProUGUI annText;
    public float showTime = 5.0f;
    public GameObject announcer;
    public Image annoucerImage;
    [SerializeField]PlayerLevelData playerLevelData;
    private void Start()
    {
        announcer.gameObject.SetActive(false);
    }

    public void LevelUpAnn(Player player)
    {
        Debug.Log(player.level);
        annoucerImage.color=playerLevelData.getLevelColors(player.level);
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
