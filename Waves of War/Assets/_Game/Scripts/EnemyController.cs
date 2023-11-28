using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    public EnemyShip enemyShip; 
    public int health = 3;
    public int maxHealth = 3;
    public GameObject enemyCannonBallPrefab;

    public Transform firePoint;
    public int tiroInimigo = 2;
    public GameController gameController;
    public float EnemyShotSpeed = 2f;
    public int enemyDamage = 1;
    private float timeSinceLastShot = 0f;
    public GameObject explosionPrefab;
    public static EnemyController enemyController;
    public Slider enemyLifeSlider;
    public Sprite[] enemyHulls;
    public Sprite[] enemySails;
    private int spriteIndex;
    public SpriteRenderer hullRenderer;
    public SpriteRenderer sailsRenderer;

    private void Awake() {
        gameController = FindObjectOfType<GameController>();
    }

    private void Start() {
        tiroInimigo = 2;
        EnemyShotSpeed = 2f;
        spriteIndex = 0;
    }

    void Update()
    {

        if(PlayerController.instance != null)
        {
            Vector3 direction = (PlayerController.instance.transform.position - transform.position);
            direction.z = 0;

            transform.rotation = Quaternion.LookRotation(Vector3.forward, -direction);

            if (enemyShip.shipType == "Shooter" && direction.magnitude < 5)
            {
                GetComponent<Rigidbody2D>().velocity = Vector2.zero;

            }
            else
            {
                direction = direction.normalized;

                GetComponent<Rigidbody2D>().velocity = direction * enemyShip.speed;
            }

            timeSinceLastShot += Time.deltaTime;

            if (enemyShip.shipType == "Shooter" && timeSinceLastShot >= tiroInimigo)
            {
                FireCannonBall();
                timeSinceLastShot = 0f;
            }
        }

        
        
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Cannonball"))
        {
            Destroy(collision.gameObject);

            Instantiate(explosionPrefab, transform.position, transform.rotation);

            TakeDamage(PlayerController.instance.PlayerDamage);

            if (health <= 0)
            {
                gameController.score += 1;
                Destroy(gameObject);
            }
        }
    }

    void FireCannonBall()
    {
        GameObject cannonBall = Instantiate(enemyCannonBallPrefab, firePoint.position, firePoint.rotation);

        Rigidbody2D rb = cannonBall.GetComponent<Rigidbody2D>();
        rb.velocity = firePoint.right * EnemyShotSpeed;
    }

    void TakeDamage(int damage)
    {
        health -= damage;

        if (spriteIndex < enemyHulls.Length && spriteIndex < enemySails.Length)
        {
            hullRenderer.sprite = enemyHulls[spriteIndex];
            sailsRenderer.sprite = enemySails[spriteIndex];
        }
        else
        {
            Destroy(gameObject);
        }

        spriteIndex++;

        enemyLifeSlider.value = health;
    }

    
}
