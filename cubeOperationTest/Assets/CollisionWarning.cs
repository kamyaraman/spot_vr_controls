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
        source.clip = clip; 
        source.Play();
    }

    // Update is called once per frame
    void Update()
    {                                                              
        
    }
}
