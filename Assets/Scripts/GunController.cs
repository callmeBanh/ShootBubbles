using UnityEngine;

public class GunController : MonoBehaviour
{
    public float rotationSpeed = 50f;
    private float rotationInput = 0f;

    [Header("Shooting Settings")]
    public GameObject bulletPrefab; // Kéo Prefab viên đạn vào đây
    public Transform shootingPoint; // Kéo điểm đầu nòng (shootingPoint) vào đây

    public void MoveLeft() => rotationInput = 1f;
    public void MoveRight() => rotationInput = -1f;
    public void StopMoving() => rotationInput = 0f;

    void Update() {
        // Xử lý xoay súng
        float angle = transform.eulerAngles.z;
        if (angle > 180) angle -= 360;

        if ((rotationInput > 0 && angle < 60) || (rotationInput < 0 && angle > -60)) {
            transform.Rotate(Vector3.forward * rotationInput * rotationSpeed * Time.deltaTime);
        }

        // Xử lý bắn đạn khi chạm/click màn hình
        if (Input.GetMouseButtonDown(0)) {
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