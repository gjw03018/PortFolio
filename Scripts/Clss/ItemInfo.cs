using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Variable;
public struct Recipe
{
    public ItemInfo itemID;
    public int count;

    public Recipe(ItemInfo itemID, int count)
    {
        this.itemID = itemID;
        this.count = count;
    }
}
[System.Serializable]
public class ItemInfo
{
   public int itemID; //고유번호
   public string itemName; // 아이템이름
   public string itemDescription; //아이템 설명
   public ItemType itemType; //아이템 타입
   public ItemGrade itemGrade; //등급
   public int itemSprtie; //스프라이트이름

    public int gWeight; //g가중치
    public int jWeight; //j가중치
    public Sprite itemIcon; // 아이템 아이콘
    [SerializeField]
    public List<Recipe> smith = new List<Recipe>();

    //장비효과
    public int Health;
    public int Shield;
    public int Damage;
    public Equipment equipType;

    public void SetSmith(ItemInfo info, int count)
    {
        smith.Add(new Recipe(info, count));
    }

    public void AddEquipPoint(int Health, int Shield, int Damage, Equipment type)
    {
        this.Health = Health;
        this.Shield = Shield;
        this.Damage = Damage;
        this.equipType = type;
    }

    public ItemInfo(int itemID, string itemName, string itemDescription, ItemType itemType, int gWeight, int jWeight, ItemGrade itemGrade, Sprite sprite)
    {
        this.itemID = itemID;
        this.itemName = itemName;
        this.itemDescription = itemDescription;
        this.itemType = itemType;
        this.gWeight = gWeight;
        this.jWeight = jWeight;
        this.itemGrade = itemGrade;
        this.itemIcon = sprite;
    }

    public ItemInfo(ItemInfo iteminfo)
    {
        this.itemID = iteminfo.itemID;
        this.itemName = iteminfo.itemName;
        this.itemDescription = iteminfo.itemDescription;
        this.itemType = iteminfo.itemType;
        this.gWeight = iteminfo.gWeight;
        this.jWeight = iteminfo.jWeight;
        this.itemGrade = iteminfo.itemGrade;
        this.itemSprtie = iteminfo.itemSprtie;
        this.itemIcon = iteminfo.itemIcon;
    }

    //Dictionary  에 class 를 키로 사용하기 위한 오버라이딩
    public override bool Equals(object obj)
    {
        ItemInfo o = obj as ItemInfo;
        return o != null && this.itemID == o.itemID;
    }

    //itemID 는 중복이 없음
    public override int GetHashCode()
    {
        return itemID;
    }
}
