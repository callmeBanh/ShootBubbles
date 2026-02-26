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
    public int currentLevel = 1; // Biến theo dõi màn chơi hiện tại
    
    private int correctAnswer;
    public GameObject[] pills; // Kéo 3 viên thuốc vào đây trong Inspector

    private bool isGameOver = false;

    void Start() {
        isGameOver = false;
        UpdateUI();
        GenerateQuestion();
    }

    void Update() {
        if (isGameOver) return;

        // Xử lý đếm ngược thời gian
        if (timeLeft > 0) {
            timeLeft -= Time.deltaTime;
            timerText.text = "00:" + Mathf.Max(0, Mathf.Ceil(timeLeft)).ToString("00");
            
            if (timeLeft <= 0) {
                GameOver();
            }
        }
    }

    void UpdateUI() {
        if (livesText != null) {
            livesText.text = lives + " mạng"; // Cập nhật số mạng hiển thị
        }
    }

    public void GenerateQuestion() {
        if (isGameOver) return;

        // YÊU CẦU: Đặt lại thời gian về 30 giây mỗi khi bắt đầu màn chơi/câu hỏi mới
        timeLeft = 30f;

        // Thiết lập dữ liệu cho từng màn chơi cố định
        if (currentLevel == 1) {
            questionText.text = "1 + 2 = ?";
            correctAnswer = 3;
            SetupPills(new int[] { 3, 2, 5 });
        } 
        else if (currentLevel == 2) {
            questionText.text = "5 + 4 = ?";
            correctAnswer = 9;
            SetupPills(new int[] { 7, 9, 10 });
        } 
        else if (currentLevel == 3) {
            questionText.text = "2 + 3 = ?";
            correctAnswer = 5;
            SetupPills(new int[] { 4, 8, 5 });
        } 
        else {
            // Nếu đã vượt qua màn 3 thì thắng cuộc
            isGameOver = true;
            loadingController.LoadScene("Win");
        }
    }

    // Hàm phụ để gán giá trị vào 3 viên thuốc
    private void SetupPills(int[] values) {
        for (int i = 0; i < pills.Length; i++) {
            if (i >= values.Length) break;

            Pill pillScript = pills[i].GetComponent<Pill>();
            TMP_Text pillText = pills[i].GetComponentInChildren<TMP_Text>();

            if (pillScript != null && pillText != null) {
                pillText.text = values[i].ToString();
                pillScript.isCorrect = (values[i] == correctAnswer); // Đánh dấu đúng/sai
            }
        }
    }

    public void RightAnswer() {
        if (isGameOver) return;

        // Tăng cấp độ lên màn tiếp theo
        currentLevel++;
        
        if (currentLevel <= 3) {
            Debug.Log("Chính xác! Chuyển sang màn " + currentLevel);
            GenerateQuestion(); // Hàm này sẽ reset lại 30 giây và đổi câu hỏi mới
        } else {
            isGameOver = true;
            loadingController.LoadScene("Win");
        }
    }

    public void WrongAnswer() {
        if (isGameOver) return;

        lives--;
        UpdateUI();
        
        if (lives <= 0) {
            GameOver();
        } else {
            // Khi bắn sai, reset lại thời gian và câu hỏi của màn hiện tại
            GenerateQuestion(); 
        }
    }

    void GameOver() {
        if (isGameOver) return;
        isGameOver = true;
        loadingController.LoadScene("Lose");
    }
}