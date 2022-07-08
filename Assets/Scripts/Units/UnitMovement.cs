using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.AI;

public class UnitMovement : NetworkBehaviour
{
	[SerializeField] private NavMeshAgent agent = null;

	#region Server
	
	[Command]
	public void CmdMove(Vector3 newPosition)
	{
		if (!NavMesh.SamplePosition(newPosition, out NavMeshHit hit, 1f, NavMesh.AllAreas)) { return; }
		
		agent.SetDestination(hit.position);
	}
	#endregion

	#region Client

	#endregion
}
