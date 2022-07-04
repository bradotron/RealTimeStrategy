using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class UnitMovement : NetworkBehaviour
{
	[SerializeField] private NavMeshAgent agent = null;
	private Camera mainCamera;

	#region Server
	[Command]
	private void CmdMove(Vector3 newPosition)
	{
		if (!NavMesh.SamplePosition(newPosition, out NavMeshHit hit, 1f, NavMesh.AllAreas)) { return; }
		
		agent.SetDestination(hit.position);
	}
	#endregion

	#region Client

	public override void OnStartAuthority()
	{
		mainCamera = Camera.main;
	}

	[ClientCallback]
	private void Update()
	{
		if (!hasAuthority) { return; }

		if (Mouse.current.rightButton.wasPressedThisFrame)
		{
			// figure out where we clicked
			Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());	
			if(Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity)) 
			{
				// we hit something with the click
				CmdMove(hit.point);
			}
		}

	}

	#endregion
}
