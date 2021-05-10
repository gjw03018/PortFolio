using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Variable;
public class SlotClass : MonoBehaviour
{
    public Image icon;
    public Text count;
    public ItemType type;

   public virtual void Init()
    {
        icon.sprite = null;
        count.text = "";
        type = ItemType.ETC;
    }
    public virtual void SetIcon(Sprite icon, int count, ItemType type)
    {
        this.icon.sprite = icon;
        this.count.text = count.ToString();
        this.type = type;
    }
    public virtual void SetIcon(Sprite icon, int count, ItemInfo info) { }
    public virtual void SetIcon(Sprite icon, ItemInfo info){}
    public virtual void SetIcon(CharacterInfo info){}
    public void SetEnable(bool b)
    {
        icon.enabled = b;
        count.enabled = b;
        this.gameObject.SetActive(b);
    }

    public virtual void ClickBtn() { Debug.Log("ClickBtn"); }
}
