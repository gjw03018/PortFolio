using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Variable;
using UnityEngine.UI;
struct Upgarde
{
    public ItemInfo info;
    public int count;
}

public class CharacterInfo
{
    //기본정보
    public string Name;
    public int Level;
    public string Description;
    public float Health;
    public float Damage;
    public float Shield;
    public float Speed;
    public string spriteID;
    //추가정보
    public Sprite Icon;
    public CharacterGrade Grade;
    levelpoint point = new levelpoint();
    //업그레이드정보
    public Dictionary<int, List<Recipe>> characterUpgarde = new Dictionary<int, List<Recipe>>();
    //착용장비
    public ItemInfo[] EquipList = new ItemInfo[5];
    public void LevelUp()
    {
        float point = this.point.GetPoint(Level);
        Health *= point;
        Damage *= point;
        Shield *= point;
        Speed  *= point;
        Level++;
    }
    public CharacterInfo() { }
    public void Equip(int index, ItemInfo info)
    {
        if(EquipList[index] != null)
        {
            UnEquip(index, EquipList[index]);
        }

        EquipList[index] = info;
        Health += info.Health;
        Damage += info.Damage;
        Shield += info.Shield;
        InventoryManager.instance.Delete(info);
    }

    public void UnEquip(int index, ItemInfo info)
    {
        InventoryManager.instance.Add(info);
        Health -= info.Health;
        Damage -= info.Damage;
        Shield -= info.Shield;
        EquipList[index] = null;
        Debug.Log(info.itemName);
    }

    public CharacterInfo(string name, int level, string description, float health, float damage, float shield, float speed, string sprite, CharacterGrade grade)
    {
        Name = name;
        Level = level;
        Description = description;
        Health = health;
        Damage = damage;
        Shield = shield;
        Speed = speed;
        Grade = grade;
        spriteID = sprite;
    }

    public CharacterInfo(string name, int level, string description, float health, float damage, float shield, float speed, Sprite icon, CharacterGrade grade)
    {
        Name = name;
        Level = level;
        Description = description;
        Health = health;
        Damage = damage;
        Shield = shield;
        Speed = speed;
        Icon = icon;
        Grade = grade;
    }

}