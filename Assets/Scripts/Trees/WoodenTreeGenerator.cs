using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodenTreeGenerator : MonoBehaviour
{
	[HideInInspector]
	public List<WoodenTree> trees;

	public GameObject treePrefab;

	public int startingTreeNb;
	public MinMaxFloat timeBetweenInstantiation;
	public int maxTreeNb;
	public MinMaxFloat distanceFromCampfire;
	public float minDistanceBetweenTrees;
	public LayerMask treeMask;

	[HideInInspector]
	public bool isActive = true;

	public void Start()
	{
		trees = new List<WoodenTree>();
		distanceFromCampfire.max = GameManager.instance.zoneRadius - 2.0f;

		treePrefab.GetComponent<WoodenTree>().baseState = 2;
		for(int i=0; i<startingTreeNb; i++)
		{
			TryToInstantiateTree();
		}
		treePrefab.GetComponent<WoodenTree>().baseState = 0;

		isActive = true;
		StartCoroutine(WoodenTreeGenerationCoroutine());
	}

	private void TryToInstantiateTree()
	{
		if (trees.Count < maxTreeNb)
		{
			Vector3 treePosition = GetPossibleTreePosition();

			if (treePosition != Vector3.zero)
			{
				trees.Add(Instantiate(treePrefab, treePosition, Quaternion.Euler(new Vector3(0.0f, Random.Range(0.0f, 360.0f), 0.0f)), this.transform).GetComponent<WoodenTree>());
			}
		}
	}

	private Vector3 GetPossibleTreePosition()
	{
		int maximumTries = 10;

		for (int i = 0; i < maximumTries; i++)
		{
			Vector3 position = Tools.RotatePosAroundPoint(Vector3.zero,
														  Vector3.right * Random.Range(distanceFromCampfire.min, distanceFromCampfire.max),
														  Random.Range(0.0f, 2* Mathf.PI));

			if (Physics.OverlapSphere(position, minDistanceBetweenTrees, treeMask).Length == 0)
			{
				return position;
			}
		}

		return Vector3.zero;
	}

	private IEnumerator WoodenTreeGenerationCoroutine()
	{
		while (isActive)
		{
			yield return new WaitForSeconds(Random.Range(timeBetweenInstantiation.min, timeBetweenInstantiation.max));
			TryToInstantiateTree();
		}
	}
}
