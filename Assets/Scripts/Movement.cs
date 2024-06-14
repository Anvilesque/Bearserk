using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    private Rigidbody2D rb;
    private CapsuleCollider2D coll;
    private InputActions inputActions;
    [SerializeField] private LayerMask platformLayer;
    private Bearzooka bz;

    public bool isSprinting = false;
    public bool canControlSprint = true;
    public float sprintCap;
    public float maxSprintCap;
    public float sprintRate;
    public float sprintMultiplier;
    private bool sprintRecoverySoftEnabled;
    private bool sprintRecoveryHardEnabled;
    public float sprintRecoveryHardMin;
    public float sprintRecoveryCooldown;
    public float sprintRecoveryTimer;
    public float sprintRecoveryRate;
    public float maxHorizSpeed;
    private float accel;

    public float jumpSpeed;
    public int jumpsTotal;
    public int jumpsAvailable;
    public Dictionary<string, float> defaults = new Dictionary<string, float>();

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<CapsuleCollider2D>();
        inputActions = new InputActions();
        bz = GetComponent<Bearzooka>();

        accel = 5f;

        defaults.Add("sprintMultiplier", 1.2f);
        defaults.Add("maxHorizSpeed", 15);
        defaults.Add("jumpSpeed", 20);
        defaults.Add("jumpsTotal", 1);
        defaults.Add("maxSprintCap", 100);
        defaults.Add("sprintRate", 60);
        defaults.Add("sprintRecoveryHardMin", 30);
        defaults.Add("sprintRecoveryCooldown", 4);
        defaults.Add("sprintRecoveryRate", 20);
    
        sprintMultiplier = defaults["sprintMultiplier"];
        maxHorizSpeed = defaults["maxHorizSpeed"];
        jumpSpeed = defaults["jumpSpeed"];
        jumpsTotal = (int)defaults["jumpsTotal"];
        maxSprintCap = defaults["maxSprintCap"];
        sprintCap = maxSprintCap;
        sprintRate = defaults["sprintRate"];
        sprintRecoveryCooldown = defaults["sprintRecoveryCooldown"];
        sprintRecoveryRate = defaults["sprintRecoveryRate"];
        sprintRecoveryHardMin = defaults["sprintRecoveryHardMin"];

        defaults.Add("shootCooldown", 5f);
        defaults.Add("shootForce", 10f);
        defaults.Add("shellSpeed", 40f);
        defaults.Add("shellLife", 3.0f);
        defaults.Add("shellDamage", 5.0f);

        defaults.Add("clawnchCooldown", 9.0f);
        defaults.Add("clawnchSpeed", 8f);
        defaults.Add("clawnchDuration", 0.3f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (TutorialManager.tutorialMode) return;
        Vector2 moveInput = inputActions.Player.Move.ReadValue<Vector2>();
        // Debug.Log(moveInput);

        #region HorizMovement
        float desiredSpeed = moveInput.x * maxHorizSpeed;
        isSprinting = false;
        if (!inputActions.Player.Sprint.inProgress)
        {
            sprintRecoverySoftEnabled = true;
        }
        else if (sprintRecoveryHardEnabled && sprintCap < sprintRecoveryHardMin) {}
        else if (moveInput.x == 0) {}
        else isSprinting = true;
        
        if (isSprinting) 
        {            
            if (sprintCap > 0)
            {
                sprintRecoveryTimer = 0f;
                sprintRecoverySoftEnabled = false;
                sprintCap = Mathf.Max(0f, sprintCap - sprintRate * Time.deltaTime);
                desiredSpeed *= sprintMultiplier;
            }
            else sprintRecoveryHardEnabled = true;
        }

        if (sprintRecoveryHardEnabled || sprintRecoverySoftEnabled) sprintRecoveryTimer += Time.deltaTime;
        if (sprintRecoveryTimer >= sprintRecoveryCooldown)
        {
            if (sprintCap < maxSprintCap) sprintCap += sprintRecoveryRate * Time.deltaTime;
        }
        if (sprintRecoveryHardEnabled && sprintCap >= sprintRecoveryHardMin) sprintRecoveryHardEnabled = false;

        float difference = desiredSpeed - rb.velocity.x;
        float finalXHoriz = Mathf.Abs(difference) * accel * Mathf.Sign(difference);
        rb.AddForce(new Vector2(finalXHoriz, 0f), ForceMode2D.Force);

        #endregion

        #region JumpReset
        float boxCastYSize = 0.3f;
        float boxCastYDist = 0.2f;
        float boxCastXShrink = 0.1f;
        RaycastHit2D floorDetector = Physics2D.BoxCast(new Vector2(coll.bounds.center.x, coll.bounds.min.y), new Vector2(coll.size.x - boxCastXShrink, boxCastYSize), 0, Vector2.down, boxCastYDist, platformLayer);
        if (floorDetector.collider != null && rb.velocity.y < 0.01)
        {
            jumpsAvailable = jumpsTotal;
        }

        // Debug.Log(floorDetector.rigidbody);
        // Debug.Log("Colliding with" + floorDetector.collider);
        #endregion

    }

    private void OnEnable()
    {
        inputActions.Player.Enable();
    }

    private void OnDisable()
    {
        inputActions.Player.Disable();
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (TutorialManager.tutorialMode) return;
        if (!context.performed) return;
        if (jumpsAvailable < 1) return;
        jumpsAvailable -= 1;
        rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
    }

}
