using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Variable;
using System.IO;
using Newtonsoft.Json;

public class DatabaseManager : MonoBehaviour
{
    static public DatabaseManager instance;

    public Sprite[] sprites;

    public Dictionary<string, GameObject> PlayerObjects = new Dictionary<string, GameObject>();
    public Dictionary<string, CharacterInfo> CharacterTotalList = new Dictionary<string, CharacterInfo>();
    public Dictionary<string, CharacterInfo> CharacterInfoList = new Dictionary<string, CharacterInfo>();
    public Dictionary<string, MonsterInfo> MonsterInfoList = new Dictionary<string, MonsterInfo>();

    const string characterPath = "/Resources/JsonCharacter.json";
    const string selectPath = "/Resources/JsonSelectCharacter.json";

    public Dictionary<string, ItemInfo> TotalList = new Dictionary<string, ItemInfo>(); //������ ��Ż ����Ʈ
    public List<ItemInfo> SmithList = new List<ItemInfo>();//���� ������
    public List<ItemInfo> RecipeList = new List<ItemInfo>();//��� ������
    public List<ItemInfo> UpgardeList = new List<ItemInfo>();//��ȭ ������
    public GameObject[] objects;

    private void Awake()
    {
        if (instance != null)
            Destroy(this.gameObject);
        else
        {
            DontDestroyOnLoad(this.gameObject);
            instance = this;
        }

        //��Ƽ ��������Ʈ ��������
        sprites = Resources.LoadAll<Sprite>("ItemIcon/Icon");

        SetItemList();
        SetCharacterList();
        SetMonsterList();
    }
    private void Start()
    {
        if(File.Exists(Application.dataPath + characterPath))
        {
            LoadCharacterDataToJson();
        }
        else
        {
            SaveCharacterDataToJson();
        }
    }
    #region - ������ �б� -
    void SetItemList()
    {
        
        //csv ���� �б�
        List<Dictionary<string, object>> data = CSVReader.Read("ItemDataBase");
        string str = "";
        for (var i = 0; i < data.Count; i++)
        {
            TotalList.Add(
                (string)data[i]["Name"],
                new ItemInfo(
                (int)data[i]["ID"],
                (string)data[i]["Name"],
                (string)data[i]["Description"],
                (ItemType)System.Enum.Parse(typeof(ItemType), (string)data[i]["Type"]),
                (int)data[i]["GWeight"],
                (int)data[i]["Jweight"],
                (ItemGrade)System.Enum.Parse(typeof(ItemGrade), (string)data[i]["Grade"]),
                sprites[(int)data[i]["Sprite"]]
                )

                );
            //��� ������ ����Ʈ
            if ((string)data[i]["Type"] == "Equip")
            {
                str = data[i]["Material"].ToString();
                string[] c_data = str.Split('-');
                for (int j = 0; j < c_data.Length; j += 2)
                {
                    TotalList[(string)data[i]["Name"]].SetSmith(TotalList[c_data[j]], int.Parse(c_data[j + 1]));
                }
                //��� ȿ��
                TotalList[(string)data[i]["Name"]].AddEquipPoint(
                    (int)data[i]["Health"],
                    (int)data[i]["Shield"],
                    (int)data[i]["Damage"],
                    (Equipment)System.Enum.Parse(typeof(Equipment), (string)data[i]["Equip"])
                    );
                SmithList.Add( TotalList[(string)data[i]["Name"]] );
            }
            //��� ������ ����Ʈ
            if ((string)data[i]["Type"] == "Smith")
            {
                RecipeList.Add(TotalList[(string)data[i]["Name"]]);
            }
            //��ȭ �ƾ��� ����Ʈ
            if ((string)data[i]["Type"] == "Upgrade")
            {
                UpgardeList.Add(TotalList[(string)data[i]["Name"]]);
            }
        }
        
    }
    #endregion

    #region - ĳ���� �б� -
    void SetCharacterList()
    {
        //csv ���� �б�
        List<Dictionary<string, object>> data = CSVReader.Read("CharacterDataBase");
        string str = "";
        for (var i = 0; i < data.Count; i++)
        {
            CharacterTotalList.Add(
                (string)data[i]["Name"],
                new CharacterInfo(
                (string)data[i]["Name"],
                (int)data[i]["Level"],
                (string)data[i]["Description"],
                (int)data[i]["Health"],
                (int)data[i]["Damage"],
                (int)data[i]["Shield"],
                (int)data[i]["Speed"],
                Resources.Load<Sprite>("Character/Icon/" + (string)data[i]["Sprite"]) as Sprite,
                (CharacterGrade)System.Enum.Parse(typeof(CharacterGrade), (string)data[i]["Grade"])
                )
                );

            PlayerObjects.Add((string)data[i]["Name"], objects[i]);

            if (!File.Exists(Application.dataPath + characterPath))
            {
                CharacterInfoList.Add(
                    (string)data[i]["Name"],
                    new CharacterInfo(
                    (string)data[i]["Name"],
                    (int)data[i]["Level"],
                    (string)data[i]["Description"],
                    (int)data[i]["Health"],
                    (int)data[i]["Damage"],
                    (int)data[i]["Shield"],
                    (int)data[i]["Speed"],
                    (string)data[i]["Sprite"],
                    (CharacterGrade)System.Enum.Parse(typeof(CharacterGrade), (string)data[i]["Grade"])
                    )
                    );
            }

            for(int z = 0; z < 5; z++)
            {
                string index = "Upgarde_0" + (z + 1).ToString();
                str = data[i][index].ToString();
                string[] c_data = str.Split('-');
                List<Recipe> list = new List<Recipe>();
                for (int j = 0; j < c_data.Length; j += 2)
                {
                    list.Add(new Recipe(TotalList[c_data[j]],int.Parse(c_data[j+1])));
                }
                CharacterTotalList[(string)data[i]["Name"]].characterUpgarde.Add(z+1, new List<Recipe>(list));
                //Debug.Log((z+1) + (string)data[i]["Name"] + "\tBefor\t" + CharacterTotalList[(string)data[i]["Name"]].characterUpgarde[z+1].Count);

                list.Clear();
                //Debug.Log((z+1) + (string)data[i]["Name"] + "\tAfter\t" + CharacterTotalList[(string)data[i]["Name"]].characterUpgarde[z+1].Count);

            }

            /*string json = JsonUtility.ToJson(CharacterTotalList[(string)data[i]["Name"]].characterUpgarde[1]);
            Debug.Log(json);*/


        }
    }
    #endregion

    #region - ���� �ϱ� -
    /**/
    void SetMonsterList()
    {

        //csv ���� �б�
        List<Dictionary<string, object>> data = CSVReader.Read("MonsterInfo");
        for (var i = 0; i < data.Count; i++)
        {
            MonsterInfoList.Add(
                (string)data[i]["Name"],
                new MonsterInfo(
                (string)data[i]["Name"],
                (int)data[i]["Level"],
                (int)data[i]["Health"],
                (int)data[i]["Damage"],
                (int)data[i]["Shield"],
                (int)data[i]["Speed"]
                )
                );
        }

    }
    #endregion
    public void SaveCharacterDataToJson()
    {
        string jdata = JsonConvert.SerializeObject(CharacterInfoList);
        File.WriteAllText(Application.dataPath + characterPath, jdata);
    }
    public void LoadCharacterDataToJson()
    {
        string jdata = File.ReadAllText(Application.dataPath + characterPath);
        CharacterInfoList = JsonConvert.DeserializeObject<Dictionary<string, CharacterInfo>>(jdata);
    }

    public void SaveSelectCharacterDataToJson()
    {
        string jdata = JsonConvert.SerializeObject(GameingManager.instance.SelectChacter);
        File.WriteAllText(Application.dataPath + selectPath, jdata);
    }

    public void LoadSelectCharacterDataToJson()
    {
        string jdata = File.ReadAllText(Application.dataPath + selectPath);
        GameingManager.instance.SelectChacter = JsonConvert.DeserializeObject<CharacterInfo>(jdata);
    }
}
