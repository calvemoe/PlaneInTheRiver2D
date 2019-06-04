using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZenvaVR;

public class Player : MonoBehaviour {

    public delegate void FuelCollectionHandler();
    public event FuelCollectionHandler OnCollect;
    
    [SerializeField]
    private float speedVertical = 1f;
    [SerializeField]
    private float speedHorizontal = 3f;
    [SerializeField]
    private float horizontalLimit = 2.8f;

    [SerializeField]
    private float bulletSpeed = 2f;
    [SerializeField]
    private ObjectPool bulletPool;

    private bool fired = false;
    private Rigidbody2D rb;

    // for sprite rotation
    private Vector3 onRight = new Vector3(0, 15, 90);
    private Vector3 onLeft = new Vector3(0, -15, 90);
    private Vector3 onDefault = new Vector3(0, 0, 90);

	void Awake () {
		rb = GetComponent<Rigidbody2D>();
	}

    // Update is called once per frame
    void Update()
    {
        //movement logic
        rb.velocity = new Vector2(Input.GetAxis("Horizontal") * speedHorizontal, speedVertical);
        if (Input.GetAxis("Horizontal") > 0)
            transform.rotation = Quaternion.Euler(onRight);
        else if (Input.GetAxis("Horizontal") < 0)
            transform.rotation = Quaternion.Euler(onLeft);
        else
            transform.rotation = Quaternion.Euler(onDefault);

        //keep player within bounce
        if (transform.position.x > horizontalLimit)
            transform.position = new Vector2(horizontalLimit, transform.position.y);
        else if (transform.position.x < -horizontalLimit)
            transform.position = new Vector2(-horizontalLimit, transform.position.y);

        //if (Input.GetKeyDown("mouse 0"))      //only for mouse 0, but without 'fired' bool and key-up logic
        if (Input.GetAxis("Fire1") == 1) {
            if (!fired) {
                fired = true;
                GameObject bulletInstance = bulletPool.GetObj();
                bulletInstance.transform.SetParent(transform.parent);
                bulletInstance.transform.position = transform.position;
                bulletInstance.GetComponent<Rigidbody2D>().velocity = new Vector2(0, bulletSpeed);
            }
        }
        else
            fired = false;

        //disabling player bullets
        foreach (GameObject bullet in bulletPool.GetAllActive()) {
            if(bullet.transform.position.y > GameController.GetMainCameraTransform().position.y + GameController.GetScreenSize())
                bullet.gameObject.SetActive(false);
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Enemy Bullet") || other.CompareTag("Enemy")) {
            Destroy(gameObject);
            other.gameObject.SetActive(false);
        }
        else if (other.CompareTag("FuelTank")) {
            other.gameObject.SetActive(false);
            if (OnCollect != null)
                OnCollect();
        }
    }
}
