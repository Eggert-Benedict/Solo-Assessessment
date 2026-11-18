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
    private Rigidbody rb;
    public float jumping = 5.0f;
    public bool jumpCheck;
    public float speed;
    public float tempSpeed;
    public Vector2 moveValue;

    enum State
    {
        STANDING,
        MOVING, 
        JUMPING
    };
    State currentState;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        moveA = InputSystem.actions.FindAction("Move");
        jumpA = InputSystem.actions.FindAction("Jump");
        sprintA = InputSystem.actions.FindAction("Sprint");
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
    }
   

    void OnCollisionExit(Collision other)
    {
        if (other.gameObject.name == "floor")
        {
            Debug.Log("no floor");
            jumpCheck = false;
        }
    }
    void Standing()
    {
        moveValue = moveA.ReadValue<Vector2>();
        Debug.Log(moveValue);
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
        Vector2 tempVect = moveValue * (Time.deltaTime * speed);
        transform.Translate(tempVect.x, 0, tempVect.y);
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
    }
    void Jumping()
    {
        Debug.Log("Weeeee!");
        rb.AddForce(Vector3.up * jumping, ForceMode.Impulse);
        currentState = State.STANDING;
        
    }
    // Update is called once per frame
    void Update()
    {
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
                
        }
    }
}
