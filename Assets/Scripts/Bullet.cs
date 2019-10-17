using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int bounces = 2;
    public Transform shooter;

    void Start()
    {
        StartCoroutine(DelayCollider());
    }

    IEnumerator DelayCollider()
    {
        yield return new WaitForSeconds(0.12f);
        GetComponent<Collider2D>().enabled = true;
        this.GetComponent<SpriteRenderer>().color = Color.red;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Wall" && bounces > 0)
        {
            bounces--;

            if (bounces == 1)
                this.GetComponent<SpriteRenderer>().color = new Color(1.0f, 0.55f, 0.0f, 1.0f);
            if (bounces == 0)
                this.GetComponent<SpriteRenderer>().color = Color.yellow;

        }
        else
        {
            var hit = collision.gameObject;
            var health = hit.GetComponent<Health>();
            if (health != null)
            {
                health.TakeDamage(1, shooter);
            }

            Destroy(gameObject);
        }
    }
}
