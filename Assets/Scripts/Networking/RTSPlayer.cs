using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class RTSPlayer : NetworkBehaviour
{
	[SerializeField] private List<Unit> myUnits = new List<Unit>();

	#region Server

	public override void OnStartServer()
	{
		Unit.ServerOnUnitSpawned += ServerHandleUnitSpawned;
		Unit.ServerOnUnitDespawned += ServerHandleUnitDepawned;
	}

	public override void OnStopServer()
	{
		Unit.ServerOnUnitSpawned -= ServerHandleUnitSpawned;
		Unit.ServerOnUnitDespawned -= ServerHandleUnitDepawned;
	}

	private void ServerHandleUnitSpawned(Unit unit)
	{
		if (unit.connectionToClient.connectionId != connectionToClient.connectionId) { return; }

		myUnits.Add(unit);
	}

	private void ServerHandleUnitDepawned(Unit unit)
	{
		if (unit.connectionToClient.connectionId != connectionToClient.connectionId) { return; }

		myUnits.Remove(unit);
	}

	#endregion

	#region Client

	public override void OnStartClient()
	{
		if (!isClientOnly) { return; }

		Unit.AuthorityOnUnitSpawned += AuthorityHandleUnitSpawned;
		Unit.AuthorityOnUnitDespawned += AuthorityHandleUnitDepawned;
	}


	public override void OnStopClient()
	{
		if (!isClientOnly) { return; }

		Unit.AuthorityOnUnitSpawned -= AuthorityHandleUnitSpawned;
		Unit.AuthorityOnUnitDespawned -= AuthorityHandleUnitDepawned;
	}

	private void AuthorityHandleUnitDepawned(Unit unit)
	{
		if (!hasAuthority) { return; }

		myUnits.Add(unit);
	}

	private void AuthorityHandleUnitSpawned(Unit unit)
	{
		if (!hasAuthority) { return; }
		
		myUnits.Remove(unit);
	}


	#endregion
}
