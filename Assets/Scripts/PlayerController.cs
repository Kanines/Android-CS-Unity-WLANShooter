using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using CnControls;

public class PlayerController : NetworkBehaviour
{
    public GameObject bulletPrefab;
    public float playerSpeed = 1.0f;
    public float bulletSpeed = 2.0f;

    [SyncVar]
    public int score = 0;

    public float fireRate = 0.5f;
    public float enemyFireRate = 1.0f;
    private float lastShot = 0.0f;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!isLocalPlayer)
        {
            if (this.transform.tag == "Enemy")
            {
                CmdEnemyFire();
            }
            return;
        }

        var x = CnInputManager.GetAxis("Horizontal") * Time.deltaTime * playerSpeed;
        var y = CnInputManager.GetAxis("Vertical") * Time.deltaTime * playerSpeed;

        transform.Translate(x, y, 0);

        if (Input.touchCount > 0)
        {
            for (int i = 0; i < Input.touchCount; i++)
            {
                Vector2 touchPosition = Input.GetTouch(i).position;
                if (touchPosition.x > 340)
                {
                    Vector2 worldPoint = Camera.main.ScreenToWorldPoint(touchPosition);
                    CmdFire(worldPoint);
                }
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            Vector2 touchPosition = Input.mousePosition;
            if (touchPosition.x > 340)
            {
                Vector2 worldPoint = Camera.main.ScreenToWorldPoint(touchPosition);
                CmdFire(worldPoint);
            }
        }
    }

    [Command]
    void CmdFire(Vector2 touchPosition)
    {
        if (Time.time > fireRate + lastShot)
        {
            // Create the Bullet from the Bullet Prefab
            var bullet = (GameObject)Instantiate(
                bulletPrefab,
                this.transform.position,
                this.transform.rotation);

            Vector2 targetPosition = touchPosition - (new Vector2(transform.position.x, transform.position.y));
            targetPosition.Normalize();

            // Add velocity to the bullet
            bullet.GetComponent<Rigidbody2D>().velocity = targetPosition * bulletSpeed;
            bullet.GetComponent<Bullet>().shooter = transform;
            NetworkServer.Spawn(bullet);

            lastShot = Time.time;
        }
    }

    [Command]
    void CmdEnemyFire()
    {
        if (Time.time > enemyFireRate + lastShot)
        {
            // Create the Bullet from the Bullet Prefab
            var bullet = (GameObject)Instantiate(
                bulletPrefab,
                this.transform.position,
                this.transform.rotation);

            Vector2 targetPosition = (new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)));

            // Add velocity to the bullet
            bullet.GetComponent<Rigidbody2D>().velocity = targetPosition * bulletSpeed;
            bullet.GetComponent<Bullet>().shooter = transform;
            NetworkServer.Spawn(bullet);

            lastShot = Time.time;
        }
    }

    public override void OnStartLocalPlayer()
    {
        GetComponent<SpriteRenderer>().material.color = Color.blue;
    }
}
