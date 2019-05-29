using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    public delegate void KillHandler();
    public event KillHandler OnKill;

    [SerializeField]
    private GameObject bulletPrefab;
    [SerializeField]
    private float speed = 2f;
    [SerializeField]
    private float shootingInterval = 3f;
    [SerializeField]
    private float bulletSpeed = 2f;

    private float shootingTimet;

	void OnEnable () {
        shootingTimet = Random.Range(0f, shootingInterval);

        GetComponent<Rigidbody2D>().velocity = new Vector2(0, -speed);
	}

	// Update is called once per frame
	void Update () {
        shootingTimet -= Time.deltaTime;
        if (shootingTimet <= 0) {
            shootingTimet = shootingInterval;

            GameObject bulletInstance = Instantiate(bulletPrefab);
            bulletInstance.transform.SetParent(transform.parent);
            bulletInstance.transform.position = transform.position;
            bulletInstance.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -bulletSpeed);
            Destroy(bulletInstance, 2f);
        }
	}

    void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player Bullet") {
            gameObject.SetActive(false);
            Destroy(other.gameObject);
            if (OnKill != null)
                OnKill();
        }

    }

    public bool HasOnKill() {
        return OnKill != null;
    }
}