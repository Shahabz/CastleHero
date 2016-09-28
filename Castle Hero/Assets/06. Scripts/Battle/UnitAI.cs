using System.Collections;
using UnityEngine;

public enum UnitState
{
    Idle = 0,
    Move,
    Attack,
    Die,
}

public class UnitAI : MonoBehaviour
{
    [SerializeField] UnitLevelData unitLevelData;
    [SerializeField] string unitName;
    [SerializeField] int unitId;
    [SerializeField] int level;
    [SerializeField] UnitState state;

    [SerializeField] GameObject targetUnit;

    NavMeshAgent navAgent;

    //Animator animator;
    
    public void InitializeUnit(string newName, int newId, int newLevel)
    {
        unitName = newName;
        unitId = newId;
        level = newLevel;
        unitLevelData = UnitDatabase.Instance.GetUnitData(unitId).GetLevelData(level);
        navAgent = GetComponent<NavMeshAgent>();
        //animator = GetComponent<Animator>();
        state = UnitState.Idle;
        navAgent.speed = unitLevelData.MoveSpeed;

        StartCoroutine(Process());
    }

    public IEnumerator Process()
    {
        while (state != UnitState.Die)
        {
            if(targetUnit != null)
            {
                Attack();
            }
            else
            {
                SetTarget();
            }

            yield return new WaitForSeconds(1.0f);
        }
    }

    public void SetTarget()
    {
        GameObject[] targets = new GameObject[10];

        if (gameObject.tag == "HomeUnit")
        {
            targets = GameObject.FindGameObjectsWithTag("AwayUnit");
        }
        else if (gameObject.tag == "AwayUnit")
        {
            targets = GameObject.FindGameObjectsWithTag("HomeUnit");
        }

        float distance = 10000;

        foreach (GameObject awayUnit in targets)
        {
            float newDistance = Vector3.Distance(transform.position, awayUnit.transform.position);

            Debug.Log(Vector3.Distance(transform.position, awayUnit.transform.position));
            
            if (newDistance < distance)
            {
                distance = newDistance;
                targetUnit = awayUnit;
            }
        }
    }

    public void Move()
    {
        state = UnitState.Move;
        navAgent.SetDestination(targetUnit.transform.position);

        //animator.SetBool("isMoving", true);
    }

    public void Attack()
    {
        float distance = GetComponent<SphereCollider>().radius + targetUnit.GetComponent<SphereCollider>().radius;

        Debug.Log(Vector3.Distance(transform.position, targetUnit.transform.position) - distance);
        Debug.Log(UnitDatabase.Instance.unitData[unitId].AttackRange);

        if (Vector3.Distance(transform.position, targetUnit.transform.position) - distance > UnitDatabase.Instance.unitData[unitId].AttackRange)
        {
            Move();
        }
        else
        {
            state = UnitState.Attack;
            navAgent.Stop();
        }
    }

    public IEnumerator AttackCheck()
    {
        while(state == UnitState.Attack)
        {
            if (state == UnitState.Attack)
                break;

            targetUnit.GetComponent<UnitAI>().unitLevelData.Damaged(unitLevelData.Attack);

            float attackDelay = 1f / unitLevelData.AttackSpeed;

            yield return new WaitForSeconds(attackDelay);
        }
    }
}