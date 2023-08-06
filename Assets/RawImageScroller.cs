using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RawImageScroller : MonoBehaviour
{
    
    private RawImage _rawImage;

    [Tooltip("Speed at which the image will scroll. Values should be chosen between 0 and 100. A value like 101 is equivalent to a value of 1.")]
    public float scrollSpeed;
    // Start is called before the first frame update
    void Start()
    {
        _rawImage = GetComponent<RawImage>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _rawImage.uvRect = new Rect(_rawImage.uvRect.x + (scrollSpeed / 100f), _rawImage.uvRect.y, _rawImage.uvRect.width, _rawImage.uvRect.height);
    }
}
