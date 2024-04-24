using UnityEngine;

public class FishSpawner : MonoBehaviour
{
    public GameObject[] fishPrefabs; // ������ ����� ������ �迭
    public float minSpawnInterval = 0.5f; // �ּ� ���� ����
    public float maxSpawnInterval = 2f; // �ִ� ���� ����

    private void Start()
    {
        // ����� ������ ����
        InvokeRepeating("SpawnFish", 0f, Random.Range(minSpawnInterval, maxSpawnInterval));
    }

    void SpawnFish()
    {
        // ���� ������Ʈ�� ��ġ�� ���� ��ġ�� ����
        Vector3 spawnPosition = transform.position;

        // ������ ����� ������ ����
        GameObject randomFishPrefab = fishPrefabs[Random.Range(0, fishPrefabs.Length)];

        // ����� ����
        GameObject fish = Instantiate(randomFishPrefab, spawnPosition, Quaternion.identity);

        // ����⿡ Rigidbody ������Ʈ�� ������ �߷� Ȱ��ȭ
        Rigidbody rb = fish.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.useGravity = true;
        }
    }
}
