using UnityEngine;
using UnityEngine.InputSystem;
public class moveCube : MonoBehaviour
{
    public GameObject[] cubes;
    private int currentCubeIndex = 0;
    private float switchCooldown = 0.5f;
    private float lastSwitchTime = -1f;

    public Transform leftRayOrigin;
    public Transform rightRayOrigin;
    public LayerMask teleportSurfaceMask;

    public Transform camera;

    private Vector3? leftTargetPos;
    private Vector3? rightTargetPos;
    private Vector3? cameraTargetPos;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.Three) && Time.time -lastSwitchTime > switchCooldown) {
            //currentCubeIndex = (currentCubeIndex + 1) % cubes.Length;
            //camera.position = cubes[0].transform.position;
            cameraTargetPos = cubes[0].transform.position;
            lastSwitchTime = Time.time;
        }
        if (cameraTargetPos.HasValue)
        {
            camera.position = Vector3.MoveTowards(camera.position, cameraTargetPos.Value, 6f * Time.deltaTime);
            if (Vector3.Distance(camera.position, cameraTargetPos.Value) < 0.1f)
            {
                cameraTargetPos = null;
            }
        }

        Vector2 leftRotate = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);
        Vector2 rightRotate = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);
        //GameObject current = cubes[currentCubeIndex];
        //current.transform.Translate(Vector3.forward * move.y * Time.deltaTime* 2f);
        cubes[0].transform.Rotate(Vector3.up, leftRotate.x * Time.deltaTime * 100f);
        cubes[1].transform.Rotate(Vector3.up, rightRotate.x * Time.deltaTime * 100f);

        Ray leftRay = new Ray(leftRayOrigin.position, leftRayOrigin.forward);
        if (Physics.Raycast(leftRay, out RaycastHit leftHit, 100f, teleportSurfaceMask))
        {
            //cubes[0].transform.position = leftHit.point;
            leftTargetPos = leftHit.point;
        }
        if (leftTargetPos.HasValue)
        {
            cubes[0].transform.position = Vector3.MoveTowards(cubes[0].transform.position, leftTargetPos.Value, 3f * Time.deltaTime);
        }

        Ray rightRay = new Ray(rightRayOrigin.position, rightRayOrigin.forward);
        if (Physics.Raycast(rightRay, out RaycastHit rightHit, 100f, teleportSurfaceMask))
        {
            //cubes[1].transform.position = rightHit.point;
            rightTargetPos = rightHit.point;
        }
        if (rightTargetPos.HasValue)
        {
            cubes[1].transform.position = Vector3.MoveTowards(cubes[1].transform.position, rightTargetPos.Value, 1f * Vector3.Distance(rightTargetPos.Value, cubes[1].transform.position) * Time.deltaTime);
        }
    }
}
