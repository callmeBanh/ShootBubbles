using UnityEngine;
using System.Collections.Generic;

public class GunController : MonoBehaviour
{
    [Header("Shooting Settings")]
    public GameObject bulletPrefab;
    public Transform shootingPoint;
    public float maxForce = 20f;
    public float forceMultiplier = 5f;

    [Header("Rotation Adjustment")]
    [Tooltip("Với nòng súng hướng lên trên, hãy thử nhập -90")]
    public float angleOffset = -90f; 

    private Vector2 startPos;
    private bool isDragging = false;

    void Start()
    {
        
    }

    void Update() {
        HandleDragShoot();
    }

    void HandleDragShoot() {
        // 1. Khi bắt đầu nhấn chuột
        if (Input.GetMouseButtonDown(0)) {
            startPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            isDragging = true;
            
        }

        // 2. Khi đang giữ và kéo chuột
        if (isDragging && Input.GetMouseButton(0)) {
            Vector2 currentPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            
            // Vector lực bắn (kéo ngược về sau để bắn về phía trước)
            Vector2 shotDirection = startPos - currentPos; 

            if (shotDirection.magnitude > 0.1f) {
                // Tính góc dựa trên vector lực
                float angle = Mathf.Atan2(shotDirection.y, shotDirection.x) * Mathf.Rad2Deg;
                
                // Xoay súng (cộng thêm Offset để nòng súng nhìn đúng hướng)
                transform.rotation = Quaternion.Euler(0, 0, angle + angleOffset);
                
                // Tính toán lực thực tế sau khi giới hạn (Clamp)
                Vector2 finalForce = shotDirection * forceMultiplier;
                if (finalForce.magnitude > maxForce) finalForce = finalForce.normalized * maxForce;
                
            }
        }

        // 3. Khi thả chuột để bắn
        if (Input.GetMouseButtonUp(0) && isDragging) {
            Vector2 endPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 finalDirection = startPos - endPos;
            Vector2 finalForce = finalDirection * forceMultiplier;

            if (finalForce.magnitude > maxForce) finalForce = finalForce.normalized * maxForce;

            Shoot(finalForce);
            
            isDragging = false;
        }
    }


    void Shoot(Vector2 force) {
        if (bulletPrefab != null && shootingPoint != null) {
            // Tạo viên đạn và xoay nó theo hướng nòng súng hiện tại
            GameObject bullet = Instantiate(bulletPrefab, shootingPoint.position, transform.rotation);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            
            if (rb != null) {
                // Đảm bảo viên đạn có Gravity Scale = 1 trong Inspector để khớp với đường vẽ
                rb.AddForce(force, ForceMode2D.Impulse);
            }
        }
    }
}