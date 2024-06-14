using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClawnchTiles : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {
        FindObjectOfType<Clawnch>().GetComponent<Clawnch>();
    }

    private void OnCollisionStay2D(Collision2D other) {
        if (other.gameObject.tag =="Player")
        {
            if (other.gameObject.GetComponent<Clawnch>().clawnchEnabled == true)
            {
                GetComponent<CompositeCollider2D>().isTrigger = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player") GetComponent<CompositeCollider2D>().isTrigger = false;    
    }
}
