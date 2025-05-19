using UnityEngine;

public class FlockManager : MonoBehaviour {
	public static FlockManager FM;
	public GameObject fishPrefab;
	public GameObject predatorPrefab;
	public int numFish = 20;
	public GameObject[] allFish;
	public Vector3 swimLimits = new Vector3(5, 5, 5);
	public Vector3 goalPos = Vector3.zero;

	[Header("Fish Settings")]
	[Range(0.0f, 5f)]
	public float minSpeed = 1.0f;
	[Range(0.0f, 5f)]
	public float maxSpeed = 3.0f;
	[Range(1f, 10f)]
	public float neighbourDistance = 3;
	[Range(1f, 10f)]
	public float rotationSpeed = 1;

	[Header("Predator Settings")]
	[Range(0.0f, 10f)]
	public float predatorSpeed = 2.0f;
	public float predatorAvoidDistance = 5.0f;
	public float predatorAvoidStrength = 1.5f;

	public GameObject predator;

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start() {

		// instantiate the predator and fish
		Vector3 predatorPos = this.transform.position + new Vector3(Random.Range(-swimLimits.x, swimLimits.x),
																	Random.Range(-swimLimits.y, swimLimits.y),
																	Random.Range(-swimLimits.z, swimLimits.z));
		predator = Instantiate(predatorPrefab, predatorPos, Quaternion.identity);
		
		allFish = new GameObject[numFish];
		for (int i = 0; i < numFish; i++) {
			Vector3 pos = this.transform.position + new Vector3(Random.Range(-swimLimits.x, swimLimits.x),
															   Random.Range(-swimLimits.y, swimLimits.y),
																Random.Range(-swimLimits.z, swimLimits.z));
			allFish[i] = Instantiate(fishPrefab, pos, Quaternion.identity);
		}
		
		FM = this;
		goalPos = this.transform.position;
	}

	// Update is called once per frame
	void Update() {

		// change flock goal position every 1% of the time
		if (Random.Range(0, 100) < 1) {
			goalPos = this.transform.position + new Vector3(Random.Range(-swimLimits.x, swimLimits.x),
														Random.Range(-swimLimits.y, swimLimits.y),
														Random.Range(-swimLimits.z, swimLimits.z));
		}
	}
}
