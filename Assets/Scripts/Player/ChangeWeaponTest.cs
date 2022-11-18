using Assets.HeroEditor.Common.CharacterScripts;
using HeroEditor.Common.Enums;
using UnityEngine;


public class ChangeWeaponTest : MonoBehaviour
{
    Character character;

    // Use this for initialization
    void Start()
    {
        character = GetComponent<Character>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            character.Equip(character.SpriteCollection.Firearms2H[0], EquipmentPart.Firearm2H);
            GetComponent<Player>().changeType(Player.PlayerType.gunner);
        }
        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            character.Equip(character.SpriteCollection.MeleeWeapon1H[11], EquipmentPart.MeleeWeapon1H);
        }
    }
}