using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ComboCanvas : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] float lifeTime = 0.8f;
    float accLifeTime;

    private void Start()
    {
    }
    public void SetCombo(int value)
    {
        text.text = "x" + value.ToString();
    }

    private void Update()
    {
        accLifeTime += Time.deltaTime;
        if (accLifeTime > lifeTime)
        {
            Destroy(this.gameObject);
        }
    }
}
