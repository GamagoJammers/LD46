using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Consummable : MonoBehaviour
{
	public float energy = 0.0f;

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Campfire"))
		{
			other.gameObject.GetComponentInParent<Campfire>().RegainVivacity(energy);
			GameManager.instance.logs.Remove(this.gameObject);
			Destroy(this.gameObject);
		}
	}
}
