using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using System;

public class Zombie : MonoBehaviour
{
    [SerializeField] ZombieData data;
    public ZombieData Data => data;

    [SerializeField] float activation;
    [SerializeField] float health;
    [SerializeField] bool isEnabled;
    [SerializeField] internal float currentPersuitTime;
    internal float attackCurTime;
    internal NavMeshAgent navMeshAgent;
    internal Player player;
    public string id;


    private ZombieTopLayer topLayer;
    public Action onDeath;


    public float Activation { get => activation; set => activation = Mathf.Clamp(value, 0, data.maxActivation); }
    
    public float Health { get => health; set { health = value; if (health <= 0) topLayer.ChangeState("ZombieDead"); } }
    public bool IsEnabled { get => isEnabled; set => isEnabled = value; }
    private void Awake()
    {
        id = CreateId();
        ZombieManager.Instance.RegisterZombie(id, this);

        health = data.maxHealth;
        activation = 0;
        currentPersuitTime = 0;
        attackCurTime = 0;
        //isEnabled = true;

        navMeshAgent = transform.AddComponent<NavMeshAgent>();
        navMeshAgent.speed = Data.maxSpeed;
        navMeshAgent.acceleration = Data.acceleration;
        navMeshAgent.angularSpeed = Data.angularSpeed;
    }

    private void Start()
    {


        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        topLayer = new ZombieTopLayer(this);
        topLayer.OnStateEnter();



    }
    // Update is called once per frame
    void Update()
    {
        if(IsEnabled) topLayer.OnStateUpdate();
    }

    string CreateId()
    {
        string str = "";
        int[] arr = new int[3] { (int)(transform.position.z * 10) + 10000, (int)(transform.position.y * 10) + 10000, (int)(transform.position.x * 10) + 10000 };

        for(int i = 0; i < arr.Length; i++)
        {
            str = arr[i] + str;
            while(str.Length < 5*(i+1))
            {
                str = "0"+str;
            }
        }
        return str;
    }

    private void FixedUpdate()
    {
        topLayer.OnStateFixedUpdate();
    }
    float detectRange { get => data.baseDetectRange * (1.0f + Mathf.Min(2.0f, activation / 20.0f)); }
    public bool DetectPlayer()
    {
        Ray ray = new Ray(transform.position, player.transform.position - transform.position);
        Debug.DrawRay(transform.position, player.transform.position - transform.position);
        RaycastHit[] hits = Physics.RaycastAll(ray, (player.transform.position - transform.position).magnitude, layerMask: LayerMask.GetMask("Wall"));
        if (hits.Length > 0) return false;
        if (Vector3.Distance(player.transform.position, transform.position) > detectRange) return false;
        return true;
    }
    public Action onEnable;
    public void Enable()
    {
        IsEnabled = true;
        onEnable.Invoke();
    }
}

class ZombieTopLayer : TopLayer<Zombie>
{
    public ZombieTopLayer(Zombie zombie) : base(zombie)
    {
        defaultState = new ZombieIdle(zombie, this);
        AddState("Idle", defaultState);
        AddState("Attack", new ZombieAttack(zombie, this));
        AddState("Dead", new ZombieDead(zombie, this));

    }
}

class ZombieIdle : State<Zombie>
{
    public ZombieIdle(Zombie origin, Layer<Zombie> parent) : base(origin, parent)
    {

    }

    public override void OnStateEnter()
    {
        SoundManager.Instance.PlaySound(origin.gameObject, "ZombieIdle", 0.5f, 1);
    }
    public override void OnStateFixedUpdate()
    {
        if (origin.DetectPlayer() || origin.Activation >= 50)
            origin.currentPersuitTime = origin.Data.pursuitTime;
        origin.currentPersuitTime -= Time.fixedDeltaTime;

        if (origin.currentPersuitTime > 0)
        {
            origin.navMeshAgent.isStopped = false;
            origin.navMeshAgent.SetDestination(origin.player.transform.position);
        }
        else origin.navMeshAgent.isStopped = true;

        if((origin.transform.position - origin.player.transform.position).magnitude <= origin.Data.attackrange)
        {
            origin.navMeshAgent.isStopped = true;
            parentLayer.ChangeState("Attack");
        }
    }
    public override void OnStateExit()
    {
        SoundManager.Instance.StopSound(origin.gameObject, "ZombieIdle");
    }
}

class ZombieAttack : State<Zombie>
{
    Zombie zombie;
    float count;
    float previousHealth;
    public ZombieAttack(Zombie zombie, Layer<Zombie> parent) : base(zombie, parent)
    {
        this.zombie = zombie;
        previousHealth = origin.Health;
    }
    Grabbed grab = null;
    public override void OnStateEnter()
    {
        base.OnStateEnter();
        grab = new Grabbed(10.0f, origin);
        origin.player.AddDebuff(grab);
        count = 0;
    }
    public override void OnStateFixedUpdate()
    {
        count += Time.fixedDeltaTime;
        if(count >= 3)
        {
            if(grab.ended == false)
            {
                Debug.Log("DamagePlayer, " + GetDamage());
                grab.EndDebuff();
            }
            parentLayer.ChangeState("Idle");
        }

    }
    public override void OnStateExit()
    {
        base.OnStateExit();
        if (!grab.ended) grab.EndDebuff();
        grab = null;
    }

    float GetDamage()
    {
        return UnityEngine.Random.Range(zombie.Data.minDamage, zombie.Data.maxDamage);
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
        origin.onDeath.Invoke();
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
