using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WoodenTreeGrowthStatus { SPROUT=0, SHRUB=1, TREE=2};

[System.Serializable]
public struct WoodenTreeState
{
	public WoodenTreeGrowthStatus growthStatus;
	public GameObject stateModelPrefab;
	//-1 if not possible to cut
	public int logAmount;
	//-1.0f if not possible to grow more
	public float timeToGrow;
}

public class WoodenTree : MonoBehaviour
{
	public WoodenTreeState[] treeStates;

	public WoodenTreeState actualState;

	public GameObject actualModel;

    // Start is called before the first frame update
    void Start()
    {
		actualState = treeStates[0];
		actualModel = Instantiate(actualState.stateModelPrefab, this.transform);
		StartCoroutine(GrowCoroutine());
	}

	IEnumerator GrowCoroutine()
	{
		while(actualState.growthStatus != WoodenTreeGrowthStatus.TREE)
		{
			yield return new WaitForSeconds(actualState.timeToGrow);
			actualState = treeStates[(int)actualState.growthStatus + 1];
			Destroy(actualModel.gameObject);
			actualModel = Instantiate(actualState.stateModelPrefab, this.transform);
		}
	}
}
