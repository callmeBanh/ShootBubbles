using UnityEngine;

public class PillMovement : MonoBehaviour
{
    [Header("Speed Settings")]
    public float baseFallSpeed = 0.6f; // Tốc độ cơ bản ban đầu
    private float speedFactorByLevel;
    private GameManager gm;

    [Header("Boundaries")]
    public float minX = -2.5f; 
    public float maxX = 2.5f;

    void Start() {
        gm = FindObjectOfType<GameManager>();
        
        // Tốc độ ban đầu được tính dựa trên Level hiện tại
        // Ví dụ: Level 1 cộng 0.2, Level 2 cộng 0.4...
        speedFactorByLevel = baseFallSpeed + (gm != null ? gm.currentLevel * 0.2f : 0);
    }

    void Update() {
        if (gm == null) return;

        // Lấy hệ số tăng dần theo thời gian thực từ GameManager
        float timeMultiplier = gm.GetDifficultyMultiplier();
        
        // Tốc độ cuối cùng = (Tốc độ Level) * (Hệ số thời gian)
        float finalSpeed = speedFactorByLevel * timeMultiplier;

        // 1. Di chuyển xuống dưới
        transform.position += Vector3.down * finalSpeed * Time.deltaTime;

        // 2. Giới hạn vị trí X để đảm bảo không bị văng khỏi tường khi va chạm
        float clampedX = Mathf.Clamp(transform.position.x, minX, maxX);
        transform.position = new Vector3(clampedX, transform.position.y, transform.position.z);

        // 3. Xử lý khi rơi quá tầm nhìn (Thua mạng)
        if (transform.position.y < -6f) {
            gm.WrongAnswer();
            Destroy(gameObject);
        }
    }
}