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
		float generalOffsetAngle = Random.Range(0.0f, 360.0f);
		Vector2 originPos = new Vector2(logDropPoint.position.x, logDropPoint.position.z);

		for(int i=0; i<actualState.logAmount; i++)
		{
			Vector2 logPos = originPos + Vector2.right;

			float deltaAngle = Random.Range(-5.0f, 5.0f);
			float logAngleOffset = 360.0f * i / (float)(actualState.logAmount);
			logPos = Tools.RotatePosAroundPoint(originPos, logPos, generalOffsetAngle + logAngleOffset + deltaAngle );

			Vector3 logPosition = new Vector3(logPos.x, logDropPoint.position.y, logPos.y);

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
