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
    [SerializeField] internal float activationDecreaseRate = 1.0f;
    internal float attackCurTime;
    internal NavMeshAgent navMeshAgent;
    internal Player player;
    public string id;

    private ZombieTopLayer topLayer;
    public Action onDeath;
    public bool isDead = false;


    public float Activation { get => activation; set => activation = Mathf.Clamp(value, 0, data.maxActivation); }
    
    public float Health { get => health; set { health = value; if (health <= 0) topLayer.ChangeState("ZombieDead"); } }
    public bool IsEnabled { get => isEnabled; set
        {
            isEnabled = value;
            navMeshAgent.enabled = isEnabled;
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
        //isEnabled = true;

        navMeshAgent = transform.AddComponent<NavMeshAgent>();
        navMeshAgent.speed = Data.maxSpeed;
        navMeshAgent.acceleration = Data.acceleration;
        navMeshAgent.angularSpeed = Data.angularSpeed;
        navMeshAgent.enabled = IsEnabled;
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
    public Action onEnable;
    public void Enable()
    {
        IsEnabled = true;
        onEnable?.Invoke();
    }
    public void GetDamage(float damage)
    {
        if (isDead) return;
        health = Mathf.Max(health - damage, 0);
        if(health <= 0)
        {
            Die();
        }
    }
    public void Die()
    {
        isDead = true;
        onDeath?.Invoke();
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
        AddState("Dead", new ZombieDead(zombie, this));
        zombie.onDeath += () => { ChangeState("Dead"); };
    }
    public override void OnStateFixedUpdate()
    {
        if (origin.Activation > 0)
        {
            if(origin.DetectPlayer()) origin.Activation = Mathf.Max(0, origin.Activation - origin.activationDecreaseRate * Time.fixedDeltaTime);
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
        SoundManager.Instance.PlaySound(origin.gameObject, "ZombieIdle", 0.5f, 1);
    }
    public override void OnStateFixedUpdate()
    {
        if ((origin.transform.position - origin.player.transform.position).magnitude <= origin.Data.attackrange)
        {
            parentLayer.ChangeState("Attack");
        }
        else if (origin.DetectPlayer() || origin.Activation >= 50)
        {
            parentLayer.ChangeState("Pursuit");
        }
    }
    public override void OnStateExit()
    {
        SoundManager.Instance.StopSound(origin.gameObject, "ZombieIdle");
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
            if(origin.Activation <= 50.0f) origin.currentPersuitTime -= Time.fixedDeltaTime;
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
