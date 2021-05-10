using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadController : PlayObjectClass
{
    public SpriteRenderer point;
    bool UP; //올라가있는 상태인지 체크

    public override void Start()
    {
        base.Start();
        SetStatInfo(DatabaseManager.instance.CharacterInfoList["Head"]);

        //특수공격에 쿨타임 넣어주기
        attack01CoolTime = 15;
        attack02CoolTime = 15;
    }
    private void FixedUpdate()
    {
        CharacterMove();
    }
    //Head - 주위에 광역 공격
    public override void MainAttack()
    {
        base.MainAttack();
        SetRange(3);
    }

    //Head - 내려가기
    public override void Attack01()
    {
        if (!UP)
            return;
        
        base.Attack01();
        point.enabled = false;
        SetRange(1.5f);
        UP = false;
    }

    //Head - 올라가기
    public override void Attack02()
    {
        if (UP)
            return;
        base.Attack02();
        point.enabled = true;
        UP = true;

    }

}
