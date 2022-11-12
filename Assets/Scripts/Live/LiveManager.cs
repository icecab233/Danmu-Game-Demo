using UnityEngine;

public class LiveManager : MonoBehaviour
{
    public PlayerManager playerManager;
    public StringEvent AddNewPlayerEvent;
    public StringEvent RandomizePlayerEvent;
    public VoidEvent RageModeForAllEvent;
    public StringEvent AddExpToPlayerFromGift1Event;

    public void OnDanmu(string userName, string danmu)
    {
        switch (danmu)
        {
            case "����":
                AddNewPlayerEvent.Raise(userName);
                break;
            case "���":
                RandomizePlayerEvent.Raise(userName);
                break;
            case "��":
                RageModeForAllEvent.Raise();
                break;
        }
    }

    public void OnGift(string userName, string giftName, int giftNum)
    {

        switch (giftName)
        {
            case "����":
                if (giftNum == 1)
                {
                    AddExpToPlayerFromGift1Event.Raise(userName);
                } else if (giftNum == 2)
                {
                    RageModeForAllEvent.Raise();
                }
                break;
        }
    }
}
