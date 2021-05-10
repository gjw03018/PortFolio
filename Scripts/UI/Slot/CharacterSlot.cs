using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CharacterSlot : MonoBehaviour
{
    public Text Name;
    public Image Icon;
    public Text Description;
    public CharacterInfo characterInfo;

    public CharacterViewUI View;
    
    public void SetSlot(string name, Sprite icon, string description, CharacterInfo info)
    {
        this.Name.text = name;
        this.Icon.sprite = icon;
        this.Description.text = description;
        this.characterInfo = info;
    }

    public void ClickBtn()
    {
        View.characterName = characterInfo.Name;
    }
}
