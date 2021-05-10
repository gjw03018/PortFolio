using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ResultView : MonoBehaviour
{
    const int count = 10;
    public Image[] icon = new Image[count];
    public List<ItemInfo> resultList = new List<ItemInfo>();
    public void SetIcon(ref List<ItemInfo> list)
    {
        for (int i =0; i < list.Count; i++)
        {
            icon[i].sprite = list[i].itemIcon;
            InventoryManager.instance.Add(list[i]);
        }

        for (int i = 0; i < count; i++)
        {
            if (icon[i].sprite != null)
            {
                icon[i].gameObject.SetActive(true);
                resultList.Add(list[i]);
            }
            else
                icon[i].gameObject.SetActive(false);
        }

        list.Clear();
    }

    public void ClickOkBtn()
    {
        for (int i = 0; i < count; i++)
        {
            icon[i].sprite = null;
        }
        resultList.Clear();
    }
}
