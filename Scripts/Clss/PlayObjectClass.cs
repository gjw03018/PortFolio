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
    bool isTarget; //ã������ �������� �ٶ󺻴�

    [Header("����")]
    public CharacterType type;
    bool isMove = true;//������ �ֳ� true ���� false
    bool isAttack = true;//�����Ҽ� �ֳ� true ���� false
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

    [Header("ȿ����")]
    float a;
    //ĳ���� ���� ����
    public void SetStatInfo(CharacterInfo info)
    {
        isAlive = true;
        Name = info.Name;
        Health = info.Health;
        maxHealth = info.Health;
        Damage = info.Damage;
        Shield = info.Shield;
        Speed = info.Speed;

        //��ư ������ ���
        joystick = GameObject.Find("Joystick").GetComponent<Joystick>();
        mainAttack = GameObject.Find("MainAttack").GetComponent<Button>();
        subAttack01 = GameObject.Find("SubAttack01").GetComponent<Button>();
        subAttack02 = GameObject.Find("SubAttack02").GetComponent<Button>();
        mainAttack.onClick.AddListener(MainAttack);
        subAttack01.onClick.AddListener(Attack01);
        subAttack02.onClick.AddListener(Attack02);

        //HP�� ���
        Hpbar = GameObject.Find("CFrontGround").GetComponent<Image>();
        Hpbar.fillAmount = 1; //ü���� 100%
    }
    //���� ���� ����
    public void SetStatInfo(MonsterInfo info)
    {
        isAlive = true;
        Name = info.Name;
        Health = info.Health;
        maxHealth = info.Health;
        Damage = info.Damage;
        Shield = info.Shield;
        Speed = info.Speed;
        Hpbar.fillAmount = 1; //ü���� 100%
        findTarget = false;
    }
    //���� ���� ����
    public void SetRange(float value) { attackRange.viewRadius = value; }
    public float GetRange() { return attackRange.viewRadius; }
    public List<Transform> GetTarget(float radius, float angle) { attackRange.FindVisibleTargets(radius, angle); return attackRange.visibleTargets; }
    
    //��ü ������Ʈ ���
    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        attackRange = GetComponent<FieldOfView>();
        rd = GetComponent<Rigidbody2D>();
        pixel = GetComponentInChildren<SpriteRenderer>();

        
    }

    //��ư ������ ���
    public virtual void Start()
    {
        
        //Debug.Log(this.gameObject.layer);
    }

    private void OnDisable()
    {
        Health = maxHealth;
        Hpbar.fillAmount = 1;
    }

    //�¿���� true ���� ���ݹ����� ������
    //���� ������ �⺻ ��������Ʈ�� �ݴ� �ϰ�� ���
    public void InvertFlipX(bool b)
    {
        attackRange.Flip = b;
        pixel.flipX = !b;
    }
    //���� ������ �⺻ ��������Ʈ�� ��ġ�Ұ�� ���
    public void FlipX(bool b)
    {
        attackRange.Flip = b;
        pixel.flipX = b;
    }

    //���� �޼ҵ� ����Ȱ� �߰��ϱ�
    public virtual void Reaction(float value, Transform transform)
    {
        if (this.gameObject.layer == 7) //���Ͱ� �¾����� �÷��̾ Ȯ������ ���Ѱ��
        {
            if (attackRange.visibleTargets.Count <= 0) //�÷��̸� Ȯ������ ��������
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
        //��ã�� �޵��� �ڽ���ġ, Ÿ�� ��ġ, Action
        //PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
        if(!findTarget)
            attackRange.FindVisibleTargets(6, 100);

        if (attackRange.visibleTargets.Count > 0) //�÷��̾ ã������
        {
            Vector3 endPos = attackRange.visibleTargets[0].position;
            findTarget = true;
            //�Ÿ����
            if (Vector3.Distance(transform.position, endPos) > 1.5f)
            {
                //�ڵ��̵��߿� ������ �ڵ��̵��� �����ϰ� �÷��̾� ���󰡱�
                StopCoroutine("Move");
                if (isMove)
                    PathRequestManager.RequestPath(transform.position, endPos, OnPathFound);
                //Debug.Log(Vector3.Distance(transform.position, endPos));
            }
            else
            {
                StopCoroutine("FollowPath");
                //Debug.Log("����");
                //Debug.Log(rd.velocity);
                if(isAttack)//���� ������ ����
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

    //���ݸ���� ������ ����Ǵ� �Լ�
    public virtual void OffAnimation() { isAttack = true; isMove = true;}
    //�������� ����
    public void AttackEvent(float angle)
    {
        foreach (var m in GetTarget(GetRange(), angle))
        {
            m.GetComponent<PlayObjectClass>().Reaction(Damage, this.transform);
            //Debug.Log("Attack Event : " + m.transform.name);
        }
    }
    //�ִϸ��̼� üũ
    IEnumerator CheckAnimationState(string str)
    {
        while (!animator.GetCurrentAnimatorStateInfo(0)
        .IsName(str))
        {
            //��ȯ ���� �� ����Ǵ� �κ�
            yield return null;
        }
        while (animator.GetCurrentAnimatorStateInfo(0)
        .normalizedTime < 1 && animator.GetCurrentAnimatorStateInfo(0).IsName(str))
        {
            //�ִϸ��̼� ��� �� ����Ǵ� �κ�
            yield return null;
        }
        //�ִϸ��̼� �Ϸ� �� ����Ǵ� �κ�
        //Debug.Log("��");
        OffAnimation();
    }
    //���� �ڵ��̵�
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
            //���Լ��� �ٽ� ����ɰ�� ���� �� ã�� ��θ� �����ϰ� �ٽ� ����
            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
        }
    }

    //��ã�� (�̵�)
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
