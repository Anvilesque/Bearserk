using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour
{

    public float health;
    public float maxHealth;
    public float deathTimer;
    private bool deathEnabled;
    private bool killHealth;
    private float killTimer;

    // Start is called before the first frame update
    void Start()
    {
        deathEnabled = false;
        deathTimer = 0f;
        if (tag == "Player")
        {
            killTimer = 0;
            killHealth = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (deathEnabled) deathTimer += Time.deltaTime;
        if (deathTimer >= 3.0f) Destroy(gameObject);

        if (killHealth)
        {
            killTimer += Time.deltaTime;
            health = (maxHealth - killTimer);
        }
        if (health <= 0) Die();
    }

    void TakeDamage(float dmg)
    {
        health -= dmg;
    }

    public void Die()
    {
        if (tag == "Player")
        {
            GetComponent<PlayerInput>().enabled = false;
            SceneManager.LoadScene("CardMenu", LoadSceneMode.Single);
            FindObjectOfType<SoundManager>().gameObject.GetComponent<SoundManager>().Play(FindObjectOfType<SoundManager>().gameObject.GetComponent<SoundManager>().songs[2]);
        }
        else deathEnabled = true;

    }

    void ResetHealth()
    {
        health = maxHealth;
    }
}
