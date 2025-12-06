
using System.Collections.Generic;
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

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddKey(Color color)
    {
        Image keyObject = Instantiate(keyImage, new Vector3(transform.position.x, transform.position.y + uiPosition, transform.position.z), Quaternion.identity);
        keyObject.transform.SetParent(transform);
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
            imageList[i].transform.position = new Vector3(transform.position.x, transform.position.y + newUIPosition, transform.position.z);
            newUIPosition -= keyHeight;
        }
    }
}
