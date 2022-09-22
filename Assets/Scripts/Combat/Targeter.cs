using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class Targeter : NetworkBehaviour
{
  [SerializeField] private Targetable target;

  public override void OnStartServer()
  {
    GameOverHandler.ServerOnGameOver += ServerHandleGameOver;
  }

  public override void OnStopServer()
  {
    GameOverHandler.ServerOnGameOver -= ServerHandleGameOver;
  }

  #region Server

  public Targetable GetTarget()
  {
    return target;
  }

  [Command]
  public void CmdSetTarget(Targetable target)
  {
    this.target = target;
  }

  [Server]
  private void ServerHandleGameOver()
  {
    ClearTarget();
  }

  [Server]
  public void ClearTarget()
  {
    this.target = null;
  }

  #endregion

  #region Client

  #endregion

}
