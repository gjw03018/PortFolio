using UnityEngine;
using System.Collections;

public class MonsterInfo
{
    //기본정보
    public string Name;
    public int Level;
    public float Health;
    public float Damage;
    public float Shield;
    public float Speed;

    public MonsterInfo(string name, int level, float health, float damage, float shield, float speed)
    {
        Name = name;
        Level = level;
        Health = health;
        Damage = damage;
        Shield = shield;
        Speed = speed;
    }
}
