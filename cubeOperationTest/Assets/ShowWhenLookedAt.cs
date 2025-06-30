using UnityEngine;

public class ShowWhenLookedAt : MonoBehaviour
{
    //public Transform head, hand;
    public Canvas canvas;

    public int whichOne;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Vector3 toHand = (hand.position - head.position).normalized;
        //float dot = Vector3.Dot(toHand, head.forward);
        //canvas.enabled = dot > 0.9f;

        //if (whichOne == 0)
        //{
        //    canvas.enabled = OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger);
        //}
        //else
        //{
        //    canvas.enabled = OVRInput.Get(OVRInput.Button.SecondaryIndexTrigger);
        //}

        transform.LookAt(Camera.main.transform);
    }
}
