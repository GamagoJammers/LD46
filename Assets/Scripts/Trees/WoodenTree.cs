using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WoodenTreeGrowthStatus { SPROUT=0, SHRUB=1, TREE=2};

[System.Serializable]
public struct WoodenTreeState
{
	public WoodenTreeGrowthStatus growthStatus;
	public GameObject stateModel;
	//-1 if not possible to cut
	public int logAmount;
	//-1.0f if not possible to grow more
	public float timeToGrow;
}

public class WoodenTree : MonoBehaviour
{
	public WoodenTreeState[] treeStates;

	[HideInInspector]
	public int baseState = 0;
	[HideInInspector]
	public WoodenTreeState actualState;

	public Transform logDropPoint;
	public GameObject logPrefab;
	public GameObject dedTreeVFX;

	// Start is called before the first frame update
	void Start()
    {
		actualState = treeStates[baseState];
		actualState.stateModel.SetActive(true);
		StartCoroutine(GrowCoroutine());
	}

	public void Die()
	{
		for(int i=0; i<actualState.logAmount; i++)
		{
			Vector3 logPosition = Tools.RotatePosAroundPoint(logDropPoint.position, logDropPoint.position + Vector3.right, Random.Range(0.0f, 360.0f));

			Pickable log = Instantiate(logPrefab, logPosition, Quaternion.LookRotation(logPosition-logDropPoint.position)).GetComponent<Pickable>();
			log.Drop(true);

			GameManager.instance.logs.Add(log.gameObject);
		}

		Instantiate(dedTreeVFX, new Vector3(transform.position.x, transform.position.y + 1.5f, transform.position.z), new Quaternion(0, 0, 0, 0));

		GameManager.instance.treeGenerator.trees.Remove(this);
		Destroy(this.gameObject);
	}

	IEnumerator GrowCoroutine()
	{
		while(actualState.growthStatus != WoodenTreeGrowthStatus.TREE)
		{
			yield return new WaitForSeconds(actualState.timeToGrow);
			actualState.stateModel.SetActive(false);
			actualState = treeStates[(int)actualState.growthStatus + 1];
			actualState.stateModel.SetActive(true);
		}
	}
}
