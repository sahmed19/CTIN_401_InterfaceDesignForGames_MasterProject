using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GunDiageticUI : MonoBehaviour
{
    

    public RectTransform crosshairParent;

    public LineRenderer heartRateMonitorLineRenderer;
    public TextMeshProUGUI heartRateMonitor;
    public Image heartRateBackground;

    public FloatReference heartRate = new FloatReference(120);
    float GetHeartRate() {return 60.0f/heartRate;}

    float heartRateBlipPosition;

    float timeSinceLastBlip = 0f;
    float timeAtLastBlip = 0f;

    float timer = 0f;

    public Vector2Variable mouseInput;
    public Vector2Variable movementInput;

    public FloatVariable accuracyModifier;

    Color dangerZoneColor = new Color(1f, 0f, 0f, .5f);  
    Color normalColor = new Color(1f, 1f, 1f, .2f);

    void Update() {
        HeartRate();
        RenderHeartRateMonitor();
        CrosshairMovement();
    }

    void HeartRate() {

        timer += Time.deltaTime;
        if(timer >= GetHeartRate()) {
            Heartbeat();
        }

    }

    void RenderHeartRateMonitor() {
        float length = Mathf.Clamp01(timer * 5f);
        int integerLength = (int) (100f * length);
        heartRateMonitorLineRenderer.positionCount = integerLength;

        if(heartRate>100f) {
            heartRateBackground.color = Color.Lerp(dangerZoneColor, normalColor, Mathf.Clamp01(Mathf.Sin(Time.time * heartRate/10f)));
        } else {
            heartRateBackground.color = normalColor;
        }

        for(int i = 0; i < integerLength; i++) {

            float factor = i/100f;

            float posX = Mathf.Lerp(-75f, 150f, factor);
            heartRateMonitorLineRenderer.SetPosition(i, 
            new Vector3(posX, 
            Random.Range(-1f, 1f) + GetHeartElectricity(factor) * 50f, 
            0f));
        }

    }

    void Heartbeat() {
        heartRateBlipPosition = Random.Range(0.1f, 0.2f);
        timeSinceLastBlip = Time.time - timeAtLastBlip;
        timeAtLastBlip = Time.time;
        heartRateMonitor.text = "" + (int) (Mathf.Clamp(60f/timeSinceLastBlip, 0f, 200f));
        timer = 0f;
            
    }

    float GetHeartElectricity(float t) {
        float sin = Mathf.Sin(t * Mathf.Rad2Deg);
        float magnitude = (1-Mathf.Clamp01(5f * Mathf.Abs(heartRateBlipPosition - t)));
        return sin * magnitude;
    }

    void CrosshairMovement() {
        float targetCrosshairSize = 700f + (300f * (movementInput.sqrMagnitude + accuracyModifier.value));
        crosshairParent.sizeDelta = Vector2.Lerp(crosshairParent.sizeDelta, Vector2.one * targetCrosshairSize, 18.0f * Time.deltaTime);
    }

}
