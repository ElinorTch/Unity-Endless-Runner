using UnityEngine;

public class CameraFollowePlayer : MonoBehaviour
{
    public GameObject player;
    public Vector3 offset;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.Find("Player");
        offset = player.transform.position - this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 targetPosition = player.transform.position - offset;
        transform.position = new Vector3(0, targetPosition.y, targetPosition.z);
    }
}
