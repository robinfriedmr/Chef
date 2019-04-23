using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    [RequireComponent(typeof(CharacterController))]
    [AddComponentMenu("Control Script/Movement Input")]
    public class movement : MonoBehaviour
    {
        public float speed = 10.0f;
        public float gravity = -9.8f;
        public Rigidbody MyRigidBody;
        public Animator myAnimator;
        public SpriteRenderer mySpriteRenderer;
        private CharacterController _charController;

        // Use this for initialization
        void Start()
        {
             myAnimator = GetComponent<Animator>();
             MyRigidBody = GetComponent<Rigidbody>();
             _charController = GetComponent<CharacterController>();
             mySpriteRenderer = GetComponent<SpriteRenderer>();
        }

        void Update()
        {

            float deltaX = Input.GetAxis("Horizontal") * speed;
            float deltaZ = Input.GetAxis("Vertical") * speed;
            Vector3 movement = new Vector3(deltaX, 0, deltaZ);
            movement = Vector3.ClampMagnitude(movement, speed);
            

        movement.y = gravity;

            movement *= Time.deltaTime;
            movement = transform.TransformDirection(movement);
            _charController.Move(movement);

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S))
        {

            if (mySpriteRenderer != null)
            {
                mySpriteRenderer.flipX = true;
                myAnimator.SetBool("Movement", true);
            }
        }

        else if (Input.GetKey(KeyCode.D))
        {
            if (mySpriteRenderer != null)
            {
                mySpriteRenderer.flipX = false;
                myAnimator.SetBool("Movement", true);
            }
        }

        else if (_charController.velocity.magnitude <= 0.1)
        {
            myAnimator.SetBool("Movement", false);
        }
 

        }

    }
