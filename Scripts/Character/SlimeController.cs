using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeController : PlayObjectClass
{
    public RandomVectorRange range;

    public override void Start()
    {
        base.Start();
        SetStatInfo(DatabaseManager.instance.MonsterInfoList["Slime"]);
    }
    private void FixedUpdate()
    {
        MonsterMove(range);
    }

    //Slime 좁은지역 전방위 공격
    public override void MainAttack()
    {
        base.MainAttack();
        attackRange.viewRadius = 2;
    }

    //발견범위를 다시 늘려주기
    public override void OffAnimation()
    {
        base.OffAnimation();
        attackRange.viewRadius = 6;
    }
}
