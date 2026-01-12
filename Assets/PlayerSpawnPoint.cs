using UnityEngine;

public class PlayerSpawnPoint : MonoBehaviour
{
    // エディタ上で位置を視覚化するためのGizmo
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(transform.position, 0.3f);
        Gizmos.DrawRay(transform.position, transform.forward);
    }
}