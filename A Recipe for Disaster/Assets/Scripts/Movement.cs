using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(CharacterController))]
    [AddComponentMenu("Control Script/Movement Input")]
public class Movement : MonoBehaviour {

    public float speed = 10.0f;
    public float gravity = -9.8f;
    public float movementThreshold = 5.0f;
        
    Animator myAnimator;
    CharacterController _charController;

    // Use this for initialization
    void Start()
        {
        _charController = GetComponent<CharacterController>();
        myAnimator = GetComponent<Animator>();
        myAnimator.SetInteger("facing", 3);     
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

            movement *= Time.deltaTime;
            movement = transform.TransformDirection(movement); // what does this do?
            _charController.Move(movement);

            // This code may need to be refined.
            if (Input.GetAxis("Horizontal") > 0)
            {
                myAnimator.SetInteger("facing", 0);
            }
            else if (Input.GetAxis("Vertical") > 0)
            {
                myAnimator.SetInteger("facing", 1);
            }
            else if (Input.GetAxis("Horizontal") < 0)
            {
                myAnimator.SetInteger("facing", 2);
            }
            else if (Input.GetAxis("Vertical") < 0)
            {
                myAnimator.SetInteger("facing", 3);    
            }

            // NO MOON-WALKING!
            if (_charController.velocity.magnitude > movementThreshold - 0.01)
            {
               myAnimator.SetBool("walking", true);
            }
            else if (_charController.velocity.magnitude < movementThreshold)
            {
               myAnimator.SetBool("walking", false);
            }
            //Debug.Log(_charController.velocity.magnitude 
            //    + ", walking is " + myAnimator.GetBool("walking")
            //    + ", facing is " + myAnimator.GetInteger("facing"));

        } else {
            myAnimator.SetBool("walking", false);
            myAnimator.SetInteger("facing", 3);
        }
    }
}
