using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerMovement : MonoBehaviour
{
    private Animator animator;
	
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("player"))
        {
            animator.SetTrigger("Move");
        }
    }
}
