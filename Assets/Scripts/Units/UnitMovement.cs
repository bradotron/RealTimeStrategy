using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.AI;
using System;

public class UnitMovement : NetworkBehaviour
{
  [SerializeField] private NavMeshAgent agent = null;
  [SerializeField] private Targeter targeter = null;
  [SerializeField] private float chaseRange;

  public override void OnStartServer()
  {
    GameOverHandler.ServerOnGameOver += ServerHandleGameOver;
  }

  public override void OnStopServer()
  {
    GameOverHandler.ServerOnGameOver -= ServerHandleGameOver;
  }

  #region Server

  [ServerCallback]
  private void Update()
  {
    Targetable target = targeter.GetTarget();
    if (target != null)
    {
      if ((target.transform.position - transform.position).sqrMagnitude > (chaseRange * chaseRange))
      {
        agent.SetDestination(target.transform.position);
      }
      else if (agent.hasPath)
      {
        agent.ResetPath();
      }

      return;
    }

    if (!agent.hasPath) { return; }
    if (agent.remainingDistance > agent.stoppingDistance) { return; }

    agent.ResetPath();
  }

  [Command]
  public void CmdMove(Vector3 newPosition)
  {
    targeter.ClearTarget();

    if (!NavMesh.SamplePosition(newPosition, out NavMeshHit hit, 1f, NavMesh.AllAreas)) { return; }

    agent.SetDestination(hit.position);
  }

  [Server]
  private void ServerHandleGameOver()
  {
    agent.ResetPath();
  }

  #endregion

  #region Client

  #endregion
}
