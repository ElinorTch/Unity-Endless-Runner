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
        this.transform.position = player.transform.position - offset;
    }
}
