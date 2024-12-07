using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class GrapplingGun : MonoBehaviour {
    [SerializeField, Tooltip("The maximum distance to grapple a point")] private float maxDistance = 100f;
    [SerializeField, Tooltip("The distance the grapple will try to keep from the target")] private float jointMaxMultiplier = 0.8f;
    [SerializeField, Tooltip("The distance the grapple will try to keep from the target")] private float jointMinMultiplier = 0.25f;
    [SerializeField, Tooltip("The spring value of the joint")] private float jointSpring = 4.5f;
    [SerializeField, Tooltip("The damping value of the joint")] private float jointDamper = 7f;
    [SerializeField, Tooltip("The massScale value of the joint")] private float jointMassScale = 4.5f;
    [SerializeField] private LayerMask whatIsGrappleable;
    [SerializeField] private Transform gunTip, cam, player;
    
    private LineRenderer lr;
    private Vector3 grapplePoint;
    private SpringJoint joint;
    private Vector3 currentGrapplePosition;

    void Awake() {
        lr = GetComponent<LineRenderer>();
    }

    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            StartGrapple();
        }
        else if (Input.GetMouseButtonUp(0)) {
            StopGrapple();
        }
    }

    //Called after Update
    void LateUpdate() {
        DrawRope();
    }

    /// <summary>
    /// Call whenever we want to start a grapple
    /// </summary>
    void StartGrapple() {
        RaycastHit hit;
        if (Physics.Raycast(cam.position, cam.forward, out hit, maxDistance, whatIsGrappleable)) {
            grapplePoint = hit.point;
            joint = player.gameObject.AddComponent<SpringJoint>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = grapplePoint;

            float distanceFromPoint = Vector3.Distance(player.position, grapplePoint);

            //The distance grapple will try to keep from grapple point. 
            joint.maxDistance = distanceFromPoint * jointMaxMultiplier;
            joint.minDistance = distanceFromPoint * jointMinMultiplier;

            //Adjust these values to fit your game.
            joint.spring = jointSpring;
            joint.damper = jointDamper;
            joint.massScale = jointMassScale;

            lr.positionCount = 2;
            currentGrapplePosition = gunTip.position;
        }
    }


    /// <summary>
    /// Call whenever we want to stop a grapple
    /// </summary>
    void StopGrapple() {
        lr.positionCount = 0;
        Destroy(joint);
    }
    
    void DrawRope() {
        //If not grappling, don't draw rope
        if (!joint) return;

        currentGrapplePosition = Vector3.Lerp(currentGrapplePosition, grapplePoint, Time.deltaTime * 8f);
        
        lr.SetPosition(0, gunTip.position);
        lr.SetPosition(1, currentGrapplePosition);
    }

    public bool IsGrappling() {
        return joint != null;
    }

    public Vector3 GetGrapplePoint() {
        return grapplePoint;
    }
}