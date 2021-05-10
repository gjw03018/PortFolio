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

    //좌우반전
    public bool Flip;
    //범위안에 오브젝트
    public List<Transform> visibleTargets = new List<Transform>();

    public void FindVisibleTargets(float radius = 4, float angle = 90)
    {
        visibleTargets.Clear();
        //동그라미 안에 콜라이더 정보 가져오기
        Collider2D[] targetInViewRadius = Physics2D.OverlapCircleAll(transform.position, viewRadius, targetMask);

        //가져온 콜라이더가 범위안에 있는 경우 찾기
        for (int i =0; i < targetInViewRadius.Length; i++)
        {
            Transform target = targetInViewRadius[i].transform;
            //바라보는 방향
            Vector3 dirToTarget = (target.position - transform.position).normalized;
            //바라보는 기준
            Vector3 point = transform.right;

            //좌우 반전
            if(!Flip)
                point = -point;
            if (Vector3.Angle(point, dirToTarget) < viewAngle / 2)
            {
                //Debug.Log(Vector3.Angle(point, dirToTarget));
                //길이
                float dstToTarget = Vector3.Distance(transform.position, target.position);
                //레이캐스트
                if (!Physics2D.Raycast(transform.position, dirToTarget, dstToTarget, obstacleMask))
                {
                    visibleTargets.Add(target);
                }
            }
        }
    }


    public Vector3 DirFromAngle(float angleInDegress, bool angleIsGloabal)
    {
        //월드좌표 일경우와 아닌경우
        if(!angleIsGloabal)
        {
            angleInDegress += transform.eulerAngles.z;
        }
        //기본 좌표계와 유니티계 좌표를 보정
        return new Vector3(Mathf.Cos(angleInDegress * Mathf.Deg2Rad), Mathf.Sin(angleInDegress * Mathf.Deg2Rad), 0);
    }

}
