using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class ThiefWolf : MonoBehaviour
{
	public enum ThiefWolfState {WANDER, CHASELOG, FLEE};

	[Header("Component")]

	public NavMeshAgent agent;
	ThiefWolfState state;
	public PickerSensor pickSensor;
	public Damageable damageable;
	public Attacker attacker;

	[Header("Stats")]

	[HideInInspector]
	public GameObject targetLog;
	public MinMaxFloat speed;
	public MinMaxFloat wanderingDistanceFromCampfire;
	private float currentWanderingTime;
	public float maxWanderingTime;

	private void Start()
	{
		wanderingDistanceFromCampfire.max = GameManager.instance.zoneRadius - 2.0f;

		agent.SetDestination(Tools.RandomPointOnCircle(wanderingDistanceFromCampfire));
		agent.speed = speed.min;
		state = ThiefWolfState.WANDER;
		currentWanderingTime = 0.0f;
		attacker.SetEnableAttack(false);
	}

	private void Update()
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

					Act();
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

	private void Act()
	{
		switch (state)
		{
			case ThiefWolfState.WANDER:
				WanderAct();
				break;
			case ThiefWolfState.CHASELOG:
				ChaseAct();
				break;
		}
	}

	private void WanderAct()
	{
		currentWanderingTime += Time.deltaTime;
	}

	private void ChaseAct()
	{
		if (GameManager.instance.logs.Count != 0)
		{
			targetLog = null;
			float minDist = float.MaxValue;
			foreach (GameObject log in GameManager.instance.logs)
			{
				if (!log.GetComponent<Pickable>().IsPickedButNotByPlayer())
				{
					float distanceFromWolf = (log.transform.position - transform.position).magnitude;

					if (distanceFromWolf < minDist)
					{
						minDist = distanceFromWolf;
						targetLog = log;
					}
				}
			}
		}

		if(targetLog != null)
			agent.SetDestination(targetLog.transform.position);
	}

	private void CheckState()
	{
		switch (state)
		{
			case ThiefWolfState.WANDER:
				CheckWanderState();
				break;
			case ThiefWolfState.CHASELOG:
				CheckChaseLogState();
				break;
			case ThiefWolfState.FLEE:
				CheckFleeState();
				break;
		}
	}

	private void CheckWanderState()
	{
		if (GameManager.instance.logs.Count != 0)
		{
			agent.speed = speed.max;
			state = ThiefWolfState.CHASELOG;
			attacker.SetEnableAttack(true);
		}
		else if(currentWanderingTime >= maxWanderingTime)
		{
			agent.SetDestination(transform.position.normalized * (GameManager.instance.zoneRadius + GameManager.instance.enemyGenerator.zoneOffset));
			agent.speed = speed.max;
			state = ThiefWolfState.FLEE;
			attacker.SetEnableAttack(true);
		}
		else if (agent.remainingDistance <= 0.1f)
		{
			agent.SetDestination(Tools.RandomPointOnCircle(wanderingDistanceFromCampfire));
		}
	}

	private void CheckChaseLogState()
	{
		if (GameManager.instance.logs.Count == 0 || targetLog == null)
		{
			agent.SetDestination(Tools.RandomPointOnCircle(wanderingDistanceFromCampfire));
			agent.speed = speed.min;
			state = ThiefWolfState.WANDER;
			currentWanderingTime = 0.0f;
			attacker.SetEnableAttack(false);
		}
		else if(pickSensor.m_selectedPickable != null)
		{
			pickSensor.PickUp();

			agent.SetDestination(transform.position.normalized * (GameManager.instance.zoneRadius + GameManager.instance.enemyGenerator.zoneOffset));
			agent.speed = speed.max;
			state = ThiefWolfState.FLEE;
			attacker.SetEnableAttack(true);
		}
	}

	private void CheckFleeState()
	{
		if (agent.remainingDistance <= 0.2f)
		{
			if (targetLog != null)
			{
				GameManager.instance.logs.Remove(targetLog);
				Destroy(targetLog.gameObject);
			}
			Die();
		}
	}

	private void OnDestroy()
	{
		GameManager.instance.enemyGenerator.enemies.Remove(this.gameObject);
	}
}
