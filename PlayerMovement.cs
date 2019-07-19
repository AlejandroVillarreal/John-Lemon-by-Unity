using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Making this variable public makes us able to
    // "tweak" in the Inspector Window in Unity 
    public float turnSpeed = 20f;

    //Animator Component reference
    Animator m_Animator;

    //Rigidbody reference variable
    Rigidbody m_Rigidbody;

    //AudioSource reference variable
    AudioSource m_AudioSource;

    // This line of code tells the computer to creat a Vector3
    // variable called m_Movement but in this case its above
    // of the method definitions, so it can be used wherever we
    // want in the PlayerMovement class
    Vector3 m_Movement;

    Quaternion m_Rotation = Quaternion.identity;

    // Start is called before the first frame update
    void Start()
    {
        m_Animator = GetComponent<Animator>();

        m_Rigidbody = GetComponent<Rigidbody>();

        m_AudioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // This line tells the computer to "Create
        // a new float variable and call it horizontal
        // set that variable equa to the result of this method
        // call"
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        m_Movement.Set(horizontal, 0f, vertical);
        // This line of code Normalizes the vector to
        // prevent that the movement of the player will be
        // faster if he moves diagonally
        m_Movement.Normalize();
        // This line of code checks if exist horizontal input movement
        bool hasHorizontalInput = !Mathf.Approximately(horizontal, 0f);
        //This line of code checks if exist vertical input movement
        bool hasVerticalInput = !Mathf.Approximately(vertical, 0f);
        //This line of code means  if hasHorizontalInput or 
        //hasVerticalInput are true then isWalking is true, 
        //and otherwise it is false.
        bool isWalking = hasHorizontalInput || hasVerticalInput;
        //Setting isWalking Animator parameter
        m_Animator.SetBool("IsWalking", isWalking);

        //This if else statement it used to play the footsteps audio if the player
        // is walking and if its not walking stops the footsteps sound.
        if (isWalking)
        {
            if (!m_AudioSource.isPlaying)
            {
                m_AudioSource.Play();
            }
        }
        else
        {
            m_AudioSource.Stop();
        }
        // This part of code creates a Vector3 variable called desiredForward.
        //It sets it to the return of a method called RotateTowards, which is
        //a static method from the Vector3 class. RotateTowards takes four 
        //parameters — the first two are Vector3s, and are the vectors that 
        //are being rotated from and towards respectively.

        //The code starts with transform.forward, and aims for the m_Movement
        //variable. transform.forward is a shortcut to access the Transform 
        //component and get its forward vector. 

        //The next two parameters are the amount of change between the starting 
        //vector and the target vector: first the change in angle (in radians) 
        //and then the change in magnitude.  This code changes the angle by 
        //turnSpeed * Time.deltaTime and the magnitude by 0.
        Vector3 desiredForward = Vector3.RotateTowards(transform.forward, m_Movement, turnSpeed * Time.deltaTime, 0f);

        //This line simply calls the LookRotation method and creates
        //a rotation looking in the direction of the given parameter.  
        m_Rotation = Quaternion.LookRotation(desiredForward);
    }

    // This method allows us to aplay root motion as we want,
    // that means movement and rotation can be applied separately
    void OnAnimatorMove()
    {
        //Reference to the Rigidbody component then call its MovePosition method
        //and passing in a single parameter its new position
        //The Animator's deltaPosition is the change in position due to root
        //motion that would have been applied to his frame.
        //taking the magnitude of that and multiplying by the movement vector 
        m_Rigidbody.MovePosition(m_Rigidbody.position + m_Movement * m_Animator.deltaPosition.magnitude);

        m_Rigidbody.MoveRotation(m_Rotation);
    }

}
