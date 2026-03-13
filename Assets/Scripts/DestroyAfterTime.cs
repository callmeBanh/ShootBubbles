using UnityEngine;
public class DestroyAfterTime : MonoBehaviour {
    public float destroyTime = 1f; // Xóa sau 1 giây
    void Start() {
        Destroy(gameObject, destroyTime);
    }
}