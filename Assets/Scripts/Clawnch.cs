using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Clawnch : MonoBehaviour
{
    public Vector2 screenMousePos;
    public bool clawnchEnabled;
    public float clawnchCooldown;
    public float clawnchTimer;
    public float clawnchDuration;
    public float clawnchDurationTimer;
    public float clawnchSpeed;
    private Movement mvmt;
    [SerializeField] private Rigidbody2D playerRB;
    [SerializeField] private GameObject clawnchIndicator;
    private GameObject curClawnchIndic;
    private Vector2 clawnchCtrlPt;

    // Start is called before the first frame update
    void Awake()
    {
        mvmt = GetComponent<Movement>();
        clawnchEnabled = false;
        
        clawnchCooldown = mvmt.defaults["clawnchCooldown"];
        clawnchSpeed = mvmt.defaults["clawnchSpeed"];
        clawnchDuration = mvmt.defaults["clawnchDuration"];
        clawnchTimer = clawnchCooldown;
        clawnchDurationTimer = clawnchDuration;
    }

    // Update is called once per frame
    void Update()
    {
        clawnchDurationTimer += Time.deltaTime;
        if (clawnchDurationTimer > clawnchDuration)
        {
            DisableClawnch();
            clawnchTimer += Time.deltaTime;
        }
    }
    
    void FixedUpdate()
    {
        if (clawnchEnabled)
        {
            HandleClawnchMovement();
            HandleClawnchIndicator();
            HandleClawnchHitbox();
        }
    }

    void HandleClawnchMovement()
    {
        Vector2 playerPos = playerRB.transform.position;
        GameObject knob = curClawnchIndic.transform.GetChild(0).gameObject;

        Vector2 point = new Vector2(screenMousePos.x - clawnchCtrlPt.x, screenMousePos.y - clawnchCtrlPt.y);
        float rad = Mathf.Atan((point.y) / (point.x));
        float x = Mathf.Sign(point.x) * Mathf.Abs(Mathf.Cos(rad / 1));
        float y = Mathf.Sign(point.y) * Mathf.Abs(Mathf.Sin(rad / 1));

        if (!float.IsNaN(x) && !float.IsNaN(y))
        {
            playerRB.velocity = new Vector2(x * clawnchSpeed, y * clawnchSpeed);            
            knob.transform.position = new Vector2(curClawnchIndic.transform.position.x + x, curClawnchIndic.transform.position.y + y);
        }
        else
        {
            knob.transform.position = curClawnchIndic.transform.position;
        }

        
    }

    void HandleClawnchIndicator()
    {
        
        
    }

    void HandleClawnchHitbox()
    {

    }

    void EnableClawnch()
    {
        clawnchTimer = 0f;
        clawnchDurationTimer = 0f;
        clawnchEnabled = true;
        clawnchCtrlPt = screenMousePos;
        Vector2 worldCoords = FindObjectOfType<Camera>().ScreenToWorldPoint(screenMousePos);
        curClawnchIndic = Instantiate(clawnchIndicator, worldCoords, Quaternion.identity, FindObjectOfType<Canvas>().transform);
    }

    void DisableClawnch()
    {
        clawnchEnabled = false;
        Destroy(curClawnchIndic);
    }

    public void ClawnchForward(InputAction.CallbackContext context)
    {
        if (TutorialManager.tutorialMode) return;
        if (!context.performed) return;
        if (clawnchTimer < clawnchCooldown) return;
        EnableClawnch();  
    }

    public void ScreenMouseUpdate(InputAction.CallbackContext context)
    {
        screenMousePos = context.ReadValue<Vector2>();
    }
    
}
