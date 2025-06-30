using UnityEngine;

public class AudioCues : MonoBehaviour
{
    public AudioClip rotateClip, teleportClip;

    private AudioSource source;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Camera.main.transform.position;

        if (
            OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger) ||
            OVRInput.GetUp(OVRInput.Button.PrimaryHandTrigger) ||
            OVRInput.GetDown(OVRInput.Button.SecondaryHandTrigger) ||
            OVRInput.GetUp(OVRInput.Button.SecondaryHandTrigger)
        )
        {
            source.clip = rotateClip;
            source.Play();
        }

        if (OVRInput.GetUp(OVRInput.Button.One))
        {
            source.clip = teleportClip;
            source.Play();
        }
     }
}
