using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Collectable : MonoBehaviour
{
    public Transform modelTransform;
    public float rotationSpeed = 90;

    // Start is called before the first frame update
    void Start()
    {
        modelTransform.Rotate(Vector3.up, Random.Range(0, 361), Space.World);
    }

    // Update is called once per frame
    void Update()
    {
        modelTransform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            UIManager.Instance.AddScore();
            Destroy(gameObject);
        }
    }
}
