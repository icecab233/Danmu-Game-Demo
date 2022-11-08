using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FullScreenPopUp : MonoBehaviour
{
    public string showText;
    public float dieTime = 3.0f;

    private TextMeshProUGUI text;

    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        text.text = showText;
        StartCoroutine(dieCoroutine());
    }

    private void Update()
    {
        text.text = showText;
    }

    IEnumerator dieCoroutine()
    {
        yield return new WaitForSeconds(dieTime);
        Destroy(gameObject);
    }
}
