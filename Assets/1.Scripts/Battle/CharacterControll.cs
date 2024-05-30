using System;
using UnityEngine;

public class CharacterControll : MonoBehaviour
{
    public enum Status
    {
        Idle,
        Move,
        Run,
        Attack,
        Back,
        Fly,
    }

    Animator animator;

    public Status status;

    public int runPercent;
    public bool isRun = false;
    public bool attackEndRun = false;

    // ������ AttackEndRun �� üũ�Ǵ� ����
    public bool confirmAttackEndRun = false;

    // isRun �н��ϴ� ����
    public bool isPass = false;

    // attackEndRun �н��ϴ� ���� �ܷ� ����
    public bool isAttackEndPass = false;

    private float moveSpeed = 3f;
    private float runSpeed = 6f;
    public static event Action<GameObject> OnCharacterControll;
    public UnityEngine.Transform MonsterTransform {  get; set; }
    public RectTransform skillCanvasRectTransform { get; private set; }

    public GameObject textBox;

    public CharacterEffect characterEffect { get; private set; }

    // Todo : ���� �й��� ����۽� ĳ���� �ִϸ��̼� ���� ��ȯ �������
    // ���� : ���� �й��� ���� â ���� ĳ���Ͱ� �ϳ� �����Ǵ� ����

    public Vector3 StopPosition = Vector3.zero;
    private void Awake()
    {
        status = Status.Move;

        animator = GetComponentInChildren<Animator>();

        skillCanvasRectTransform = GetComponentInChildren<Canvas>().GetComponent<RectTransform>();
        characterEffect = GetComponent<CharacterEffect>();
        // �⺻ ���� ������ ���� �ϱ�
        Flip(true);
    }

    private void OnEnable()
    {
        Flip(true);
        AnimationMove();
    }

    private void FixedUpdate()
    {

        switch (status)
        {
            case Status.Idle:

                break;

            case Status.Move:
                UpdateMoveToMonster();
                break;

            case Status.Run:
                UpdateLeftMove();
                if(transform.position.x < -15)
                {
                    gameObject.SetActive(false);
                }
                break;

            case Status.Attack:

                break;

            case Status.Back:
                {
                    UpdateIdlePointMove();
                }
                break;

            case Status.Fly:
                UpdateFly();
                break;

        }
    }
    public void Flip(bool isFlip)
    {
        //transform.rotation = isFlip ? Quaternion.AngleAxis(-180, Vector3.up) : Quaternion.AngleAxis(0, Vector3.up);

        Quaternion quaternion = Quaternion.identity;
        quaternion.eulerAngles = isFlip ? new Vector3(0f, -180, 0) : Vector3.zero;

        transform.rotation = quaternion;
        skillCanvasRectTransform.rotation = quaternion;
        
        //if (textBox != null)
        //{
        //    Debug.Log(textBox.name);
        //    quaternion.eulerAngles = isFlip ? new Vector3(0f, -180, 90) : new Vector3(0f, 0, 90);
        //    textBox.transform.rotation = quaternion;
        //}

        //transform.localScale = isFlip ? new Vector3(-1, 1, 1) : new Vector3(1, 1, 1);
    }

    public void RunMode(bool isRun)
    {
        if (isPass && isRun) return;
        else if (!isRun && isAttackEndPass) return;

        int randomInt = UnityEngine.Random.Range(0, 101);

        if (this.isRun) return;

        if (isRun) this.isRun = randomInt > runPercent ? false : true;
        else attackEndRun = randomInt > runPercent ? false : true;

        if (confirmAttackEndRun) attackEndRun = true;
    }

    public void ChangeStatus(Status status)
    {
        this.status = status;
    }

    public void RunModeChange()
    {
        if (!isRun) return;

        Flip(false);
        status = Status.Run;
        characterEffect.PlayCryParticle();

        AnimationMove();
    }

    public void AttackMode()
    {
        if (isRun) return;

        AnimationMove();

        ChangeStatus(Status.Move);
    }
    public void AttackEndRunModeChange()
    {
        if (!attackEndRun) return;

        Flip(false);
        status = Status.Run;
        characterEffect.PlayCryParticle();

        AnimationMove();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Run 
        if (other.tag == "RunCollider")
        {
            // float randomTime = UnityEngine.Random.Range(0.5f, 1f); (���� ��) ������ ���� Ÿ�̹� ����
            float randomTime = UnityEngine.Random.Range(0, 2); // (���� ��) Ư���� �� Ÿ�̹� ����
            Invoke("RunModeChange", randomTime);
        }

        // ����
        if (other.tag == "Monster")
        {
            status = Status.Attack;

            animator.SetBool("Move", false);

            animator.SetBool("Attack",true);
        }
    }

    public void AnimationMove()
    {
        animator.SetBool("Attack", false);
        animator.SetBool("Idle", false);
        animator.SetBool("Move", true);
    }

    public void UpdateMoveToMonster()
    {
        Vector3 dir = MonsterTransform.position - transform.position;

        dir = new Vector3(-1f * dir.x, dir.y, dir.z);
        dir.Normalize();

        transform.Translate(dir * Time.deltaTime * moveSpeed);
    }

    public void UpdateLeftMove()
    {
        transform.Translate(new Vector3(-1, 0, 0) * Time.deltaTime * runSpeed);
    }

    public void UpdateFly()
    {
        Vector3 flyDir = new Vector3(-1, 0.5f, 0);

        flyDir.Normalize();

        transform.Translate(flyDir * Time.deltaTime * 10f, Space.World);

        // Todo : ȸ�� �߰�

        Vector3 currentRotation = transform.rotation.eulerAngles;
        currentRotation.z += Time.deltaTime * 720f;
        transform.rotation = Quaternion.Euler(currentRotation);
    }

    public void UpdateIdlePointMove()
    {
        Vector3 dir = StopPosition - transform.position;

        var distance = Vector3.Distance(StopPosition, transform.position);

        if (distance < 0.1)
        {
            ChangeStatus(Status.Idle);
            animator.SetBool("Move", false);
            animator.SetBool("Idle", true);
            Flip(true);
            OnCharacterControll?.Invoke(gameObject);
            return;
        }
        
        dir.Normalize();

        transform.Translate(dir * Time.deltaTime * moveSpeed);

    }

    public void CharacterAwake()
    {
        isPass = false;
        isAttackEndPass = false;
        attackEndRun = false;
        isRun = false;
        confirmAttackEndRun = false;
        textBox = null;
        Flip(true);
        ChangeStatus(Status.Move);
        gameObject.transform.position = Vector3.zero;

        gameObject.SetActive(true);
        AnimationMove();
        gameObject.SetActive(false);
        StopPosition = Vector3.zero;
        transform.rotation = Quaternion.identity;
    }

    public void GameRestart()
    {
        OnCharacterControll = null;
    }

    public void ChangeAnimationIdle()
    {
        animator.SetBool("Attack", false);
        animator.SetBool("Move", false);
        animator.SetBool("Idle", true);
    }

}
