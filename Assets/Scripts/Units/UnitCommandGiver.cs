using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UnitCommandGiver : MonoBehaviour
{
  [SerializeField] private UnitSelectionHandler unitSelectionHandler;
  [SerializeField] private LayerMask layerMask = new LayerMask();

  private Camera mainCamera;

  private void Start()
  {
    mainCamera = Camera.main;

    GameOverHandler.ClientOnGameOver += ClientHandleGameOver;
  }

  private void ClientHandleGameOver(string winnerName)
  {
    enabled = false;
  }

  private void Update()
  {
    if (!Mouse.current.rightButton.wasPressedThisFrame) { return; }

    Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

    if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layerMask)) { return; }

    if (hit.collider.TryGetComponent<Targetable>(out Targetable target))
    {
      if (target.hasAuthority)
      {
        TryMove(hit.point);
        return;
      }

      TryTarget(target);
      return;
    }

    TryMove(hit.point);
  }

  private void TryTarget(Targetable target)
  {
    foreach (Unit unit in unitSelectionHandler.SelectedUnits)
    {
      unit.GetTargeter().CmdSetTarget(target);
    }
  }

  private void TryMove(Vector3 point)
  {
    foreach (Unit unit in unitSelectionHandler.SelectedUnits)
    {
      unit.GetUnitMovement().CmdMove(point);
    }
  }
}
