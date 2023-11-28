using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float speed = 5.0f;
    public int life = 3;
    public int maxLife = 3;
    private SpriteRenderer spriteRenderer;
    public Slider playerLifeSlider;
    public GameObject cannonballPrefab;
    public float cannonballSpeed = 10f;
    public static PlayerController instance;
    public GameObject[] cannons = new GameObject[6]; 
    public Vector3[] cannonPositions = new Vector3[6];
    public Sprite[] playerHulls;
    public Sprite[] playerSails;
    public GameObject explosionPrefab;
    public UIController uiController;
    public GameController gameController;
    public int PlayerDamage = 1;
    public int enemyDamage = 1;
    

    private void Awake() {
        uiController = FindObjectOfType<UIController>();
        gameController = FindObjectOfType<GameController>();

        if (instance == null)
        {
            instance = this;
        }
        else if(instance != this)
        {
            Destroy(gameObject);
        }

    }

    private void Start() 
    {
        transform.Find("Hull").GetComponent<SpriteRenderer>().sprite = playerHulls[0];

        transform.Find("Sail").GetComponent<SpriteRenderer>().sprite = playerSails[0];

        spriteRenderer = GetComponent<SpriteRenderer>();

        for (int i = 0; i < cannons.Length; i++)
        {
            cannons[i] = transform.Find("Cannon (" + (i + 1) + ")").gameObject;

            cannonPositions[i] = cannons[i].transform.position;
        }
    }

    void Update()
    {

        for (int i = 0; i < cannons.Length; i++)
        {
            cannonPositions[i] = cannons[i].transform.position;
        }

        FollowMouse();
        MoveTowardsMouse();
        Shot();
        FireCannons();
    }

    void FollowMouse()
    {
        if (Time.timeScale != 0)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 difference = mousePosition - transform.position;
            float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0.0f, 0.0f, rotationZ+90);

        }

        
    }

    void MoveTowardsMouse()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = (mousePosition - transform.position).normalized;
        direction.z = 0;

        GetComponent<Rigidbody2D>().velocity = direction * speed;
    }



    void Shot()
    {
        if (Input.GetMouseButtonDown(0))
        {

            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0;

            Vector3 direction = (mousePosition - transform.position).normalized;

            GameObject cannonball = Instantiate(cannonballPrefab, transform.position, Quaternion.identity);

            Rigidbody2D rb = cannonball.GetComponent<Rigidbody2D>();

            rb.velocity = direction * cannonballSpeed;
        }
    }


    void FireCannons()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Vector2[] directions = new Vector2[6];
            
            float playerRotation = transform.eulerAngles.z * Mathf.Deg2Rad;

            directions[0] = new Vector2(Mathf.Cos(playerRotation + Mathf.PI), Mathf.Sin(playerRotation + Mathf.PI)); 
            directions[1] = new Vector2(Mathf.Cos(playerRotation + Mathf.PI), Mathf.Sin(playerRotation + Mathf.PI)); 
            directions[2] = new Vector2(Mathf.Cos(playerRotation + Mathf.PI), Mathf.Sin(playerRotation + Mathf.PI)); 
            directions[3] = new Vector2(Mathf.Cos(playerRotation), Mathf.Sin(playerRotation)); 
            directions[4] = new Vector2(Mathf.Cos(playerRotation), Mathf.Sin(playerRotation)); 
            directions[5] = new Vector2(Mathf.Cos(playerRotation), Mathf.Sin(playerRotation)); 

            for (int i = 0; i < cannonPositions.Length; i++)
            {
                GameObject cannonball = Instantiate(cannonballPrefab, cannonPositions[i], Quaternion.identity);

                Rigidbody2D rb = cannonball.GetComponent<Rigidbody2D>();

                rb.velocity = directions[i % directions.Length] * cannonballSpeed;
            }
        }
        
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        EnemyController enemy = collision.gameObject.GetComponent<EnemyController>();
        if (enemy != null && enemy.enemyShip.shipType == "Chaser")
        {
            TakeDamage(enemyDamage);

            Instantiate(explosionPrefab, collision.transform.position, collision.transform.rotation);

            Destroy(collision.gameObject);

            if (life >= 0 && life < playerHulls.Length)
            {
                int spriteIndex = playerHulls.Length - life - 1;

                transform.Find("Hull").GetComponent<SpriteRenderer>().sprite = playerHulls[spriteIndex];

                transform.Find("Sail").GetComponent<SpriteRenderer>().sprite = playerSails[spriteIndex];

            }

            if (life <= 0)
            {
                Destroy(gameObject);

                Instantiate(explosionPrefab, transform.position, transform.rotation);
            }


        }

        if (collision.gameObject.CompareTag("EnemyCannonball"))
        {
            TakeDamage(enemyDamage);

            Destroy(collision.gameObject);

            Instantiate(explosionPrefab, collision.transform.position, collision.transform.rotation);

            if (life >= 0 && life < playerHulls.Length)
            {
                int spriteIndex = playerHulls.Length - life - 1;

                transform.Find("Hull").GetComponent<SpriteRenderer>().sprite = playerHulls[spriteIndex];

                transform.Find("Sail").GetComponent<SpriteRenderer>().sprite = playerSails[spriteIndex];

            }

            if (life <= 0)
            {
                Destroy(gameObject);

                Instantiate(explosionPrefab, transform.position, transform.rotation);
            }
        }
        
    }

    void TakeDamage(int damage)
    {

        life -= damage;

        playerLifeSlider.value = life;
    }

}
