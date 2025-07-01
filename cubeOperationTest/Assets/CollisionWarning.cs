using Microsoft.Extensions.Logging.Abstractions;
using UnityEngine;

public class CollisionWarning : MonoBehaviour
{
    public AudioClip clip;
    public AudioSource source;
    public RedFlasher redFlasher; 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (redFlasher != null)
        {
            redFlasher.Flash();
        }
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit; 
        if (Physics.Raycast(transform.position, transform.forward, out hit, 0.5f))
        {

        }                                                                    
        
    }
}
