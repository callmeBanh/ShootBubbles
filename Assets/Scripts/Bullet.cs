using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 15f;
    public float lifeTime = 3f; // Tự xóa sau 3 giây nếu không trúng gì

    void Start() {
        Destroy(gameObject, lifeTime);
    }

    void Update() {
        // Đạn bay theo hướng 'lên' của chính nó (đã xoay theo súng)
        transform.Translate(Vector3.up * speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other) {
        // Kiểm tra xem có chạm vào viên thuốc không (Nhớ đặt Tag là "Pill")
        if (other.CompareTag("Pill")) {
            Pill pillScript = other.GetComponent<Pill>();
            GameManager gm = FindObjectOfType<GameManager>();

            if (pillScript != null && gm != null) {
                if (pillScript.isCorrect) {
                    gm.RightAnswer(); // Bắn trúng đáp án đúng -> Thắng
                } else {
                    gm.WrongAnswer(); // Bắn sai -> Trừ mạng
                }
            }
            
            Destroy(gameObject); // Xóa viên đạn ngay khi va chạm
        }
    }
}