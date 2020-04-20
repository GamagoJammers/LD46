using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AttackerWolf : MonoBehaviour
{
	public enum AttackWolfState { ATTACK, GETBACK, FLEE};

	[Header("Component")]

	public NavMeshAgent agent;
	AttackWolfState state;
	public Damageable damageable;
	public Attacker attacker;

	[Header("Stats")]

	[Header("VFX")]
	public GameObject dedVFX;

	[HideInInspector]
	public GameObject target;
	public MinMaxFloat speed;
	public MinMaxFloat wanderingDistanceFromCampfire;
	private int currentAttackNb = 0;
	public int maxAttack;
	
	private void Start()
	{
		wanderingDistanceFromCampfire.max = GameManager.instance.zoneRadius - 2.0f;

		attacker.m_attackEvent.AddListener(OnAttack);

		if (Random.Range(0.0f, 1.0f) > 0.5f)
			target = GameManager.instance.player.gameObject;
		else
			target = GameManager.instance.ramNPC.gameObject;

		Vector3 toTargetDirection = (target.transform.position - this.transform.position).normalized;
		agent.SetDestination(target.transform.position + toTargetDirection * 2.5f);
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
		Instantiate(dedVFX, transform.position, new Quaternion(0, 0, 0, 0));

		Destroy(this.gameObject);
	}

	private void Act()
	{
		if(state == AttackWolfState.ATTACK)
		{
			Vector3 toTargetDirection = (target.transform.position - this.transform.position).normalized;
			agent.SetDestination(target.transform.position + toTargetDirection * 2.5f);
		}
	}

	private void OnAttack(Damageable damageable)
	{
		currentAttackNb++;

		if (damageable.gameObject == GameManager.instance.player.gameObject)
		{
			target = GameManager.instance.ramNPC.gameObject;
		}
		if (damageable.gameObject == GameManager.instance.ramNPC.gameObject)
		{
			target = GameManager.instance.player.gameObject;
		}

		if(currentAttackNb < maxAttack)
		{
			agent.SetDestination(Tools.RandomPointOnCircle(wanderingDistanceFromCampfire));
			agent.speed = speed.min;
			state = AttackWolfState.GETBACK;
			attacker.SetEnableAttack(false);
		}
		else
		{
			agent.SetDestination(transform.position.normalized * (GameManager.instance.zoneRadius + GameManager.instance.enemyGenerator.zoneOffset));
			agent.speed = speed.min;
			state = AttackWolfState.FLEE;
			attacker.SetEnableAttack(false);
		}

	}

	private void CheckState()
	{
		switch (state)
		{
			case AttackWolfState.GETBACK:
				CheckGetBackState();
				break;
			case AttackWolfState.FLEE:
				CheckFleeState();
				break;
		}
	}

	private void CheckGetBackState()
	{
		if (agent.remainingDistance <= 0.1f)
		{
			Vector3 toTargetDirection = (target.transform.position - this.transform.position).normalized;
			agent.SetDestination(target.transform.position + toTargetDirection * 2.5f);
			agent.speed = speed.max;
			state = AttackWolfState.ATTACK;
			attacker.SetEnableAttack(true);
		}
	}

	private void CheckFleeState()
	{
		if (agent.remainingDistance <= 0.2f)
		{
			Die();
		}
	}

	private void OnDestroy()
	{
		GameManager.instance.enemyGenerator.enemies.Remove(this.gameObject);
	}
}
