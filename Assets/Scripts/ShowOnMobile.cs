using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowOnMobile : MonoBehaviour
{
    [Tooltip("Game Objects in this array will only be shown on mobile devices")]
    [SerializeField] public List<GameObject> _objectsToEnable = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject obj in _objectsToEnable)
        {
            obj.SetActive(Application.isMobilePlatform);
        }
    }
}
