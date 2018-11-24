using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using ZenvaVR;

public class GameController : MonoBehaviour {

    public Text scoteText;
    public Text fuelText;
    public Player player;
    public GameObject mainCamera;

    public ObjectPool enemyPool;
    public float enemySpawnInterval = 1f;
    public float horizontalLimit = 2.8f;

    public GameObject fuelPrefab;
    public float fuelSpawnInterval = 9f;
    public float fuelDecreaseSpeed = 5f;

    private float enemySpawnTimer;
    private int score = 0;
    private float fuel = 100f;
    private float fuelSpawnTimer;
    private float restartTimer = 3f;

	// Use this for initialization
	void Start () {
        enemySpawnTimer = enemySpawnInterval;
        fuelSpawnTimer = Random.Range(0f, fuelSpawnInterval);

        player.OnCollect += OnCollectFuel;
    }
	
	// Update is called once per frame
	void Update () {
        //enemy spawning
        if (player != null)
        {
            enemySpawnTimer -= Time.deltaTime;
            if (enemySpawnTimer <= 0)
            {
                enemySpawnTimer = enemySpawnInterval;
                GameObject enemyInstance = enemyPool.GetObj();

                enemyInstance.transform.SetParent(transform);
                enemyInstance.transform.position = new Vector2(
                    Random.Range(-horizontalLimit, horizontalLimit),
                    player.transform.position.y + Screen.height / 100f);

                if(!enemyInstance.GetComponent<Enemy>().HasOnKill())
                    enemyInstance.GetComponent<Enemy>().OnKill += OnEnemyKill;

            }

            fuelSpawnTimer -= Time.deltaTime;
            if (fuelSpawnTimer <= 0)
            {
                fuelSpawnTimer = fuelSpawnInterval;
                GameObject fuelInstance = Instantiate(fuelPrefab);

                fuelInstance.transform.SetParent(transform);
                fuelInstance.transform.position = new Vector2(
                    Random.Range(-horizontalLimit, horizontalLimit),
                    player.transform.position.y + Screen.height / 100f);
            }

            fuel -= Time.deltaTime * fuelDecreaseSpeed;
            fuelText.text = "Fuel: " + (int)fuel;
            if (fuel < 50 && fuel > 15)
            {
                fuelText.color = Color.yellow;
            }
            if (fuel < 15)
            {
                fuelText.color = Color.red;
            }
            if (fuel <= 0)
            {
                fuelText.text = "Fuel: EMPTY!";
                fuelText.color = Color.red;
                Destroy(player.gameObject);
            }
        }
        else
        {
            restartTimer -= Time.deltaTime;
            if (restartTimer <= 0)
                SceneManager.LoadScene("Game");
        }

        //disabled  enemies
        foreach (Enemy enemy in GetComponentsInChildren<Enemy>())
        {
            if(mainCamera.transform.position.y - enemy.transform.position.y > Screen.height / 100f - 1f)
            {
                enemy.gameObject.SetActive(false);
            }
        }
    }

    void OnEnemyKill()
    {
        score += 25;
        scoteText.text = "Score: " + score;
    }

    void OnCollectFuel()
    {
        fuel = 100f;
        fuelText.color = Color.white;
    }
}