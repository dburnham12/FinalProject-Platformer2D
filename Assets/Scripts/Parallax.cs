using UnityEngine;

public class Parallax : MonoBehaviour
{
    private float length, startpos;
    [SerializeField] private new GameObject camera;
    [SerializeField] private float parallaxEffect;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startpos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    // Update is called once per frame
    void Update()
    {
        float temp = camera.transform.position.x * (1 - parallaxEffect);
        float distance = camera.transform.position.x * parallaxEffect;

        transform.position = new Vector3(startpos + distance, transform.position.y, transform.position.z);

        if (temp > startpos + length)
            startpos += length * 2;
        else if (temp < startpos - length)
            startpos -= length * 2;
    }
}
