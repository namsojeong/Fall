using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallRun : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField]
    Transform orientation;
    [Header("Detection")]
    [SerializeField] float wallDistance;
    [SerializeField] float minimumJumpHeight = 1.5f;
    [Header("Wall Running")]
    [SerializeField] private float wallRunGravity;
    [SerializeField] private float wallRunJumpForce; 

    bool wallLeft = false;
    bool wallRight = false;

    RaycastHit leftWallHit;
    RaycastHit rightWallHit;

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    bool CanWallRun()
    {
        return !Physics.Raycast(transform.position, Vector3.down, minimumJumpHeight);
    }
    void CheckWall()
    {
        wallLeft = Physics.Raycast(transform.position, -orientation.right,out leftWallHit, wallDistance);
        wallRight = Physics.Raycast(transform.position, orientation.right,out rightWallHit, wallDistance);
    }

    private void Update()
    {
        CheckWall();

        if(CanWallRun())
        {
            if(wallLeft)
            {
                StartWallRun();
                Debug.Log("left");
            }
            else if(wallRight)
            {
                StartWallRun();
                Debug.Log("right");

            }
            else
            {
                StopWallRun();
            }
        }
    }

    void StartWallRun()
    {
        rb.useGravity = false;

        rb.AddForce(Vector3.down * wallRunGravity, ForceMode.Force);

        if(Input.GetKeyDown(KeyCode.Space))
        {
            if(wallLeft)
            {
                Vector3 wallRunDir = transform.up + leftWallHit.normal;
                rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
                rb.AddForce(wallRunDir * wallRunJumpForce, ForceMode.Force);
            }
            else if (wallRight)
            {
                Vector3 wallRunDir = transform.up + rightWallHit.normal;
                rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
                rb.AddForce(wallRunDir * wallRunJumpForce, ForceMode.Force);
            }
        }
    }

    void StopWallRun()
    {
        rb.useGravity = true;
    }
}
