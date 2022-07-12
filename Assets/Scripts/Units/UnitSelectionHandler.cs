using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.InputSystem;

public class UnitSelectionHandler : MonoBehaviour
{
  [SerializeField] private LayerMask layerMask;
  [SerializeField] private RectTransform unitSelectBox = null;


  private Vector2 startSelectBoxPosition;
  private RTSPlayer player;
  private Camera mainCamera;

  public List<Unit> SelectedUnits { get; } = new List<Unit>();

  private void Start()
  {
    mainCamera = Camera.main;

  }

  private void TrySetPlayer()
  {
    player = NetworkClient.connection.identity.GetComponent<RTSPlayer>();
  }

  private void Update()
  {
    if (player == null)
    {
      TrySetPlayer();
    }

    if (Mouse.current.leftButton.wasPressedThisFrame)
    {
      if (!Keyboard.current.leftShiftKey.isPressed)
      {
        DeselectAllUnits();
      }

      StartUnitSelectBox();
      UpdateUnitSelectBox();
    }
    else if (Mouse.current.leftButton.wasReleasedThisFrame)
    {
      if (!Keyboard.current.leftShiftKey.isPressed)
      {
        DeselectAllUnits();
      }

      StopUnitSelectBox();
      SelectUnitsInBox();
    }
    else if (Mouse.current.leftButton.isPressed)
    {
      UpdateUnitSelectBox();
    }
  }

  private void StartUnitSelectBox()
  {
    unitSelectBox.gameObject.SetActive(true);
    startSelectBoxPosition = Mouse.current.position.ReadValue();
  }

  private void UpdateUnitSelectBox()
  {
    Vector2 mousePosition = Mouse.current.position.ReadValue();
    float width = mousePosition.x - startSelectBoxPosition.x;
    float height = mousePosition.y - startSelectBoxPosition.y;

    unitSelectBox.sizeDelta = new Vector2(Mathf.Abs(width), Mathf.Abs(height));
    unitSelectBox.anchoredPosition = startSelectBoxPosition + new Vector2(width / 2, height / 2);
  }

  private void StopUnitSelectBox()
  {
    unitSelectBox.gameObject.SetActive(false);
  }

  private void SelectUnitsInBox()
  {
    if (unitSelectBox.sizeDelta.magnitude == 0)
    {
      Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

      if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layerMask)) { return; }
      if (!hit.collider.TryGetComponent<Unit>(out Unit unit)) { return; }
      if (!unit.hasAuthority) { return; }
      if (SelectedUnits.Contains(unit)) { return; }

      SelectedUnits.Add(unit);
      unit.Select();
    }
    else
    {
      Vector2 min = unitSelectBox.anchoredPosition - (unitSelectBox.sizeDelta / 2);
      Vector2 max = unitSelectBox.anchoredPosition + (unitSelectBox.sizeDelta / 2);

      foreach (Unit unit in player.GetMyUnits())
      {
        if (SelectedUnits.Contains(unit)) { continue; }

        Vector3 unitScreenPosition = mainCamera.WorldToScreenPoint(unit.transform.position);
        if (
          unitScreenPosition.x > min.x &&
          unitScreenPosition.x < max.x &&
          unitScreenPosition.y > min.y &&
          unitScreenPosition.y < max.y)
        {
          SelectedUnits.Add(unit);
          unit.Select();
        }
      }
    }
  }

  private void DeselectAllUnits()
  {
    foreach (Unit selectedUnit in SelectedUnits)
    {
      selectedUnit.Deselect();
    }
    SelectedUnits.Clear();
  }
}
