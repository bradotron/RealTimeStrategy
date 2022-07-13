using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class Targeter : NetworkBehaviour
{
	[SerializeField] private Targetable target;
	
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
	public void ClearTarget() 
	{
		this.target = null;
	}
	
	#endregion
	
	#region Client
	
	#endregion
	
}
