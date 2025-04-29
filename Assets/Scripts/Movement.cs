using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    private Rigidbody2D playerBody;
    private CapsuleCollider2D playerCollider;
    private InputActions inputActions;
    [SerializeField] private LayerMask platformLayer;
    private Bearzooka bearzooka;

    private bool canControlSprint = true;
    private float sprintAmount;
    private float maxSprintAmount;
    private float sprintDepletionRate;
    private float sprintMultiplier;
    private bool sprintRecoveryFromZeroEnabled;
    private float sprintRecoveryFromZeroMin;
    private float sprintRecoveryCooldown;
    private float sprintRecoveryTimer;
    private float sprintRecoveryRate;
    private float maxHorizSpeed;
    private float acceleration;

    private float jumpSpeed;
    private int jumpsTotal;
    private int jumpsAvailable;
    private Dictionary<string, float> defaults = new Dictionary<string, float>();

    // Start is called before the first frame update
    void Awake()
    {
        playerBody = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<CapsuleCollider2D>();
        inputActions = new InputActions();
        bearzooka = GetComponent<Bearzooka>();

        acceleration = 5f;

        defaults.Add("sprintMultiplier", 1.2f);
        defaults.Add("maxHorizSpeed", 15);
        defaults.Add("jumpSpeed", 20);
        defaults.Add("jumpsTotal", 1);
        defaults.Add("maxSprintAmount", 100);
        defaults.Add("sprintDepletionRate", 60);
        defaults.Add("sprintRecoveryFromZeroMin", 30);
        defaults.Add("sprintRecoveryCooldown", 4);
        defaults.Add("sprintRecoveryRate", 20);
    
        sprintMultiplier = defaults["sprintMultiplier"];
        maxHorizSpeed = defaults["maxHorizSpeed"];
        jumpSpeed = defaults["jumpSpeed"];
        jumpsTotal = (int)defaults["jumpsTotal"];
        maxSprintAmount = defaults["maxSprintAmount"];
        sprintAmount = maxSprintAmount;
        sprintDepletionRate = defaults["sprintDepletionRate"];
        sprintRecoveryCooldown = defaults["sprintRecoveryCooldown"];
        sprintRecoveryRate = defaults["sprintRecoveryRate"];
        sprintRecoveryFromZeroMin = defaults["sprintRecoveryFromZeroMin"];

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

        #region HorizontalMovement
        if (IsSprinting(moveInput)) 
        {            
            HandleSprintingAmount();
        }
        else
        {
            sprintRecoveryTimer += Time.deltaTime;
        }

        if (sprintRecoveryTimer >= sprintRecoveryCooldown)
        {
            sprintAmount = Mathf.Min(maxSprintAmount, sprintAmount + sprintRecoveryRate * Time.deltaTime);
        }

        if (sprintRecoveryFromZeroEnabled && sprintAmount >= sprintRecoveryFromZeroMin) sprintRecoveryFromZeroEnabled = false;

        float goalVelocity = moveInput.x * maxHorizSpeed;
        goalVelocity *= IsSprinting(moveInput) ? sprintMultiplier : 1;
        float difference = goalVelocity - playerBody.velocity.x;
        float finalXHoriz = difference * acceleration;
        playerBody.AddForce(new Vector2(finalXHoriz, 0f), ForceMode2D.Force);

        #endregion

        #region JumpReset
        float boxCastYSize = 0.3f;
        float boxCastYDistance = 0.2f;
        float boxCastXShrink = 0.1f;
        Vector2 origin = new Vector2(playerCollider.bounds.center.x, playerCollider.bounds.min.y);
        Vector2 size = new Vector2(playerCollider.size.x - boxCastXShrink, boxCastYSize);
        RaycastHit2D floorDetector = Physics2D.BoxCast(origin, size, 0, Vector2.down, boxCastYDistance, platformLayer);
        if (floorDetector.collider != null && playerBody.velocity.y < 0.01)
        {
            jumpsAvailable = jumpsTotal;
        }

        // Debug.Log(floorDetector.rigidbody);
        // Debug.Log("Colliding with" + floorDetector.collider);
        #endregion

    }

    private bool IsSprinting(Vector2 moveInput)
    {
        if (!inputActions.Player.Sprint.inProgress) return false;
        if (sprintRecoveryFromZeroEnabled && sprintAmount < sprintRecoveryFromZeroMin) return false;
        if (moveInput.x == 0) return false;
        return true;
    }

    private void HandleSprintingAmount()
    {
        if (sprintAmount <= 0)
        {
            sprintRecoveryFromZeroEnabled = true;
            return;
        }
        sprintRecoveryTimer = 0f;
        sprintAmount = Mathf.Max(0f, sprintAmount - sprintDepletionRate * Time.deltaTime);
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (TutorialManager.tutorialMode) return;
        if (!context.performed) return;
        if (jumpsAvailable < 1) return;
        jumpsAvailable -= 1;
        playerBody.velocity = new Vector2(playerBody.velocity.x, jumpSpeed);
    }
}
