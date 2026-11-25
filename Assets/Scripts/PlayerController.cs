using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
// things that helped me code this:
//https://www.geeksforgeeks.org/c-sharp/state-machines-in-unity/
//https://r4.vlebooks.com/EpubReader?ean=1780990582915
//https://docs.unity3d.com/Packages/com.unity.inputsystem@1.8/manual/RespondingToActions.html
//https://stackoverflow.com/questions/69691268/rigidbody-addforce-dont-make-my-player-jump-at-the-same-height-everytime-in-u
//https://docs.unity3d.com/Packages/com.unity.inputsystem@1.11/manual/RespondingToActions.html
//https://discussions.unity.com/t/wasd-with-inputsystem/943982/6
//https://www.reddit.com/r/Unity3D/comments/mdnv61/stop_every_object_except_player/?captcha=1
public class PlayerController : MonoBehaviour
{
    public InputAction moveA;
    public InputAction jumpA;
    public InputAction sprintA;
    public InputAction pauseA;
    private Rigidbody rb;
    public float jumping;
    public bool jumpCheck;
    public float speed;
    public float tempSpeed;
    public Vector2 moveValue;
    public bool pauseState;
    public bool hangChoice = false;
    public bool inWall = false;
    enum State
    {
        STANDING,
        MOVING, 
        JUMPING,
        HANGING
    };
    State currentState;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        pauseState = false;
        rb = GetComponent<Rigidbody>();
        moveA = InputSystem.actions.FindAction("Move");
        jumpA = InputSystem.actions.FindAction("Jump");
        sprintA = InputSystem.actions.FindAction("Sprint");
        pauseA = InputSystem.actions.FindAction("Pause");
        currentState = State.STANDING;
        tempSpeed = speed;

    }
        
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.name == "floor")
        {
            Debug.Log("floor");
            jumpCheck = true;
        }
        if (other.gameObject.CompareTag("Wall"))
        {

        }
    }
   

    void OnCollisionExit(Collision other)
    {
        if (other.gameObject.name == "floor")
        {
            Debug.Log("no floor");
            jumpCheck = false;
        }
        if (other.gameObject.CompareTag("Wall"))
        {
            inWall = false;
        }
    }
    void Standing()
    {
        moveValue = moveA.ReadValue<Vector2>();
        if (moveValue != new Vector2 (0f,0f))
        {
            currentState = State.MOVING;
        }
        //idle anim here
        if (jumpA.WasPressedThisFrame() && jumpCheck == true)
        {
            Debug.Log("Ima jump");
            currentState = State.JUMPING;
        }
        if (pauseA.WasPressedThisFrame())
        {
            GameObject.Find("Sphere (1)").GetComponent<SphereTest>().TriggerPause();
        } 
    }
    void Moving()
    {
        //https://discussions.unity.com/t/wasd-with-inputsystem/943982/6
        if(sprintA.IsPressed())
        {
            tempSpeed = speed;
            speed += 3.0f;
            Debug.Log("SPRINT");
        }
        Vector2 tempVect = moveValue * (Time.unscaledDeltaTime * speed);
        if (!inWall)
        {
            rb.AddForce(new Vector3(tempVect.x * 1.5f, 0.0f, tempVect.y * 1.5f), ForceMode.Impulse);
        }
        speed = tempSpeed;
        moveValue = moveA.ReadValue<Vector2>();
        if (jumpA.WasPressedThisFrame() && jumpCheck == true)
        {
            currentState = State.JUMPING;
        }
        else if (moveValue == new Vector2(0.0f, 0.0f))
        {
            currentState = State.STANDING;
        }
        if (pauseA.WasPressedThisFrame())
        {
            if (pauseState == false)
            {
                GameObject.Find("Sphere (1)").GetComponent<SphereTest>().TriggerPause();
            }
        }
    }
    void Jumping()
    {
        Debug.Log("Weeeee!");
        rb.AddForce(Vector3.up * jumping, ForceMode.Impulse);
        if (pauseA.WasPressedThisFrame())
        {
            GameObject.Find("Sphere (1)").GetComponent<SphereTest>().TriggerPause();
        }
        jumpCheck = false;
        currentState = State.STANDING;

    }

    void Hanging()
    {

        if (jumpA.WasPressedThisFrame())
        {
            hangChoice = false;
            jumpCheck = true;
            currentState = State.JUMPING;

        }


    }
    // Update is called once per frame
    void Update()
    {

        if (hangChoice == true)
        {
            currentState = State.HANGING;
        }
        switch (currentState)
        {
            case State.STANDING:
                Standing();
                break;
            case State.JUMPING:
                Jumping();
                break;

            case State.MOVING:
                Moving();
                break;
            case State.HANGING:
                Hanging();
                break;
                
        }
    }
}
