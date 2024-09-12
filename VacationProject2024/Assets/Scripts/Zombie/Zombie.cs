using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using System;
using static UnityEngine.UI.Image;

public class Zombie : MonoBehaviour
{
    [SerializeField] ZombieData data;
    [SerializeField] internal Animator anim;
    public ZombieData Data => data;

    [SerializeField] string FSMPath;
    [SerializeField] float activation;
    [SerializeField] float health;
    [SerializeField] bool isEnabled;
    [SerializeField] internal float currentPersuitTime;
    [SerializeField] internal float activationDecreaseRate = 1.0f;
    internal Collider hitbox;
    internal float attackCurTime;
    internal NavMeshAgent navMeshAgent;
    internal Player player;
    public string id;

    private TopLayer<Zombie> topLayer;
    public Action onDeath;
    public bool isDead = false;

    public float Activation { get => activation; set => activation = Mathf.Clamp(value, 0, data.maxActivation); }

    public float Health { get { return health; } set { health = value; } }
    public bool IsEnabled
    {
        get => isEnabled; set
        {
            isEnabled = value;
/*            navMeshAgent.enabled = isEnabled;*/
        }
    }

    private void Awake()
    {
        id = CreateId();
        ZombieManager.Instance.RegisterZombie(id, this);

        health = data.maxHealth;
        activation = 0;
        currentPersuitTime = 0;
        attackCurTime = 0;

        navMeshAgent = transform.AddComponent<NavMeshAgent>();
        navMeshAgent.speed = Data.maxSpeed;
        navMeshAgent.acceleration = Data.acceleration;
        navMeshAgent.angularSpeed = Data.angularSpeed;
        navMeshAgent.enabled = false;

        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        topLayer = InitTop();
        topLayer.OnStateEnter();
        FSMPath = topLayer.GetCurrentFSM();
        topLayer.onFSMChange += () => { FSMPath = topLayer.GetCurrentFSM(); };

        hitbox = GetComponent<Collider>();

        GameManager.Instance.onGameOver += () => { isEnabled = false; };
    }
    protected virtual TopLayer<Zombie> InitTop() => new ZombieTopLayer(this);

    protected virtual void Start()
    {
        SoundManager.Instance.PlaySound(gameObject, "ZombieIdle", 0.3f, 999);
    }

    // Update is called once per frame
    void Update()
    {
        if (IsEnabled) topLayer.OnStateUpdate();
    }

    string CreateId()
    {
        string str = "";
        int[] arr = new int[3] { (int)(transform.position.z * 10) + 10000, (int)(transform.position.y * 10) + 10000, (int)(transform.position.x * 10) + 10000 };

        for (int i = 0; i < arr.Length; i++)
        {
            str = arr[i] + str;
            while (str.Length < 5 * (i + 1))
            {
                str = "0" + str;
            }
        }
        return str;
    }

    private void FixedUpdate()
    {
        if (isEnabled) topLayer.OnStateFixedUpdate();
    }

    float detectRange { get => data.baseDetectRange * (1.0f + Mathf.Min(2.0f, activation / 25.0f)); }

    public bool DetectPlayer()
    {
        Ray ray = new Ray(transform.position, player.transform.position - transform.position);
        Debug.DrawRay(transform.position, player.transform.position - transform.position);
        RaycastHit[] hits = Physics.RaycastAll(ray, (player.transform.position - transform.position).magnitude, layerMask: LayerMask.GetMask("Wall"));
        if (hits.Length > 0) return false;
        if (Vector3.Distance(player.transform.position, transform.position) > detectRange) return false;
        return true;
    }

    public void Enable()
    {
        IsEnabled = true;

        // onEnable ���õ� �κ��� ����
    }

    public void GetDamage(float damage)
    {
        if (isDead || !IsEnabled) return;
        health = Mathf.Max(health - damage, 0);
        if (health <= 0)
        {
            anim.SetTrigger("Death");
            Die();
        }
    }

    public void AddActivation(float value)
    {
        if (!isEnabled || isDead) return;
        activation = Mathf.Min(Data.maxActivation, activation + value);
    }

    public virtual void Die()
    {
        if (isDead) return;
        isDead = true;
        topLayer.ChangeState("Dead");
    }

    // Gizmos�� attackrange�� �ð������� ǥ���ϴ� �޼���
    private void OnDrawGizmosSelected()
    {
        // ������ ��ġ�� �������� attackrange�� ��Ÿ���� ���� �׸��ϴ�.
        Gizmos.color = Color.red;  // ���� ���� ����w
        Gizmos.DrawWireSphere(transform.position, data.attackrange);  // ���� ���� (attackrange) ũ���� ���� �׸��ϴ�.
    }
}

class ZombieTopLayer : TopLayer<Zombie>
{
    public ZombieTopLayer(Zombie zombie) : base(zombie)
    {
        defaultState = new ZombieIdle(zombie, this);
        AddState("Idle", defaultState);
        AddState("Pursuit", new ZombiePursuit(zombie, this));
        AddState("Attack", new ZombieAttack(zombie, this));
        AddState("PostAttack", new ZombiePostAttack(zombie, this));
        AddState("Dead", new ZombieDead(zombie, this));
    }
    public override void OnStateFixedUpdate()
    {
        if (origin.Activation > 0)
        {
            if (origin.DetectPlayer()) origin.Activation = Mathf.Max(0, origin.Activation - origin.activationDecreaseRate * Time.fixedDeltaTime);
            else origin.Activation = Mathf.Max(0, origin.Activation - origin.activationDecreaseRate * Time.fixedDeltaTime * 2.0f);
        }
        base.OnStateFixedUpdate();
    }
}

class ZombieIdle : State<Zombie>
{
    public ZombieIdle(Zombie origin, Layer<Zombie> parent) : base(origin, parent)
    {

    }

    public override void OnStateEnter()
    {
        
    }
    public override void OnStateFixedUpdate()
    {
        if ((origin.transform.position - origin.player.transform.position).magnitude <= origin.Data.attackrange)
        {
            parentLayer.ChangeState("Attack");
        }
        else if (origin.DetectPlayer() || origin.Activation >= 50.0f)
        {
            parentLayer.ChangeState("Pursuit");
        }
    }
    public override void OnStateExit()
    {

    }
}

class ZombiePursuit : State<Zombie>
{
    public ZombiePursuit(Zombie origin, Layer<Zombie> parent) : base(origin, parent)
    {

    }
    public override void OnStateEnter()
    {
        base.OnStateEnter();
        origin.currentPersuitTime = origin.Data.pursuitTime;
        origin.navMeshAgent.isStopped = false;
        origin.anim.SetBool("Pursuit", true);

        origin.navMeshAgent.enabled = true;
    }
    public override void OnStateFixedUpdate()
    {
        base.OnStateFixedUpdate();
        if ((origin.transform.position - origin.player.transform.position).magnitude <= origin.Data.attackrange)
        {
            parentLayer.ChangeState("Attack");
        }
        else if (origin.currentPersuitTime > 0)
        {
            if (origin.Activation <= 50.0f) origin.currentPersuitTime -= Time.fixedDeltaTime;
            origin.navMeshAgent.SetDestination(origin.player.transform.position);
        }
        else
        {
            origin.currentPersuitTime = 0;
            parentLayer.ChangeState("Idle");
        }
    }
    public override void OnStateExit()
    {
        base.OnStateExit();
        origin.navMeshAgent.isStopped = true;
        origin.anim.SetBool("Pursuit", false);
        origin.navMeshAgent.enabled = false;
    }
}

class ZombieAttack : State<Zombie>
{
    Zombie zombie;
    float count;
    public ZombieAttack(Zombie zombie, Layer<Zombie> parent) : base(zombie, parent)
    {
        this.zombie = zombie;
    }
    Grabbed grab = null;
    public override void OnStateEnter()
    {
        base.OnStateEnter();
        exit = false;
        grab = new Grabbed(10.0f, origin);
        grab.onDebuffEnd += ExitState;
        origin.player.AddDebuff(grab);
        count = 0;
        origin.anim.SetBool("Attacking", true);
    }
    public override void OnStateFixedUpdate()
    {
        count += Time.fixedDeltaTime;
        Vector3 pos2 = origin.transform.position;
        Vector3 pos1 = origin.player.transform.position;
        origin.transform.rotation = Quaternion.Euler(0, Mathf.Atan2(pos1.x - pos2.x, pos1.z - pos2.z) * Mathf.Rad2Deg, 0);
        if (count >= 3)
        {
            if (grab.ended == false)
            {
                grab.EndDebuff();
                origin.player.GetDamage(GetDamage(), true);
            }
            ExitState();
        }

    }
    bool exit = false;
    void ExitState()
    {
        if (exit) return;
        exit = true;
        parentLayer.ChangeState("PostAttack");
    }
    public override void OnStateExit()
    {
        base.OnStateExit();
        if (!grab.ended) grab.EndDebuff();
        grab = null;
        origin.anim.SetBool("Attacking", false);
    }

    float GetDamage()
    {
        return UnityEngine.Random.Range(zombie.Data.minDamage, zombie.Data.maxDamage);
    }
}

class ZombiePostAttack : State<Zombie>
{
    public ZombiePostAttack(Zombie origin, Layer<Zombie> parent) : base(origin, parent)
    {

    }
    float counter = 0.0f;
    public override void OnStateEnter()
    {
        base.OnStateEnter();
        counter = 0.0f;
    }
    public override void OnStateUpdate()
    {
        base.OnStateUpdate();
        if (counter < origin.Data.postAttackEndlag) counter += Time.deltaTime;
        if (counter >= origin.Data.postAttackEndlag)
        {
            if ((origin.transform.position - origin.player.transform.position).magnitude <= origin.Data.attackrange)
            {
                parentLayer.ChangeState("Attack");
            }
            else if (origin.DetectPlayer() || origin.Activation >= 50)
            {
                parentLayer.ChangeState("Pursuit");
            }
            else
            {
                parentLayer.ChangeState("Idle");
            }
        }
    }
}

class ZombieDead : State<Zombie>
{
    public ZombieDead(Zombie zombie, Layer<Zombie> parent) : base(zombie, parent)
    {

    }

    public override void OnStateEnter()
    {
        base.OnStateEnter();
        origin.onDeath?.Invoke();
        origin.hitbox.enabled = false;
        SoundManager.Instance.StopSound(origin.gameObject, "ZombieIdle");
        SoundManager.Instance.PlaySound(origin.gameObject, "ZombieScream", 1, 1);
    }
    public override void OnStateFixedUpdate()
    {
        base.OnStateFixedUpdate();
    }
    public override void OnStateExit()
    {
        base.OnStateExit();
    }
}
