using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelController : PlayObjectClass
{
    public GameObject bullet;
    public GameObject bigBullet;

    public float dash;
    public override void Start()
    {
        base.Start();
        SetStatInfo(DatabaseManager.instance.CharacterInfoList["Wheel"]);

        //Ư�����ݿ� ��Ÿ�� �־��ֱ�
        attack01CoolTime = 5;
        attack02CoolTime = 3;
    }
    private void FixedUpdate()
    {
        CharacterMove();
    }

    //Wheel - ���� �ѽ��
    public override void MainAttack()
    {
        base.MainAttack();
        float rAngle = Random.Range(-45, 45);
        Vector2 dd = new Vector2(Mathf.Cos(rAngle * Mathf.Deg2Rad), Mathf.Sin(rAngle * Mathf.Deg2Rad));
        Vector2 pos;//������ ��ġ��
        Vector2 pos2;//�Ѿ��� ���ư��� ����
        bool flip;//�¿����
        if (attackRange.Flip)//������
        {
            pos = new Vector2(0.85f + transform.position.x, 0.9f + transform.position.y);
            pos2 = (Vector2.right + dd).normalized;
            flip = false;
        }
        else
        {
            pos = new Vector2(-0.85f + transform.position.x, 0.9f + transform.position.y);
            pos2 = (Vector2.left + dd).normalized;
            flip = true;
        }
        GameObject b = Instantiate(bullet, pos, transform.rotation);
        b.GetComponent<Bullect>().player = this.transform;
        b.GetComponent<Rigidbody2D>().AddForce(pos2 * 10, ForceMode2D.Impulse);
        b.GetComponent<SpriteRenderer>().flipX = flip;

    }

    //Wheel - �뽬
    public override void Attack01()
    {
        base.Attack01();
        if (attackRange.Flip)//������ �뽬
            rd.AddForce(Vector2.right* dash, ForceMode2D.Impulse);
        
        else
            rd.AddForce(Vector2.left* dash, ForceMode2D.Impulse);
        
    }
    //Wheel - �ٰŸ� ������ �ѽ��
    public override void Attack02()
    {
        base.Attack02();

        Vector2 pos;//������ ��ġ��
        Vector2 pos2;//�Ѿ��� ���ư��� ����
        bool flip;//�¿����
        if (attackRange.Flip)//������
        {
            pos = new Vector2(1.0f + transform.position.x, 0.9f + transform.position.y);
            pos2 = (Vector2.right).normalized;
            flip = false;
        }
        else
        {
            pos = new Vector2(-1.0f + transform.position.x, 0.9f + transform.position.y);
            pos2 = (Vector2.left).normalized;
            flip = true;
        }
        GameObject b = Instantiate(bigBullet, pos, transform.rotation);
        b.GetComponent<BigBullect>().SetBullet(5, Damage/3, this.transform, 0.2f);
        b.GetComponent<Rigidbody2D>().AddForce(pos2 * 2, ForceMode2D.Impulse);
        b.GetComponent<SpriteRenderer>().flipX = flip;
    }
}
