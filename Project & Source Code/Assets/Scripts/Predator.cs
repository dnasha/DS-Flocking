using UnityEngine;

public class Predator : MonoBehaviour {
	private float speed;
	private bool turning = false;
	private Vector3 targetPosition;

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start() {
		speed = FlockManager.FM.predatorSpeed;
		PickNewTarget();
	}

	// Update is called once per frame

	void Update() {

		// stay in bounds
		Bounds b = new Bounds(FlockManager.FM.transform.position, FlockManager.FM.swimLimits * 2);

		if (!b.Contains(transform.position) || Vector3.Distance(transform.position, targetPosition) < 1.0f) {
			turning = true;
			PickNewTarget();
		}
		else {
			turning = false;
		}

		// similar logic to regular fish
		if (turning) {
			Vector3 directionToCenter = FlockManager.FM.transform.position - transform.position;
			transform.rotation = Quaternion.Slerp(transform.rotation,
												  Quaternion.LookRotation(directionToCenter),
												  FlockManager.FM.rotationSpeed * Time.deltaTime);
		}
		else {

			// chase random fish sometimes (OPTIONAL)
			if (FlockManager.FM.allFish.Length > 0 && Random.Range(0, 100) < 10) {
				int randomFishIndex = Random.Range(0, FlockManager.FM.allFish.Length);
				if (FlockManager.FM.allFish[randomFishIndex] != null) {
					targetPosition = FlockManager.FM.allFish[randomFishIndex].transform.position;
				}
			}

			// go to target position otherwise
			Vector3 directionToTarget = targetPosition - transform.position;
			if (directionToTarget != Vector3.zero) {
				transform.rotation = Quaternion.Slerp(transform.rotation,
													  Quaternion.LookRotation(directionToTarget),
													  FlockManager.FM.rotationSpeed * Time.deltaTime * 0.5f);
			}
		}

		// move forward
		transform.Translate(0, 0, speed * Time.deltaTime);
	}

	// get new target position
	void PickNewTarget() {
		targetPosition = FlockManager.FM.transform.position + new Vector3(
			Random.Range(-FlockManager.FM.swimLimits.x, FlockManager.FM.swimLimits.x),
			Random.Range(-FlockManager.FM.swimLimits.y, FlockManager.FM.swimLimits.y),
			Random.Range(-FlockManager.FM.swimLimits.z, FlockManager.FM.swimLimits.z)
		);
	}
}