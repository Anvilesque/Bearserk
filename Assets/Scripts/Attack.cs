using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Attack : MonoBehaviour
{
    private Camera cam;
    private Rigidbody2D rb;
    public GameObject larm, rarm;
    private Rigidbody2D larmBody, rarmBody;
    private CircleCollider2D larmColl, rarmColl;
    private InputActions inputActions;
    public Vector2 mousePos;
    public float armRadius = 3.0f;
    public bool larmAvailable, rarmAvailable;
    public float larmForce, rarmForce;
    public float larmCooldown, rarmCooldown;
    public float larmTimer, rarmTimer;
    public float larmDistance, rarmDistance;
    public float suplexCooldown;
    public float suplexTimer;
    
    // Start is called before the first frame update
    void Start()
    {
        cam = FindObjectOfType<Camera>();
        rb = GetComponent<Rigidbody2D>();
        larmBody = larm.GetComponent<Rigidbody2D>();
        rarmBody = rarm.GetComponent<Rigidbody2D>();
        larmColl = larm.GetComponent<CircleCollider2D>();
        rarmColl = rarm.GetComponent<CircleCollider2D>();
        larmColl.radius = armRadius;
        rarmColl.radius = armRadius;
        larmForce = 2.0f;
        rarmForce = 2.0f;
        larmCooldown = 2.0f;
        rarmCooldown = 2.0f;
        larmDistance = 10f;
        rarmDistance = 10f;
        inputActions = new InputActions();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (larmTimer < larmCooldown) larmTimer += Time.deltaTime;
        else larmAvailable = true;
        if (rarmTimer < rarmCooldown) rarmTimer += Time.deltaTime;
        else rarmAvailable = true;

        if (larmAvailable) {
            // larm.transform.position = new Vector2(Mathf.Lerp(larm.transform.position.x, transform.position.x, 0.3f), Mathf.Lerp(rarm.transform.position.y, transform.position.y, 0.3f));
        }

        if (Mathf.Sqrt(Mathf.Pow(larm.transform.position.x - rb.transform.position.x, 2) + Mathf.Pow(larm.transform.position.y - rb.transform.position.y, 2)) > larmDistance)
        {
            ArmRestore(0);
        }
    }

    public void ArmSwing(InputAction.CallbackContext context)
    {
        // Debug.Log(context);
        if (!context.started) return;
        if (context.control.path == "/Mouse/leftButton")
        {
            if (!larmAvailable) return;
            larmTimer = 0;
            larmAvailable = false;
            //TODO: Switch linear to force (spring)
            larmBody.AddForce(new Vector2(mousePos.x * larmForce, mousePos.y * larmForce), ForceMode2D.Force);
        }
        if (context.control.path == "/Mouse/rightButton")
        {
            if (!rarmAvailable) return;
            rarmTimer = 0;
            rarmAvailable = false;
            rarmBody.AddForce(new Vector2(mousePos.x * rarmForce, mousePos.y * rarmForce), ForceMode2D.Force);
        }
        
    }

    public void ArmRestore (int arm)
    {
        switch (arm)
        {
            case 0:
                larmBody.AddForce(-larmBody.velocity);
                // go in opp direction
                break;

            case 1:
                rarmBody.AddForce(-rarmBody.velocity);
                break;
        }
    }

}
