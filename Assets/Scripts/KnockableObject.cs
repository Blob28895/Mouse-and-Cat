using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class KnockableObject : MonoBehaviour
{
    [Tooltip("Material that will outline the object when it is in range")]
    [SerializeField] private Material outlineMaterial;

    [Tooltip("Object that will spawn when the player knocks this object over")]
    [SerializeField] private GameObject fallingObject;

    [Tooltip("Amount of time that it will take for this object to respawn to be able to be knocked over again")]
    [SerializeField] private float respawnTime = 20f;

	[SerializeField] private InputReaderSO _inputReader = default;

	private Material _defaultMaterial;      //Material the object started with so we can return to that material when the player isnt in range to knock it over
    private SpriteRenderer _spriteRenderer; //Sprite renderer reference to avoid having too many GetComponent's
    private bool _playerInRange;             //Used so that the update function knows if the player is close enough to knock something over


	// Start is called before the first frame update
	void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _defaultMaterial = _spriteRenderer.material;

    }

	private void KnockOver()
    {
        if (!_spriteRenderer.enabled || !_playerInRange) { return; }
		_spriteRenderer.enabled = false;
        GameObject fallingObj = Instantiate(fallingObject, gameObject.transform);
        
        StartCoroutine(Respawn());
    }

    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(respawnTime);
		_spriteRenderer.enabled = true;
    }
	private void OnTriggerStay2D(Collider2D collision)
	{
		if(!collision.gameObject.CompareTag("Player")) { return; }
		_inputReader.KnockOverEvent += KnockOver;
		_spriteRenderer.material = outlineMaterial;
        _playerInRange = true;
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (!collision.gameObject.CompareTag("Player")){ return; }
		_inputReader.KnockOverEvent -= KnockOver;
		_spriteRenderer.material = _defaultMaterial;
        _playerInRange = false;
	}
}
