using UnityEngine;

public class Fish : MonoBehaviour
{
    public int score = 1; // ������� ����

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // ����Ⱑ �÷��̾�� �浹���� �� ���� ���� �� ����� ����
            GameManager.Instance.AddScore(score);
            Destroy(gameObject);
        }
    }
}