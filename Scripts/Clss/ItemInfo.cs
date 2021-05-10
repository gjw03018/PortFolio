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
   public int itemID; //������ȣ
   public string itemName; // �������̸�
   public string itemDescription; //������ ����
   public ItemType itemType; //������ Ÿ��
   public ItemGrade itemGrade; //���
   public int itemSprtie; //��������Ʈ�̸�

    public int gWeight; //g����ġ
    public int jWeight; //j����ġ
    public Sprite itemIcon; // ������ ������
    [SerializeField]
    public List<Recipe> smith = new List<Recipe>();

    //���ȿ��
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

    //Dictionary  �� class �� Ű�� ����ϱ� ���� �������̵�
    public override bool Equals(object obj)
    {
        ItemInfo o = obj as ItemInfo;
        return o != null && this.itemID == o.itemID;
    }

    //itemID �� �ߺ��� ����
    public override int GetHashCode()
    {
        return itemID;
    }
}
