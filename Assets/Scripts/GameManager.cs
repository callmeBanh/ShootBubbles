using UnityEngine;
using TMPro;
using System.Collections;

public class GameManager : MonoBehaviour
{
    [Header("UI Elements")]
    public TMP_Text questionText;
    public TMP_Text timerText;
    public TMP_Text livesText;

    [Header("Game Settings")]
    public float timeLeft = 30f;
    public int lives = 3;
    
    private int correctAnswer;
    public GameObject[] pills; // Kéo 3 viên thuốc vào đây trong Inspector

    void Start() {
        UpdateUI();
        GenerateQuestion();
    }

    void Update() {
        if (timeLeft > 0) {
            timeLeft -= Time.deltaTime;
            timerText.text = "00:" + Mathf.Ceil(timeLeft).ToString("00");
            if (timeLeft <= 0) GameOver();
        }
    }

    void UpdateUI() {
        livesText.text = lives + " mạng"; // Cập nhật số mạng hiển thị
    }

    public void GenerateQuestion() {
        // Câu đố cố định
        int a = 1;
        int b = 2;
        correctAnswer = 3;
        questionText.text = $"{a} + {b} = ?";

        // Gán đáp án vào các viên thuốc
        int[] pillValues = {3, 2, 5};
        for (int i = 0; i < pills.Length; i++) {
            pills[i].GetComponentInChildren<TMP_Text>().text = pillValues[i].ToString();
            pills[i].GetComponent<Pill>().isCorrect = (pillValues[i] == correctAnswer);
        }
    }

    public void WrongAnswer() {
        lives--;
        UpdateUI();
        if (lives <= 0) {
            GameOver();
        } else {
            // Có thể thêm hiệu ứng rung màn hình ở đây
            GenerateQuestion(); 
        }
    }

    public void RightAnswer() {
        // Sử dụng loadingController đã có để chuyển sang cảnh Win
        loadingController.LoadScene("Win");
    }

    void GameOver() {
        // Sử dụng loadingController đã có để chuyển sang cảnh Lose
        loadingController.LoadScene("Lose");
    }
}