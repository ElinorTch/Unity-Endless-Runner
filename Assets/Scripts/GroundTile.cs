using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
        Destroy(gameObject, 10f);
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
        float laneDistance = 3f;         // Distance entre les lanes
        float spacingZ = 2f;             // Espace entre les pièces
        float arcWidth = spawnAmount * spacingZ;

        // Choix aléatoire de la lane : 0 = gauche, 1 = centre, 2 = droite
        int laneIndex = Random.Range(0, 3);
        float xPos = (laneIndex - 1) * laneDistance;

        // Position de départ en Z (devant le joueur)
        float startZ = transform.position.z + 10f;

        for (int i = 0; i < spawnAmount; i++)
        {
            Vector3 spawnPos = new Vector3(xPos, 1f, startZ + i * spacingZ);
            Instantiate(coinPrefab, spawnPos, Quaternion.identity);
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

}
