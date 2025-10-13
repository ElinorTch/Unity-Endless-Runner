using UnityEngine;

public class GroundTile : MonoBehaviour
{
    private GroundSpawner groundSpawner;

    public GameObject coinPrefab;
    public GameObject[] obstaclePrefabs;
    public Transform UpDownSpawnPoint;
    public Transform[] coneSpawnPoints;

    private void Awake()
    {
        groundSpawner = GameObject.FindAnyObjectByType<GroundSpawner>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SpawnObstacle();
        SpawnCoins();
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

        //if (randomIndex == 0)
        //{
            Instantiate(obstaclePrefabs[0], coneSpawnPoints[spawnPointIndex].transform.position, Quaternion.identity);
            
        //}
        //else
        //{
            Instantiate(obstaclePrefabs[randomIndex], UpDownSpawnPoint.position, Quaternion.identity);

            //if (randomIndex == 1)
            //{
            //    Instantiate(obstaclePrefabs[2], UpDownSpawnPoint.position, Quaternion.identity);
            //}
            //else
            //{
            //    Instantiate(obstaclePrefabs[1], UpDownSpawnPoint.position, Quaternion.identity);
            //}
            return;
        //}
    }

    public void SpawnCoins()
    {
        int spawnAmount = 5;
        for (int i = 0; i < spawnAmount; i++)
        {
            Debug.Log("Coin");
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
