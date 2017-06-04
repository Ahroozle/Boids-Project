using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Boid : MonoBehaviour {

	//the radius within which another boid has to be inside of to be considered a flockmate
	public float flockRadius = 2;

	public float maxSpeed = 5;

	public float avoidanceModifier = 1;

    public float queueGapSizeModifier = 1;
    public float queueingRadius = 1;
    public float queueingHalfFOV = 0.5f;

	public virtual float avoidanceRadius{ get { return FlockConstants.avoidance_Radius * avoidanceModifier; } }

	Vector2 averageForward;
	Vector2 averagePosition;

	List<Boid> flock;

	// Use this for initialization
	void Start () {
		flock = new List<Boid> ();

		GetComponent<Rigidbody2D> ().velocity = Random.insideUnitCircle;
	}
	
	// Update is called once per frame
	void Update () {

		CalculateAveragesAndGetFlock ();

		if (FlockConstants.alignment_On && FlockConstants.alignment_Strength != 0)
			Align ();

		if (FlockConstants.cohesion_On && FlockConstants.cohesion_Strength != 0)
			Cohese ();

		if (FlockConstants.separation_On && FlockConstants.separation_Strength != 0)
			Separate ();

        if (FlockConstants.queueing_On && FlockConstants.queueing_Strength != 0)
            Queue();

		if (FlockConstants.avoid_Mouse)
			AvoidMouse ();

		if (FlockConstants.cohese_Mouse)
			CoheseMouse ();

		if (GetComponent<Rigidbody2D> ().velocity.magnitude > maxSpeed)
			GetComponent<Rigidbody2D> ().velocity = GetComponent<Rigidbody2D> ().velocity.normalized * maxSpeed;

		transform.up = GetComponent<Rigidbody2D> ().velocity;

		LoopBounds ();
	}

	void LoopBounds(){

		if (!!GetComponent<Renderer> ().isVisible) {
			if (transform.position.x > Controller.cameraMax.x && GetComponent<Rigidbody2D> ().velocity.x > 0) {
				
				transform.position = new Vector2 (Controller.cameraMin.x - (transform.position.x - Controller.cameraMax.x),
				                                  transform.position.y);
				
			} else if (transform.position.x < Controller.cameraMin.x && GetComponent<Rigidbody2D> ().velocity.x < 0) {
				
				transform.position = new Vector2 (Controller.cameraMax.x + (Controller.cameraMin.x - transform.position.x),
				                                  transform.position.y);
			}
			
			if (transform.position.y > Controller.cameraMax.y && GetComponent<Rigidbody2D> ().velocity.y > 0) {

				transform.position = new Vector2 (transform.position.x,
				                                  Controller.cameraMin.y - (transform.position.y - Controller.cameraMax.y));
				
			} else if (transform.position.y < Controller.cameraMin.y && GetComponent<Rigidbody2D> ().velocity.y < 0) {
				
				transform.position = new Vector2 (transform.position.x,
				                                  Controller.cameraMax.y + (Controller.cameraMin.y - transform.position.y));
			}
		}
	}

	void CalculateAveragesAndGetFlock(){

		flock.Clear ();
		averagePosition = averageForward = Vector3.zero;

		Boid[] allBoids = GameObject.FindObjectsOfType<Boid> ();

		Vector2 toBoid;
		foreach (Boid currentBoid in allBoids) {

			if(currentBoid != this){

				toBoid = currentBoid.transform.position - transform.position;

				if(toBoid.magnitude < flockRadius){

					flock.Add(currentBoid);
					averageForward += currentBoid.GetComponent<Rigidbody2D>().velocity.normalized;
					averagePosition += toBoid;
				}
			}
		}

		averageForward.Normalize ();

		averagePosition /= flock.Count;
		averagePosition += (Vector2)transform.position;
	}

	void CoheseMouse(){

		Vector2 urge = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - (Vector2)transform.position;

		if (urge.magnitude < flockRadius) {

			float urgeMagnitude = urge.magnitude / maxSpeed;
			
			urge = urge.normalized * urgeMagnitude;
			
			urge *= FlockConstants.mouse_Cohesion_Strength;
			
			if (urge.magnitude > Mathf.Epsilon)
				GetComponent<Rigidbody2D> ().velocity += urge;
		}
	}

	void AvoidMouse(){
		Vector2 urge = (Vector2)transform.position - (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
		
		if (urge.magnitude < flockRadius) {
			
			float urgeMagnitude = urge.magnitude / maxSpeed;
			
			urge = urge.normalized * urgeMagnitude;
			
			urge *= FlockConstants.mouse_Separation_Strength;
			
			if (urge.magnitude > Mathf.Epsilon)
				GetComponent<Rigidbody2D> ().velocity += urge;
		}
	}

    void Queue(){

        Vector2 sumUrges = Vector3.zero;

        foreach(Boid other in flock){

            if (other != this){

                Vector2 targetPos = other.transform.position - (other.transform.forward * queueGapSizeModifier);

                Vector2 toTarg = targetPos - (Vector2)transform.position;

                if (toTarg.magnitude < queueingRadius && Vector2.Dot(toTarg.normalized, GetComponent<Rigidbody2D>().velocity.normalized) > 1 - queueingHalfFOV){

                    if (toTarg.magnitude > 1)
                        toTarg.Normalize();

                    toTarg *= FlockConstants.queueing_Strength;

                    sumUrges += toTarg;
                }
            }
        }

        if (sumUrges.magnitude > 1)
            sumUrges.Normalize();

        GetComponent<Rigidbody2D>().velocity += sumUrges * FlockConstants.queueing_Strength;

    }

	void Separate(){

		if (flock.Count > 0) {
			Vector2 sumUrges = Vector2.zero;

			foreach (Boid currentBoid in flock) {

				Vector2 currentUrge = transform.position - currentBoid.transform.position;

				float safeDistance = avoidanceRadius;
				if (currentUrge.magnitude < safeDistance) {

					if(currentUrge.magnitude > 1)
						currentUrge.Normalize ();

					currentUrge *= (safeDistance - currentUrge.magnitude) / safeDistance;

					sumUrges += currentUrge;
				}
			}

			if (sumUrges.magnitude > 1)
				sumUrges.Normalize ();

			sumUrges *= FlockConstants.separation_Strength;

			GetComponent<Rigidbody2D> ().velocity += sumUrges;
		}
	}

	void Cohese(){

		if (averagePosition != (Vector2)transform.position) {

			Vector2 urge = averagePosition - (Vector2)transform.position;

			float urgeMagnitude = urge.magnitude / maxSpeed;

			urge = urge.normalized * urgeMagnitude;

			urge *= FlockConstants.cohesion_Strength;

			if(urge.magnitude > Mathf.Epsilon)
				GetComponent<Rigidbody2D>().velocity += urge;
		}
	}

	void Align(){

		Vector2 urge = averageForward / maxSpeed;

		if (urge.magnitude > 1)
			urge.Normalize ();

		urge *= FlockConstants.alignment_Strength;

		GetComponent<Rigidbody2D>().velocity += urge;
	}
}
