using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
// These videos take long to make so I hope this helps you out and if you want to help me out you can by leaving a like and subscribe, thanks!
 
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] Transform playerCamera;
    [SerializeField][Range(0.0f, 0.5f)] float mouseSmoothTime = 0.03f;
    [SerializeField] bool cursorLock = true;
    [SerializeField] float mouseSensitivity = 3.5f;
    [SerializeField] float Speed = 6.0f;
    [SerializeField][Range(0.0f, 0.5f)] float moveSmoothTime = 0.3f;
    [SerializeField] float gravity = -60f;
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask ground;  
    [SerializeField] float dashDistance = 10.0f; // Distância do dash
    [SerializeField] float dashDuration = 0.2f; // Duração do dash
        int dashCharges = 2;
    bool isDashing = false; // Verifica se está ocorrendo um dash
    Vector3 dashDirection; // Direção do dash
    float dashStartTime; // Tempo de início do dash

 
    public float jumpHeight = 6f;
    float velocityY;
    bool isGrounded;
 
    float cameraCap;
    Vector2 currentMouseDelta;
    Vector2 currentMouseDeltaVelocity;
    
    CharacterController controller;
    Vector2 currentDir;
    Vector2 currentDirVelocity;
    Vector3 velocity;
 
    void Start()
    {
        controller = GetComponent<CharacterController>();
 
        if (cursorLock)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = true;
        }
    }
 
    void Update()
    {
        UpdateMouse();
        UpdateMove();
        UpdateDash();
   
    }
 
    void UpdateMouse()
    {
        Vector2 targetMouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
 
        currentMouseDelta = Vector2.SmoothDamp(currentMouseDelta, targetMouseDelta, ref currentMouseDeltaVelocity, mouseSmoothTime);
 
        cameraCap -= currentMouseDelta.y * mouseSensitivity;
 
        cameraCap = Mathf.Clamp(cameraCap, -90.0f, 90.0f);
 
        playerCamera.localEulerAngles = Vector3.right * cameraCap;
 
        transform.Rotate(Vector3.up * currentMouseDelta.x * mouseSensitivity);

        // Armazena a direção para a qual o jogador está olhando
        Vector3 playerDirection = transform.forward;
    }

     void UpdateDash()
    {
        if (dashCharges > 0 && Input.GetKeyDown(KeyCode.E) && !isDashing)
        {
            dashDirection = transform.forward;

            if (Input.GetKey(KeyCode.A))
            {
                dashDirection = -transform.right;
            }
            else if (Input.GetKey(KeyCode.D))
            {
                dashDirection = transform.right;
            }
            else if (Input.GetKey(KeyCode.W))
            {
                dashDirection = transform.forward;
            }
            else if (Input.GetKey(KeyCode.S))
            {
                dashDirection = -transform.forward;
            }

            dashDirection.Normalize();
            isDashing = true;
            dashStartTime = Time.time;
            dashCharges--;

            StartCoroutine(PerformDash());
        }

        if (isDashing)
        {
            float dashElapsed = Time.time - dashStartTime;
            if (dashElapsed >= dashDuration)
            {
                isDashing = false;
            }
            else
            {
                controller.Move(dashDirection * dashDistance / dashDuration * Time.deltaTime);
            }
        }
    }

    IEnumerator PerformDash()
    {
        yield return new WaitForSeconds(dashDuration);
        isDashing = false;
    }
    
 
    void UpdateMove()
    {
        isGrounded = controller.isGrounded;

        if (isGrounded && velocityY < 0)
        {
            velocityY = -1f;
        }

        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            velocityY = Mathf.Sqrt(jumpHeight * -2f * Physics.gravity.y);
        }

        velocityY += Physics.gravity.y * Time.deltaTime;
        Vector3 move = transform.right * Input.GetAxis("Horizontal") + transform.forward * Input.GetAxis("Vertical");
        controller.Move(move * Time.deltaTime * 6f + Vector3.up * velocityY * Time.deltaTime);
    }
}