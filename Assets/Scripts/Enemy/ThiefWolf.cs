using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class ThiefWolf : MonoBehaviour
{
	public enum ThiefWolfState {CHASELOG, FLEE};

	public NavMeshAgent agent;
	ThiefWolfState state;

	public GameObject targetLog;

	public PickerSensor pickSensor;
	public Damageable damageable;

	private void Start()
	{
		state = ThiefWolfState.CHASELOG;
	}

	void Update()
    {
		if(damageable.CanPerformActions())
		{
			if (agent.isStopped)
				agent.isStopped = false;

			if(state == ThiefWolfState.CHASELOG)
				UpdateTarget();

			checkState();
		}
		else
		{
			agent.isStopped = true;
		}
    }

	public void UpdateTarget()
	{
		if(GameManager.instance.logs.Count != 0)
		{
			float minDist = float.MaxValue;
			foreach (GameObject log in GameManager.instance.logs)
			{
				float distanceFromWolf = (log.transform.position - transform.position).magnitude;

				if (distanceFromWolf < minDist)
				{
					minDist = distanceFromWolf;
					targetLog = log;
				}
			}
			agent.SetDestination(targetLog.transform.position);
		}
		else
		{
			state = ThiefWolfState.FLEE;
			agent.SetDestination(transform.position.normalized * (GameManager.instance.zoneRadius + 2.0f));
		}
	}

	public void checkState()
	{
		if (state == ThiefWolfState.CHASELOG && pickSensor.m_selectedPickable != null)
		{
			pickSensor.PickUp();
			state = ThiefWolfState.FLEE;

			agent.SetDestination(transform.position.normalized * (GameManager.instance.zoneRadius+2.0f));
		}
		else if(state == ThiefWolfState.FLEE && agent.remainingDistance <= 0.2f)
		{
			GameManager.instance.logs.Remove(targetLog);
			Destroy(targetLog.gameObject);
			GameManager.instance.enemyGenerator.enemies.Remove(this.gameObject);
			Destroy(this.gameObject);
		}
	}
}
