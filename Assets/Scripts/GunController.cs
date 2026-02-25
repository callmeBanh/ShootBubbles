using UnityEngine;

public class GunController : MonoBehaviour
{
    public float rotationSpeed = 50f;
    private float rotationInput = 0f;
    private bool isMoving = false; // Biến để kiểm tra trạng thái di chuyển

    [Header("Shooting Settings")]
    public GameObject bulletPrefab; // Kéo Prefab viên đạn vào đây
    public Transform shootingPoint; // Kéo điểm đầu nòng (shootingPoint) vào đây

    public void MoveLeft() {
        rotationInput = 1f;
        isMoving = true;
    }

    public void MoveRight() {
        rotationInput = -1f;
        isMoving = true;
    }

    public void StopMoving() {
        rotationInput = 0f;
        isMoving = false;
    }

    void Update() {
        // Xử lý xoay súng
        float angle = transform.eulerAngles.z;
        if (angle > 180) angle -= 360;

        if ((rotationInput > 0 && angle < 60) || (rotationInput < 0 && angle > -60)) {
            transform.Rotate(Vector3.forward * rotationInput * rotationSpeed * Time.deltaTime);
        }

        // Xử lý bắn đạn khi chạm/click màn hình, chỉ bắn khi không di chuyển
        if (Input.GetMouseButtonDown(0) && !isMoving) {
            Shoot();
        }
    }

    void Shoot() {
        if (bulletPrefab != null && shootingPoint != null) {
            // Tạo viên đạn tại đầu nòng và xoay theo hướng súng
            Instantiate(bulletPrefab, shootingPoint.position, transform.rotation);
        }
    }
}