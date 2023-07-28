using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TutorialObject : MonoBehaviour
{
	[SerializeField] private InputReaderSO _inputReader = default;

	private static bool textShownBefore = false;

	private void OnEnable()
	{
		_inputReader.KnockOverEvent += disableObject;

	}

	private void OnDisable()
	{
		_inputReader.KnockOverEvent -= disableObject;
	}

	// Start is called before the first frame update
	void Start()
	{
		if (textShownBefore)
		{
			gameObject.SetActive(false);
		}
	}

	private void disableObject()
	{
		if (!textShownBefore)
		{
			gameObject.SetActive(false);
			textShownBefore = true;
		}
	}
}
