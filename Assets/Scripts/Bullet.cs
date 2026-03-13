using UnityEngine;

public class Bullet : MonoBehaviour
{
    private GameManager gm;

    void Start()
    {
        // Tìm đối tượng GameManager trong Scene
        gm = FindObjectOfType<GameManager>();
    }

   private void OnTriggerEnter2D(Collider2D collision)
    {
        Pill pill = collision.GetComponent<Pill>();
        
        if (pill != null && gm != null)
        {
            Vector3 contactPos = collision.transform.position;

            // BƯỚC 1: Ẩn viên thuốc ngay lập tức
            collision.gameObject.SetActive(false);

            // BƯỚC 2: Gọi logic xử lý màn chơi (Hàm này sẽ gọi SetupPills để bật lại các viên)
            if (pill.isCorrect)
            {
                gm.RightAnswer(contactPos);
            }
            else
            {
                gm.WrongAnswer(contactPos, true);
            }

            // BƯỚC 3: Xóa viên đạn
            Destroy(gameObject);
        }
    }
}