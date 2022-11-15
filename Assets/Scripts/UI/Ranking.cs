using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

// ÒÀÀµÓÚPlayerManagerµ¥ÊµÀý
public class Ranking : MonoBehaviour
{
    public List<TextMeshProUGUI> PlayerTextList;
    public List<Player> SortedPlayer;
    public float rankingRefreshTime = 1.0f;

    private void Start()
    {
        StartCoroutine(RefreshRankingCoroutine());
    }

    IEnumerator RefreshRankingCoroutine()
    {
        while (true)
        {
            SortedPlayer = PlayerManager.Instance.playerList.OrderBy(o => o.exp).ToList();
            SortedPlayer.Reverse();

            foreach (var text in PlayerTextList)
            {
                text.text = "";
            }

            for (int i = 0; i < SortedPlayer.Count; i++)
            {
                if (i >= PlayerTextList.Count) break;
                PlayerTextList[i].text = SortedPlayer[i].playerName + "  " + SortedPlayer[i].exp;
            }

            yield return new WaitForSeconds(rankingRefreshTime);
        }
    }
}
