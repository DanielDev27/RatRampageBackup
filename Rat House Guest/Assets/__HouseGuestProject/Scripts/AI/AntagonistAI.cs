using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Action = AI.UtilitySystem.Action;

[CreateAssetMenu(fileName = "AIUtilityActionObject", menuName = "AI/Utility/ActionObject", order = 0)]
public class AntagonistAI : ActionObject
{
    public static AntagonistAI Instance;
    [Header("Settings")]
    [SerializeField] private float updateFrequency;
    [SerializeField] private AIActions aIActionsRef;
    [Header("References")]
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private List<Action> actions = new List<Action>();
    [SerializeField] private Action bestAction;
    [SerializeField] private CharacterController characterController;
    public override void Awake()
    {
        Instance = this;
    }
    public override void OnEnable() { }
    public override void OnDisable() { }
    public override void Start()
    {
        antagonistActions.characterController = CharacterController.Instance;
        antagonistActions.target = null;
        antagonistActions.canSeeTarget = false;
        agent = antagonistActions.agent;
        actions = new List<Action>()
        {
            new IdleAction(),
            new RoamAction(),
            new AttackAction(),
        };
        antagonistActions.agent.destination = antagonistActions.roamPoints[antagonistActions.roamIndex].position;
        antagonistActions.distanceToTarget = Vector3.Distance(antagonistActions.characterController.transform.position, antagonistActions.transform.position);
        antagonistActions.remainingDistance = Vector3.Distance(antagonistActions.agent.destination, antagonistActions.transform.position);
        antagonistActions.StartCoroutine(UtilitySystemCoroutine());
        AudioManager.Instance.PlayRatRoam();
    }

    public override void Update()
    {
        if (antagonistActions.agent.isActiveAndEnabled)
        {
            if (antagonistActions.target == null || !antagonistActions.canSeeTarget)
            {
                antagonistActions.aiTransform.LookAt(agent.nextPosition);
            }
            if (antagonistActions.target != null && antagonistActions.canSeeTarget)
            {
                antagonistActions.aiTransform.LookAt(new Vector3(antagonistActions.target.position.x, antagonistActions.transform.position.y, antagonistActions.target.position.z));
            }
        }
        antagonistActions.distanceToTarget = Vector3.Distance(antagonistActions.characterController.transform.position, antagonistActions.transform.position);
        antagonistActions.remainingDistance = new Vector3(antagonistActions.agent.destination.x - antagonistActions.transform.position.x, 0, antagonistActions.agent.destination.z - antagonistActions.transform.position.z).magnitude;
        //VisionCheck();
        FieldOfViewCheck();
    }

    public override void FixedUpdate() { }

    public override void OnDrawGizmos() { }

    IEnumerator UtilitySystemCoroutine()
    {
        while (true)
        {
            EvaluateUtility();
            yield return new WaitForSeconds(updateFrequency);
        }
    }
    private void EvaluateUtility()
    {
        bestAction = null;
        float _bestUtility = float.NegativeInfinity;
        foreach (var _action in actions)
        {
            float _utility = _action.Evaluate(this);
            //Debug.Log ($"Evaluating: {_action} | {_utility}");
            if (_utility > _bestUtility)
            {
                _bestUtility = _utility;
                bestAction = _action;
            }
        }

        //Debug.Log ($"<color=green>{soldierActions.name} Executing: {_bestAction}</color>");
        bestAction?.Execute(this);
    }

    public void VisionCheck()
    {
        RaycastHit _hit;
        //Debug.DrawRay(antagonistActions.rayTransform.position, antagonistActions.transform.TransformDirection(antagonistActions.targetDirection) * 20, Color.red);
        if (Physics.Raycast(antagonistActions.rayTransform.position, antagonistActions.targetDirection, out _hit, 20, antagonistActions.ratLayerMask))
        {
            //Debug.Log(_hit.collider);
            Debug.DrawRay(antagonistActions.rayTransform.position, antagonistActions.targetDirection * 20, Color.red);
        }
        else
        {
            //Debug.Log(_hit.collider);
            Debug.DrawRay(antagonistActions.rayTransform.position, antagonistActions.targetDirection * 20, Color.green);
        }
    }

    public void FieldOfViewCheck()
    {
        RaycastHit _hit;
        antagonistActions.targetDirection = antagonistActions.characterController.transform.position - antagonistActions.rayTransform.position;
        if (Vector3.Angle(antagonistActions.rayTransform.forward, antagonistActions.targetDirection) <= antagonistActions.fovAngle / 2)
        {
            bool hitLayer = Physics.Raycast(antagonistActions.rayTransform.position, antagonistActions.targetDirection, out _hit, 20, antagonistActions.blockedLayers, QueryTriggerInteraction.Ignore);
            if (hitLayer && _hit.collider.TryGetComponent<CharacterController>(out CharacterController _characterController) && antagonistActions.distanceToTarget < antagonistActions.visionRange)
            {
                antagonistActions.canSeeTarget = true;
                antagonistActions.target = _characterController.transform;
                antagonistActions.aiTransform.LookAt(new Vector3(antagonistActions.target.position.x, antagonistActions.target.position.y, antagonistActions.target.position.z));
                Debug.DrawRay(antagonistActions.rayTransform.position, antagonistActions.targetDirection * 20, Color.red);
            }
            else
            {
                Debug.DrawRay(antagonistActions.rayTransform.position, antagonistActions.targetDirection * 20, Color.green);
                antagonistActions.canSeeTarget = false;
                antagonistActions.target = null;
            }
        }
    }
    public GameObject MakeProjectile()
    {
        GameObject currentProjectile;
        Transform spawnTransform = antagonistActions.projectileSpawn;
        currentProjectile = Instantiate(antagonistActions.projectile[UnityEngine.Random.Range(0, antagonistActions.projectile.Length)]);
        return currentProjectile;
    }
}


public class IdleAction : Action
{
    public override float Evaluate(ActionObject actionObject)
    {
        if (actionObject.antagonistActions.remainingDistance < 0.1f && actionObject.antagonistActions.waitCount < 5)
        {
            return 1 / actionObject.antagonistActions.remainingDistance;
        }
        else
        {
            return 0;
        }
    }
    public override void Execute(ActionObject actionObject)
    {
        AudioManager.Instance.PlayRatRoam();
        actionObject.antagonistActions.isMoving = false;
        actionObject.antagonistActions.isIdle = true;
        actionObject.antagonistActions.isAttacking = false;
        actionObject.antagonistActions.agent.isStopped = true;
        while (actionObject.antagonistActions.waitCount < 0.5)
        {
            actionObject.antagonistActions.isSearching = true;
            actionObject.antagonistActions.waitCount += Time.fixedDeltaTime;
            return;
        }
        if (actionObject.antagonistActions.remainingDistance < 0.1f && actionObject.antagonistActions.waitCount >= 0.5)
        {
            actionObject.antagonistActions.roamIndex++;
            if (actionObject.antagonistActions.roamIndex >= actionObject.antagonistActions.roamPoints.Count)
            {
                actionObject.antagonistActions.roamIndex = 0;
            }
            actionObject.antagonistActions.agent.destination = actionObject.antagonistActions.roamPoints[actionObject.antagonistActions.roamIndex].position;
            actionObject.antagonistActions.waitCount = 0f;
            actionObject.antagonistActions.isSearching = false;
        }
    }
}

public class RoamAction : Action
{
    public override float Evaluate(ActionObject actionObject)
    {
        float val = 0;
        float _targetDistance = ((AntagonistAI)actionObject).antagonistActions.distanceToTarget;
        if (actionObject.antagonistActions.waitCount < 0.5 && actionObject.antagonistActions.remainingDistance < 0.1f)
        {
            val = 0;
        }
        else
            switch (actionObject.antagonistActions.remainingDistance)
            {
                case >= 0.1f when ((AntagonistAI)actionObject).antagonistActions.target != null:
                    {
                        switch (_targetDistance)
                        {
                            case > 1: val = 1; break;
                            case < 1: val = 0; break;
                        }
                        break;
                    }
                case >= 0.1f:
                    val = actionObject.antagonistActions.remainingDistance;
                    break;
                case < 0.1f:
                    val = 0;
                    break;
            }
        return val;
    }
    public override void Execute(ActionObject actionObject)
    {
        actionObject.antagonistActions.isIdle = false;
        actionObject.antagonistActions.isMoving = true;
        actionObject.antagonistActions.isAttacking = false;
        actionObject.antagonistActions.agent.isStopped = false;
        actionObject.antagonistActions.agent.SetDestination(actionObject.antagonistActions.roamPoints[actionObject.antagonistActions.roamIndex].position);
        actionObject.antagonistActions.agent.speed = 2.5f;
        if (actionObject.antagonistActions.currentClip == actionObject.antagonistActions.walkingAudio[1])
        {
            actionObject.antagonistActions.walkingAudio[0].Play();
            actionObject.antagonistActions.currentClip = actionObject.antagonistActions.walkingAudio[0];
        }
        else
        {
            actionObject.antagonistActions.walkingAudio[1].Play();
            actionObject.antagonistActions.currentClip = actionObject.antagonistActions.walkingAudio[1];
        }
        while (actionObject.antagonistActions.agent.pathPending || actionObject.antagonistActions.remainingDistance > actionObject.antagonistActions.agent.stoppingDistance + 0.01f)
        {
            return;
        }
    }
}
internal class AttackAction : Action
{
    public override float Evaluate(ActionObject actionObject)
    {
        float _val = 0;
        float _targetDistance = ((AntagonistAI)actionObject).antagonistActions.distanceToTarget;
        if (actionObject.antagonistActions.target != null)
        {
            switch (_targetDistance)
            {
                case > 5:
                    _val = 0;
                    break;
                case <= 5:
                    _val = actionObject.antagonistActions.visionRange / _targetDistance;
                    break;
            }
        }
        return _val;
    }

    public override void Execute(ActionObject actionObject)
    {
        AudioManager.Instance.PlayRatChase();
        actionObject.antagonistActions.isMoving = false;
        actionObject.antagonistActions.isIdle = false;
        actionObject.antagonistActions.isAttacking = true;
        actionObject.antagonistActions.agent.isStopped = true;
    }
}
namespace AI.UtilitySystem
{
    public abstract class Action
    {
        public abstract float Evaluate(ActionObject actionObject);

        public abstract void Execute(ActionObject actionObject);
    }
}

