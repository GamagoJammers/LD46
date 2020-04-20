using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public static GameManager instance;

	[HideInInspector]
	public bool isPaused;

	public float zoneRadius = 0.0f;

	public WoodenTreeGenerator treeGenerator;
	public List<GameObject> logs;

	public EnemyGenerator enemyGenerator;

	public RamNPC ramNPC;
	public PlayerControl player;

	public ScoreUI scriptScoreUI;
	public bool isDeadFire;

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(Vector3.zero, zoneRadius);
	}

	void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
		else if (instance != this)
		{
			Destroy(this.gameObject);
		}
	}

	public void GameOver()
	{
		scriptScoreUI.EndScore();
	}
}
