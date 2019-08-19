using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Image image;
    public RectTransform healthScreen;
    public Vector2 offset;

    private Image[] hearts = null;

    public void RedrawHearts(int heartCount)
    {
        if (hearts != null)
            foreach (Image h in hearts)
                Destroy(h.gameObject);
        if (heartCount > 0)
            hearts = new Image[heartCount];
        for (int i = 0; i < heartCount; i++)
        {
            Vector2 ttmp;
            RectTransform rt = image.GetComponent<RectTransform>();
            ttmp = new Vector2(rt.sizeDelta.x * i + rt.sizeDelta.x/2 - (rt.sizeDelta.x * heartCount) / 2f, 0);
            Debug.Log((rt.sizeDelta.x * heartCount) / 2f);
            hearts[i] = Instantiate(image, healthScreen.transform);
            hearts[i].GetComponent<RectTransform>().anchoredPosition = ttmp + offset;
        }
    }
}
