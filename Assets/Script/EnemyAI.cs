using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float speed = 2f;
    public float patrolDistance = 3f;
    private float initialPosition;
    private bool movingRight = true;
    public Transform player; 
    public float chaseRange = 5f; 


    void Start()
    {
        initialPosition = transform.position.x;
    }

    void Update()
    {
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);

            if (distanceToPlayer < chaseRange)
            {
                ChasePlayer();
            }
            else
            {
                Patrol();
            }
    }

    void Patrol()
    {
        // Bewegt den Enemy in die aktuelle Richtung
        transform.Translate(Vector2.right * speed * Time.deltaTime * (movingRight ? 1 : -1));

        // Prüft, ob die Grenze der Patrouille erreicht wurde
        if (movingRight && transform.position.x >= initialPosition + patrolDistance)
        {
            Flip();
        }
        else if (!movingRight && transform.position.x <= initialPosition - patrolDistance)
        {
            Flip();
        }
    }

    void Flip()
    {
        movingRight = !movingRight;

        // Umkehren der Skalierung
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }


    void DetectEdges()
    {
        RaycastHit2D groundInfo = Physics2D.Raycast(transform.position, Vector2.down, 1f);
        if (groundInfo.collider == false)
        {
            Flip();
        }
    }
    void ChasePlayer()
    {
        // Bewegt den Enemy in Richtung des Spielers
        if (player.position.x > transform.position.x)
        {
            // Spieler ist rechts vom Enemy
            transform.Translate(Vector2.right * speed * Time.deltaTime);
            if (!movingRight) Flip();
        }
        else
        {
            // Spieler ist links vom Enemy
            transform.Translate(Vector2.left * speed * Time.deltaTime);
            if (movingRight) Flip();
        }
    }


}
