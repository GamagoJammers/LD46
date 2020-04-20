using UnityEngine;
using UnityEngine.AI;

public class WolfAnimationController : MonoBehaviour
{
	public AnimationClip m_attackAnimationClip;
	public AnimationClip m_getUpAnimationClip;

	public Animator m_animator;
	public AttackerWolf m_attackerWolf;
	public ThiefWolf m_thiefWolf;
	public Damageable m_damageable;
	public Attacker m_attacker;
	public bool m_isThief;
	public PickerSensor m_picker;
	private float m_attackTimer;

	private void Start()
	{
		m_damageable.m_startStunEvent.AddListener(Stun);
		m_damageable.m_deathEvent.AddListener(Die);
		m_attacker.m_attackEvent.AddListener(Attack);
		if(m_isThief)
			m_picker.m_pickEvent.AddListener(Pick);
		m_attackTimer = 0.0f;
	}

	private void OnDisable()
	{
		if (m_damageable != null)
		{
			m_damageable.m_startStunEvent.RemoveListener(Stun);
			m_damageable.m_deathEvent.RemoveListener(Die);

		}

		if (m_attacker != null)
		{
			m_attacker.m_attackEvent.RemoveListener(Attack);
		}

		if(m_isThief && m_picker != null)
		{
			m_picker.m_pickEvent.RemoveListener(Pick);
		}
	}

	void Update()
	{
		if(m_isThief)
		{
			m_animator.SetBool("IsWalking", m_thiefWolf.agent.speed == m_thiefWolf.speed.min);
			m_animator.SetBool("IsRunning", m_thiefWolf.agent.speed == m_thiefWolf.speed.max);
		}
		else
		{
			m_animator.SetBool("IsWalking", m_attackerWolf.agent.velocity.sqrMagnitude > 0 && m_attackerWolf.agent.speed == m_attackerWolf.speed.min);
			m_animator.SetBool("IsRunning", m_attackerWolf.agent.velocity.sqrMagnitude > 0 && m_attackerWolf.agent.speed == m_attackerWolf.speed.max);
		}
		m_animator.SetBool("IsStun", m_damageable.IsStunned());
		m_animator.SetBool("IsAttacking", m_attackTimer > 0);
		m_animator.SetBool("IsDown", m_animator.GetCurrentAnimatorStateInfo(0).IsName("Stun"));
		if (m_attackTimer > 0)
		{
			m_attackTimer -= Time.deltaTime;
		}
		if (m_damageable.IsStunned() && m_damageable.GetStunnedTimer() < m_getUpAnimationClip.averageDuration)
		{
			m_animator.SetTrigger("TGetUp");
		}
	}

	void Stun()
	{
		m_animator.SetTrigger("THit");
		m_animator.SetBool("IsStun", true);
	}

	void Attack(Damageable damageable)
	{
		m_animator.SetTrigger("TAttack");
		m_animator.SetBool("IsAttacking", true);
		m_attackTimer = m_attackAnimationClip.averageDuration;
	}

	void Pick()
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
