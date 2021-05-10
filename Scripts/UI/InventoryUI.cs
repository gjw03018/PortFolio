using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Variable;
public class InventoryUI : MonoBehaviour
{
    public InventorySlot[] slots;

    int index = 0;

    private void OnEnable()
    {
        
    }

    public void UpdateInven()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].SetEnable(false);
            slots[i].Init();
        }
        //정렬
        var dic = InventoryManager.instance.invenList.OrderBy(x => x.Key.itemID);
        //인벤 슬롯 세팅
        foreach (var pair in dic)
        {
            slots[index].SetIcon(pair.Key.itemIcon, pair.Value, pair.Key.itemType);
            slots[index].ItemSetting(pair.Key.itemName, pair.Key.equipType);
            index++;
        }
        //보이는가 안보이게 하는가
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].icon.sprite != null )
            {
                slots[i].SetEnable(true);
            }
            else
            {
                slots[i].SetEnable(false);
                slots[i].Init();
            }
        }
        index = 0;
    }
    public void SortLayoutEquip(Equipment type, string characterName)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].icon.sprite != null && slots[i].equipType == type)
            {
                slots[i].SetEnable(true);
                slots[i].characterName = characterName;
            }
            else
            {
                slots[i].SetEnable(false);
            }
        }
    }
    public void ClickUIBtn(string type) 
    {
        ItemType _type = ItemType.ETC;
        try
        {
            _type = (ItemType)System.Enum.Parse(typeof(ItemType), type);
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i].icon.sprite != null && slots[i].type == _type)
                {
                    slots[i].SetEnable(true);
                }
                else
                {
                    slots[i].SetEnable(false);
                }
            }
        }
        catch
        {
            Debug.Log("오류~맨");
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i].icon.sprite != null)
                {
                    slots[i].SetEnable(true);
                }
                else
                {
                    slots[i].SetEnable(false);
                }
            }
        }
        
    }

}
