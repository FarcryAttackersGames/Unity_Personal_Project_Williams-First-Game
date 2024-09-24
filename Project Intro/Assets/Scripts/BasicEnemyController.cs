using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BasicEnemyController : MonoBehaviour
{
    public PlayerController player;
    public NavMeshAgent agent;

    [Header("EnemyStats")]
    public int Health = 5;
    public int MaxHealth = 5;
    public int damageGiven = 1;
    public int damageTaken = 1;
    public int knockbackForce = 10000;
    public int knockbackForceTaken = 10000;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        agent.destination = player.transform.position;

        if (Health <= 0)
            Destroy(gameObject);
    }
        private void OnCollisionEnter(Collision collision)
    {
            if (collision.gameObject.tag == "shot")
            {
                Health -= damageGiven;
                Destroy(collision.gameObject);
            }
    }
}
