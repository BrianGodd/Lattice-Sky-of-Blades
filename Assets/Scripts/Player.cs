using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private PlayerCharacter playerCharacter;
    [SerializeField] private Animator animator;
    // [SerializeField] private CameraSpring cameraSpring;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        playerCharacter.Initialize();
        // cameraSpring.Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        var deltaTime = Time.deltaTime;
        CharacterInput input = new CharacterInput
        {
            Move = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")),
            Rotation = Camera.main.transform.rotation,
            Jump = Input.GetButtonDown("Jump"),
            JumpSustain = Input.GetButton("Jump"),
            Crouch = Input.GetKeyDown(KeyCode.C) ? CrouchInput.Toggle : CrouchInput.None,
            Attack = Input.GetButtonDown("Fire1")
        };
        playerCharacter.UpdateInput(input);
        playerCharacter.UpdateBody(deltaTime);
        animator.SetBool("attack", input.Attack);
        // cameraSpring.UpdateSpring(deltaTime, Camera.main.transform.up);
    }
}
