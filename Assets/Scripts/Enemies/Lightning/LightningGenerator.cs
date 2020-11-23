using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningGenerator : MonoBehaviour
{
    private Transform next = null;
    
    public int nextIndex = -1;
    public Transform firePoint;
    public Transform lightning;
    public Transform pointer;

    [SerializeField] private Animator lightningAnim = null;
    [SerializeField] private Animator lightningFloorEffect = null;

    private float distanceFromTarget;

    private void Start()
    {
        lightning.gameObject.SetActive(false);
    }

    public void shoot(bool shooting)
    {
        lightning.gameObject.SetActive(shooting);
    }

    public void playAnimations(bool play)
    {
        lightningAnim.SetBool("shooting", play);
        lightningFloorEffect.SetBool("shooting", play);
    }

    private float getDistanceFromTarget(Transform target)
    {
        Vector2 targetPos = target.position;
        Vector2 currentPos = firePoint.transform.position;
        Vector2 lookDir = targetPos - currentPos;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
        Quaternion rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        firePoint.transform.localRotation = rotation;
        //lightning.transform.localRotation = rotation;
        RaycastHit2D hitInfo = Physics2D.Raycast(pointer.position, pointer.up);
        if (hitInfo && hitInfo.collider.CompareTag("LightningPoint"))
        {
            return hitInfo.distance;
        }
        return Vector2.Distance(currentPos, targetPos);
    }

    public void setNext(Transform next)
    {
        this.next = next;
        distanceFromTarget = getDistanceFromTarget(next);
        Debug.Log(distanceFromTarget);
        lightning.localScale = new Vector2(1, distanceFromTarget);
    }
}
