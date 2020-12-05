using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrackEffectManager : MonoBehaviour
{
    private float waitTime = 0f;
    [SerializeField]
    private Sprite[] crackSprites = null;
    [SerializeField]
    private SpriteRenderer spriteRenderer = null;

    private Coroutine co;

    public void startEffect(float maxTime)
    {
        Debug.Log("started");
        waitTime = maxTime / crackSprites.Length;
        spriteRenderer.sprite = null;
        co = StartCoroutine("setSprite");
    }

    public void stopCoroutine()
    {
        if(co != null)
        {
            StopCoroutine(co);
        }
    }

    IEnumerator setSprite()
    {
        for(int i = 0; i < crackSprites.Length; i++)
        {
            yield return new WaitForSeconds(waitTime);
            spriteRenderer.sprite = crackSprites[i];
        }
    }
}
