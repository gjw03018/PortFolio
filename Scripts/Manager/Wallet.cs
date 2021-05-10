using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Wallet : MonoBehaviour
{
    static public Wallet instance;

    public int Gold = 100000;
    public int Jewelry = 100000;

    public Text goldText;
    public Text jewelryText;
    public Image SelectCharacterSprite;

    private void Awake()
    {
        if (instance != null)
            Destroy(this.gameObject);
        else
        {
            instance = this;
        }
    }

    private void Start()
    {
        Debug.Log("TestMessage");
        ChangeSprite();
        //������ Ŭ���� �ϰų� ���������� ������ȭ�� �߰���Ų��
        if (GameingManager.instance.Gold != 0 || GameingManager.instance.Jewerly != 0)
        {
            Gold += GameingManager.instance.Gold;
            Jewelry += GameingManager.instance.Jewerly;
            GameingManager.instance.Gold = 0;
            GameingManager.instance.Jewerly = 0;
        }
        ChangeGoods();
    }
    public void ChangeSprite()
    {
        SelectCharacterSprite.sprite = GameingManager.instance.CharacterIcon();
    }
    public void ChangeGoods()
    {
        goldText.text = Gold.ToString();
        jewelryText.text = Jewelry.ToString();
    }

    public void AddMoney(int Gold, int Jewelry)
    {
        this.Gold += Gold;
        this.Jewelry += Jewelry;
        ChangeGoods();
    }
}
