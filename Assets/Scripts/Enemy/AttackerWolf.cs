using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AttackerWolf : MonoBehaviour
{
	[Header("Component")]

	public NavMeshAgent agent;
	public Damageable damageable;
	public Attacker attacker;

	[Header("Stats")]

	private bool targetPlayer;
	private GameObject target;

	private void Start()
	{
		if (Random.Range(0.0f, 1.0f) > 0.5f)
		{
			targetPlayer = true;
			target = GameManager.instance.player.gameObject;
			attacker.m_attackEvent.AddListener(OnPlayerAttack);
		}
		else
		{
			targetPlayer = false;
			target = GameManager.instance.ramNPC.gameObject;
		}

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
						agent.isStopped = false;

					Act();
				}
				else if (!agent.isStopped)
				{
					agent.isStopped = true;
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
		Vector3 toTargetDirection = (target.transform.position - this.transform.position).normalized;
		agent.SetDestination(target.transform.position + toTargetDirection * 2.5f);
	}

	private void OnPlayerAttack(Damageable damageable)
	{
		if(damageable.gameObject == GameManager.instance.player.gameObject)
		{
			targetPlayer = false;
			target = GameManager.instance.ramNPC.gameObject;

			Vector3 toTargetDirection = (target.transform.position - this.transform.position).normalized;
			agent.SetDestination(target.transform.position + toTargetDirection * 2.5f);

			attacker.m_attackEvent.RemoveListener(OnPlayerAttack);
		}
	}

	private void OnDestroy()
	{
		GameManager.instance.enemyGenerator.enemies.Remove(this.gameObject);
	}
}
