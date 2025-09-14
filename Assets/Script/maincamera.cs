using UnityEngine;

public class maincamera : MonoBehaviour
{
    public Transform target; // 따라갈 타겟
    public float speed = 5f; // 따라가는 속도 (값이 클수록 빠름)

    void LateUpdate()
    {
        if (target != null)
        {
            // 타겟의 위치에 카메라 위치를 부드럽게 이동
            Vector3 targetPosition = new Vector3(target.position.x, target.position.y, transform.position.z);
            transform.position = Vector3.Lerp(transform.position, targetPosition, speed * Time.deltaTime);
        }
    }
}
