using UnityEngine;

public class GroundTile : MonoBehaviour
{
    private GroundSpawner groundSpawner;

    public GameObject[] obstaclePrefabs;
    public Transform[] spawnPoints;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       
    }

    private void OnTriggerExit(Collider other)
    {

        Debug.Log("Tile Destroyed");
        groundSpawner.SpawnTile();
        Destroy(gameObject, 5f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
