using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UnitSelectionHandler : MonoBehaviour
{
  [SerializeField] private LayerMask layerMask;
  private Camera mainCamera;

  public List<Unit> SelectedUnits { get; } = new List<Unit>();

  private void Start()
  {
    mainCamera = Camera.main;
  }

  private void Update()
  {
    if (Mouse.current.leftButton.wasPressedThisFrame)
    {
      DeselectAllUnits();

      // this is where you start the selection area
    }
    else if (Mouse.current.leftButton.wasReleasedThisFrame)
    {
      ClearSelectionArea();

    }


  }

  private void ClearSelectionArea()
  {
    DeselectAllUnits();

    Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

    if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layerMask)) { return; }
    if (!hit.collider.TryGetComponent<Unit>(out Unit unit)) { return; }
    if (!unit.hasAuthority) { return; }

    SelectedUnits.Add(unit);
    SelectSelectedUnits();
  }

  private void DeselectAllUnits()
  {
    foreach (Unit selectedUnit in SelectedUnits)
    {
      selectedUnit.Deselect();
    }
    SelectedUnits.Clear();
  }

  private void SelectSelectedUnits()
  {
    foreach (Unit selectedUnit in SelectedUnits)
    {
      selectedUnit.Select();
    }
  }
}
