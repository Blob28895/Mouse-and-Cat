using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class plusScore : MonoBehaviour
{
	
	public void setText(string text)
	{
		GetComponent<TextMeshProUGUI>().text = text;
	}
}
