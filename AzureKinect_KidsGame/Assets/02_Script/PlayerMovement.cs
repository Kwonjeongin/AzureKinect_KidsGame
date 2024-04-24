using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // �÷��̾� �̵� �ӵ�

    private Rigidbody rb; // Rigidbody ������Ʈ ����

    private void Start()
    {
        // Rigidbody ������Ʈ ��������
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        // ���� �Է� ����
        float horizontalInput = Input.GetAxis("Horizontal");

        // Ű���� �Է°��� �ִ� ��쿡�� �����̱�
        if (Mathf.Abs(horizontalInput) > 0.1f)
        {
            // �÷��̾� �̵� ���� ���� ���
            Vector3 movement = new Vector3(horizontalInput, 0f, 0f) * moveSpeed * Time.deltaTime;

            // Raycast�� ����Ͽ� �̵��� ��ġ�� ���� �ִ��� Ȯ��
            RaycastHit hit;
            if (!Physics.Raycast(transform.position, movement.normalized, out hit, movement.magnitude))
            {
                // ���� ���� ��쿡�� Rigidbody�� �̿��Ͽ� �÷��̾� �̵�
                rb.MovePosition(transform.position + movement);
            }
        }
    }
}
