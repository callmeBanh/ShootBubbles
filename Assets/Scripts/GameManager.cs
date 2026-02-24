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
        // Tạo câu đố toán đơn giản cho lớp 1
        int a = Random.Range(1, 6);
        int b = Random.Range(1, 5);
        correctAnswer = a + b;
        questionText.text = $"{a} + {b} = ?";

        // Gán đáp án vào các viên thuốc
        int correctPillIndex = Random.Range(0, pills.Length);
        for (int i = 0; i < pills.Length; i++) {
            int val = (i == correctPillIndex) ? correctAnswer : correctAnswer + Random.Range(-2, 3);
            if (i != correctPillIndex && val == correctAnswer) val++; 
            
            // Cập nhật text trên viên thuốc
            pills[i].GetComponentInChildren<TMP_Text>().text = val.ToString();
            // Đánh dấu viên đúng/sai vào script Pill
            pills[i].GetComponent<Pill>().isCorrect = (i == correctPillIndex);
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