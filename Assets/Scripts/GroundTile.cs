using System.Collections;
using System.Collections.Generic;
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

    public LayerMask obstacleMask;

    private void Awake()
    {
        groundSpawner = GameObject.FindAnyObjectByType<GroundSpawner>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SpawnObstacle();
        SpawnCoins1();
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

    List<Vector3> GenerateCoinArc(Vector3 startPos, int coinCount, float arcWidth, float arcHeight)
    {
        List<Vector3> positions = new List<Vector3>();

        for (int i = 0; i < coinCount; i++)
        {
            float t = (float)i / (coinCount - 1); // Normalisé entre 0 et 1
            float zOffset = t * arcWidth;
            float yOffset = Mathf.Sin(t * Mathf.PI) * arcHeight; // Courbe en arc

            Vector3 pos = new Vector3(startPos.x, startPos.y + yOffset, startPos.z + zOffset);
            positions.Add(pos);
        }

        return positions;
    }

    void SpawnCoinsOverObstacle(float laneX, float zStart)
    {
        float zEnd = zStart + 6f;
        if (IsObstacleOnLane(laneX, zStart, zEnd))
        {
            Vector3 arcStart = new Vector3(laneX, 1f, zStart);
            List<Vector3> arcPositions = GenerateCoinArc(arcStart, 5, 6f, 2f);

            foreach (Vector3 pos in arcPositions)
            {
                Instantiate(coinPrefab, pos, Quaternion.identity);
            }
        }
    }

    bool IsObstacleOnLane(float x, float zStart, float zEnd)
    {
        RaycastHit hit;
        Vector3 origin = new Vector3(x, 1f, zStart);
        Vector3 direction = Vector3.forward;
        float distance = zEnd - zStart;

        return Physics.Raycast(origin, direction, out hit, distance, obstacleMask);
    }

    public void SpawnCoins1()
    {
        int spawnAmount = 5;
        float laneDistance = 3f; // Distance entre les lanes
        int laneIndex = Random.Range(0, 3); // 0 = gauche, 1 = centre, 2 = droite
        float xPos = (laneIndex - 1) * laneDistance;

        float startZ = transform.position.z + 10f; // Distance devant le joueur
        float spacingZ = 2f; // Espace entre les pièces

        for (int i = 0; i < spawnAmount; i++)
        {
            Vector3 spawnPos = new Vector3(xPos, 1f, startZ + i * spacingZ);
            GameObject tempCoin = Instantiate(coinPrefab, spawnPos, Quaternion.identity);
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


    Vector3 SpawnCoinInLane(float laneOffset, float z)
    {
        int laneIndex = Random.Range(0, 3); // 0 = gauche, 1 = centre, 2 = droite
        float x = (laneIndex - 1) * laneOffset;
        return new Vector3(x, 1f, z);
    }

}
