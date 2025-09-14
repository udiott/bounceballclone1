using UnityEngine;

public class BreakingPlatform : MonoBehaviour
{
    public GameObject breakEffectPrefab;
    public float destroyDelay = 0.1f;

    private bool isBroken = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isBroken) return;

        if (collision.gameObject.CompareTag("Player"))
        {
            foreach (ContactPoint2D contact in collision.contacts)
            {
                if (contact.point.y > transform.position.y)
                {
                    isBroken = true;

                    // 이펙트 생성
                    if (breakEffectPrefab != null)
                    {
                        GameObject effect = Instantiate(breakEffectPrefab, transform.position, Quaternion.identity);

                        ParticleSystem ps = effect.GetComponent<ParticleSystem>();
                        if (ps != null)
                        {
                            var emission = ps.emission;
                            emission.rateOverTime = 0;

                            var bursts = new ParticleSystem.Burst[1];
                            bursts[0] = new ParticleSystem.Burst(0f, 30);
                            emission.SetBursts(bursts);

                            ps.Play();

                            Destroy(effect, ps.main.duration + ps.main.startLifetime.constantMax);
                        }
                    }

                    // 1. 본체 Sprite 비활성화
                    var sprite = GetComponent<SpriteRenderer>();
                    if (sprite) sprite.enabled = false;

                    // 2. 자식들의 렌더러 비활성화
                    foreach (Transform child in transform)
                    {
                        var childRenderer = child.GetComponent<Renderer>();
                        if (childRenderer != null)
                            childRenderer.enabled = false;
                    }

                    // 3. Collider 비활성화
                    var col = GetComponent<Collider2D>();
                    if (col) col.enabled = false;

                    // 4. 일정 시간 후 완전히 제거
                    Destroy(gameObject, destroyDelay + 0.1f);
                    break;
                }
            }
        }
    }
}
