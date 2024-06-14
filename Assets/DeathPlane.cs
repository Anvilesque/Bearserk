using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathPlane : MonoBehaviour
{
    private BoxCollider2D deathCollider;
    // Start is called before the first frame update
    void Start()
    {
        deathCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player")
        {
            other.gameObject.GetComponent<Health>().Die();
        }
    }
}
