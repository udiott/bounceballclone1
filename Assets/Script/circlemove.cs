#pragma warning disable 4996
using UnityEngine;
using System.Collections;

public class circlemove : MonoBehaviour
{
    public Rigidbody2D rb;
    public float jumpForce;
    public float speed = 5f;

    public float jumpBoostMultiplier = 1.5f;   // 점프력 강화 비율
    private float dashForce=15f;                    // 대시 힘
    private float dashDoubleTapTime = 0.25f;     // 더블탭 인식 시간

    private bool canMove = true;
    private float originalJumpForce;

    private bool hasDash = false;
    private bool isDashing = false;            // 좌우 대시 상태

    private bool hasHighJump = false;          // 위 점프 아이템 여부
    private bool isHighJumping = false;        // 위 점프 중 여부
    private float highJumpForce = 10f;         // 위 점프 대시 힘

    private SpriteRenderer innerRenderer;      // 안쪽 원 색 변경용

    // 더블탭 체크용
    private float lastLeftTapTime = -1f;
    private float lastRightTapTime = -1f;
    private float lastUpTapTime = -1f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        originalJumpForce = jumpForce;

        // 안쪽 원 가져오기
        innerRenderer = transform.Find("inCircle").GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (!canMove) return;

        // 좌우 이동 (대시 중에는 무시)
        if (!isDashing && !isHighJumping)
        {
            float x = Input.GetAxisRaw("Horizontal");
            rb.linearVelocity = new Vector2(x * speed, rb.linearVelocity.y);
        }

        // 좌우 대시 입력
        if (hasDash)
        {
            // 오른쪽 대시
            if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            {
                if (Time.time - lastRightTapTime <= dashDoubleTapTime)
                    DashForward(1);

                lastRightTapTime = Time.time;
            }

            // 왼쪽 대시
            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
            {
                if (Time.time - lastLeftTapTime <= dashDoubleTapTime)
                    DashForward(-1);

                lastLeftTapTime = Time.time;
            }
        }

        // 위 점프 대시 입력
        if (hasHighJump)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
            {
                if (Time.time - lastUpTapTime <= dashDoubleTapTime)
                    DashUp();

                lastUpTapTime = Time.time;
            }
        }
    }

    void DashForward(int direction)
    {
        Debug.Log("대시 발동! 방향: " + direction);
        isDashing = true;

        if (innerRenderer != null)
            innerRenderer.color = Color.white;

        hasDash = false; // 대시 사용 후 해제
        StartCoroutine(DashDuration(direction));
    }

    IEnumerator DashDuration(int direction)
    {
        float dashTime = 0.35f;
        float dashSpeed = dashForce;

        float elapsed = 0f;
        while (elapsed < dashTime)
        {
            elapsed += Time.deltaTime;

            // 반대 방향키 누르면 즉시 멈춤
            if ((direction == 1 && (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))) ||
                (direction == -1 && (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))))
            {
                Debug.Log("반대 키 눌러서 대쉬 취소!");
                break;
            }

            rb.linearVelocity = new Vector2(direction * dashSpeed, rb.linearVelocity.y);
            yield return null;
        }

        isDashing = false;
    }

    void DashUp()
    {
        Debug.Log("위 점프 대시 발동!");
        isHighJumping = true;

        if (innerRenderer != null)
            innerRenderer.color = Color.white;

        hasHighJump = false; // 아이템 사용 후 해제
        StartCoroutine(DashUpDuration());
    }

    IEnumerator DashUpDuration()
    {
        float dashTime = 0.25f; // 위 대시 지속 시간
        float elapsed = 0f;

        while (elapsed < dashTime)
        {
            elapsed += Time.deltaTime;

            // Y축만 강제 점프, X축은 기존 이동 유지
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, highJumpForce);

            yield return null;
        }

        isHighJumping = false;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Trap"))
        {
            Debug.Log("사망!");
            StartCoroutine(DeathAnimation());
        }

        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("JumpBlock"))
        {
            foreach (ContactPoint2D contact in collision.contacts)
            {
                if (Vector2.Dot(contact.normal, Vector2.up) > 0.75f)
                {
                    float jumpPower = originalJumpForce;

                    if (collision.gameObject.CompareTag("JumpBlock"))
                    {
                        jumpPower *= jumpBoostMultiplier;
                        Debug.Log("점프 블럭 점프력 강화!");
                    }

                    rb.linearVelocity = new Vector2(0, jumpPower);
                    isDashing = false;
                    isHighJumping = false;
                    break;
                }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Goal"))
        {
            GoalManager.Instance.CollectGoal();
            Destroy(collision.gameObject);
        }

        if (collision.CompareTag("doublejump"))
        {
            Destroy(collision.gameObject);
            hasDash = true;
            hasHighJump = false;
            if (innerRenderer != null)
                innerRenderer.color = Color.black;
        }
        if (collision.CompareTag("jumphigh"))
        {
            Destroy(collision.gameObject);
            hasHighJump = true;
            hasDash = false;
            if (innerRenderer != null)
                innerRenderer.color = Color.black;
        }
    }

    IEnumerator DeathAnimation()
    {
        canMove = false;
        rb.linearVelocity = Vector2.zero;
        rb.isKinematic = true;

        float t = 0;
        Vector3 originalScale = transform.localScale;

        while (t < 1f)
        {
            t += Time.deltaTime * 3f;
            transform.localScale = Vector3.Lerp(originalScale, Vector3.zero, t);
            yield return null;
        }

        yield return new WaitForSeconds(0.2f);

        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex
        );
        Time.timeScale = 1f;
    }
}
