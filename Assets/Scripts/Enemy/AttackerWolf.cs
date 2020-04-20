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
	private GameObject target;

	[Header("VFX")]
	public GameObject dedVFX;

	private void Start()
	{
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
		Vector3 toTargetDirection = (target.transform.position - this.transform.position).normalized;
		agent.SetDestination(target.transform.position + toTargetDirection * 2.5f);
	}

	private void OnAttack(Damageable damageable)
	{
		if (damageable.gameObject == GameManager.instance.player.gameObject)
		{
			target = GameManager.instance.ramNPC.gameObject;
			Vector3 toTargetDirection = (target.transform.position - this.transform.position).normalized;
			agent.SetDestination(target.transform.position + toTargetDirection * 2.5f);
		}
		if (damageable.gameObject == GameManager.instance.ramNPC.gameObject)
		{
			target = GameManager.instance.player.gameObject;
			Vector3 toTargetDirection = (target.transform.position - this.transform.position).normalized;
			agent.SetDestination(target.transform.position + toTargetDirection * 2.5f);
		}
	}

	private void OnDestroy()
	{
		GameManager.instance.enemyGenerator.enemies.Remove(this.gameObject);
	}
}
