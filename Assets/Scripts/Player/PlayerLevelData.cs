using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLevelData:ScriptableObject
{
    [SerializeField] int[] LastLevelIndex;
    [SerializeField] Sprite[] levelSprites;
    [SerializeField] Color[] levelColors;
    public Sprite getLevelSprites(int levelNum)
    {
        int index=0;
        for(int i=0;i<LastLevelIndex.Length;i++)
        {
            if(levelNum<=LastLevelIndex[i])
            {
                index=i;
                break;
            }
        }
        return levelSprites[index];
    }
    public Color getLevelColors(int levelNum)
    {
        int index=0;
        for(int i=0;i<LastLevelIndex.Length;i++)
        {
            if(levelNum<=LastLevelIndex[i])
            {
                index=i;
                break;
            }
        }
        return levelColors[index];
    }
}
