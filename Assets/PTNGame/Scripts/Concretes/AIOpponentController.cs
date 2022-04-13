using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class AIOpponentController : Character
{
    private NavMeshAgent agent;
    private readonly Vector3 finishDestinationPos = new Vector3(0, 0, 50f);

    private void Awake()
    {
        Init();

        agent = GetComponent<NavMeshAgent>();
        ObstacleForce = 3000f;
    }


    private void SetAgent(bool active)
    {
        if (active)
            StartCoroutine(SetAgentDestination());
        else
            agent.enabled = false;
    }

    private IEnumerator SetAgentDestination()
    {
        yield return new WaitWhile(() => CanMove);
        yield return new WaitForSeconds(1f);

        if (IsFalling)
        {
            CanMove = false;
            yield break;
        }

        CanMove = true;
        agent.enabled = true;
        agent.SetDestination(finishDestinationPos);
    }

    protected override float SetRunAniSpeed() => Math.Abs(agent.velocity.normalized.magnitude);

    protected override void OnCharacterSpawning() => SetAgent(false);

    protected override void OnSpawnObstacle() => SetAgent(false);

    protected override void OnAddForceObstacle() => SetAgent(false);

    protected override void StandingUpAniFinished() => SetAgent(true);

    protected override void OnFinishLine()
    {
        SetAgent(false);
        gameObject.SetActive(false);
    }
}