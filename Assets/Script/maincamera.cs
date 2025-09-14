using UnityEngine;

public class maincamera : MonoBehaviour
{
    public Transform target; // ���� Ÿ��
    public float speed = 5f; // ���󰡴� �ӵ� (���� Ŭ���� ����)

    void LateUpdate()
    {
        if (target != null)
        {
            // Ÿ���� ��ġ�� ī�޶� ��ġ�� �ε巴�� �̵�
            Vector3 targetPosition = new Vector3(target.position.x, target.position.y, transform.position.z);
            transform.position = Vector3.Lerp(transform.position, targetPosition, speed * Time.deltaTime);
        }
    }
}
