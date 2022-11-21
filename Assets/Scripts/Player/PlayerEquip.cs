using Assets.HeroEditor.Common.CharacterScripts.Firearms;
using Assets.HeroEditor.Common.CharacterScripts;
using Assets.HeroEditor.Common.Data;
using HeroEditor.Common.Enums;
using System;
using UnityEngine;
using Assets.HeroEditor.Common.CommonScripts;
using System.Linq;
using System.Collections.Generic;
using HeroEditor.Common;

public partial class Player : MonoBehaviour
{
    public EquipData equipData;
    public enum WeaponType
    {
        Bow,
        Firearm1H,
        Firearm2H,
        Mega
    }
    public WeaponType weaponType;

    private PlayerWeaponBase playerWeapon;
    private string weaponName;

    // 角色升级后，根据当前等级刷新职业装备和武器
    private void UpdateEquip()
    {
        // 弓箭手
        if (level < PlayerData.gunnerMinLevel)
        {
            playerType = PlayerType.archer;

            weaponName = PlayerData.bowNameOfLevel[level];
        }
        // 枪手
        else if (level >= PlayerData.gunnerMinLevel && level < PlayerData.wizardMinLevel)
        {

            playerType = PlayerType.gunner;

            // 是否为双手持枪
            if (level >= PlayerData.gunner2HLevel)
            {
                weaponType = WeaponType.Firearm2H;
            } else
            {
                weaponType = WeaponType.Firearm1H;
            }

            weaponName = PlayerData.gunNameOfLevel[level-PlayerData.gunnerMinLevel];
        } 
        // 法师
        else if (level >= PlayerData.wizardMinLevel)
        {
            playerType = PlayerType.wizard;
            weaponType = WeaponType.Mega;
            weaponName = PlayerData.mageNameOfLevel[level - PlayerData.wizardMinLevel];
        }

        UpdateWeapon();
    }

    private void UpdateWeapon()
    {
        switch (playerType)
        {
            case PlayerType.archer:
                playerWeapon = GetComponent<PlayerBow>();
                break;
            case PlayerType.defender:
                break;
            case PlayerType.gunner:
                playerWeapon = GetComponent<PlayerGun>();
                break;
            case PlayerType.warrior:
                break;
            case PlayerType.wizard:
                playerWeapon = GetComponent<PlayerMage>();
                break;
        }

        if (weaponType == WeaponType.Bow)
        {
            character.Equip(character.SpriteCollection.Bow.Find(weapon => weapon.Name == weaponName), EquipmentPart.Bow);
        }

        if (weaponType == WeaponType.Firearm1H)
        {
            character.Equip(character.SpriteCollection.Shield[12], EquipmentPart.Shield);
            character.Equip(null, EquipmentPart.Back);

            character.GetFirearm().Params = FindFirearmParams(weaponName);
            character.Equip(character.SpriteCollection.Firearms1H.Find(weapon => weapon.Name == weaponName), EquipmentPart.Firearm1H);

            character.Animator.SetBool("Ready", true);
            character.Animator.SetBool("Action", false);

            // Projectile
            playerWeapon.ProjectilePrefab = equipData.gunProjectileOfLevel[level - PlayerData.gunnerMinLevel];
        }

        if (weaponType == WeaponType.Firearm2H)
        {

            character.Equip(null, EquipmentPart.Back);

            character.GetFirearm().Params = FindFirearmParams(weaponName);
            character.Equip(character.SpriteCollection.Firearms2H.Find(weapon => weapon.Name == weaponName), EquipmentPart.Firearm2H);
            
            character.Animator.SetBool("Ready", true);
            character.Animator.SetBool("Action", false);

            // Projectile
            playerWeapon.ProjectilePrefab = equipData.gunProjectileOfLevel[level - PlayerData.gunnerMinLevel];
        }

        if (weaponType == WeaponType.Mega)
        {
            character.Equip(null, EquipmentPart.Shield);
            character.Equip(null, EquipmentPart.Back);

            character.Equip(character.SpriteCollection.MeleeWeapon1H.Find(weapon => weapon.Name == weaponName), EquipmentPart.MeleeWeapon1H);
            character.Animator.SetBool("Ready", true);
            character.Animator.SetBool("Action", false);

            // Projectile
            playerWeapon.ProjectilePrefab = equipData.mageProjectileOfLevel[level - PlayerData.wizardMinLevel];
        }
    }

    private FirearmParams FindFirearmParams(string weaponName)
    {
        foreach (var collection in FirearmCollection.Instances.Values)
        {
            var found = collection.Firearms.SingleOrDefault(i => i.Name == weaponName);

            if (found != null) return found;
        }

        throw new Exception($"Can't find firearm params for {weaponName}.");
    }

    // 外部调用，角色随机化
    public void Randomize()
    {
        character.Equip(character.SpriteCollection.Helmet.Random(), EquipmentPart.Helmet);
        character.Equip(character.SpriteCollection.Armor.Random(), EquipmentPart.Armor);
        character.SetBody(character.SpriteCollection.Hair.Random(), BodyPart.Hair, CharacterExtensions.RandomColor);
        character.SetBody(character.SpriteCollection.Eyebrows.Random(), BodyPart.Eyebrows);
        character.SetBody(character.SpriteCollection.Eyes.Random(), BodyPart.Eyes, CharacterExtensions.RandomColor);
        character.SetBody(character.SpriteCollection.Mouth.Random(), BodyPart.Mouth);
    }

    private static string SpriteToString(IEnumerable<SpriteGroupEntry> collection, SpriteRenderer renderer)
    {
        return SpriteToString(collection, renderer.sprite, renderer.color);
    }

    private static string SpriteToString(IEnumerable<SpriteGroupEntry> collection, Sprite sprite, Color color)
    {
        if (sprite == null) return null;

        var entry = collection.SingleOrDefault(i => i.Sprites.Contains(sprite));

        if (entry == null)
        {
            throw new Exception($"Can't find {sprite.name} in SpriteCollection.");
        }

        var result = color == Color.white ? entry.Id : entry.Id + "#" + ColorUtility.ToHtmlStringRGBA(color);

        return result;
    }

    // 外部调用，保存装备
    private List<string> SaveEquip()
    {
        List<string> equip = new List<string>();
        equip.Add(character.Helmet.texture.name);
        equip.Add(character.ArmorRenderers[0].sprite.texture.name);
        equip.Add(character.Hair.texture.name);
        equip.Add(character.EyebrowsRenderer.sprite.texture.name);
        equip.Add(character.EyesRenderer.sprite.texture.name);
        equip.Add(character.MouthRenderer.sprite.texture.name);

        return equip;
    }

    private void LoadEquip(List<string> equipName)
    {
        character.Equip(character.SpriteCollection.Helmet.Find(item => item.Name == equipName[0]), EquipmentPart.Helmet);
        character.Equip(character.SpriteCollection.Armor.Find(item => item.Name == equipName[1]), EquipmentPart.Armor);
        character.SetBody(character.SpriteCollection.Hair.Find(item => item.Name == equipName[2]), BodyPart.Hair, CharacterExtensions.RandomColor);
        character.SetBody(character.SpriteCollection.Eyebrows.Find(item => item.Name == equipName[3]), BodyPart.Eyebrows);
        character.SetBody(character.SpriteCollection.Eyes.Find(item => item.Name == equipName[4]), BodyPart.Eyes, CharacterExtensions.RandomColor);
        character.SetBody(character.SpriteCollection.Mouth.Find(item => item.Name == equipName[5]), BodyPart.Mouth);
    }

    // 外部调用，加载人物
    public void LoadPlayer(int _level, int _exp, List<string> equipName)
    {
        level = _level;
        exp = _exp;
        LoadEquip(equipName);
        UpdateSkill();
        UpdateEquip();
    }

    // 外部调用，保存人物
    public void SavePlayer(out int _level, out int _exp, out List<string> epuipName)
    {
        _level = level;
        _exp = exp;
        epuipName = SaveEquip();
    }
}