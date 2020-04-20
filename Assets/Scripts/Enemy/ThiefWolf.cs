using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class ThiefWolf : MonoBehaviour
{
	public enum ThiefWolfState {WANDER, CHASELOG, FLEE};

	public NavMeshAgent agent;
	ThiefWolfState state;

	public GameObject targetLog;

	public PickerSensor pickSensor;
	public Damageable damageable;
	public Attacker attacker;

	public float maxWanderingTime;

	private void Start()
	{
		state = ThiefWolfState.CHASELOG;
	}

	void Update()
    {
		if(!GameManager.instance.isPaused)
		{
			if(damageable.IsAlive())
			{
				if (damageable.CanPerformActions())
				{
					if (agent.isStopped)
					{
						agent.isStopped = false;
						attacker.SetEnableAttack(true);
					}

					if (state == ThiefWolfState.CHASELOG)
						UpdateTarget();

					CheckState();
				}
				else if (!agent.isStopped)
				{
					agent.isStopped = true;
					attacker.SetEnableAttack(false);
				}
			}
			else
			{
				Die();
			}
		}
		else if (!agent.isStopped)
		{
			agent.isStopped = true;
		}
    }

	public void Die()
	{
		Destroy(this.gameObject);
	}

	public void UpdateTarget()
	{
		bool flee = false;

		if(GameManager.instance.logs.Count != 0)
		{
			targetLog = null;
			float minDist = float.MaxValue;
			foreach (GameObject log in GameManager.instance.logs)
			{
				if(!log.GetComponent<Pickable>().IsPickedButNotByPlayer())
				{
					float distanceFromWolf = (log.transform.position - transform.position).magnitude;

					if (distanceFromWolf < minDist)
					{
						minDist = distanceFromWolf;
						targetLog = log;
					}
				}
			}
			if (targetLog == null)
				flee = true;
		}
		else
		{
			flee = true;
		}

		if(flee)
		{
			state = ThiefWolfState.FLEE;
			agent.SetDestination(transform.position.normalized * (GameManager.instance.zoneRadius + 2.0f));
		}
		else
		{
			agent.SetDestination(targetLog.transform.position);
		}
	}

	public void CheckState()
	{
		if (state == ThiefWolfState.CHASELOG && pickSensor.m_selectedPickable != null)
		{
			pickSensor.PickUp();
			state = ThiefWolfState.FLEE;

			agent.SetDestination(transform.position.normalized * (GameManager.instance.zoneRadius+2.0f));
		}
		else if(state == ThiefWolfState.FLEE && agent.remainingDistance <= 0.2f)
		{
			if(targetLog != null)
			{
				GameManager.instance.logs.Remove(targetLog);
				Destroy(targetLog.gameObject);
			}
			GameManager.instance.enemyGenerator.enemies.Remove(this.gameObject);
			Destroy(this.gameObject);
		}
	}

	private void OnDestroy()
	{
		GameManager.instance.enemyGenerator.enemies.Remove(this.gameObject);
	}
}
