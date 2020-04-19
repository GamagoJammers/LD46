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

	public WoodenTreeState actualState;

	public Transform logDropPoint;
	public GameObject logPrefab;

	//TEST
	public bool makeItDie;

	// Start is called before the first frame update
	void Start()
    {
		actualState = treeStates[0];
		actualState.stateModel.SetActive(true);
		StartCoroutine(GrowCoroutine());
	}

	//TEST
	private void Update()
	{
		if (makeItDie)
			Die();
	}
	//TEST

	public void Die()
	{
		for(int i=0; i<actualState.logAmount; i++)
		{
			Vector2 originPos = new Vector2(logDropPoint.position.x, logDropPoint.position.z);
			Vector2 logPos = originPos + Vector2.right;
			float angle = Random.Range(0.0f, 360.0f);
			logPos = Tools.RotatePosAroundPoint(originPos, logPos, angle);

			Vector3 logPosition = new Vector3(logPos.x, logDropPoint.position.y, logPos.y);

			Pickable log = Instantiate(logPrefab, logPosition, Quaternion.LookRotation(logPosition-logDropPoint.position)).GetComponent<Pickable>();
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
			actualState.stateModel.SetActive(false);
			actualState = treeStates[(int)actualState.growthStatus + 1];
			actualState.stateModel.SetActive(true);
		}
	}
}
