using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private CharacterController characterController;

    [SerializeField] private GameObject body;
    [SerializeField] private GameObject hands;

    [SerializeField] private GameObject projectileSpawn;
    [SerializeField] private Projectile bulletPrefab;

    [SerializeField] private float walkingSpeed;
    [SerializeField] private float runningSpeed;
    [SerializeField] private float jumpSpeed;
    [SerializeField] private float gravity;
    [SerializeField] private float lookSpeed;
    [SerializeField] private float lookXLimit;
    [SerializeField] private float momentumLerp;
    [SerializeField] private float fireFrequency;
    [Space]
    [SerializeField] private float bulletSpeed;
    [Space]
    [SerializeField] private int startingCoreHealth;

    private GameObject buildingUI;

    public bool isBuilding;
    private int coins;
    public int Coins
    {
        get
        {
            return coins;
        }
        set
        {
            coins = value;
            GameManager.I.coinsText.text = ": " + coins;
        }
    }

    private int coreHealth;
    public int CoreHealth
    {
        get
        {
            return coreHealth;
        }
        set
        {
            coreHealth = value;
            GameManager.I.coreHealthText.text = ": " + coreHealth;
        }
    }

    private float rotationX = 0;
    private Vector3 moveDirection = Vector3.zero;
    private float lastSpeedX = 0;
    private float lastSpeedY = 0;

    private double allowedFiringTime = 0;

    private void Awake()
    {

    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Coins = 0;
        CoreHealth = startingCoreHealth;
        buildingUI = GameObject.Find("BuildingUI");
        buildingUI.SetActive(false);
    }

    private void Update()
    {
        Move();
        Shoot();

        BuildMode();

        
    }

    private void Move()
    {
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        float speedX = (isRunning ? runningSpeed : walkingSpeed) * Input.GetAxis("Vertical");
        float speedY = (isRunning ? runningSpeed : walkingSpeed) * Input.GetAxis("Horizontal");
        if (speedX < lastSpeedX)
            speedX = Mathf.Lerp(speedX, lastSpeedX, momentumLerp);
        if (speedY < lastSpeedY)
            speedY = Mathf.Lerp(speedY, lastSpeedY, momentumLerp);
        lastSpeedX = speedX;
        lastSpeedY = speedY;
        float movementDirectionY = moveDirection.y;
        moveDirection = (forward * speedX) + (right * speedY);

        if (characterController.isGrounded && Input.GetButtonDown("Jump"))
            moveDirection.y = jumpSpeed;
        else
            moveDirection.y = movementDirectionY;

        if (!characterController.isGrounded)
            moveDirection.y -= gravity * Time.deltaTime;

        characterController.Move(moveDirection * Time.deltaTime);

        rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
        rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
        Camera.main.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
        transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);


        if(transform.position.y < -20)
        {
            transform.position = new Vector3(0, 25, 0);
        }
    }

    private void Shoot()
    {
        double time = Time.timeAsDouble;
        if (Input.GetMouseButtonDown(0) && time >= allowedFiringTime && !isBuilding)
        {
            allowedFiringTime = time + (1 / fireFrequency);

            Projectile bullet = Instantiate(bulletPrefab, projectileSpawn.transform.position, Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(-90 + rotationX, 0, 0)));
            bullet.rb.velocity = characterController.velocity;
            bullet.rb.AddRelativeForce(Vector3.down * bulletSpeed, ForceMode.VelocityChange);
            GameManager.I.projectile.Add(bullet);
        }
    }

    private void BuildMode()
    {
        
        if (Input.GetKeyDown(KeyCode.Tab) && isBuilding)
        {
            isBuilding = false;
            Debug.Log("Not Building!");
            buildingUI.SetActive(false);
        }
        else if (Input.GetKeyDown(KeyCode.Tab) && !isBuilding)
        {
            isBuilding = true;
            Debug.Log("Building!");
            buildingUI.SetActive(true);
        }
    }
}
