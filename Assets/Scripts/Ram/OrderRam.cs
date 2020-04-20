using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderRam : MonoBehaviour
{
	public Attacker attacker;

	public void Start()
	{
		attacker.m_attackEvent.AddListener(OnOrderRam);
	}

	void OnOrderRam(Damageable damageable)
	{
		GameManager.instance.ramNPC.OrderToAttack(damageable.gameObject);
	}
}
