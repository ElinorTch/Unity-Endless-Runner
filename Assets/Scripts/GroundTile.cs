using System.Collections;
using UnityEngine;

public class GroundTile : MonoBehaviour
{
    private GroundSpawner groundSpawner;

    public GameObject coinPrefab;
    public GameObject[] obstaclePrefabs;

    public Transform UpDownSpawnPoint;
    public Transform[] coneSpawnPoints;

    public GameObject[] powerPrefabs; 
    public Transform[] powerSpawnPoints;


    private void Awake()
    {
        groundSpawner = GameObject.FindAnyObjectByType<GroundSpawner>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SpawnObstacle();
        SpawnCoins();
        SpawnPower();   
    }

    private void OnTriggerExit(Collider other)
    {
        groundSpawner.SpawnTile();
        Destroy(gameObject, 5f);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SpawnObstacle()
    {
        int randomIndex = Random.Range(1, obstaclePrefabs.Length);
        int spawnPointIndex = Random.Range(0, coneSpawnPoints.Length);
        Instantiate(obstaclePrefabs[0], coneSpawnPoints[spawnPointIndex].transform.position, Quaternion.identity);
        Instantiate(obstaclePrefabs[randomIndex], UpDownSpawnPoint.position, Quaternion.identity);
        return;
    }

    public void SpawnCoins()
    {
        int spawnAmount = 5;
        for (int i = 0; i < spawnAmount; i++)
        {
            GameObject tempCoin = Instantiate(coinPrefab);
            tempCoin.transform.position = spawnRandomPoint(GetComponent<Collider>());
        }
    }

    void SpawnPower()
    {
        int canSpawn = Random.Range(0, 2);
        if (canSpawn == 0) return;
        
        int powerIndex = Random.Range(0, powerPrefabs.Length);
        int pointIndex = Random.Range(0, powerSpawnPoints.Length);
        Instantiate(powerPrefabs[powerIndex], powerSpawnPoints[pointIndex].position, Quaternion.identity);
        return;
    }


    Vector3 spawnRandomPoint(Collider col)
    {
        Vector3 point = new Vector3(
            Random.Range(col.bounds.min.x, col.bounds.max.x),
            1f,
            Random.Range(col.bounds.min.z, col.bounds.max.z)
        );
        return point;
    }
}
