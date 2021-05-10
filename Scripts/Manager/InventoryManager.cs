using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{//ItemInfo

    public Dictionary<ItemInfo, int> invenList = new Dictionary<ItemInfo, int>();
    static public InventoryManager instance;
    public InventoryUI invenUI;

    public CharacterInfo info;
    public GameObject[] Slots;

    private void Awake()
    {
        if (instance != null)
            Destroy(this.gameObject);
        else
        {
            instance = this;
        }
    }

    public void Add(ItemInfo iteminfo)
    {

        if(!invenList.ContainsKey(iteminfo))
        {
            invenList.Add(iteminfo, 1);
        }
        else
        {
            invenList[iteminfo]++;
        }
        invenUI.UpdateInven();
    }

    public void Delete(ItemInfo iteminfo, int count = 1)
    {
        Debug.Log("Delete");
        if(invenList[iteminfo] > 0)
        {
            invenList[iteminfo] -= count;
        }
        if(invenList[iteminfo] <= 0)
        {
            invenList.Remove(iteminfo);
        }
        invenUI.UpdateInven();
    }

}
