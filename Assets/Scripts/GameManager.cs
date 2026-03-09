using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement; // Thêm để xử lý chuyển cảnh nếu cần

public class GameManager : MonoBehaviour
{
    [Header("UI Elements")]
    public TMP_Text questionText;
    public TMP_Text timerText;
    public TMP_Text livesText;

    [Header("Game Settings")]
    public float timeLeft = 40f;
    public int lives = 3;
    public int currentLevel = 1;
    
    [Header("Difficulty Scaling")]
    public float timeSpentInLevel = 0f;
    public float speedIncreaseInterval = 10f; // Cứ mỗi 10 giây tăng tốc 1 lần
    public float speedMultiplierPerInterval = 0.2f; // Tăng 20% mỗi lần

    private int correctAnswer;
    public GameObject[] pills; 

    private bool isGameOver = false;

    void Start() {
        isGameOver = false;
        UpdateUI();
        GenerateQuestion();
    }

    void Update() {
        if (isGameOver) return;

        if (timeLeft > 0) {
            timeLeft -= Time.deltaTime;
            timeSpentInLevel += Time.deltaTime; // Đếm thời gian đã trôi qua trong màn
            
            timerText.text = "00:" + Mathf.Max(0, Mathf.Ceil(timeLeft)).ToString("00");
            
            if (timeLeft <= 0) {
                GameOver();
            }
        }
    }

    // Hàm tính toán độ khó dựa trên thời gian thực
    public float GetDifficultyMultiplier() {
        // Công thức: 1 + (thời gian trôi qua / quãng nghỉ) * tỷ lệ tăng
        return 1f + (timeSpentInLevel / speedIncreaseInterval) * speedMultiplierPerInterval;
    }

    void UpdateUI() {
        if (livesText != null) {
            livesText.text = lives + " mạng";
        }
    }

    public void GenerateQuestion() {
        if (isGameOver) return;

        timeLeft = 40f;
        timeSpentInLevel = 0f; // Reset thời gian đếm tăng khi có câu hỏi mới

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
            isGameOver = true;
            // Giả sử bạn có script loadingController hoặc dùng SceneManager
            SceneManager.LoadScene("Win");
        }
    }

    private void SetupPills(int[] values) {
        float[] xPositions = { -2.2f, 0f, 2.2f };
        ShuffleArray(xPositions);

        for (int i = 0; i < pills.Length; i++) {
            if (i >= values.Length) break;

            // Đặt vị trí X cố định theo cột, Y so le để tránh dính nhau
            float startY = 7f + (i * 1.2f);
            pills[i].transform.position = new Vector3(xPositions[i], startY, 0);
            pills[i].SetActive(true);

            Pill pillScript = pills[i].GetComponent<Pill>();
            TMP_Text pillText = pills[i].GetComponentInChildren<TMP_Text>();

            if (pillScript != null && pillText != null) {
                pillText.text = values[i].ToString();
                pillScript.isCorrect = (values[i] == correctAnswer);
            }
        }
    }

    void ShuffleArray(float[] array) {
        for (int i = 0; i < array.Length; i++) {
            float temp = array[i];
            int randomIndex = Random.Range(i, array.Length);
            array[i] = array[randomIndex];
            array[randomIndex] = temp;
        }
    }

    public void RightAnswer() {
        if (isGameOver) return;
        currentLevel++;
        if (currentLevel <= 3) {
            GenerateQuestion();
        } else {
            isGameOver = true;
            SceneManager.LoadScene("Win");
        }
    }

    public void WrongAnswer() {
        if (isGameOver) return;
        lives--;
        UpdateUI();
        if (lives <= 0) {
            GameOver();
        } else {
            GenerateQuestion(); 
        }
    }

    void GameOver() {
        if (isGameOver) return;
        isGameOver = true;
        SceneManager.LoadScene("Lose");
    }
}