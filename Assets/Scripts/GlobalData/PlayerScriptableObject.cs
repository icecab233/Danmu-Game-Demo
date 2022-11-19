using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "New PlayerScriptableObjectData" ,menuName = "ScriptableObjects/PlayerScriptableObjectData")]
public class PlayerScriptableObject : ScriptableObject
{
    // 枪手每个等级的子弹投射物prefab
    public GameObject[] gunProjectileOfLevel;

    // 法师每个等级的魔法导弹投射物prefab
    public GameObject[] mageProjectileOfLevel;
}