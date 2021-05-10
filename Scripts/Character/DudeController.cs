using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DudeController : PlayObjectClass
{
    public override void Start()
    {
        base.Start();
        SetStatInfo(DatabaseManager.instance.CharacterInfoList["Dude"]);

        //Ư�����ݿ� ��Ÿ�� �־��ֱ�
        attack01CoolTime = 5;
        attack02CoolTime = 3;
    }
    private void FixedUpdate()
    {
        CharacterMove();
    }
    //Dude - ���� Ⱦ����
    public override void MainAttack()
    {
        base.MainAttack();
    }
    //Dude - ���� ���Ӱ���
    public override void Attack01()
    {
        base.Attack01();
    }
    //Dude - ���� Į�Ⱦ� ����
    public override void Attack02()
    {
        base.Attack02();
    }
}
