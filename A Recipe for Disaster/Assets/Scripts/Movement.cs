using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(CharacterController))]
    [AddComponentMenu("Control Script/Movement Input")]
    public class Movement : MonoBehaviour
    {
        public float speed = 10.0f;
        public float gravity = -9.8f;
        Animator myAnimator;
        //Rigidbody MyRigidBody;
        CharacterController _charController;
        SpriteRenderer mySpriteRenderer;

    // Use this for initialization
    void Start()
        {
             myAnimator = GetComponent<Animator>();
             //MyRigidBody = GetComponent<Rigidbody>();
             _charController = GetComponent<CharacterController>();
             mySpriteRenderer = GetComponent<SpriteRenderer>();
        }

    void Update()
    {

        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;
        if (sceneName != "BattleScene")
        {
            float deltaX = Input.GetAxis("Horizontal") * speed;
            float deltaZ = Input.GetAxis("Vertical") * speed;
            Vector3 movement = new Vector3(deltaX, 0, deltaZ);
            movement = Vector3.ClampMagnitude(movement, speed);
            //movement.y = gravity;

            movement *= Time.deltaTime;
            movement = transform.TransformDirection(movement); // what does this do?
            _charController.Move(movement);

            // This code can be refined.
            if (Input.GetKey(KeyCode.A))
            {
                if (mySpriteRenderer != null)
                {
                    mySpriteRenderer.flipX = true;
                }
            }
            else if (Input.GetKey(KeyCode.D))
            {
                if (mySpriteRenderer != null)
                {
                    mySpriteRenderer.flipX = false;
                }
            }

            if (_charController.velocity.magnitude > 0f)
            {
                myAnimator.SetBool("Movement", true);
            }
            else if (_charController.velocity.magnitude < 8f)
            {
                myAnimator.SetBool("Movement", false);
            }
            //Debug.Log(_charController.velocity.magnitude);

        }
    }
    }
