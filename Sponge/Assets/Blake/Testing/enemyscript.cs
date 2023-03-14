using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class enemyscript : MonoBehaviour
{
    CharacterController _enemyAI;
    public GameObject _playerScript;
    public float _speed;
    public float _gravityMultiplier;
    public float _enemiesYVelocity;
    public float _something;

    // Start is called before the first frame update
    void Start()
    {
        _enemyAI = GetComponent<CharacterController>();

        if (_enemyAI == null)
            Debug.LogError("Enemy Script is Null");

        if (_playerScript == null)
            Debug.LogError("Player Script is Null");

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = _playerScript.transform.position - transform.position;
        if (direction.magnitude > _something)
            return;
        Vector3 velocity = direction * _speed;
        float _gravity = _gravityMultiplier * Time.deltaTime;


        if (_enemyAI.isGrounded)
        {
            _enemiesYVelocity = - _gravityMultiplier;
        }
        else
        {
            _enemiesYVelocity -= _gravity;
        }
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(velocity), _speed * Time.deltaTime);
        velocity.y = _enemiesYVelocity;

        //velocity.Normalize();

        

        _enemyAI.Move(velocity * Time.deltaTime);
    }
}
