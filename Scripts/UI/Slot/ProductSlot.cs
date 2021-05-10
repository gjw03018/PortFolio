using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Variable;

public class ProductSlot : MonoBehaviour
{
    Wallet wallet;
    public ResultView resultView; //���â
    public Text priceText;
    public int Price;

    public GoodsType type; //�Ҹ� ��ȭ

    public char selectList; //���� ������ ����Ʈ�� �������� ����
    public List<ItemInfo> RandomDrawList = new List<ItemInfo>(); // ���� �������� ���ִ� ����Ʈ
    public List<ItemInfo> DrawList = new List<ItemInfo>(); // ���� �������� �� �ִ� ����Ʈ
    
    int gTotal = 0;
    int jTotal = 0;

    private void OnEnable()
    {
        priceText.text = Price.ToString();
    }
    private void Start()
    {
        switch(selectList)
        {
            case 'S':
                //��� ������
                RandomDrawList = DatabaseManager.instance.RecipeList;
                break;
            default:
                //��ȭ ������
                RandomDrawList = DatabaseManager.instance.UpgardeList;
                break;
        }


        for (int i = 0; i < RandomDrawList.Count; i++)
        {
            gTotal += RandomDrawList[i].gWeight;
        }
        for (int i = 0; i < RandomDrawList.Count; i++)
        {
            jTotal += RandomDrawList[i].jWeight;
        }
    }

    public void ClickBuyButton(int count)
    {
        wallet = Wallet.instance;
        switch (type)
        {
            case GoodsType.Gold:
                if(wallet.Gold >= Price)
                    wallet.Gold -= Price;
                RandomDraw(gTotal, 'g', count);
                resultView.SetIcon(ref DrawList);
                break;
            case GoodsType.Jewelry:
                if(wallet.Jewelry >= Price)
                    wallet.Jewelry -= Price;
                RandomDraw(jTotal, 'j', count);
                resultView.SetIcon(ref DrawList);
                break;
        }
        Wallet.instance.ChangeGoods();
    }

    void RandomDraw(int total, char c, int count = 1)
    {
        for(int i = 0; i < count; i++)
        {
            DrawList.Add(RandomItem(total, c));
        }
    }

    public ItemInfo RandomItem(int total, char c)
    { 
        int weight = 0;
        int selectNum = 0;
        selectNum = Mathf.RoundToInt(total * Random.Range(0.0f, 1.0f));

        for(int i = 0; i < RandomDrawList.Count; i++)
        {
            if (c == 'g')
                weight += RandomDrawList[i].gWeight;
            else
                weight += RandomDrawList[i].jWeight;

            if(selectNum <= weight)
            {
                ItemInfo temp = new ItemInfo(RandomDrawList[i]);
                return temp;
            }
        }
        return null;
    }

   
}
