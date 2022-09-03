using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] GameObject deathFX;
    [SerializeField] GameObject hitFX;
    [SerializeField] int addScore=10;
    [SerializeField] int health = 2;

    ScoreBoard scoreboard;
    GameObject parentGameObject;

    private void Start()
    {
        scoreboard = FindObjectOfType<ScoreBoard>();
        parentGameObject = GameObject.FindWithTag("SpawnRuntime");
        AddRigidbody();
    }

    private void AddRigidbody()
    {
        Rigidbody rb = gameObject.AddComponent<Rigidbody>();
        rb.useGravity = false;
    }

    private void OnParticleCollision(GameObject other)
    {
        
        ProcessHit();
        if(health<1)
        {
            KillEnemy();
        }

        
    }

    private void KillEnemy()
    {
        scoreboard.IncreaseScore(addScore);
        GameObject fx = Instantiate(deathFX, transform.position, Quaternion.identity);
        fx.transform.parent = parentGameObject.transform;
        Destroy(gameObject);
    }

    private void ProcessHit()
    {
        GameObject fx = Instantiate(hitFX, transform.position, Quaternion.identity);
        fx.transform.parent = parentGameObject.transform;
        health--;

        scoreboard.IncreaseScore(addScore);
    }
}
