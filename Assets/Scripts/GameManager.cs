using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI; // Cần thiết để quản lý Image trái tim
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("UI Elements (Text)")]
    public TMP_Text questionText;
    public TMP_Text timerText;

    [Header("UI Elements (Hearts)")]
    // Kéo 3 Image trái tim từ Hierarchy vào mảng này theo thứ tự
    public GameObject[] heartImages; 

    [Header("Game Settings")]
    public float timeLeft = 40f;
    public int lives = 3;
    public int currentLevel = 1;
    
    [Header("Difficulty Scaling")]
    public float timeSpentInLevel = 0f;
    public float speedIncreaseInterval = 10f; // Mỗi 10 giây tăng tốc 1 lần
    public float speedMultiplierPerInterval = 0.2f; // Tăng 20% mỗi lần

    [Header("Particle Effects")]
    public GameObject explosionPrefab; // Prefab hiệu ứng nổ khi bắn trúng

    private int correctAnswer;
    public GameObject[] pills; // Danh sách 3 viên thuốc (Pills) trong Scene

    private bool isGameOver = false;

    void Start() {
        isGameOver = false;
        
        // Đảm bảo số mạng khớp với số lượng trái tim được kéo vào Inspector
        if (heartImages != null && heartImages.Length > 0) {
            lives = heartImages.Length;
        }

        UpdateHeartUI();
        GenerateQuestion();
    }

    void Update() {
        if (isGameOver) return;

        // Xử lý đếm ngược thời gian
        if (timeLeft > 0) {
            timeLeft -= Time.deltaTime;
            timeSpentInLevel += Time.deltaTime; 
            
            timerText.text = "00:" + Mathf.Max(0, Mathf.Ceil(timeLeft)).ToString("00");
            
            if (timeLeft <= 0) {
                GameOver();
            }
        }
    }

    // Hàm trả về hệ số tốc độ dựa trên thời gian thực cho PillMovement truy cập
    public float GetDifficultyMultiplier() {
        return 1f + (timeSpentInLevel / speedIncreaseInterval) * speedMultiplierPerInterval;
    }

    // Cập nhật hiển thị trái tim trên UI
    void UpdateHeartUI() {
        if (heartImages == null) return;
        for (int i = 0; i < heartImages.Length; i++) {
            if (heartImages[i] != null) {
                heartImages[i].SetActive(i < lives);
            }
        }
    }

    public void GenerateQuestion() {
        if (isGameOver) return;

        timeLeft = 40f;
        timeSpentInLevel = 0f; 

        // Thiết lập dữ liệu cho từng màn chơi
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
            SceneManager.LoadScene("Win");
        }
    }

    private void SetupPills(int[] values) {
        float[] xPositions = { -2.2f, 0f, 2.2f };
        ShuffleArray(xPositions);

        for (int i = 0; i < pills.Length; i++) {
            if (pills[i] == null) continue;

            // Tắt đi trước khi bật lại để ép Unity cập nhật trạng thái
            pills[i].SetActive(false); 
            pills[i].SetActive(true); 

            if (i >= values.Length) {
                pills[i].SetActive(false);
                continue;
            }

            // Đặt vị trí
            float startY = 7f + (i * 1.5f);
            pills[i].transform.position = new Vector3(xPositions[i], startY, 0);

            // Reset Rigidbody2D nếu có (tránh việc vừa hiện ra đã rơi cực nhanh do vận tốc cũ)
            Rigidbody2D rb = pills[i].GetComponent<Rigidbody2D>();
            if (rb != null) {
                rb.linearVelocity = Vector2.zero;
                rb.angularVelocity = 0f;
            }

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

    // Được gọi từ Bullet.cs khi bắn trúng số đúng
    public void RightAnswer(Vector3 position) {
        if (isGameOver) return;

        // Sinh hiệu ứng nổ tại vị trí va chạm
        if (explosionPrefab != null) {
            Instantiate(explosionPrefab, position, Quaternion.identity);
        }

        currentLevel++;
        if (currentLevel <= 3) {
            GenerateQuestion();
        } else {
            isGameOver = true;
            SceneManager.LoadScene("Win");
        }
    }

    // Được gọi từ Bullet.cs khi bắn trúng số sai, hoặc PillMovement.cs khi rơi quá thấp
    public void WrongAnswer(Vector3 position, bool pillWasShot) {
        if (isGameOver) return;

        // Chỉ nổ nếu nguyên nhân mất mạng là do bị đạn bắn trúng
        if (pillWasShot && explosionPrefab != null) {
            Instantiate(explosionPrefab, position, Quaternion.identity);
        }

        lives--;
        UpdateHeartUI();
        
        if (lives <= 0) {
            GameOver();
        } else {
            // Reset lại câu hỏi của màn hiện tại
            GenerateQuestion(); 
        }
    }
    // Overload để gọi nhanh từ PillMovement (không truyền tham số)
    public void WrongAnswer() {
        WrongAnswer(Vector3.zero, false);
    }

    void GameOver() {
        if (isGameOver) return;
        isGameOver = true;
        SceneManager.LoadScene("Lose");
    }
}