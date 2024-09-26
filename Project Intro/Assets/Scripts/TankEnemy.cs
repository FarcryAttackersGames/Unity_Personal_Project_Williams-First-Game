using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TankEnemy : MonoBehaviour
{
    public PlayerController player;
    public NavMeshAgent agent;

    [Header("EnemyStats")]
    public int maxhealth = 5;
    public float health = 5f;
    public float damagetaken = 0.5f;
    public int damagedealt = 0;
    public float knockbackResistance = 0.1f;

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

        if (health <= 0)
            Destroy(gameObject);
    }
        private void OnCollisionEnter(Collision collision)
    {
            if (collision.gameObject.tag == "shot");
            {
                health -= damagetaken;
                Destroy(collision.gameObject);
            }
    }
}
