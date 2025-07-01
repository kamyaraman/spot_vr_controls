using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RedFlasher : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Image redFlash;
    public float flashDuration = 0.5f;

    public void Flash()
    {
        StopAllCoroutines();
        StartCoroutine(FlashRoutine()); 
    }

    private IEnumerator FlashRoutine()
    {
        redFlash.color = new Color(1f, 0f, 0f, 0.3f);
        yield return new WaitForSeconds(flashDuration);
        redFlash.color = new Color(1f, 0f, 0f, 0f); 

    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
