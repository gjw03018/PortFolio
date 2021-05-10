using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SmithUI : MonoBehaviour
{
    public SmithSlot[] slots;
    public RecipeSlot[] recipeSlots;

    public ItemInfo info;
    public Image icon;
    public Text Description;
    public Text effectEquip;

    string itemName;
    int recipeCount;

    bool isMaking;

    private void OnEnable()
    {
        Init();
    }

    private void Start()
    {
        SetRecipeUI();
    }

    private void Init()
    {
        info = null;
        icon.enabled = false;
        Description.text = "";
        effectEquip.text = "";
        isMaking = false;
        foreach (var i in recipeSlots)
        {
            i.SetEnable(false);
        }
        foreach (var i in recipeSlots)
            i.Init();
    }

    public void SetRecipeUI()
    {
        var list = DatabaseManager.instance.SmithList;
        for (int i = 0; i < list.Count; i++)
        {
            slots[i].SetIcon(list[i].itemIcon, list[i]);
        }
    }

    void EffectEquip(ItemInfo info)
    {
        string str = "���ݷ� : " + "<color=#ff0000>" + info.Damage + "</color>" + 
            " ���� : " + "<color=#ff00ff>" + info.Shield + "</color>" + 
            " ü�� : " + "<color=#00ff00>" + info.Health + "</color>";
        effectEquip.text = str;
    }

    public void SetSmithUI(ItemInfo info, Sprite icon, string description, List<Recipe> list)
    {
        isMaking = true; //����� �ִ��� ���� & �������ϱ����� true
        //������ ��������
        this.icon.enabled = true;
        this.info = info;
        this.icon.sprite = icon;
        this.Description.text = description;
        EffectEquip(info);//��� ����â

        foreach (var i in recipeSlots)
            i.Init();

        for (int i=0; i < list.Count; i++)
        {
            recipeSlots[i].SetIcon(list[i].itemID.itemIcon, list[i].count, list[i].itemID);
            recipeSlots[i].gameObject.SetActive(true);
            recipeSlots[i].SetEnable(true);
            //���� �Ҹ���
            recipeSlots[i].count.color = Color.red;
            recipeSlots[i].Ready = false;
            //���� ����
            if (InventoryManager.instance.invenList.ContainsKey(list[i].itemID))
            {
                if (InventoryManager.instance.invenList[list[i].itemID] >= list[i].count)
                {
                    //Debug.Log(InventoryManager.instance.invenList[list[i].itemID]);
                    recipeSlots[i].count.color = Color.green;
                    recipeSlots[i].Ready = true;
                }
            }

            isMaking &= recipeSlots[i].Ready;
        }
    }

    void SetColorText()
    {
        for (int i = 0; i < recipeSlots.Length; i++)
        {
            if (InventoryManager.instance.invenList.ContainsKey(info.smith[i].itemID))
            {
                if (InventoryManager.instance.invenList[info.smith[i].itemID] > info.smith[i].count)
                {
                    Debug.Log(InventoryManager.instance.invenList[info.smith[i].itemID]);
                    recipeSlots[i].count.color = Color.green;
                }
            }
            else
            {
                recipeSlots[i].count.color = Color.red;
                recipeSlots[i].Ready = false;
            }

            isMaking &= recipeSlots[i].Ready;

        }
    }

    public void ClickMaking()
    {
        Debug.Log(isMaking);
        if(isMaking)
        {
            for(int i = 0; i < info.smith.Count; i++)
            {
                InventoryManager.instance.Delete(info.smith[i].itemID,
                    info.smith[i].count);
            }
            InventoryManager.instance.Add(info);
            Init();
        }
    }
}
