﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
	[HideInInspector]
	public List<GameObject> enemies;

	public GameObject whiteThiefWolfPrefab;
	public float thiefWolfChance;

	public GameObject blackAttackerWolfPrefab;
	public float AttackerWolfChance;

	public MinMaxFloat timeBetweenInstantiation;

	public int maxEnemyNb;

	[HideInInspector]
	public bool isActive = true;

	public void Start()
	{
		enemies = new List<GameObject>();

		isActive = true;
		StartCoroutine(EnemyGenerationCoroutine());
	}

	private void TryToInstantiateEnemy()
	{
		Vector3 enemyPosition = GetEnemyStartPosition();

		if (enemies.Count < maxEnemyNb)
		{

			Debug.Log("bonjour 2");
			if (GameManager.instance.logs.Count == 0)
			{
				//enemies.Add(Instantiate(blackAttackerWolfPrefab, enemyPosition, Quaternion.identity, this.transform));
			}
			else
			{
				float randomChance = Random.Range(0.0f, 100.0f);
				if(randomChance <= thiefWolfChance)
				{
					Debug.Log("bonjour");
					enemies.Add(Instantiate(whiteThiefWolfPrefab, enemyPosition, Quaternion.identity, this.transform));
				}
				else if (randomChance <= thiefWolfChance + AttackerWolfChance)
				{
					enemies.Add(Instantiate(blackAttackerWolfPrefab, enemyPosition, Quaternion.identity, this.transform));
				}
			}
		}
	}

	private Vector3 GetEnemyStartPosition()
	{
		Vector2 pos2d = Tools.RotatePosAroundPoint(Vector2.zero,
													new Vector2(0.0f, GameManager.instance.zoneRadius + 2.0f),
													Random.Range(0.0f, 360.0f));

		return new Vector3(pos2d.x, 0.0f, pos2d.y);
	}

	private IEnumerator EnemyGenerationCoroutine()
	{
		while (isActive)
		{
			yield return new WaitForSeconds(Random.Range(timeBetweenInstantiation.min, timeBetweenInstantiation.max));
			TryToInstantiateEnemy();
		}
	}
}