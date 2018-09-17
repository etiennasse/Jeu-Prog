using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour {

    [SerializeField] public GameObject prefab;
    [SerializeField] GameObject spawnPoint;
    Image buttonImage;

    bool isCoolingDown = false;
    float coolDownTimer = 0f;
    public float coolDownTime = 2f;

    // Use this for initialization
    void Start () {
        buttonImage = GetComponentInChildren<Image>();
	}
	
	// Update is called once per frame
	void Update () {
        if(isCoolingDown)
        {
            UpdateTimer();
        }
    }

    public void UpdateTimer()
    {
        coolDownTimer += Time.deltaTime;
        CheckIfTimerIsOver();
    }

    public void CheckIfTimerIsOver()
    {
        if(coolDownTimer >= coolDownTime)
        {
            ResetTimer();
        }
    }

    public void ResetTimer()
    {
        coolDownTimer = 0;
        isCoolingDown = false;
        buttonImage.color = Color.white;
    }

    public void StartTimer()
    {
        isCoolingDown = true;
        buttonImage.color = Color.grey;
    }

    public void spawn()
    {
        if (!isCoolingDown)
        {
            Quaternion rot = Quaternion.Euler(0, 90, 0);
            Instantiate(prefab, spawnPoint.transform.position, rot);
            StartTimer();
        }
    }
}
