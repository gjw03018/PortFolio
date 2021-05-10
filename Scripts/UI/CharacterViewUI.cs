using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Variable;
public class CharacterViewUI : MonoBehaviour
{
    static public CharacterViewUI instance;

    public string characterName;

    CharacterInfo info;

    public Image Icon;
    public Text Level;
    public Text Name;
    public Text Grade;
    public Text Health;
    public Text Damage;
    public Text Shield;
    public Text Speed;

    public RecipeSlot[] slots;
    public EquipSlot[] equipSlots;

    int level;
    bool isMaking = true;

    private void Awake()
    {
        instance = this;
    }
    private void OnEnable()
    {
        info = DatabaseManager.instance.CharacterInfoList[characterName];
        for(int i = 0; i < equipSlots.Length; i++)
        {
            equipSlots[i].characterName = info.Name;
        }
        Init();
    }

    void SaveCharacter()
    {
        DatabaseManager.instance.CharacterInfoList[characterName].LevelUp();
        DatabaseManager.instance.SaveCharacterDataToJson();
        //DatabaseManager.instance.LoadCharacterDataToJson();
    }

    public void Init()
    {
        Icon.sprite = DatabaseManager.instance.CharacterTotalList[info.Name].Icon;
        level = info.Level;
        Level.text = "레벨 : " + info.Level.ToString();
        Name.text = info.Name;
        Grade.text = "등급 : " + info.Grade.ToString();
        Health.text = "체력 : " + (info.Health).ToString();
        Damage.text = "공격력 : " + (info.Damage).ToString();
        Shield.text = "방어력 : " + info.Shield.ToString();
        Speed.text = "이동속도 : " + info.Speed.ToString();
        isMaking = true;

        SetRecipeSlot();

        for(int i = 0; i < equipSlots.Length; i++)
        {
            if(info.EquipList[i] != null)
            {
                equipSlots[i].icon.sprite = info.EquipList[i].itemIcon;
                equipSlots[i].SetUnequip(true);
                equipSlots[i].icon.enabled = true;
            }
            else
            {
                equipSlots[i].icon.sprite = null;
                equipSlots[i].icon.enabled = false;
                equipSlots[i].SetUnequip(false);
            }

        }
    }

    void SetRecipeSlot()
    {
        List<Recipe> list = new List<Recipe>();
        if (DatabaseManager.instance.CharacterTotalList[characterName].characterUpgarde.ContainsKey(level))
        {
            list = DatabaseManager.instance.CharacterTotalList[characterName].characterUpgarde[info.Level];
            for (int i = 0; i < list.Count; i++)
            {
                slots[i].SetIcon(list[i].itemID.itemIcon, list[i].count, list[i].itemID);
                slots[i].gameObject.SetActive(true);
                slots[i].SetEnable(true);
                if (InventoryManager.instance.invenList.ContainsKey(list[i].itemID))
                {
                    if (InventoryManager.instance.invenList[list[i].itemID] > list[i].count)
                    {
                        slots[i].count.color = Color.green;
                        slots[i].Ready = true;
                    }
                }
                else
                {
                    slots[i].count.color = Color.red;
                    slots[i].Ready = false;
                }

                isMaking &= slots[i].Ready;
            }

        }
        else
        {
            for(int i = 0; i < slots.Length; i++)
            {
                slots[i].SetEnable(false);
            }
        }
    }

    public void ClickBtn()
    {
        Debug.Log(isMaking);
        if(isMaking)
        {
            if (level < 6)
            {
                Debug.Log("Level UP");
                SaveCharacter();
                Init();
            }
        }
    }
}
