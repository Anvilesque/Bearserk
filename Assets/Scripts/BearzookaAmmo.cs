using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearzookaAmmo : MonoBehaviour
{
    public float damage;
    public float life;
    public float timer;
    
    [SerializeField] private GameObject bzExplode;
    private GameObject curExplode;

    // Start is called before the first frame update
    void Start()
    {
        timer = 0f;
        life = 3.0f;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > life)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        Explode();
    }

    void Explode()
    {
        GetComponent<SpriteRenderer>().enabled = false;
        Destroy(GetComponent<Rigidbody2D>());
        Destroy(GetComponent<CapsuleCollider2D>());
        curExplode = Instantiate(bzExplode, transform.position, Quaternion.identity);
        StartCoroutine("ExplodeGrowNFade");
    }

    private IEnumerator ExplodeGrowNFade()
    {
        curExplode.transform.localScale = Vector2.zero;
        float scaleIncr = 0.05f;
        float fadeLerpT = 0.2f;
        while (curExplode.transform.localScale.x < 1f)
        {
            curExplode.transform.localScale = new Vector2(curExplode.transform.localScale.x + scaleIncr, curExplode.transform.localScale.y + scaleIncr);
            yield return null;
        }        
        while (curExplode.GetComponent<SpriteRenderer>().color.a > 0.01f)
        {
            curExplode.GetComponent<SpriteRenderer>().color = new Color(curExplode.GetComponent<SpriteRenderer>().color.r, curExplode.GetComponent<SpriteRenderer>().color.g, curExplode.GetComponent<SpriteRenderer>().color.b, Mathf.Lerp(curExplode.GetComponent<SpriteRenderer>().color.a, 0, fadeLerpT));
            yield return new WaitForSeconds(0.05f);
        }
        Destroy(curExplode);
        Destroy(gameObject);
        yield break;
    }
}
