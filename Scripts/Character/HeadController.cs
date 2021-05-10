using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadController : PlayObjectClass
{
    public SpriteRenderer point;
    bool UP; //�ö��ִ� �������� üũ

    public override void Start()
    {
        base.Start();
        SetStatInfo(DatabaseManager.instance.CharacterInfoList["Head"]);

        //Ư�����ݿ� ��Ÿ�� �־��ֱ�
        attack01CoolTime = 15;
        attack02CoolTime = 15;
    }
    private void FixedUpdate()
    {
        CharacterMove();
    }
    //Head - ������ ���� ����
    public override void MainAttack()
    {
        base.MainAttack();
        SetRange(3);
    }

    //Head - ��������
    public override void Attack01()
    {
        if (!UP)
            return;
        
        base.Attack01();
        point.enabled = false;
        SetRange(1.5f);
        UP = false;
    }

    //Head - �ö󰡱�
    public override void Attack02()
    {
        if (UP)
            return;
        base.Attack02();
        point.enabled = true;
        UP = true;

    }

}
