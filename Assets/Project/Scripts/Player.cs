using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public delegate void FuelCollectionHandler();
    public event FuelCollectionHandler OnCollect;
    
    public float speedVertical = 1f;
    public float speedHorizontal = 3f;
    public float horizontalLimit = 2.8f;

    public float bulletSpeed = 2f;
    public GameObject bulletPrefab;

    private bool fired = false;

	// Use this for initialization
	void Start () {
		
	}

    // Update is called once per frame
    void Update()
    {
        //movement logic
        GetComponent<Rigidbody2D>().velocity = new Vector2(Input.GetAxis("Horizontal") * speedHorizontal, speedVertical);
        if (Input.GetAxis("Horizontal") > 0)
            transform.rotation = Quaternion.Euler(new Vector3(0, 15, 90));
        else if (Input.GetAxis("Horizontal") < 0)
            transform.rotation = Quaternion.Euler(new Vector3(0, -15, 90));
        else
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90));

        //keep player within bounce
        if (transform.position.x > horizontalLimit)
        {
            transform.position = new Vector2(horizontalLimit, transform.position.y);
        }
        else if (transform.position.x < -horizontalLimit)
        {
            transform.position = new Vector2(-horizontalLimit, transform.position.y);
        }

        //if (Input.GetKeyDown("mouse 0"))      //only for mouse 0, but without 'fired' bool and key-up logic
        if (Input.GetAxis("Fire1") == 1)
        {
            if (!fired)
            {
                fired = true;
                GameObject bulletInstance = Instantiate(bulletPrefab);
                bulletInstance.transform.SetParent(transform.parent);
                bulletInstance.transform.position = transform.position;
                bulletInstance.GetComponent<Rigidbody2D>().velocity = new Vector2(0, bulletSpeed);
                Destroy(bulletInstance, 2f);
            }
        }
        else
            fired = false;

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy Bullet" || other.tag == "Enemy")
        {
            Destroy(gameObject);
            Destroy(other.gameObject);
        }
        else
        if (other.tag == "FuelTank")
        {
            Destroy(other.gameObject);
            if (OnCollect != null)
                OnCollect();
        }
    }
}
