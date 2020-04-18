﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodenTreeGenerator : MonoBehaviour
{
	[HideInInspector]
	public List<WoodenTree> trees;

	public GameObject treePrefab;

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

		isActive = true;
		StartCoroutine(WoodenTreeGenerationCoroutine());
	}

	private void TryToInstantiateTree()
	{
		if(trees.Count < maxTreeNb)
		{
			Vector3 treePosition = GetPossibleTreePosition();

			if(treePosition != Vector3.zero)
			{
				trees.Add(Instantiate(treePrefab, treePosition, Quaternion.Euler(new Vector3(0.0f, Random.Range(0.0f, 360.0f), 0.0f))).GetComponent<WoodenTree>());
			}
		}
	}

	private Vector3 GetPossibleTreePosition()
	{
		int maximumTries = 10;

		for(int i = 0; i < maximumTries; i++)
		{
			Vector2 pos2d = Tools.RotatePosAroundPoint(Vector2.zero,
													   new Vector2(0.0f, Random.Range(distanceFromCampfire.min, distanceFromCampfire.max)),
													   Random.Range(0.0f, 360.0f));

			Vector3 position = new Vector3(pos2d.x, 0.0f, pos2d.y);

			if(Physics.OverlapSphere(position, minDistanceBetweenTrees, treeMask).Length == 0)
			{
				return position;
			}
		}

		return Vector3.zero;
	}

	private IEnumerator WoodenTreeGenerationCoroutine()
	{
		while(isActive)
		{
			yield return new WaitForSeconds(Random.Range(timeBetweenInstantiation.min, timeBetweenInstantiation.max));
			TryToInstantiateTree();
		}
	}
}