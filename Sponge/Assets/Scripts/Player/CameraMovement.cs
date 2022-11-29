using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;
    Vector3 velocity;

    [Space(15)]
    public float snapTime = 0.5f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 localOffset = target.rotation * offset;

        transform.position = Vector3.SmoothDamp(transform.position, target.position + localOffset, ref velocity, snapTime);
        transform.LookAt(target.position);
    }

    private void OnDrawGizmos()
    {
        Matrix4x4 matrix = new Matrix4x4();

        matrix.SetTRS(target.position + offset, Quaternion.LookRotation(-offset, Vector3.up), Vector3.one);
        Gizmos.matrix = matrix;

        Gizmos.DrawSphere(Vector3.zero, 0.5f);
        Gizmos.DrawFrustum(Vector3.zero, 60, 1000, 0.5f, 16f / 9f);
    }
}
