using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RamAnimationController : MonoBehaviour
{
	public AnimationClip m_attackAnimationClip;
	public AnimationClip m_hitAnimationClip;

	public Animator m_animator;
	public RamNPC m_ram;
	private float m_hitTimer;
	public Damageable m_damageable;
	public Attacker m_attacker;
	private float m_attackTimer;

	private void Start()
	{
		m_attacker.m_attackEvent.AddListener(Attack);
		m_attackTimer = 0.0f;
		m_damageable.m_onDamageEvent.AddListener(Hit);
		m_hitTimer = 0.0f;
		m_damageable.m_deathEvent.AddListener(Die);
	}

	private void OnDisable()
	{
		if (m_attacker != null)
		{
			m_attacker.m_attackEvent.RemoveListener(Attack);
		}

		if (m_damageable != null)
		{
			m_damageable.m_startStunEvent.RemoveListener(Hit);
			m_damageable.m_deathEvent.RemoveListener(Die);
		}
	}

	void Update()
	{
		m_animator.SetBool("IsWalking", m_ram.agent.speed == m_ram.speed.min);
		m_animator.SetBool("IsRunning", m_ram.agent.speed == m_ram.speed.max);
		m_animator.SetBool("IsHitten", m_hitTimer > 0);
		m_animator.SetBool("IsAttacking", m_attackTimer > 0);
		if (m_hitTimer > 0)
		{
			m_hitTimer -= Time.deltaTime;
		}
		if (m_attackTimer > 0)
		{
			m_attackTimer -= Time.deltaTime;
		}
	}

	void Hit()
	{
		m_animator.SetTrigger("THit");
		m_animator.SetBool("IsHitten", true);
		m_hitTimer = m_hitAnimationClip.averageDuration;
	}

	void Attack(Damageable damageable)
	{
		m_animator.SetTrigger("TAttack");
		m_animator.SetBool("IsAttacking", true);
		m_attackTimer = m_attackAnimationClip.averageDuration;
	}

	void Die()
	{
		m_animator.SetTrigger("TDie");
		m_animator.SetBool("IsDying", true);
	}
}
