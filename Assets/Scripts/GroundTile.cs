using UnityEngine;

public class GroundTile : MonoBehaviour
{
    private GroundSpawner groundSpawner;

    public GameObject coinPrefab;
    public GameObject[] obstaclePrefabs;
    public Transform[] spawnPoints;

    private void Awake()
    {
        groundSpawner = GameObject.FindAnyObjectByType<GroundSpawner>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SpawnObstacle();
        SpawnCoins();
        Debug.Log("Test"); 
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
        int randomIndex = Random.Range(0, obstaclePrefabs.Length);
        int spawnPointIndex = Random.Range(0, spawnPoints.Length);
        Instantiate(obstaclePrefabs[randomIndex], spawnPoints[spawnPointIndex].transform.position, Quaternion.identity, transform);
    }

    public void SpawnCoins()
    {
        int spawnAmount = 1;
        for (int i = 0; i < spawnAmount; i++)
        {
            GameObject tempCoin = Instantiate(coinPrefab);
            tempCoin.transform.position = spawnRandomPoint(GetComponent<Collider>());
        }
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
