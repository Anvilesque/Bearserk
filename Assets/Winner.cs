using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Winner : MonoBehaviour
{
    public Text text;
    private BoxCollider2D myCollider;
    // Start is called before the first frame update
    void Start()
    {
        myCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player")
        {
            StartCoroutine("WinnerFadeIn");
        }
        Debug.Log("entered");
    }

    private IEnumerator WinnerFadeIn()
    {
        while (text.color.a < 1f)
        {
            text.color = new Color(text.color.r,text.color.g, text.color.b, Mathf.Lerp(text.color.a, 1, 0.2f));
            yield return new WaitForSeconds(0.05f);
        }
        yield break;
    }
}
