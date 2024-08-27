using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Light))]
public class LightBlinker : MonoBehaviour
{
    Light m_target;
    Light target { get
        {
            if(m_target == null) m_target = GetComponent<Light>();
            return m_target;
        } }
    const float minBlinkCooldown = 2.0f, maxBlinkCooldown = 4.0f;
    float nextBlinkTime, counter = 0.0f;
    private void Start()
    {
        nextBlinkTime = Random.Range(minBlinkCooldown, maxBlinkCooldown);
    }
    private void Update()
    {
        counter += Time.deltaTime;
        if(counter >= nextBlinkTime)
        {
            counter -= nextBlinkTime;
            StartCoroutine(Blink());
            nextBlinkTime = Random.Range(minBlinkCooldown, maxBlinkCooldown);
        }
    }
    IEnumerator Blink()
    {
        int blinkCount = Random.Range(1, 3);
        float blinkTime = Random.Range(0.05f, 0.2f);
        for(int i = 0; i < blinkCount; i++)
        {
            target.enabled = false;
            yield return new WaitForSeconds(blinkTime);
            target.enabled = true;
            yield return new WaitForSeconds(blinkTime);
        }
    }
}
