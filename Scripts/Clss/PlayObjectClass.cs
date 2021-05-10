using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Variable;

[System.Serializable]
public class PlayObjectClass : MonoBehaviour
{
    Animator animator;
    protected FieldOfView attackRange;
    Joystick joystick;
    protected Rigidbody2D rd;
    SpriteRenderer pixel;

    public string Name;
    public float Health;
    float maxHealth;
    public float Damage;
    public float Shield;
    public float Speed;


    [Header("Astar")]
    Vector3[] path;
    int targetIndex;
    bool isTarget; //찾았을때 그쪽으로 바라본다

    [Header("상태")]
    public CharacterType type;
    bool isMove = true;//걸을수 있냐 true 없다 false
    bool isAttack = true;//공격할수 있냐 true 없다 false
    protected bool findTarget;
    protected bool isAlive;
    bool isRandomMove = true;


    [Header("UI")]
    Button mainAttack;
    Button subAttack01;
    Button subAttack02;
    public Image Hpbar;
    public float attack01CoolTime;
    public float attack02CoolTime;

    [Header("효과음")]
    float a;
    //캐릭터 정보 세팅
    public void SetStatInfo(CharacterInfo info)
    {
        isAlive = true;
        Name = info.Name;
        Health = info.Health;
        maxHealth = info.Health;
        Damage = info.Damage;
        Shield = info.Shield;
        Speed = info.Speed;

        //버튼 리스너 등록
        joystick = GameObject.Find("Joystick").GetComponent<Joystick>();
        mainAttack = GameObject.Find("MainAttack").GetComponent<Button>();
        subAttack01 = GameObject.Find("SubAttack01").GetComponent<Button>();
        subAttack02 = GameObject.Find("SubAttack02").GetComponent<Button>();
        mainAttack.onClick.AddListener(MainAttack);
        subAttack01.onClick.AddListener(Attack01);
        subAttack02.onClick.AddListener(Attack02);

        //HP바 등록
        Hpbar = GameObject.Find("CFrontGround").GetComponent<Image>();
        Hpbar.fillAmount = 1; //체력이 100%
    }
    //몬스터 정보 세팅
    public void SetStatInfo(MonsterInfo info)
    {
        isAlive = true;
        Name = info.Name;
        Health = info.Health;
        maxHealth = info.Health;
        Damage = info.Damage;
        Shield = info.Shield;
        Speed = info.Speed;
        Hpbar.fillAmount = 1; //체력이 100%
        findTarget = false;
    }
    //공격 범위 설정
    public void SetRange(float value) { attackRange.viewRadius = value; }
    public float GetRange() { return attackRange.viewRadius; }
    public List<Transform> GetTarget(float radius, float angle) { attackRange.FindVisibleTargets(radius, angle); return attackRange.visibleTargets; }
    
    //객체 컴포넌트 등록
    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        attackRange = GetComponent<FieldOfView>();
        rd = GetComponent<Rigidbody2D>();
        pixel = GetComponentInChildren<SpriteRenderer>();

        
    }

    //버튼 리스너 등록
    public virtual void Start()
    {
        
        //Debug.Log(this.gameObject.layer);
    }

    private void OnDisable()
    {
        Health = maxHealth;
        Hpbar.fillAmount = 1;
    }

    //좌우반전 true 기준 공격범위는 오른쪽
    //공격 범위와 기본 스프라이트가 반대 일경우 사용
    public void InvertFlipX(bool b)
    {
        attackRange.Flip = b;
        pixel.flipX = !b;
    }
    //공격 범위와 기본 스프라이트가 일치할경우 사용
    public void FlipX(bool b)
    {
        attackRange.Flip = b;
        pixel.flipX = b;
    }

    //행위 메소드 공통된거 추가하기
    public virtual void Reaction(float value, Transform transform)
    {
        if (this.gameObject.layer == 7) //몬스터가 맞았을때 플레이어를 확인하지 못한경우
        {
            if (attackRange.visibleTargets.Count <= 0) //플레이를 확인하지 못했을대
            {
                isRandomMove = false;
                findTarget = true;
                StopCoroutine("Move");
                attackRange.visibleTargets.Add(transform);
                PathRequestManager.RequestPath(transform.position, transform.position, OnPathFound);
            }
        }
        if (Shield < value)
        {
            if(isAttack)
                animator.SetTrigger("Reaction");
            Hpbar.fillAmount = Health / maxHealth;
            if (Health > 0)
            {
                Health -= (value - Shield);
            }
            else
            {
                Death();
                StageManager.instance.StageEnd();
            }
        }
    }
    public virtual void Death() 
    { 
        animator.SetTrigger("Death");
        isAlive = false;
        this.gameObject.SetActive(false);
    }
    public virtual void CharacterMove() 
    {
        if(isMove)
        {
            Vector2 dir = new Vector2(joystick.Horizontal, joystick.Vertical);

            rd.MovePosition(rd.position + dir * Speed * Time.fixedDeltaTime);

            animator.SetFloat("Speed", dir.magnitude);
            if (joystick.Horizontal > 0)
                InvertFlipX(true);
            else if(joystick.Horizontal < 0)
                InvertFlipX(false);
        }
    }
    public virtual void MonsterMove(RandomVectorRange range)
    {
        //길찾기 메도스 자신위치, 타겟 위치, Action
        //PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
        if(!findTarget)
            attackRange.FindVisibleTargets(6, 100);

        if (attackRange.visibleTargets.Count > 0) //플레이어를 찾았을때
        {
            Vector3 endPos = attackRange.visibleTargets[0].position;
            findTarget = true;
            //거리재기
            if (Vector3.Distance(transform.position, endPos) > 1.5f)
            {
                //자동이동중에 만나면 자동이동을 중지하고 플레이어 따라가기
                StopCoroutine("Move");
                if (isMove)
                    PathRequestManager.RequestPath(transform.position, endPos, OnPathFound);
                //Debug.Log(Vector3.Distance(transform.position, endPos));
            }
            else
            {
                StopCoroutine("FollowPath");
                //Debug.Log("중지");
                //Debug.Log(rd.velocity);
                if(isAttack)//공격 가능한 상태
                {
                    MainAttack(); //animator "Attack01"
                }
            }

            isTarget = transform.position.x - endPos.x > 0 ? false : true;
            FlipX(isTarget);
        }

        else
        {
            findTarget = false;
            Vector3 pos = range.RandomVector3();
            if (isRandomMove)
                StartCoroutine("Move", pos);

        }
    }
    public virtual void MainAttack() 
    {
        if(isAttack)
        {
            isAttack = false;
            isMove = false;
            animator.SetTrigger("Attack01");
            StartCoroutine("CheckAnimationState", "Attack01");
        }
    }
    public virtual void Attack01() 
    {
        if (isAttack)
        {
            isAttack = false;
            isMove = false;
            animator.SetTrigger("Attack02");
            StartCoroutine("CheckAnimationState", "Attack02");
            subAttack01.GetComponent<AttackBtn>().StartCollTime(attack01CoolTime);
        }
    }
    public virtual void Attack02() 
    {
        if (isAttack)
        {
            isAttack = false;
            isMove = false;
            animator.SetTrigger("Attack03");
            StartCoroutine("CheckAnimationState", "Attack03");
            subAttack02.GetComponent<AttackBtn>().StartCollTime(attack02CoolTime);
        }
    }
    public virtual void MonsterAttack() { }

    //공격모션이 끝나고 실행되는 함수
    public virtual void OffAnimation() { isAttack = true; isMove = true;}
    //실직적인 공격
    public void AttackEvent(float angle)
    {
        foreach (var m in GetTarget(GetRange(), angle))
        {
            m.GetComponent<PlayObjectClass>().Reaction(Damage, this.transform);
            //Debug.Log("Attack Event : " + m.transform.name);
        }
    }
    //애니메이션 체크
    IEnumerator CheckAnimationState(string str)
    {
        while (!animator.GetCurrentAnimatorStateInfo(0)
        .IsName(str))
        {
            //전환 중일 때 실행되는 부분
            yield return null;
        }
        while (animator.GetCurrentAnimatorStateInfo(0)
        .normalizedTime < 1 && animator.GetCurrentAnimatorStateInfo(0).IsName(str))
        {
            //애니메이션 재생 중 실행되는 부분
            yield return null;
        }
        //애니메이션 완료 후 실행되는 부분
        //Debug.Log("끝");
        OffAnimation();
    }
    //몬스터 자동이동
    IEnumerator Move(Vector3 pos)
    {
        isRandomMove = false;
        while (true)
        {
            transform.position = Vector3.MoveTowards(transform.position, pos, Speed / 2 * Time.deltaTime);

            bool a = transform.position.x - pos.x > 0 ? false : true;
            FlipX(a);

            if (Vector3.Distance(transform.position, pos) < 1.0f)
            {
                isRandomMove = true;
                break;
            }
            yield return null;
        }
    }

    

    #region - Astar -
    public void OnPathFound(Vector3[] newPath, bool pathSuccessful)
    {
        if (pathSuccessful && isAlive)
        {
            path = newPath;
            //이함수가 다시 실행될경우 기존 에 찾은 경로를 무시하고 다시 시작
            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
        }
    }

    //길찾기 (이동)
    IEnumerator FollowPath()
    {
        if (path.Length > 0)
        {
            Vector3 currentWayPoint = path[0];

            while (true)
            {
                if (transform.position == currentWayPoint)
                {
                    targetIndex++;
                    if (targetIndex >= path.Length)
                    {
                        yield break;
                    }
                    currentWayPoint = path[targetIndex];
                }
                transform.position = Vector3.MoveTowards(transform.position, currentWayPoint, Speed * Time.deltaTime);
                yield return null;
            }

        }
    }

    public void OnDrawGizmos()
    {
        if (path != null)
        {
            for (int i = targetIndex; i < path.Length; i++)
            {
                Gizmos.color = Color.black;
                Gizmos.DrawCube(path[i], Vector3.one);

                if (i == targetIndex)
                {
                    Gizmos.DrawLine(transform.position, path[i]);
                }
                else
                {
                    Gizmos.DrawLine(path[i - 1], path[i]);
                }
            }
        }
    }
    #endregion
}
