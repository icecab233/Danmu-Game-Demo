using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "New EquipData", menuName = "ScriptableObjects/EquipData")]
public class EquipData : ScriptableObject
{
    // 枪手每个等级的子弹投射物prefab
    public GameObject[] gunProjectileOfLevel;

    // 法师每个等级的魔法导弹投射物prefab
    public GameObject[] mageProjectileOfLevel;
}