using UnityEngine;

public class GrabLocomotion : MonoBehaviour
{
    public Transform rightHand, cameraRig;

    private Vector3 grabbedPos;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.SecondaryHandTrigger))
        {
            grabbedPos = rightHand.position;
        }
        else if (OVRInput.Get(OVRInput.Button.SecondaryHandTrigger))
        {
            if (Vector3.Distance(rightHand.position, grabbedPos) > 0.1)
            {
                Vector3 displacement = grabbedPos - rightHand.position;
                cameraRig.position = new Vector3(
                    cameraRig.position.x + displacement.x,
                    cameraRig.position.y,
                    cameraRig.position.z + displacement.z
                );
            }
        }
    }
}
