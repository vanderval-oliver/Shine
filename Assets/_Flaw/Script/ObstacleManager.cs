using UnityEngine;
using System.Collections;

public class ObstacleManager : MonoBehaviour
{	

	// This script will take care of the city generation. It will generate the building and the cars once and will
	// then move them accordingly to the player position.
	// The city has a particular pattern as there is always 3 rows of 6 building ahead of the player.
	// One of tow row is shifted in the x axis to block the player point of view.
	// Between each row is a line of car who will all go in the same direction depending on there z position.

	public GameObject player;

    public GameObject[] buildingList;

	public GameObject car;
    public float carSpeed = 10f;

	// all these gameobject are pretty self-explanatory.
	

	private GameObject[] buildingIns; // The array where all the building gameobject will be stored.
	private GameObject[] carIns; // The array where all the cars will be stored.
	private int Building_Number = 18; // Total number of Building. Changing it wont work.
	private int Car_Number = 72; // Total number of cars. Same as above.
	private int shift = 0; // Global int who will help us to know whether we need to shift the position of our building

	int[] GetRandomArray(int length)
	{
		// This function will return a integer array of Random Integer between 0 and 5 because we have 6 type of Buildings.
		// This function also make sur that there wont be the same number twice in the array.

		int[] style = new int[length]; // the array.
		int	i = -1; // the counter.

		while (++i < length)
			style[i] = i;

        i = -1;
        while (++i < length)
        {
            int random = (int)Mathf.Floor(Random.Range(0.0f, length - 0.01f));

            // swap
            int tmp = style[random];
            style[random] = style[i];
            style[i] = tmp;
        }
		return (style);
	}

	void SpawnBuilding()
	{
        buildingIns = new GameObject[Building_Number]; // The array where we will store our Building GameObject.
		int[] style; // The array that will decide the type of building in each row.
		int i = 0; // Global counter of our building.
		int	j = 0; // row counter.
		int	x = 0; // building type.

		Vector3 pos = new Vector3(-125.0f, 0.0f, 150.0f);
		// As this function will only be called once in the begging of the game, we can predict where we want our
		// buildings to be placed.

		while (i < Building_Number)
		{
			j = i - 1;
			x = 0;
			shift++;
			style = GetRandomArray(6); // Get a random set of buildings.
			while (++j < i + 6)
			{
				// Instantiate all the buildings in a row.
                buildingIns[j] = Instantiate(buildingList[style[j % 6]], pos, Quaternion.identity) as GameObject;
                // Randomly rotate the building
                buildingIns[j].transform.eulerAngles = new Vector3((Random.value > 0.5f ? 180f : 0f), (Random.value > 0.5f ? 180f : 0f), 0.0f);
				pos.x += 50.0f;
				x++;
			}
			pos.x = -125.0f;
			pos.x += (shift % 2 == 0 ? 0.0f : -25.0f); // Depending is shift is an odd number, the position in x of the
			// next row will change.
			pos.z += 75.0f;
			i += 6; // go to the next row.
		}
	}

	void SpawnCar()
	{

		// This function is basically the same as SpawnBuilding();
		carIns = new GameObject[Car_Number];
		int	i = 0;
		int j = 0;
		Vector3 pos = new Vector3(-200.0f, 0.0f, 187.5f);

		while (i < Car_Number)
		{
			j = i - 1;
            //pos.y = Random.Range(-100.0f, 0.0f);
            //Between each row of building, instantiate two line of cars.
            int[] carY = GetRandomArray(24);

            while (++j < i + 24)
            {
                Vector3 carPos = pos;
                carPos.y += (carY[j % 24] - 12) * 5f;
                carIns[j] = Instantiate(car, carPos, Quaternion.identity) as GameObject; // instantiate the car.
                pos.x += 15.0f;               
			}
			pos.z += 75.0f; // next row.
			pos.x = -200.0f;
			i += 24;
		}
	}

	void ManageBuilding()
	{
		int i = 0; //Global Counter 
		int	j = 0; // row counter
        int[] style; // array to make our row "random" (building won't have the same place as before).
		Vector3 newCoord = player.transform.position; //position of first building created.

		//The coordinate will be modified.
		//The x coordinate will be put left of the player (because we're placing our buildings starting by the left)
		//The z coordinate is rounded down to make it more "tiled" and put 175f ahead.
		//The y coordinate is rouned down for the same reason as above.
		newCoord.x = (int)newCoord.x - (((int)newCoord.x % 50) + 125.0f);
		newCoord.z = Mathf.Floor(newCoord.z) + 175.0f;
		newCoord.y = Mathf.Floor(newCoord.y);

		//We will go through every row to see if our player isn't ahead of them.
		//If it is, we need to update the position of the row.
		while (i < Building_Number)
		{
			if (buildingIns[i].transform.position.z < player.transform.position.z - 50.0f)
			{
				j = -1;
				newCoord.x += (shift % 2 == 0 ? 0.0f : -25.0f); 
				// Depending is shift is an odd number, the position in x of the row will change.
				shift++;
				style = GetRandomArray(6);
				// Get randomly generated building style.
				while (++j < 6)
				{
                    buildingIns[i + style[j]].transform.position = newCoord;
					// Then again random rotation
					if (Random.value > 0.5f)
                        buildingIns[i + style[j]].transform.eulerAngles = new Vector3(0.0f, 180.0f, 0.0f);
					else
                        buildingIns[i + style[j]].transform.eulerAngles = new Vector3(0.0f, 0.0f, 0.0f);
					newCoord.x += 50.0f;
				}
				return ;
			}
			i += 6;
		}
	}

	void ManageCar()
	{
		//Same pattern as ManageBuilding.
		//It has some differences considering we manage two lines of car in one row but it's still really similar.

		int	i = 0;
		int	j = 0;
		Vector3 newCoord = player.transform.position;
		Vector3 pos;

		newCoord.x = (int)newCoord.x - (((int)newCoord.x % 50) + 200.0f);
		newCoord.z = Mathf.Floor(newCoord.z) + 175.0f;
		newCoord.y = Mathf.Floor(newCoord.y);
		while (i < Car_Number)
		{
			j = i - 1;

            int[] carY = GetRandomArray(24);
            while (++j < i + 24)
			{
				pos = carIns[j].transform.position;
				if ((int)j % 2 == 0)
					pos.x += carSpeed * Time.deltaTime;
				else
					pos.x -= carSpeed * Time.deltaTime;
                carIns[j].transform.position = pos;
			}
			if (carIns[i].transform.position.z < player.transform.position.z - 50.0f)
			{
				j = i - 1;
				while (++j < i + 24)
				{
                    Vector3 carPos = newCoord;
                    carPos.y += (carY[j % 24] - 12) * 5f;
                    carIns[j].transform.position = carPos;
					newCoord.x += 15.0f;
				}
			}
			i += 24;
		}
		
	}

	void Awake()
	{
		Time.timeScale = 1.0f; // make the speed of the game normal.
		RenderSettings.fogEndDistance = 0.0f; // Make the fog really thick (to make a cool transition).

		//Spawn the Building and the cars.
		SpawnBuilding();
		SpawnCar();
	}
	
	void Update()
	{
		float fog = RenderSettings.fogEndDistance;
		// At the beginning of the game, the fog will be really thick but will become normal in a second. Here's how.
		if (fog < 150.0f) // the fog is still too thick
		{
			fog += 150.0f * Time.deltaTime; //add 150f in a second.
			RenderSettings.fogEndDistance = fog; // apply fog;
		}
		// Performance saver : One frame out of two, we will update the building position, the other frame we
		// will update the car positions.
		if ((int)Time.frameCount % 2 == 0)
			ManageBuilding();
		else
			ManageCar();
	}
}