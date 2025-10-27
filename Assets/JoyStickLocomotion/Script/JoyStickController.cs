using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityStandardAssets.Utility;
using UnityStandardAssets.Characters.FirstPerson;


public class JoyStickController : MonoBehaviour
{
    public Animator CineCameraAnim;
    public ViewTurnController TurnController;
    public JoyStickArea JoyStick;
    public Transform CameraV, fakePlayer;
    public GameObject body;
    public VariableJoystick variableJoystick;
    public float Speed = 1.2f, rollSpeed = 2f, turnSpeed = 0.5f, cameraTurnSpeed = 2f, standTurnSpeed = 3f;
    public float lastVertical = 0;
    
    public float groundCheckDistance = 0.1f;
    public float gravity = -9.81f;
    public bool isGrounded = false;
    private Vector3 velocity;

    public float Vertical, Horizontal;
    public MouseLook m_MouseLook;
    public GameManager GMaster;

    public bool isJoystick = false;

    // Start is called before the first frame update
    void Start()
    {
        variableJoystick = GameObject.Find("Joystick").GetComponent<JoyStickArea>().VariableJoystick;
        if(!GMaster.isFirst) m_MouseLook.Init(fakePlayer , CameraV);
        else m_MouseLook.Init(transform , CameraV);
    }

    // Update is called once per frame
    void Update()
    {
        Vertical = variableJoystick.Vertical;
        Horizontal = variableJoystick.Horizontal;

        if(!isJoystick)
        {
            RotateView();
            if(Input.GetKey(KeyCode.W))
            {
                JoyStickArea.isMove = true;
                Vertical = 1;
            }
            else if(Input.GetKey(KeyCode.S))
            {
                JoyStickArea.isMove = true;
                Vertical = -1;
            }
            else
            {
                Vertical = 0;
            }
            if(Input.GetKey(KeyCode.A))
            {
                JoyStickArea.isMove = true;
                Horizontal = -1;
            }
            else if(Input.GetKey(KeyCode.D))
            {
                JoyStickArea.isMove = true;
                Horizontal = 1;
            }
            else
            {
                Horizontal = 0;
            }
            if(!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
            {
                JoyStickArea.isMove = false;
            }
        }

        //if(JoyStickArea.isMove && !CineCameraAnim.GetBool("fov_up")) CineCameraAnim.SetBool("fov_up", true);
        //else if(!JoyStickArea.isMove && CineCameraAnim.GetBool("fov_up")) CineCameraAnim.SetBool("fov_up", false);
        
        Vector3 direction;
        direction = CameraV.forward * Vertical + CameraV.right * Horizontal;
        direction.y = 0;
        
        /*if(!(ViewTurnController.isTurn && JoyStickArea.isMove))
        {
            if(ViewTurnController.isTurn && !JoyStickArea.isMove)
            {
                CameraV.rotation =
                Quaternion.Slerp(CameraV.rotation, transform.rotation, Time.deltaTime * standTurnSpeed);
                CameraV.rotation = Quaternion.Euler(new Vector3(19.19f, CameraV.eulerAngles.y, 0));
            }
            else
            {
                CameraV.rotation =
                Quaternion.Slerp(CameraV.rotation, transform.rotation, Time.deltaTime * cameraTurnSpeed);
                CameraV.rotation = Quaternion.Euler(new Vector3(19.19f, CameraV.eulerAngles.y, 0));
            }
        }*/
        /*if(!ViewTurnController.isTurn && !JoyStickArea.isRoll)
        {
               CameraV.rotation =
                Quaternion.Slerp(CameraV.rotation, transform.rotation, Time.deltaTime * cameraTurnSpeed);
                CameraV.rotation = Quaternion.Euler(new Vector3(19.19f, CameraV.eulerAngles.y, 0)); 
        }*/

        if(!JoyStickArea.isMove)
        {
            body.transform.GetComponent<Animator>().SetBool("run", false);
            return;
        }
        else if(!JoyStickArea.isRoll)
        {
            body.transform.GetComponent<Animator>().SetBool("run", true);
        }
        
        var trackingPosition = transform.position + 10* direction;
        if (Vector3.Distance(trackingPosition, transform.position) < 0.1)
        {
            return;
        }
        if(ViewTurnController.isTurn && !JoyStickArea.isMove) return;

        Quaternion lookRotation;
        //if(Vertical < 0 && (Horizontal < 0.3f && Horizontal > -0.3f))
        //    lookRotation = Quaternion.LookRotation(transform.position - trackingPosition);
        //else
            lookRotation = Quaternion.LookRotation(trackingPosition - transform.position);

        if(!JoyStickArea.isRoll)
        {
        transform.rotation =
            Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * turnSpeed);
        }

        if(!AttackController.isAttack)
        {
            GetComponent<CharacterController>().Move(direction* Speed * Time.deltaTime);
        //transform.position =
        //    Vector3.MoveTowards(transform.position, trackingPosition, Speed * Time.deltaTime);
        }

        ApplyGravity();
    }

    private void RotateView()
    {
        if(GMaster.isFirst) m_MouseLook.LookRotation (transform, CameraV);
        else m_MouseLook.LookRotation (fakePlayer, CameraV);
    }

    void ApplyGravity()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, groundCheckDistance);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = 0f;
        }
        else
        {
            velocity.y += gravity * Time.deltaTime;
        }

        GetComponent<CharacterController>().Move(velocity * Time.deltaTime);
    }

    public void RollOnce()
    {
        AttackController.isAttack = false;
        PlayerEventController.isAbleSlash = true;
        body.transform.GetComponent<Animator>().SetBool("slashleft", false);
        body.transform.GetComponent<Animator>().SetBool("slashright", false);
        StartCoroutine(MoveForwardForDuration(0.8f));
    }

    IEnumerator MoveForwardForDuration(float duration)
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            GetComponent<CharacterController>().Move(transform.forward * rollSpeed * Time.deltaTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}
