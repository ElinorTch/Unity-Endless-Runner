using UnityEngine;

public class Coin : MonoBehaviour
{
    public float turnSpeed = 90f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.Rotate(0, 0, turnSpeed * Time.deltaTime);
    }
}
