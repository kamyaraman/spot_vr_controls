using UnityEngine;

public class RotateFlash : MonoBehaviour
{
    public Transform cameraTransform; 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(cameraTransform.position);
    }
}
