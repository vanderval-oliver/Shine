using UnityEngine;
using System.Collections;

public class DestroyTimer : MonoBehaviour {

	public float counter = 10.0f;

	void Awake()
	{
		// REALLY basic function to destroy a gameobject after a given time.
		// Here 10 seconds. (10.0f)
		Destroy(this.gameObject, counter);
	}
}
