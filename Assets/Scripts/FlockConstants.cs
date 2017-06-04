using UnityEngine;
using System.Collections;

public class FlockConstants : MonoBehaviour {

	static public bool cohesion_On;
	static public float cohesion_Strength = 1;

	static public bool separation_On;
	static public float separation_Strength = 1;

	static public float avoidance_Radius;

	static public bool alignment_On;
	static public float alignment_Strength = 1;

    static public bool queueing_On;
    static public float queueing_Strength = 1;

	static public bool cohese_Mouse;
	static public float mouse_Cohesion_Strength = 1;

	static public bool avoid_Mouse;
	static public float mouse_Separation_Strength = 1;
}
