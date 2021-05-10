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

    //Slime �������� ������ ����
    public override void MainAttack()
    {
        base.MainAttack();
        attackRange.viewRadius = 2;
    }

    //�߰߹����� �ٽ� �÷��ֱ�
    public override void OffAnimation()
    {
        base.OffAnimation();
        attackRange.viewRadius = 6;
    }
}
