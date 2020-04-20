using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Consummable : MonoBehaviour
{
	public bool isForFire;
	public float energy = 0.0f;

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Campfire") && isForFire)
		{
			other.gameObject.GetComponentInParent<Campfire>().RegainVivacity(energy);
			GameManager.instance.logs.Remove(this.gameObject);
			Destroy(this.gameObject);
		}
		else if (other.CompareTag("ram") && !isForFire)
		{
			other.gameObject.GetComponentInParent<Damageable>().Heal((int)energy);
			Destroy(this.transform.parent.gameObject);
		}
	}
}
