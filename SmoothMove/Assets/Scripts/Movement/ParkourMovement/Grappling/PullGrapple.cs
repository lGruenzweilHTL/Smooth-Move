using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PullGrapple : MonoBehaviour
{
    [SerializeField, Tooltip("The maximum distance to grapple a point")] private float maxDistance = 100f;
    [SerializeField, Range(0, 1)] private float pullSpeed = 0.15f; 
    [SerializeField] private LayerMask whatIsGrappleable;
    [SerializeField] private Transform gunTip, cam, player;
    
    private LineRenderer lr;
    private Vector3 grapplePoint;
    private SpringJoint joint;
    private Vector3 currentGrapplePosition;
    private bool grappleActive = false;

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
        GrapplePull();
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
            joint.maxDistance = distanceFromPoint;
            joint.minDistance = 0;

            lr.positionCount = 2;
            currentGrapplePosition = gunTip.position;

            grappleActive = true;
        }
    }


    /// <summary>
    /// Call whenever we want to stop a grapple
    /// </summary>
    void StopGrapple() {
        lr.positionCount = 0;
        Destroy(joint);
        grappleActive = false;
    }
    
    void DrawRope() {
        //If not grappling, don't draw rope
        if (!grappleActive) return;

        currentGrapplePosition = Vector3.Lerp(currentGrapplePosition, grapplePoint, Time.deltaTime * 8f);
        
        lr.SetPosition(0, gunTip.position);
        lr.SetPosition(1, currentGrapplePosition);
    }
    void GrapplePull() {
        if (!grappleActive) return;

        joint.maxDistance = Mathf.Min(joint.maxDistance, Vector3.Distance(player.position, grapplePoint) * (1 - pullSpeed));
    }

    public bool IsGrappling => grappleActive;

    public Vector3 GrapplePoint => grapplePoint;
}
