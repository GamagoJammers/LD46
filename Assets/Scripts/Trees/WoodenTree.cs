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

	public Transform logDropPoint;
	public GameObject logPrefab;

	// Start is called before the first frame update
	void Start()
    {
		actualState = treeStates[0];
		actualModel = Instantiate(actualState.stateModelPrefab, this.transform);
		StartCoroutine(GrowCoroutine());
	}

	public void Die()
	{
		float actualAngle = 360.0f;
		float anglePart = actualAngle / actualState.logAmount;

		for(int i=0; i<actualState.logAmount; i++)
		{
			actualAngle = actualAngle - anglePart;
			Pickable log = Instantiate(logPrefab, logDropPoint.position, Quaternion.Euler(new Vector3(0.0f, actualAngle, 0.0f))).GetComponent<Pickable>();
			log.Drop(true);
		}

		//VFX TREE DIYING

		Destroy(this.gameObject);
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
