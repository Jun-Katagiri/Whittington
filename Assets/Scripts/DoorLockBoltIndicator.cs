using UnityEngine;
using System.Collections;
public class DoorLockBoltIndicator : MonoBehaviour
{

    [SerializeField] Transform pivot;
    [SerializeField] float occupiedZ = 0f; // ボルトが使用中の位置のZ回転角
    [SerializeField] float vacantZ = 180f; // ボルトが空きの位置のZ回転角
    Quaternion baseRot;
    [SerializeField] float rotateSpeed = 360f; // 度/秒
    Coroutine co;

    void Awake()
    {
        if (pivot == null)
            pivot = transform;
        baseRot = pivot.localRotation;
    }

    public void SetOccupied(bool occupied)
    {
        float targetZ = occupied ? occupiedZ : vacantZ;

        Quaternion target =
            baseRot * Quaternion.Euler(0f, 0f, targetZ);

        if (co != null) StopCoroutine(co);
        co = StartCoroutine(RotateTo(target));
    }

    IEnumerator RotateTo(Quaternion target)
    {
        while (Quaternion.Angle(pivot.localRotation, target) > 0.1f)
        {
            pivot.localRotation =
                Quaternion.RotateTowards(
                    pivot.localRotation,
                    target,
                    rotateSpeed * Time.deltaTime
                );
            yield return null;
        }

        pivot.localRotation = target;
    }

}