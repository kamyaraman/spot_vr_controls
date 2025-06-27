using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothLocomotion : MonoBehaviour
{
    public Transform rayOrigin;
    public Transform cameraRig;

    public GameObject indicatorPrefab;
    public LayerMask groundLayer;
    public float moveDuration;
    public bool teleportMode;

    public bool parabolaMode;
    public float parabolaHeight;
    public int parabolaSegments;

    private GameObject indicator;
    private LineRenderer lineRenderer;

    private Vector3? moveStart = null;
    private Vector3? moveEnd = null;
    private float moveElapsed;

    // Start is called before the first frame update
    void Start()
    {
        indicator = Instantiate(indicatorPrefab);
        indicator.SetActive(false);

        lineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (moveStart.HasValue)
        {
            moveElapsed += Time.deltaTime;

            float smoothT = Mathf.SmoothStep(0f, 1f, Mathf.Clamp01(moveElapsed / moveDuration));
            cameraRig.position = Vector3.Lerp(moveStart.Value, moveEnd.Value, smoothT);

            if (moveElapsed >= moveDuration)
            {
                moveStart = moveEnd = null;
            }

            return;
        }

        Ray ray = new Ray(rayOrigin.position, rayOrigin.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, 100f, groundLayer))
        {
            indicator.transform.position = hit.point;

            if (OVRInput.Get(OVRInput.Button.One))
            {
                indicator.SetActive(true);

                Vector3 groundOrigin = new(ray.origin.x, 0.2f, ray.origin.z);
                Vector3 groundHit = new(hit.point.x, 0.2f, hit.point.z);

                lineRenderer.enabled = true;
                if (parabolaMode)
                {
                    lineRenderer.positionCount = parabolaSegments + 1;
                    for (int x = 0; x <= parabolaSegments; x++)
                    {
                        Vector3 groundPoint = Vector3.Lerp(groundOrigin, groundHit, (float)x / parabolaSegments);

                        float n = parabolaSegments;
                        float fx = ray.origin.y * (2 * x * x / (n * n) - 3 * x / n + 1) + parabolaHeight * (-4 * x * (x - n) / (n * n));

                        lineRenderer.SetPosition(x, new Vector3(groundPoint.x, fx, groundPoint.z));
                    }
                }
                else
                {
                    lineRenderer.positionCount = 2;
                    lineRenderer.SetPosition(0, groundOrigin);
                    lineRenderer.SetPosition(1, groundHit);
                }

                return;
            }
            else if (OVRInput.GetUp(OVRInput.Button.One))
            {
                moveStart = cameraRig.position;
                moveEnd = new(hit.point.x, cameraRig.position.y, hit.point.z);
                moveElapsed = 0f;

                if (teleportMode)
                {
                    moveElapsed = moveDuration;
                }

                lineRenderer.enabled = false;

                return;
            }
        }

        indicator.SetActive(false);

        lineRenderer.enabled = false;
    }
}