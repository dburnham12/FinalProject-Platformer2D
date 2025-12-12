
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;

public class KeyUI : MonoBehaviour
{
    public Image keyImage;
    private List<Image> imageList;
    private float uiPosition = 0;
    private float keyHeight = 30;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        imageList = new List<Image>();
    }

    public void AddKey(Color color)
    {
        Image keyObject = Instantiate(keyImage, transform);
        //keyObject.GetComponent<RectTransform>().transform.SetParent(transform.GetComponent<RectTransform>());
        keyObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(0.0f, uiPosition);
        keyObject.color = color;
        uiPosition -= keyHeight;
        imageList.Add(keyObject);
    }

    public void RemoveKey(Color color)
    {
        Image keyToRemove = imageList.Find(x => x.color == color);
        imageList.Remove(keyToRemove);
        Destroy(keyToRemove.gameObject);
        uiPosition += keyHeight;
        float newUIPosition = 0;
        for (int i = 0; i < imageList.Count; i++)
        {
            imageList[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(0.0f, newUIPosition);
            newUIPosition -= keyHeight;
        }
    }
}
