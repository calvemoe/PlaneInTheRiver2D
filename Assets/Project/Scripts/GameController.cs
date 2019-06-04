using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using ZenvaVR;

public class GameController : MonoBehaviour {

    [SerializeField]
    private Text scoteText;
    [SerializeField]
    private Text fuelText;
    [SerializeField]
    private Player player;
    [SerializeField]
    private GameObject mainCamera;

    [SerializeField]
    private ObjectPool enemyPool;
    [SerializeField]
    private ObjectPool fuelBarrelPool;

    [SerializeField]
    private float enemySpawnInterval = 1f;
    [SerializeField]
    private float horizontalLimit = 2.8f;


    [SerializeField]
    private float fuelSpawnInterval = 9f;
    [SerializeField]
    private float fuelDecreaseSpeed = 5f;

    private float enemySpawnTimer;
    private int score = 0;
    private float fuel = 100f;
    private float fuelSpawnTimer;
    private float restartTimer = 3f;

    private static float screenSizeDividedByHandred = Screen.height / 100f;

    private static Transform mainCameraTransform;
    Transform playerTransform;

    void Awake() {
        mainCameraTransform = mainCamera.transform;
        playerTransform = player.transform;
    }

	void Start () {
        enemySpawnTimer = enemySpawnInterval;
        fuelSpawnTimer = Random.Range(0f, fuelSpawnInterval);

        player.OnCollect += OnCollectFuel;
        scoteText.text = score.ToString();
    }
	
	// Update is called once per frame
	void Update () {
        //enemy spawning
        if (player != null) {
            enemySpawnTimer -= Time.deltaTime;
            if (enemySpawnTimer <= 0) {
                enemySpawnTimer = enemySpawnInterval;
                GameObject enemyInstance = enemyPool.GetObj();

                enemyInstance.transform.SetParent(transform);
                enemyInstance.transform.position = new Vector2(
                    Random.Range(-horizontalLimit, horizontalLimit),
                    playerTransform.position.y + screenSizeDividedByHandred);

                if(!enemyInstance.GetComponent<Enemy>().HasOnKill())
                    enemyInstance.GetComponent<Enemy>().OnKill += OnEnemyKill;
            }

            fuelSpawnTimer -= Time.deltaTime;
            if (fuelSpawnTimer <= 0) {
                fuelSpawnTimer = fuelSpawnInterval;
                GameObject fuelInstance = fuelBarrelPool.GetObj();

                fuelInstance.transform.SetParent(transform);
                fuelInstance.transform.position = new Vector2(
                    Random.Range(-horizontalLimit, horizontalLimit),
                    playerTransform.position.y + screenSizeDividedByHandred);
            }

            fuel -= Time.deltaTime * fuelDecreaseSpeed;
            fuelText.text = fuel.ToString("F0");
            if (fuel < 50 && fuel > 15)
                fuelText.color = Color.yellow;

            else if (fuel < 15)
                fuelText.color = Color.red;

            if (fuel <= 0) {
                fuelText.text = "EMPTY!";
                fuelText.color = Color.red;
                Destroy(player.gameObject);
            }
        }
        else {
            restartTimer -= Time.deltaTime;
            if (restartTimer <= 0)
                SceneManager.LoadScene("Game");
        }

        //disabling enemies
        foreach (GameObject enemy in enemyPool.GetAllActive()) {
            if(mainCameraTransform.position.y - enemy.transform.position.y > screenSizeDividedByHandred)
                enemy.gameObject.SetActive(false);
        }

        //disablicng fuel barrels
        foreach (GameObject barrel in fuelBarrelPool.GetAllActive()) {
            if(mainCameraTransform.position.y - barrel.transform.position.y > screenSizeDividedByHandred)
                barrel.gameObject.SetActive(false);
        }
    }

    void OnEnemyKill() {
        score += 25;
        scoteText.text = score.ToString();
    }

    void OnCollectFuel() {
        fuel = 100f;
        fuelText.color = Color.white;
    }

    public static float GetScreenSize() {
        return screenSizeDividedByHandred / 1.8f;
    }

    public static Transform GetMainCameraTransform() {
        return mainCameraTransform;
    }

}