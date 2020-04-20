using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RamNPC : MonoBehaviour
{
	public enum RamNPCState { WANDER, CHARGE};

	[Header("Component")]

	public NavMeshAgent agent;
	RamNPCState state;
	public Damageable damageable;
	public Attacker attacker;

	[Header("Stats")]

	[HideInInspector]
	public GameObject target;
	public bool nextChargeReady;
	public float chargeCooldown;
	public Coroutine currentChargeCooldownCoroutine;
	public MinMaxFloat speed;
	public MinMaxFloat wanderingDistanceFromCampfire;

	[Header("VFX")]

	public ParticleSystem stompVFX;
	public ParticleSystem healVFX;

	private void Start()
	{
		damageable.m_onHealEvent.AddListener(HealVFX);

		wanderingDistanceFromCampfire.max = GameManager.instance.zoneRadius - 1.0f;

		agent.SetDestination(Tools.RandomPointOnCircle(wanderingDistanceFromCampfire));
		agent.speed = speed.min;
		state = RamNPCState.WANDER;
		attacker.SetEnableAttack(false);
		damageable.m_canBeKilled = true;
	}

	private void Update()
	{
		if (!GameManager.instance.isPaused)
		{
			if (damageable.IsAlive())
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
				GameManager.instance.isDeadFire = false;
				GameManager.instance.GameOver();
			}
		}
		else if (!agent.isStopped)
		{
			agent.isStopped = true;
		}
	}

	private void Act()
	{
		if(state == RamNPCState.CHARGE && target != null)
		{
			Vector3 toEnemyDirection = (target.transform.position - this.transform.position).normalized;
			agent.SetDestination(target.transform.position + toEnemyDirection * 2.5f);
		}
	}

	private void CheckState()
	{
		switch (state)
		{
			case RamNPCState.CHARGE:
				CheckChargeState();
				break;
			case RamNPCState.WANDER:
				CheckWanderState();
				break;
		}
	}

	private void CheckChargeState()
	{
		if(agent.remainingDistance <= 0.1f)
		{
			currentChargeCooldownCoroutine = StartCoroutine(ChargeCooldownCoroutine());

			agent.SetDestination(Tools.RandomPointOnCircle(wanderingDistanceFromCampfire));
			agent.speed = speed.min;
			state = RamNPCState.WANDER;

			if (stompVFX.isPlaying)
				stompVFX.Stop();

			attacker.SetEnableAttack(false);
			damageable.m_canBeKilled = true;
		}
	}

	private void CheckWanderState()
	{
		if(nextChargeReady)
		{
			List<GameObject> enemies = GameManager.instance.enemyGenerator.enemies;
			if (enemies.Count > 0)
			{
				target = enemies[Random.Range(0, enemies.Count)];
				agent.speed = speed.max;
				state = RamNPCState.CHARGE;

				if (!stompVFX.isPlaying)
					stompVFX.Play();
				
				attacker.SetEnableAttack(true);
				damageable.m_canBeKilled = false;
			}
		}
		if (agent.remainingDistance <= 0.1f)
		{
			agent.SetDestination(Tools.RandomPointOnCircle(wanderingDistanceFromCampfire));
		}
	}

	public void OrderToAttack(GameObject givenTarget)
	{
		if(currentChargeCooldownCoroutine != null)
			StopCoroutine(currentChargeCooldownCoroutine);

		target = givenTarget;
		agent.speed = speed.max;
		state = RamNPCState.CHARGE;

		if (!stompVFX.isPlaying)
			stompVFX.Play();

		attacker.SetEnableAttack(true);
		damageable.m_canBeKilled = false;
	}

	IEnumerator ChargeCooldownCoroutine()
	{
		nextChargeReady = false;
		yield return new WaitForSeconds(chargeCooldown);
		nextChargeReady = true;
	}


	public RamNPCState GetState()
	{
		return state;
	}

	private void HealVFX()
	{
		if (healVFX.isPlaying)
			healVFX.Stop();
		healVFX.Play();
	}
}
