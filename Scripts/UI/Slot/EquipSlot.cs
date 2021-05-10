using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Variable;
public class EquipSlot : SlotClass
{
    //캐릭터 이름
    public string characterName;
    public string itemName;

    public ItemInfo info;
    public GameObject unequipBtn;

    public InventoryUI invenUI;

    public Equipment equipType;
    private void OnEnable()
    {
        if (icon.sprite == null)
        {
            unequipBtn.SetActive(false);
        }
    }

    public void SetUnequip(bool b)
    {
        unequipBtn.SetActive(b);
    }

    public void UnEquipBtn()
    {
        DatabaseManager.instance.CharacterInfoList[characterName].UnEquip((int)equipType-1,
            DatabaseManager.instance.CharacterInfoList[characterName].EquipList[(int)equipType - 1]);
        CharacterViewUI.instance.Init();
    }

    public override void ClickBtn()
    {
        invenUI.SortLayoutEquip(equipType, characterName);
    }
}
