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
    private float enemySpawnInterval = 1f;
    [SerializeField]
    private float horizontalLimit = 2.8f;

    [SerializeField]
    private GameObject fuelPrefab;
    [SerializeField]
    private float fuelSpawnInterval = 9f;
    [SerializeField]
    private float fuelDecreaseSpeed = 5f;

    private float enemySpawnTimer;
    private int score = 0;
    private float fuel = 100f;
    private float fuelSpawnTimer;
    private float restartTimer = 3f;

    Transform mainCameraTransform;
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
                    playerTransform.position.y + Screen.height / 100f);

                if(!enemyInstance.GetComponent<Enemy>().HasOnKill())
                    enemyInstance.GetComponent<Enemy>().OnKill += OnEnemyKill;
            }

            fuelSpawnTimer -= Time.deltaTime;
            if (fuelSpawnTimer <= 0) {
                fuelSpawnTimer = fuelSpawnInterval;
                GameObject fuelInstance = Instantiate(fuelPrefab);

                fuelInstance.transform.SetParent(transform);
                fuelInstance.transform.position = new Vector2(
                    Random.Range(-horizontalLimit, horizontalLimit),
                    playerTransform.position.y + Screen.height / 100f);
            }

            fuel -= Time.deltaTime * fuelDecreaseSpeed;
            fuelText.text = fuel.ToString("F0");
            if (fuel < 50 && fuel > 15)
                fuelText.color = Color.yellow;

            else if (fuel < 15)
                fuelText.color = Color.red;

            else if (fuel <= 0) {
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

        //disabled  enemies
        foreach (Enemy enemy in GetComponentsInChildren<Enemy>()) {
            if(mainCameraTransform.position.y - enemy.transform.position.y > Screen.height / 100f - 1f)
                enemy.gameObject.SetActive(false);
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
}