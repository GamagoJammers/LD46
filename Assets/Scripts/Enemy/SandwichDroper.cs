using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandwichDroper : MonoBehaviour
{
	public GameObject sandwichPrefab;
	public float dropChance;

	private void OnDestroy()
	{
		float randomChance = Random.Range(0.0f, 100.0f);
		if (randomChance <= dropChance)
		{
			GameManager.instance.StartCoroutine(WaitForSandwichDropCoroutine(sandwichPrefab, new Vector3(transform.position.x, 1.0f, transform.position.z)));
		}
	}

	IEnumerator WaitForSandwichDropCoroutine(GameObject sandwichPrefab, Vector3 position)
	{
		yield return new WaitForSeconds(0.5f);
		Instantiate(sandwichPrefab, position, Random.rotation);
	}
}
