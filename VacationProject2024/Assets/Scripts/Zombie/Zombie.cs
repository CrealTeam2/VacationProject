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
    [SerializeField] bool isEnabled = true;
    [SerializeField] internal float currentPersuitTime;
    internal float attackCurTime;
    internal NavMeshAgent navMeshAgent;
    internal GameObject player;
    public string id;


    private ZombieTopLayer topLayer;


    public float Activation { get => activation; set => activation = Mathf.Clamp(value, 0, data.maxActivation); }
    public float Health { get => health; set { health = value; if (health <= 0) topLayer.ChangeState("ZombieDead"); } }
    public bool IsEnabled { get => enabled; set => enabled = value; }

    private void Awake()
    {
        id = CreateId();
        ZombieManager.Instance.RegisterZombie(id, this);

        health = data.maxHealth;
        activation = 0;
        currentPersuitTime = 0;
        attackCurTime = 0;

        player = GameObject.FindWithTag("Player");
        topLayer = new ZombieTopLayer(this);
        topLayer.OnStateEnter();

        navMeshAgent = transform.AddComponent<NavMeshAgent>();
        navMeshAgent.speed = Data.maxSpeed;
        navMeshAgent.acceleration = Data.acceleration;
        navMeshAgent.angularSpeed = Data.angularSpeed;

    }
    // Update is called once per frame
    void Update()
    {
        topLayer.OnStateUpdate();
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

    public bool DetectPlayer()
    {
        Ray ray = new Ray(transform.position, player.transform.position - transform.position);
        Debug.DrawRay(transform.position, player.transform.position - transform.position);
        RaycastHit[] hits = Physics.RaycastAll(ray, (player.transform.position - transform.position).magnitude, layerMask: LayerMask.GetMask("Wall"));
        //Array.Sort(hits, (a, b) => (a.collider.transform.position - transform.position).magnitude.CompareTo((b.collider.transform.position - transform.position).magnitude));
        if (hits.Length > 0) return false;
        return true;
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

    public override void OnStateEnter()
    {
        base.OnStateEnter();
        Debug.Log("EnterAttackState");
        count = 0;
    }
    public override void OnStateFixedUpdate()
    {
        count += Time.fixedDeltaTime;
        if(origin.Health < previousHealth)
        {
            parentLayer.ChangeState("Idle");
        }
        if(count >= 3)
        {
            if((origin.transform.position - origin.player.transform.position).magnitude <= origin.Data.attackrange)
                Debug.Log("DamagePlayer, " + GetDamage());
            parentLayer.ChangeState("Idle");
        }

    }
    public override void OnStateExit()
    {
        base.OnStateExit();
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
