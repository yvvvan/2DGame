using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

//[RequireComponent(typeof(InputMaster))]
public class Knight : MonoBehaviour
{   
    // character status
    [Header("Character Status")]
    public int attackDelay = 20; // in frames
    public float moveSpeed = 1.0f;
    public float slashSpeed = 2.0f;
    public int health = 100;

    [Header("Setting")]
    public float Y_OFFSET = 0.22f; // center.y - pilvot.y

    [Header("References")]
    public Animator animator;
    public InputController controls;
    public GameObject crossHair;
    public GameObject slashPrefab;
    public Rigidbody2D rb;

    // INPUT
    // movement input
    Vector2 move = Vector2.zero;
    // aiming input
    Vector2 moveCrossHair = Vector2.zero;
    Vector3 mousePosition = Vector3.zero;
    // attack input
    bool isAimed = false;
    bool isFired = false;
    int holdFire = -1;

    // GAME START ------------------------------------------------
    // Awake is called before Start()
    void Awake(){

        Screen.SetResolution(1920, 1080, true);
        controls = new InputController();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {   
        // move
        animator.SetFloat("MoveHorizontal", move.x);
        animator.SetFloat("MoveVertical", move.y);
        animator.SetFloat("MoveMagnitude", move.magnitude);
        rb.velocity=new Vector2(move.x, move.y)*moveSpeed;

        // aim 
        if (moveCrossHair.magnitude > 0.0f){
            // if is aiming
            isAimed = true;
            crossHair.SetActive(true);
            controls.Player.Fire.Enable();
            moveCrossHair.Normalize();
            crossHair.transform.localPosition = moveCrossHair;
        } else {
            // if not aiming
            isAimed = false;
            crossHair.SetActive(false);
            controls.Player.Fire.Disable();  
        }
        
        // attack
        animator.SetFloat("AttackHorizontal", moveCrossHair.x);
        animator.SetFloat("AttackVertical", moveCrossHair.y);
        animator.SetFloat("AttackMagnitude", moveCrossHair.magnitude);  
        animator.SetBool("isAttacked", isFired);  
        if (isFired && isAimed) {
            if (holdFire < 0 || holdFire>=attackDelay){
                holdFire = 0;
                Vector2 slashDirection = new Vector2(moveCrossHair.x, moveCrossHair.y);    
                slashDirection.Normalize();
                Vector3 centerPosition = transform.position;
                centerPosition.y += Y_OFFSET;
                GameObject slash = Instantiate(slashPrefab, centerPosition, Quaternion.identity);
                Slash slashScript = slash.GetComponent<Slash>();
                slashScript.velocity = slashDirection*slashSpeed;
                slashScript.knight = gameObject;
                slashScript.degree = Mathf.Atan2(slashDirection.y, slashDirection.x)*Mathf.Rad2Deg;
                slash.transform.Rotate(0.0f, 0.0f, slashScript.degree);
                Destroy(slash, 2.0f);
            }
        } 
        if (holdFire >= 0) {
            holdFire += 1;
        }
    }
    // GAME END --------------------------------------------------

    // INPUT START -----------------------------------------------
    public void OnMove(CallbackContext context){
        move = context.ReadValue<Vector2>();
    }
    

    public void OnAim(CallbackContext context){
        moveCrossHair = context.ReadValue<Vector2>();
    }

    public void OnFire(CallbackContext context){
        isFired = context.action.triggered;
    }

    public void OnMousePosition(CallbackContext context){
        mousePosition = context.ReadValue<Vector2>();
        mousePosition.z = 20;
        mousePosition =  Camera.main.ScreenToWorldPoint(mousePosition) - transform.position;
        moveCrossHair.x = mousePosition.x;
        moveCrossHair.y = mousePosition.y - Y_OFFSET;
    }

    // must do
    private void OnEnable(){
        controls.Enable();
    }

    private void OnDisable(){
        controls.Disable();
    }
    // INPUT END -------------------------------------------------
}
