using UnityEngine;

public class Flock : MonoBehaviour {
	float speed;
	bool turning = false;
	public float predatorAvoidDistance;
	public float predatorAvoidStrength;
	public GameObject predator;
	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start() {
		predator = FlockManager.FM.predator;
		predatorAvoidDistance = FlockManager.FM.predatorAvoidDistance;
		predatorAvoidStrength = FlockManager.FM.predatorAvoidStrength;
		speed = Random.Range(FlockManager.FM.minSpeed, FlockManager.FM.maxSpeed);
	}

	// Update is called once per frame
	// flock logic:
	// move toward average position of the group
	// align with the average heading of the group
	// avoid crowding neighbors
	// -also avoid predator
	void Update() {

		// stay in bounds
		Bounds b = new Bounds(FlockManager.FM.transform.position, FlockManager.FM.swimLimits * 2);
		if (!b.Contains(transform.position)) {
			turning = true;
		}
		else {
			turning = false;
		}

		// only turn sometimes to be more realistic
		if (turning) {
			Vector3 direction = FlockManager.FM.transform.position - transform.position;
			transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction),
													FlockManager.FM.rotationSpeed * Time.deltaTime);
		}

		// lower the < values to make updates less frequent
		else {
			if (Random.Range(0, 100) < 10) {
				speed = Random.Range(FlockManager.FM.minSpeed, FlockManager.FM.maxSpeed);
			}
			if (Random.Range(0, 100) < 40) {
				ApplyRules();
			}

		}
		this.transform.Translate(0, 0, speed * Time.deltaTime);
	}

	void ApplyRules() {
		GameObject[] gos = FlockManager.FM.allFish;

		Vector3 vcenter = Vector3.zero;
		Vector3 vavoid = Vector3.zero;
		Vector3 vpredatorAvoid = Vector3.zero;
		float gSpeed = 0.01f;
		float nDistance;
		int groupSize = 0;

		// average the group spacing and speed
		foreach (GameObject go in gos) {
			if (go != this.gameObject) {
				nDistance = Vector3.Distance(go.transform.position, this.transform.position);

				if (nDistance <= FlockManager.FM.neighbourDistance) {
					vcenter += go.transform.position;
					groupSize++;

					if (nDistance < 1.0f) {
						vavoid = vavoid + (this.transform.position - go.transform.position);
					}

					Flock otherFlock = go.GetComponent<Flock>();
					gSpeed = gSpeed + otherFlock.speed;
				}
			}
		}

		// predator logic
		if (predator != null) {
			Debug.Log("Predator active");
			float distanceToPredator = Vector3.Distance(predator.transform.position, transform.position);
			if (distanceToPredator < predatorAvoidDistance) {
				vpredatorAvoid = (this.transform.position - FlockManager.FM.predator.transform.position);
				
				float avoidanceForce = predatorAvoidStrength * (predatorAvoidDistance / (distanceToPredator + 0.001f)); 
				vpredatorAvoid = vpredatorAvoid.normalized * avoidanceForce;

				// increase speed when running
				speed = FlockManager.FM.maxSpeed * 1.2f;
			}
		}

		// average the group direction
		if (groupSize > 0) {
			vcenter = vcenter / groupSize + (FlockManager.FM.goalPos - this.transform.position);
			speed = gSpeed / groupSize;

			speed = Mathf.Clamp(speed, FlockManager.FM.minSpeed, FlockManager.FM.maxSpeed);
			Vector3 direction = vcenter + vavoid + vpredatorAvoid - this.transform.position;

			if (direction != Vector3.zero) {
				transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction),
													FlockManager.FM.rotationSpeed * Time.deltaTime);
			}
		}
	}
}
