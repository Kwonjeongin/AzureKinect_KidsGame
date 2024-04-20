using UnityEngine;

public class FishMovement : MonoBehaviour
{
    public float fallSpeed = 5f; // ������� �������� �ӵ�

    private void Update()
    {
        // �Ʒ� �������� ����⸦ �̵�
        transform.Translate(Vector3.down * fallSpeed * Time.deltaTime);

        // ����Ⱑ ȭ�� �Ʒ��� ����� ����
        if (transform.position.y < -5f)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // �÷��̾�� �浹�ϸ� ���� �Ŵ������� ���� �߰� ��û
        if (collision.CompareTag("Player"))
        {
            FishGameManager gameManager = FindObjectOfType<FishGameManager>();
            if (gameManager != null)
            {
                int scoreToAdd = 0;
                if (gameObject.CompareTag("LargeFish"))
                    scoreToAdd = 20;
                else if (gameObject.CompareTag("MediumFish"))
                    scoreToAdd = 50;
                else if (gameObject.CompareTag("SmallFish"))
                    scoreToAdd = 100;

                gameManager.AddScore(scoreToAdd);
            }

            // �浹�� ����� ����
            Destroy(gameObject);
        }
    }
}
