using UnityEngine;
using System.Collections;
public class DoorLockBoltIndicator : MonoBehaviour
{
    [SerializeField] float occupiedZ = 0f;
    [SerializeField] float vacantZ = 180f;
    Quaternion baseRot;
    [SerializeField] float rotateSpeed = 360f;
    Coroutine co;

    void Awake()
    {
        baseRot = transform.localRotation;
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
        while (Quaternion.Angle(transform.localRotation, target) > 0.1f)
        {
            transform.localRotation =
                Quaternion.RotateTowards(
                    transform.localRotation,
                    target,
                    rotateSpeed * Time.deltaTime
                );
            yield return null;
        }

        transform.localRotation = target;
    }
}
