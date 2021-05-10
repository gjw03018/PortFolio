using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class SmithSlot : SlotClass
{
    public SmithUI smithUI;
    public ItemInfo info;

    public override void SetIcon(Sprite icon, ItemInfo info)
    {
        this.icon.sprite = icon;
        this.info = info;
    }

    public override void ClickBtn()
    {
        base.ClickBtn();
        smithUI.SetSmithUI(info, info.itemIcon, info.itemDescription, info.smith);
    }
}
