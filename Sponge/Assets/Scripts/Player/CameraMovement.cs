using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMovement : MonoBehaviour
{
    public InputActionAsset input;
    InputAction look;

    [Space(15)]
    public Transform target;
    public Vector3 offset;
    Vector3 velocity;
    Vector3 rotation;

    [Space(15)]
    public LayerMask cameraCollision;

    public Quaternion moveRotation
    {
        get
        {
            return Quaternion.Euler(new Vector3(0, rotation.y));
        }
    }

    [Space(15)]
    public float snapTime = 0.5f;
    public float lookSpeed = 20f;

    // Start is called before the first frame update
    void Start()
    {
        look = input.FindAction("Look");
        look.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        //lookSpeed= 20; james removed to test. 19 is perfect for mouse. Need to sort a seperate one for controller. 225 is good for controller
        rotation += new Vector3(0, look.ReadValue<Vector2>().x * lookSpeed * Time.deltaTime, 0);

        Vector3 localOffset = Quaternion.Euler(rotation) * offset;

        RaycastHit hit;
        Ray ray = new Ray(target.position, localOffset);
        if (Physics.Raycast(ray, out hit, localOffset.magnitude, cameraCollision))
            transform.position = hit.point;
        else
            transform.position = target.position + localOffset;
        //transform.position = Vector3.SmoothDamp(transform.position, target.position + localOffset, ref velocity, snapTime);
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
