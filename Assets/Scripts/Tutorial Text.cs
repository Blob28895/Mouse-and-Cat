using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialText : MonoBehaviour
{
    [Tooltip("Amount of time that the object will exist in the scene before despawning")]
    [SerializeField] private float activeTime;

    private static bool textShownBefore = false;

    // Start is called before the first frame update
    void Start()
    {
        if (textShownBefore)
        {
            gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(Time.time >= activeTime)
        {
            gameObject.SetActive(false);
        }
    }
}
