using UnityEngine;
using System.Collections;

public class Carmovement : MonoBehaviour {
	
	void Awake () 
	{
		// When we create a car. We need it to look in a particular direction.
		// ...even tough it kinda look the same with our model x)

		if ((int)transform.position.z % 2 == 0)
			transform.eulerAngles = new Vector3(0.0f, 90.0f, 0.0f);
		else
			transform.eulerAngles = new Vector3(0.0f, 270.0f, 0.0f);
		// So depending on the z position of the car, it will look to the right or to the left.
	}
}
