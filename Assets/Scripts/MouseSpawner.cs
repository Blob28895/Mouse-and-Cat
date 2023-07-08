using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseSpawner : MonoBehaviour
{
    [Tooltip("Gameobject that will be spawned")]
    [SerializeField] private GameObject Mouse;
    [SerializeField] private Transform spawnLocation;
    [Tooltip("Numer of seconds between mice spawning")]
    [SerializeField] private float spawningInterval = 6f;

    private float _spawnTime = 0f;

    // Update is called once per frame
    void Update()
    {
        if(Time.time > _spawnTime)
        {
            Debug.Log("making it in");
            GameObject mouse = Instantiate(Mouse/*, spawnLocation*/);
            mouse.transform.position = spawnLocation.position;
            _spawnTime =  Time.time + spawningInterval; 
        }
    }
}
