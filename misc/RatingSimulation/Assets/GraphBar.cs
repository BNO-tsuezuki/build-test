using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class GraphBar : MonoBehaviour
{
	[SerializeField]
	Image image;

	[SerializeField]
	Text text;

	public float Value {
		set
		{
			image.rectTransform.sizeDelta = new Vector2() { x = value, y = image.rectTransform.sizeDelta.y };
		}
	}

	public string Caption
	{
		set
		{
			text.text = value;
		}
	}

	public Color Color
	{
		set
		{
			image.color = value;
		}
	}
}
