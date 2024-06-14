using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    private Transform p1, p2, platform;
    private Vector2 p1Pos, p2Pos;
    private Rigidbody2D platformRB;
    private bool movingForward;
    [SerializeField] private float travelRate;
    private bool onCooldown;
    private float cooldown;
    public float cooldownTimer;

    // Start is called before the first frame update
    void Start()
    {
        p1 = transform.Find("Point 1");
        p2 = transform.Find("Point 2");
        p1.gameObject.GetComponent<SpriteRenderer>().enabled = false;
        p2.gameObject.GetComponent<SpriteRenderer>().enabled = false;
        platform = transform.Find("Platform");
        platformRB = platform.gameObject.GetComponent<Rigidbody2D>();
        p1Pos = p1.transform.position;
        p2Pos = p2.transform.position;
        cooldown = 3f;
        onCooldown = false;
        movingForward = true;
        platformRB.velocity = new Vector2((p2Pos.x - platform.position.x) / (Vector2.Distance(p1Pos, p2Pos) / travelRate), (p2Pos.y - platform.position.y) / (Vector2.Distance(p1Pos, p2Pos) / travelRate));
    }

    // Update is called once per frame
    void Update()
    {
        if (onCooldown)
        {
            cooldownTimer += Time.deltaTime;
            if (cooldownTimer > cooldown)
            {
                movingForward = !movingForward;
                if (movingForward)
                {
                    platformRB.velocity = new Vector2((p2Pos.x - platform.position.x) / (Vector2.Distance(p1Pos, p2Pos) / travelRate), (p2Pos.y - platform.position.y) / (Vector2.Distance(p1Pos, p2Pos) / travelRate));
                }
                else
                {
                    platformRB.velocity = new Vector2((p1Pos.x - platform.position.x) / (Vector2.Distance(p1Pos, p2Pos) / travelRate), (p1Pos.y - platform.position.y) / (Vector2.Distance(p1Pos, p2Pos) / travelRate));
                }
                onCooldown = false;
            }
        }
        else if (movingForward)
        {
            if (Mathf.Abs(platform.position.x - p2Pos.x) < 0.1 && Mathf.Abs(platform.position.y - p2Pos.y) < 0.1)
            {
                cooldownTimer = 0f;
                platformRB.velocity = Vector2.zero;
                onCooldown = true;
            }  
        }
        else
        {
            if (Mathf.Abs(platform.position.x - p1Pos.x) < 0.1 && Mathf.Abs(platform.position.y - p1Pos.y) < 0.1)
            {
                cooldownTimer = 0f;
                platformRB.velocity = Vector2.zero;
                onCooldown = true;
            }
        }
    }
}
