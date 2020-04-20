using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
	//[HideInInspector]
	public List<GameObject> enemies;

	public GameObject whiteThiefWolfPrefab;
	public float thiefWolfChance;

	public GameObject blackAttackerWolfPrefab;
	public float AttackerWolfChance;

	public MinMaxFloat timeBetweenInstantiation;
	public float zoneOffset = 4.0f;

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
			float randomChance = Random.Range(0.0f, 100.0f);
			if(randomChance <= thiefWolfChance)
			{
				enemies.Add(Instantiate(whiteThiefWolfPrefab, enemyPosition, Quaternion.identity, this.transform));
			}
			else if (randomChance <= thiefWolfChance + AttackerWolfChance)
			{
				enemies.Add(Instantiate(blackAttackerWolfPrefab, enemyPosition, Quaternion.identity, this.transform));
			}
		}
	}

	private Vector3 GetEnemyStartPosition()
	{
		return Tools.RotatePosAroundPoint(Vector3.zero,
										  Vector3.right * (GameManager.instance.zoneRadius + zoneOffset),
										  Random.Range(0.0f, 2* Mathf.PI));
	}

	private IEnumerator EnemyGenerationCoroutine()
	{
		while (isActive)
		{
			yield return new WaitForSeconds(Random.Range(timeBetweenInstantiation.min, timeBetweenInstantiation.max));
			if (!GameManager.instance.isPaused)
			{
				TryToInstantiateEnemy();
			}
		}
	}
}
