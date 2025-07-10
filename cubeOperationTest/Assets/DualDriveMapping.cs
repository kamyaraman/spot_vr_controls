using UnityEngine;

public class DualDriveMapping : MonoBehaviour, SimpleControlMapping
{
    public void Enable() => Debug.Log("Dual robot control enabled");
    public void Disable() { }
    public void Update()
    { 
        //put new control logic here
    }
    public void Start() { }
}
