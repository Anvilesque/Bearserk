using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Bearzooka : MonoBehaviour
{
    private Camera cam;
    public Vector2 mousePos;
    public float shootCooldown;
    public float shootTimer;
    public float shootForce;
    public float shellSpeed;
    public float shellLife;
    public float shellDamage;

    private Movement mvmt;
    [SerializeField] private GameObject bzAmmo;
    [SerializeField] private Rigidbody2D playerRB;

    // Start is called before the first frame update
    void Awake()
    {
        mvmt = GetComponent<Movement>();
        cam = FindObjectOfType<Camera>();

        shootCooldown = mvmt.defaults["shootCooldown"];
        shootForce = mvmt.defaults["shootForce"];
        shootTimer = shootCooldown;
        shellSpeed = mvmt.defaults["shellSpeed"];
        shellLife = mvmt.defaults["shellLife"];
    }

    // Update is called once per frame
    void Update()
    {
        shootTimer += Time.deltaTime;
    }

    public void MouseUpdate(InputAction.CallbackContext context)
    {
        Vector2 mouseInput = context.ReadValue<Vector2>();
        mousePos = cam.ScreenToWorldPoint(mouseInput);
        // Debug.Log(mousePos);
    }

    public void Shoot(InputAction.CallbackContext context)
    {
        if (TutorialManager.tutorialMode) return;
        if (!context.performed) return;
        if (shootTimer < shootCooldown) return;
        HandleShoot();    
    }

    public void HandleShoot()
    {
        shootTimer = 0f;
        Vector2 playerPos = playerRB.transform.position;

        Vector2 point = new Vector2(mousePos.x - playerPos.x, mousePos.y - playerPos.y);
        float rad = Mathf.Atan((point.y) / (point.x));
        float x = Mathf.Sign(point.x) * Mathf.Abs(Mathf.Cos(rad / 1));
        float y = Mathf.Sign(point.y) * Mathf.Abs(Mathf.Sin(rad / 1));
        
        playerRB.AddForce((new Vector2(-x, -y)) * shootForce, ForceMode2D.Impulse);

        var shellRotation = rad * 180.0f / Mathf.PI;
        if (Mathf.Sign(point.x) < 0) shellRotation += 180f;
        GameObject shell = Instantiate(bzAmmo, playerRB.transform.position, Quaternion.Euler(0, 0, shellRotation), playerRB.transform.Find("Bearzooka"));
        // Debug.Log(rad * 180.0f / Mathf.PI);

        Rigidbody2D shellRB = shell.GetComponent<Rigidbody2D>();
        BearzookaAmmo ammoStats = shell.GetComponent<BearzookaAmmo>();
        ammoStats.life = shellLife;
        ammoStats.damage = shellDamage;
        
        shellRB.AddForce((new Vector2(x, y)) * shellSpeed, ForceMode2D.Impulse);
    }
}
