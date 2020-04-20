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

	[Header("VFX")]
	public GameObject dedVFX;
	public AnimationClip deathClip;

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
					}

					Act();
					CheckState();
				}
				else if (!agent.isStopped)
				{
					agent.isStopped = true;
				}
			}
			else
			{
				StartCoroutine(DeathCoroutine());
			}
		}
		else if (!agent.isStopped)
		{
			agent.isStopped = true;
		}
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
		targetLog = TryToGetNearestLog();

		if(targetLog != null)
			agent.SetDestination(targetLog.transform.position);
	}

	private GameObject TryToGetNearestLog()
	{
		GameObject nearestLog = null;
		if (GameManager.instance.logs.Count != 0)
		{
			float minDist = float.MaxValue;
			foreach (GameObject log in GameManager.instance.logs)
			{
				if(log != null)
				{
					if (!log.GetComponent<Pickable>().IsPickedButNotByPlayer())
					{
						float distanceFromWolf = (log.transform.position - transform.position).magnitude;

						if (distanceFromWolf < minDist)
						{
							minDist = distanceFromWolf;
							nearestLog = log;
						}
					}
				}
			}
		}
		return nearestLog;
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
		targetLog = TryToGetNearestLog();
		if (targetLog != null)
		{
			agent.SetDestination(targetLog.transform.position);
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
			Destroy(this.gameObject);
		}
	}

	private void OnDestroy()
	{
		GameManager.instance.enemyGenerator.enemies.Remove(this.gameObject);
	}

	IEnumerator DeathCoroutine()
	{
		agent.isStopped = true;

		yield return new WaitForSeconds(deathClip.averageDuration * 2.0f);

		Instantiate(dedVFX, transform.position, new Quaternion(0, 0, 0, 0));
		Destroy(this.gameObject);
	}
}
