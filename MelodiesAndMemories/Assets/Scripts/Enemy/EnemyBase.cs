using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.AI;
using DG.Tweening;

public enum EnemyState { patroling, chasing }

public class EnemyBase : MonoBehaviour
{
    [Range(0, 10)] public float pathSpeed;
    [Range(0, 10)] public float chaseSpeed;
    [Range(0, 20)] public float viewDistance;
    [Range(0, 15)] public int viewRange;
    [Range(0, 20)] public int viewWidth;

    public Transform player, path;
    public SoundsAtRandom ghostVoice;
    public GameController gameController;
    public AudioSource staticSound, bassSound;
    public ParticleSystem exclamation;
    public bool playerInRange, playerSeen;

    [SerializeField]
    private EnemyState currentState;

    private NavMeshAgent agent;
    private Vector3[] waypoints;
    private Ray[] viewingRays;
    private Vector3 nextWaypoint;
    private Rigidbody myRigidbody;
    private Animator myAnimator;
    private int pathCounter;

    private void Start()
    {
        myAnimator = GetComponent<Animator>();
        myRigidbody = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();

        currentState = EnemyState.patroling;
        myAnimator.SetBool("isChasing", false);
        agent.speed = pathSpeed;
        StartWaypoints();
        playerSeen = false;
        viewingRays = new Ray[viewRange];
    }

    void StartWaypoints()
    {
        pathCounter = 0;
        waypoints = new Vector3[path.childCount];

        for (int i = 0; i < path.childCount; i++)
            waypoints[i] = path.GetChild(i).position;

        NextWaitpoint(pathCounter);
        agent.SetDestination(nextWaypoint);
    }

    void FixedUpdate()
    {
        if (playerInRange)
            Search();
    }

    private void Update()
    {
        if (currentState.Equals(EnemyState.patroling))
            Patrol();
        else
            Chase();
    }

    void Search()
    {
        for (int i = -viewRange; i <= viewRange; i++)
        {
            RaycastHit rayHitData; 
            Quaternion spreadAngle = Quaternion.AngleAxis((i) * viewWidth, transform.up);
            Ray ray = new Ray(transform.position, spreadAngle * transform.forward);
            Debug.DrawRay(transform.position, spreadAngle * transform.forward * viewDistance);

            playerSeen = Physics.Raycast(ray, out rayHitData, viewDistance);

            if (playerSeen)
            {
                gameController.Spotted();
                PrepareChase();
                break;
            }
        }
    }

    void PrepareChase()
    {
        if (!currentState.Equals(EnemyState.chasing)) {

            myAnimator.SetBool("isChasing", true);

            NextAttackpoint();
            ghostVoice.PlaySounds();
            AudioManager.manager.UseSound(staticSound);
            AudioManager.manager.UseSound(bassSound);
            exclamation.Play();
            currentState = EnemyState.chasing;
        }
    }

    void Chase()
    {
        transform.LookAt(player.transform);

        if (Vector3.Distance(transform.position, nextWaypoint) <= 1)
            NextAttackpoint();
            
        if (gameController.isHidden())
        {
            myAnimator.SetBool("isChasing", false);
            currentState = EnemyState.patroling;
            agent.SetDestination(nextWaypoint);
        }
    }

    void Patrol()
    {
        if (transform.position.x.Equals(nextWaypoint.x) && transform.position.z.Equals(nextWaypoint.z))
        {
            agent.speed = pathSpeed;
            NextWaitpoint(pathCounter);
            agent.SetDestination(nextWaypoint);
            transform.DOLookAt(nextWaypoint, .3f);
            
            pathCounter++;
            if (pathCounter == path.childCount)
                pathCounter = 0;
        }
    }

    void NextWaitpoint(int pathCounter)
    {
        nextWaypoint = new Vector3(waypoints[pathCounter].x, transform.position.y, waypoints[pathCounter].z);
    }

    /* 
     * Instead of setting a FIXED destination, we continiously update the path.
     */
    private void NextAttackpoint()
    {
        NavMeshPath newPath = new NavMeshPath();

        agent.CalculatePath(player.position, newPath);
        agent.SetPath(newPath);
        agent.speed = chaseSpeed;
        nextWaypoint = new Vector3(player.position.x, transform.position.y, player.position.z);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        foreach (Transform waypoint in path)
            Gizmos.DrawSphere(waypoint.position, .3f);
    }
}