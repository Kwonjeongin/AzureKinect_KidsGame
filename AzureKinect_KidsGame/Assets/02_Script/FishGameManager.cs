using UnityEngine;
using UnityEngine.UI;

public class FishGameManager : MonoBehaviour
{
    public Text scoreText;
    public Text scoreText2;
    public Text scoreText3;
    private int score = 0;

    public GameObject[] fishPrefabs; // ��, ��, �� ����� ������ �迭
    public Transform[] spawnPoints; // ����Ⱑ ������ ��ġ �迭

    private void Start()
    {
        // 1�� 30�ʸ��� SpawnFish �Լ� ȣ��
        InvokeRepeating("SpawnFish", 0f, 90f);
    }

    private void SpawnFish()
    {
        // ����� ����
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            // �����ϰ� ������� ũ�⸦ ����
            int fishIndex = Random.Range(0, fishPrefabs.Length);
            GameObject fishPrefab = fishPrefabs[fishIndex];

            // ����� �������� ���� ����Ʈ�� ����
            GameObject newFish = Instantiate(fishPrefab, spawnPoints[i].position, Quaternion.identity);

            // �� ������� ũ�⿡ ���� �������� �ӵ� ����
            FishMovement fishMovement = newFish.GetComponent<FishMovement>();
            if (fishMovement != null)
            {
                if (fishIndex == 0) // �� �����
                    fishMovement.fallSpeed = 6f;
                else if (fishIndex == 1) // �� �����
                    fishMovement.fallSpeed = 4f;
                else // �� �����
                    fishMovement.fallSpeed = 3f;
            }
        }
    }

    public void AddScore(int scoreToAdd)
    {
        // ���� �߰� �� ����� �ؽ�Ʈ ������Ʈ
        score += scoreToAdd;
        scoreText.text = "����: " + score.ToString();
        scoreText2.text = "����: " + score.ToString();
        scoreText3.text = "����: " + score.ToString();

    }
}
