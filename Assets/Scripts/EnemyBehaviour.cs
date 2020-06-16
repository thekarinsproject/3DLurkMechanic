using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehaviour : MonoBehaviour
{
    [SerializeField]
    int speed;

    // Enemy possible states in game
    [SerializeField]
    enum EnemyStates { STALK, CHASE }

    // Mark if the enemy is in frenzy status
    [SerializeField]
    bool frenzied;

    [SerializeField]
    // The amount of frenzy acumulated so far
    float frenzyCharge;

    // The frenzy charge rate (x1 ,x2, etc...)
    [SerializeField]
    int frenzyChargeRate;

    [SerializeField]
    float distanceToPlayer;

    EnemyStates currentState;

    [SerializeField]
    GameObject player;

    NavMeshAgent navMesh;


    private void Awake()
    {
        navMesh = GetComponent<NavMeshAgent>();
    }

    // Start is called before the first frame update
    void Start()
    {
        frenzied = false;
        frenzyCharge = 0;
        frenzyChargeRate = 1;
        currentState = EnemyStates.STALK;
        navMesh.speed = 2f;

    }

    // Update is called once per frame
    void Update()
    {
        // check if frenzy is full and change to chase state
        if (frenzyCharge >= 100) {
            currentState = EnemyStates.CHASE;
            frenzied = true;
        }
    }

    private void FixedUpdate()
    {
        
        switch (currentState) {
            case EnemyStates.STALK:
                StalkPlayer();
                break;
            case EnemyStates.CHASE:
                ChasePlayer();
                break;
            default:
                Debug.LogError($"The state {currentState} does not exist");
                break;
        }
        transform.LookAt(player.transform); // Look at player's position
        navMesh.SetDestination(player.transform.position);
    }

    void StalkPlayer() {
        distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        if (distanceToPlayer > 10)
        {
            navMesh.speed = 2f;
            frenzyChargeRate = 1;
        }
        else {
            navMesh.speed = 1f;
            frenzyChargeRate = 5;
        }
        RaycastHit hit;

        // if raycast touches an object and it has the Player tag
        if (Physics.Raycast(this.transform.position, transform.TransformDirection(Vector3.forward) * 1000, out hit, Mathf.Infinity) && hit.collider.tag.Equals("Player")) {
            frenzyCharge += (1f / 100) * frenzyChargeRate;
        }

        // Shows the ray in the scene
        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.red);

    }

    void ChasePlayer() {
        navMesh.speed = 4.5f;
        if (frenzyCharge > 0 && frenzied)
        {
            frenzyCharge -= 10f / 100; // decrease 0.5
        }
        else {
            frenzyCharge = 0;
            currentState = EnemyStates.STALK;
            frenzied = false;
        }
    }
}
