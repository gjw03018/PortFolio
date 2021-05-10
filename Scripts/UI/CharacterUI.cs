using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterUI : MonoBehaviour
{
    public CharacterSlot[] slots;
    int index = 0;

    private void Start()
    {
        foreach(var i in DatabaseManager.instance.CharacterTotalList)
        {
            slots[index].SetSlot(i.Value.Name, i.Value.Icon, i.Value.Description, i.Value);
            index++;
        }
        index = 0;
    }

    //ĳ���� ���� ��ư
    public void ClickSelectBtn(int index)
    {
        GameingManager.instance.SelectCharacter(slots[index].characterInfo);
        Wallet.instance.ChangeSprite();
    }

    //ĳ���� ���׷��̵� ��ư
    public void ClickBtn()
    {
        Debug.Log("Select Btn");
        DatabaseManager.instance.SaveCharacterDataToJson();
    }
}
