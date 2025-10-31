using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement; 

public class Player : MonoBehaviour
{
    [Header("Movement Settings")]
    public float laneDistance = 2.5f;
    public float moveSpeed = 8f;
    public float jumpHeight = 2f;
    public float jumpDuration = 0.6f;
    public int currentLane = 1;

    [Header("Health Settings")]
    public int maxHP = 100;
    public int currentHP = 100;
    public TextMeshProUGUI hpText;

    [Header("Regeneration Settings")]
    public float regenRate = 1f; 
    private float regenTimer = 0f;

    private bool isJumping = false;
    private float jumpTimer = 0f;
    private float baseY;
    private Vector3 targetPosition;

    void Start()
    {
        baseY = transform.position.y;
        targetPosition = transform.position;
        UpdateHPText();
    }

    void Update()
    {
        HandleInput();
        MovePlayer();
        HandleJump();
        RegenerateHP();
    }

    void RegenerateHP()
    {
        if (currentHP >= maxHP) return;

        regenTimer += Time.deltaTime;
        if (regenTimer >= 1f) 
        {
            currentHP += Mathf.FloorToInt(regenRate);
            if (currentHP > maxHP) currentHP = maxHP;
            UpdateHPText();
            regenTimer = 0f;
        }
    }

    void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (currentLane > 0)
            {
                currentLane--;
                UpdateTargetPosition();
            }
        }

        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (currentLane < 2)
            {
                currentLane++;
                UpdateTargetPosition();
            }
        }

        if (Input.GetKeyDown(KeyCode.Space) && !isJumping)
        {
            isJumping = true;
            jumpTimer = 0f;
        }
    }

    void UpdateTargetPosition()
    {
        float xPos = (currentLane - 1) * laneDistance;
        targetPosition = new Vector3(xPos, baseY, transform.position.z);
    }

    void MovePlayer()
    {
        Vector3 pos = transform.position;
        pos.x = Mathf.Lerp(pos.x, targetPosition.x, Time.deltaTime * moveSpeed);
        transform.position = pos;
    }

    void HandleJump()
    {
        if (isJumping)
        {
            jumpTimer += Time.deltaTime;
            float normalized = jumpTimer / jumpDuration;
            float heightOffset = Mathf.Sin(normalized * Mathf.PI) * jumpHeight;
            Vector3 pos = transform.position;
            pos.y = baseY + heightOffset;
            transform.position = pos;

            if (normalized >= 1f)
            {
                isJumping = false;
                jumpTimer = 0f;
                pos.y = baseY;
                transform.position = pos;
            }
        }
    }

    public float YOffset => transform.position.y - baseY;
    public int LaneIndex => currentLane;

    public void TakeDamage(int amount = 5) 
    {
        currentHP -= amount;
        if (currentHP < 0) currentHP = 0;
        UpdateHPText();
        Debug.Log("Player HP: " + currentHP);
        if (currentHP <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    private void UpdateHPText()
    {
        if (hpText != null)
            hpText.text = $"Health: {currentHP}";
    }
}
