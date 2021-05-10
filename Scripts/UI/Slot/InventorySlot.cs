using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Variable;
public class InventorySlot : SlotClass
{
    public string itemName;
    public string characterName;
    public Equipment equipType = Equipment.ETC;

    public void ItemSetting(string itemName, Equipment type)
    {
        this.itemName = itemName;
        this.equipType = type;
    }

    public override void Init()
    {
        base.Init();
        itemName = "";
        characterName = "";
    }
    public override void ClickBtn()
    {
        if(equipType != Equipment.ETC && characterName != "")
        {
            //����
            DatabaseManager.instance.CharacterInfoList[characterName].Equip(
                (int)DatabaseManager.instance.TotalList[itemName].equipType - 1,
                DatabaseManager.instance.TotalList[itemName]);
            //���� ������Ʈ
            CharacterViewUI.instance.Init();
            //������ ���� ����
            GetComponentInParent<InventoryUI>().UpdateInven();

            //Debug.Log(InventoryManager.instance.invenList.ContainsKey(DatabaseManager.instance.TotalList[itemName]));
            GetComponentInParent<InventoryUI>().gameObject.SetActive(false);
        }
    }

}
