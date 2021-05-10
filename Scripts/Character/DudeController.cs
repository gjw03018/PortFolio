using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DudeController : PlayObjectClass
{
    public override void Start()
    {
        base.Start();
        SetStatInfo(DatabaseManager.instance.CharacterInfoList["Dude"]);

        //특수공격에 쿨타임 넣어주기
        attack01CoolTime = 5;
        attack02CoolTime = 3;
    }
    private void FixedUpdate()
    {
        CharacterMove();
    }
    //Dude - 주위 횡베기
    public override void MainAttack()
    {
        base.MainAttack();
    }
    //Dude - 전방 지속공격
    public override void Attack01()
    {
        base.Attack01();
    }
    //Dude - 전방 칼꽂아 공격
    public override void Attack02()
    {
        base.Attack02();
    }
}
