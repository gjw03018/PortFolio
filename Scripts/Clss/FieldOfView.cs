using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    public float viewRadius;
    [Range(0,360)]
    public float viewAngle;

    public LayerMask targetMask;
    public LayerMask obstacleMask;

    //�¿����
    public bool Flip;
    //�����ȿ� ������Ʈ
    public List<Transform> visibleTargets = new List<Transform>();

    public void FindVisibleTargets(float radius = 4, float angle = 90)
    {
        visibleTargets.Clear();
        //���׶�� �ȿ� �ݶ��̴� ���� ��������
        Collider2D[] targetInViewRadius = Physics2D.OverlapCircleAll(transform.position, viewRadius, targetMask);

        //������ �ݶ��̴��� �����ȿ� �ִ� ��� ã��
        for (int i =0; i < targetInViewRadius.Length; i++)
        {
            Transform target = targetInViewRadius[i].transform;
            //�ٶ󺸴� ����
            Vector3 dirToTarget = (target.position - transform.position).normalized;
            //�ٶ󺸴� ����
            Vector3 point = transform.right;

            //�¿� ����
            if(!Flip)
                point = -point;
            if (Vector3.Angle(point, dirToTarget) < viewAngle / 2)
            {
                //Debug.Log(Vector3.Angle(point, dirToTarget));
                //����
                float dstToTarget = Vector3.Distance(transform.position, target.position);
                //����ĳ��Ʈ
                if (!Physics2D.Raycast(transform.position, dirToTarget, dstToTarget, obstacleMask))
                {
                    visibleTargets.Add(target);
                }
            }
        }
    }


    public Vector3 DirFromAngle(float angleInDegress, bool angleIsGloabal)
    {
        //������ǥ �ϰ��� �ƴѰ��
        if(!angleIsGloabal)
        {
            angleInDegress += transform.eulerAngles.z;
        }
        //�⺻ ��ǥ��� ����Ƽ�� ��ǥ�� ����
        return new Vector3(Mathf.Cos(angleInDegress * Mathf.Deg2Rad), Mathf.Sin(angleInDegress * Mathf.Deg2Rad), 0);
    }

}
