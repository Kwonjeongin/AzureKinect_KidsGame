using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public enum GameState
{
    Ready,
    Start,
    TimeOver
}

public class GameManager : MonoBehaviour
{
    public TMP_Text gameStateText;
    public GameObject countdownUI;
    public Image countdownImage;
    public Sprite[] countdownSprites; // 0: 3, 1: 2, 2: 1
    public TMP_Text timerTextR;
    public TMP_Text timerTextL;
    public Slider timeSlider;
    public GameObject timeOverUI;
    private float totalTime = 10f;
    private float timer;
    private bool timerStopped = false; // Ÿ�̸Ӱ� ���� �������� ���θ� ��Ÿ���� ����
    private int countdownValue = 3;
    private float countdownDuration = 1f;

    void Start()
    {
        timer = totalTime;
        timeSlider.maxValue = totalTime;
        timeOverUI.SetActive(false); // ó���� TimeOverUI�� ��Ȱ��ȭ�մϴ�.
        StartCoroutine(StartCountdown());
    }

    IEnumerator StartCountdown()
    {
        countdownUI.SetActive(true); // ī��Ʈ �ٿ� UI Ȱ��ȭ

        yield return new WaitForSeconds(3f); // 3�� ���

        SetGameState(GameState.Start); // ���� ���� ���·� ����

        while (countdownValue > 0)
        {
            countdownImage.sprite = countdownSprites[countdownValue - 1]; // ���� �̹��� ������Ʈ
            countdownValue--;
            yield return new WaitForSeconds(countdownDuration);
        }

        countdownUI.SetActive(false); // ī��Ʈ �ٿ� UI ��Ȱ��ȭ
    }

    void Update()
    {
        if (timerStopped) // Ÿ�̸Ӱ� ���߾��ٸ� ������Ʈ�� �����մϴ�.
            return;

        timer -= Time.deltaTime;
        timer = Mathf.Max(timer, 0); // Ÿ�̸Ӱ� ������ ���� �ʵ��� �����մϴ�.

        UpdateTimerUI();
        UpdateSliderValue();

        if (timer <= 0)
        {
            timer = 0; // Ÿ�̸Ӱ� 0 ���Ϸ� ���� �ʵ��� �����մϴ�.
            timerStopped = true; // Ÿ�̸Ӱ� ���ߵ��� �÷��׸� �����մϴ�.
            TimeOver();
        }
    }

    void UpdateTimerUI()
    {
        int minutes = Mathf.FloorToInt(timer / 60);
        int seconds = Mathf.FloorToInt(timer % 60);

        timerTextR.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        timerTextL.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    void UpdateSliderValue()
    {
        timeSlider.value = totalTime - timer; // �����̴��� ���� ��ü �ð����� ���� �ð��� �� ���Դϴ�.
    }

    void SetGameState(GameState state)
    {
        //gameState = state;

        switch (state)
        {
            case GameState.Ready:
                gameStateText.text = "Ready";
                break;
            case GameState.Start:
                gameStateText.text = "Start";
                break;
            case GameState.TimeOver:
                gameStateText.text = "Time Over";
                break;
        }
    }

    public void TimeOver()
    {
        SetGameState(GameState.TimeOver); // Ÿ�� ���� ���·� ����
        timeOverUI.SetActive(true); // Ÿ�̸Ӱ� ����Ǹ� TimeOverUI�� Ȱ��ȭ�մϴ�.
        // ���⿡ Ÿ�̸Ӱ� ���߾��� �� ������ �۾��� �߰��� �� �ֽ��ϴ�.
    }
}
