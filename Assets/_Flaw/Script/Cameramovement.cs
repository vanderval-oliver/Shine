using UnityEngine;
using System.Collections;

public class Cameramovement : MonoBehaviour {

	// Two global float to smooth movement and rotation.
	private float zrotation = 0.0f;
	private float xrotation = 0.0f;

	private Quaternion calibration; // The quaternion used to calibrate our vector direction.

	void Start()
	{
		// We will use calibration to ... well.. calibrate the accelerometer.
		// only work on mobile since you need an accelerometer.
		// Here we simply make the quaternion out of the 4 variable we previously saved.
		calibration = new Quaternion(PlayerPrefs.GetFloat("quadx"), PlayerPrefs.GetFloat("quady"), 
		                             PlayerPrefs.GetFloat("quadz"), PlayerPrefs.GetFloat("quadw"));
	}

	Vector3 FixAcceleration (Vector3 acceleration)
	{
		// This function will calibrate our vector acceleration with the quaternion we created in the start function.
		Vector3 fixedAcceleration = calibration * acceleration;
		return (fixedAcceleration);
	}

	void Update()
	{
		Vector3 dir; // Where will the camera will point.
		Vector3 fixeddir; // Camera direction calibrated.
		Vector3 cam_pos; // Position of the camera.

        calibration = new Quaternion(PlayerPrefs.GetFloat("quadx"), PlayerPrefs.GetFloat("quady"),
                                     PlayerPrefs.GetFloat("quadz"), PlayerPrefs.GetFloat("quadw"));

        cam_pos = transform.position;
		cam_pos.y = (Mathf.Cos(Time.time * 0.1f) * 90.0f) - 10.0f;
		transform.position = cam_pos;
		// The y position of our camera will slowly goes up and down depending on time value;

		dir = new Vector3(0.0f, 0.0f, 0.0f); //First we initialise dir to a null value.
		dir.x += Input.GetAxis("Horizontal");
		dir.y += Input.GetAxis ("Vertical");
		// Then we add Input value from horizontal and vertical axis.
		// Note that this wont work on mobile as we don't have controller like up and down key on mobile.

		dir += Input.acceleration; // Take the accelerometer input into account.
		fixeddir = FixAcceleration(dir); // Fix the direction with our Fixacceleration function.

		fixeddir.x = Mathf.Clamp(fixeddir.x * 3.0f, -1.0f, 1.0f);
		fixeddir.y = Mathf.Clamp(fixeddir.y * 3.0f, -1.0f, 1.0f);
        // The multiply these value by 3 but clamp them so they wont be higher than 1 or lower than -1.

        if (PlayerPrefs.GetInt("revCon") == 1)
            fixeddir.y = -fixeddir.y;

        zrotation = Mathf.Lerp(zrotation, -fixeddir.x / 10.0f, 0.5f);
		xrotation = Mathf.Lerp(xrotation, -fixeddir.y / 10.0f, 0.5f);
		// We then lerp these value with last frame value so it smooth it a bit.

		transform.rotation = new Quaternion(xrotation, transform.rotation.y, zrotation, transform.rotation.w);
		//apply the rotation.
	}
}
