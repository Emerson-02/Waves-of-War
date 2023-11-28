using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannonball : MonoBehaviour
{
    public GameObject explosionPrefab;

    void Start()
    {
        StartCoroutine(DestroyAfterSeconds(5f));
    }

    void OnCollisionEnter2D(Collision2D collision)
    {

        if (gameObject.CompareTag("EnemyCannonball"))
        {

            if (collision.gameObject.CompareTag("Island"))
            {
                Instantiate(explosionPrefab, collision.transform.position, collision.transform.rotation);

                Destroy(gameObject);
            }

            if (collision.gameObject.CompareTag("Cannonball"))
            {
                Instantiate(explosionPrefab, collision.transform.position, collision.transform.rotation);

                Destroy(gameObject);
                Destroy(collision.gameObject);
            }
        }

        if (gameObject.CompareTag("Cannonball"))
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Island"))
            {
                Instantiate(explosionPrefab, transform.position, transform.rotation);

                Destroy(gameObject);
            }

            if (collision.gameObject.CompareTag("Cannonball"))
            {

                Instantiate(explosionPrefab, collision.transform.position, collision.transform.rotation);

                Destroy(gameObject);

                Destroy(collision.gameObject);
            }


        }
    }


    IEnumerator DestroyAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        Instantiate(explosionPrefab, transform.position, transform.rotation);

        Destroy(gameObject);
    }
}
