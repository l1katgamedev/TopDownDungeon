using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Character attributes:")]
    public float MOVEMENT_BASE_SPEED = 1.0f;
    public float CROSSHAIR_DISTANCE = 1.0f;
    public float AIMING_BASE_PENALTY = 0.5f;
    public float BULLET_BASE_SPEED = 1.0f;
    public int MAX_HEALTH_POINT;
    public int currentHealth;

    [Space]
    [Header("Character statistics:")]
    public Vector2 movementDirection;
    public Vector3 mouseDirection;
    private Vector3 shootingDirection;
    private float rotationZ;
    public float movementSpeed;
    public bool endOfAiming;
    public bool isAiming;
    public bool isAlive;

    [Space]
    [Header("References:")]
    public Rigidbody2D rigidBody;
    public Animator characterAnimator;
    public Animator weaponAnimator;
    public GameObject crosshair;
    public Transform firePoint;
    public Camera cam;
    public GameObject weapon;
    public HealthBar healthBar;

    [Space]
    [Header("Prefabs:")]
    public GameObject bulletPrefab;

    [Space]
    [Header("Weapon")]
    public int maxAmmo = 5;
    public int currentAmmo;
    public float reloadTime;
    public bool isReloading = false;


    private void Awake()
    {
        // WEAPON
        currentAmmo = maxAmmo;

        // HEALTH
        currentHealth = MAX_HEALTH_POINT;
        healthBar.SetMaxHealth(MAX_HEALTH_POINT);

        // Spawn Character
        GameObject.Find("GameManager").GetComponent<GameManager>().SpawnCharacter(gameObject);

        // Cursor
/*      Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;*/
    }

    private void OnDeath()
    {
        characterAnimator.SetBool("isDead", true);
        weapon.SetActive(false);
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        gameObject.GetComponent<CircleCollider2D>().enabled = false;
        StartCoroutine(Respawn());
    }

    public void HurtCharacter(int damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
    }


    public void Heal(int healingAmount)
    {
        if (currentHealth == MAX_HEALTH_POINT)
            return;

        currentHealth += healingAmount;
        if (currentHealth > MAX_HEALTH_POINT)
            currentHealth = MAX_HEALTH_POINT;

        GameManager.instance.ShowText("+" + healingAmount.ToString() + "HP", 40, Color.green, transform.position, Vector3.up * 30, 1.0f);
    }

    IEnumerator Respawn()
    {
        yield return new WaitForSeconds(5.0f);
        Heal(MAX_HEALTH_POINT);
        healthBar.SetHealth(currentHealth);
        characterAnimator.SetBool("isDead", false);
        isAlive = true;
        gameObject.GetComponent<BoxCollider2D>().enabled = true;
        gameObject.GetComponent<CircleCollider2D>().enabled = true;
        weapon.SetActive(true);
    }

    private void Update()
    {

        if(currentAmmo <= 0)
        {
            StartCoroutine(Reload());
        }


        if (isAlive == true)
        {
            ProcessInputs();
            Move();
            AnimateSprite();
        }

        Aim();



        if (currentHealth <= 0)
        {
            isAlive = false;
            OnDeath();
        }

        if (currentHealth > 0)
        {
            isAlive = true;
        }
    }

    void ProcessInputs()
    {
        movementDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        movementSpeed = Mathf.Clamp(movementDirection.magnitude, 0.0f, 1.0f);
        movementDirection.Normalize();

        if (Input.GetMouseButtonDown(0))
        {
            endOfAiming = true;
            float distance = shootingDirection.magnitude;
            Vector2 direction = shootingDirection / distance;
            direction.Normalize();
            Shoot(direction, rotationZ);
        }

        isAiming = Input.GetButton("Fire1");

        if(isAiming)
        {
            movementSpeed *= AIMING_BASE_PENALTY;
        }
    }

    void Move()
    {
        rigidBody.velocity = movementDirection * movementSpeed * MOVEMENT_BASE_SPEED;
    }

    void AnimateSprite()
    {
        characterAnimator.SetFloat("Horizontal", mouseDirection.x);
        characterAnimator.SetFloat("Vertical", mouseDirection.y);
        characterAnimator.SetFloat("Speed", movementSpeed);

        weaponAnimator.SetFloat("Horizontal", mouseDirection.x);
        weaponAnimator.SetFloat("Vertical", mouseDirection.y);
        weaponAnimator.SetFloat("Speed", movementSpeed);
    }

    void Aim()
    {
        mouseDirection = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, transform.position.z));

        crosshair.transform.position = new Vector2(mouseDirection.x, mouseDirection.y);

        shootingDirection = mouseDirection - gameObject.transform.position;
        rotationZ = Mathf.Atan2(shootingDirection.y, shootingDirection.x) * Mathf.Rad2Deg;
    }

    void Shoot(Vector2 direction, float rotationZ)
    {
        if (isReloading == false)
        {
            currentAmmo--;
            GameObject bullet = Instantiate(bulletPrefab) as GameObject;
            Bullet bulletScript = bullet.GetComponent<Bullet>();
            bullet.transform.position = weapon.transform.position;
            bullet.transform.rotation = Quaternion.Euler(0.0f, 0.0f, rotationZ);
            bullet.GetComponent<Rigidbody2D>().velocity = direction * BULLET_BASE_SPEED;
        }

    }

    IEnumerator Reload()
    {
        isReloading = true;
        yield return new WaitForSeconds(reloadTime);
        currentAmmo = maxAmmo;
        isReloading = false;
    }
}
