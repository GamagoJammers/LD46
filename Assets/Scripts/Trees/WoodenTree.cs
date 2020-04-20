using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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

	public UnityEvent m_growEvent;
	public UnityEvent m_dieEvent;

	private void OnAwake()
	{
		m_growEvent = new UnityEvent();
		m_dieEvent = new UnityEvent();
	}

	private void OnDisable()
	{
		m_growEvent.RemoveAllListeners();
		m_dieEvent.RemoveAllListeners();
	}

	void Start()
    {
		actualState = treeStates[baseState];
		actualState.stateModel.SetActive(true);
		StartCoroutine(GrowCoroutine());
	}

	public void Die()
	{
		m_dieEvent.Invoke();
		float generalOffsetAngle = Random.Range(0.0f, 2 * Mathf.PI);

		for(int i=0; i<actualState.logAmount; i++)
		{
			Vector3 logPos = logDropPoint.position + Vector3.right;

			float deltaAngle = Random.Range(-0.25f, 0.25f);
			float logAngleOffset = 2 * Mathf.PI * i / (float)(actualState.logAmount);
			Vector3 logPosition = Tools.RotatePosAroundPoint(logDropPoint.position, logDropPoint.position + Vector3.right, generalOffsetAngle + logAngleOffset + deltaAngle );

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
			m_growEvent.Invoke();
			actualState.stateModel.SetActive(false);
			actualState = treeStates[(int)actualState.growthStatus + 1];
			actualState.stateModel.SetActive(true);
		}
	}
}
