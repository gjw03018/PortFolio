using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecipeSlot : SlotClass
{
    public ItemInfo info;
    public CharacterInfo characterinfo;

    int recipeCount;
    public bool Ready;
    public override void SetIcon(Sprite icon, int count, ItemInfo info)
    {
        this.icon.sprite = icon;
        recipeCount = count;
        this.count.text = count.ToString();
        this.info = info;
    }

    public override void SetIcon(CharacterInfo info)
    {
        characterinfo = info;
    }

    public override void Init()
    {
        info = null;
        this.icon.sprite = null;
        this.icon.enabled = false;
        this.count.text = "";
        this.count.enabled = false;
        this.gameObject.SetActive(false);
        Ready = true;
    }
}
