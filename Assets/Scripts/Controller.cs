using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Controller : MonoBehaviour {

	public GameObject[] boidTypes;
	int currentBoidType = 0;

	public float startingNumBoids = 30;

	static public Vector2 cameraMax;
	static public Vector2 cameraMin;

	//Menu
	public Toggle showMenuToggle;

	public Image Backing;

	public Slider cohesionStrength;
	public Toggle cohesionToggle;
	public Text cohesionStrengthText;

	public Slider separationStrength;
	public Toggle separationToggle;
	public Text separationStrengthText;

	public Slider avoidanceRadius;
	public Text avoidanceRadiusText;

	public Slider alignmentStrength;
	public Toggle alignmentToggle;
	public Text alignmentStrengthText;

    public Slider queueingStrength;
    public Toggle queueingToggle;
    public Text queueingStrengthText;

	public Slider avoidMouseStrength;
	public Toggle avoidMouseToggle;
	public Text avoidMouseStrengthText;

	public Slider attractMouseStrength;
	public Toggle attractToMouseToggle;
	public Text attractMouseStrengthText;

	
	public Toggle shuffleToggle;
	float shuffleTime;
	readonly float shuffleTimeMax = 10.0f;

	// Use this for initialization
	void Start () {

		for (int i = 0; i < startingNumBoids; ++i)
			Instantiate (boidTypes [currentBoidType/*Random.Range(0,boidTypes.Length)*/], Random.insideUnitCircle, Quaternion.identity);

		cameraMax = Camera.main.ViewportToWorldPoint (Camera.main.rect.max);
		cameraMin = Camera.main.ViewportToWorldPoint (Camera.main.rect.min);

		UpdateMenu ();
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetMouseButtonDown (0))
			Instantiate (boidTypes [currentBoidType], (Vector2)Camera.main.ScreenToWorldPoint (Input.mousePosition), Quaternion.identity);

		if (Input.GetKeyDown (KeyCode.Tab))
			currentBoidType = (currentBoidType + 1) % boidTypes.Length;

		if (shuffleToggle.isOn) {
			if(shuffleTime <= 0.0f)
				Shuffle();
			else
				shuffleTime-=Time.deltaTime;
		}

	}

	void Shuffle(){

		cohesionToggle.isOn = Random.value > 0.5;
		cohesionStrength.value = Random.Range(0.01f,cohesionStrength.maxValue);
		
		separationToggle.isOn = Random.value > 0.5;
		separationStrength.value = Random.Range(0.01f,separationStrength.maxValue);
		
		avoidanceRadius.value = Random.Range(0.01f,avoidanceRadius.maxValue);
		
		alignmentToggle.isOn = Random.value > 0.5;
		alignmentStrength.value = Random.Range(0.01f,alignmentStrength.maxValue);
		
		queueingToggle.isOn = Random.value > 0.5;
		queueingStrength.value = Random.Range(0.01f,queueingStrength.maxValue);
		
		avoidMouseToggle.isOn = Random.value > 0.5;
		avoidMouseStrength.value = Random.Range(0.01f,avoidMouseStrength.maxValue);
		
		attractToMouseToggle.isOn = Random.value > 0.5;
		attractMouseStrength.value = Random.Range(0.01f,attractMouseStrength.maxValue);

		UpdateMenu ();

		shuffleTime = shuffleTimeMax;
	}

	public void UpdateMenu(){

		if (shuffleToggle.isOn)
			shuffleTime = shuffleTimeMax;

		if (!showMenuToggle.isOn) {

			Backing.enabled = false;

			cohesionStrength.gameObject.SetActive(false);
			cohesionToggle.gameObject.SetActive(false);

			separationStrength.gameObject.SetActive(false);
			separationToggle.gameObject.SetActive(false);

			avoidanceRadius.gameObject.SetActive(false);

			alignmentStrength.gameObject.SetActive(false);
			alignmentToggle.gameObject.SetActive(false);

            queueingStrength.gameObject.SetActive(false);
            queueingToggle.gameObject.SetActive(false);

			avoidMouseStrength.gameObject.SetActive(false);
			avoidMouseToggle.gameObject.SetActive(false);

			attractMouseStrength.gameObject.SetActive(false);
			attractToMouseToggle.gameObject.SetActive(false);

		} else {

			Backing.enabled = true;

			cohesionStrength.gameObject.SetActive(true);
			cohesionToggle.gameObject.SetActive(true);
			
			separationStrength.gameObject.SetActive(true);
			separationToggle.gameObject.SetActive(true);
			
			avoidanceRadius.gameObject.SetActive(true);
			
			alignmentStrength.gameObject.SetActive(true);
			alignmentToggle.gameObject.SetActive(true);

            queueingStrength.gameObject.SetActive(true);
            queueingToggle.gameObject.SetActive(true);

			avoidMouseStrength.gameObject.SetActive(true);
			avoidMouseToggle.gameObject.SetActive(true);

			attractMouseStrength.gameObject.SetActive(true);
			attractToMouseToggle.gameObject.SetActive(true);
		}

		cohesionStrength.interactable = cohesionToggle.isOn;

		separationStrength.interactable = separationToggle.isOn;
		avoidanceRadius.interactable = separationToggle.isOn;

		alignmentStrength.interactable = alignmentToggle.isOn;

        queueingStrength.interactable = queueingToggle.isOn;

		avoidMouseStrength.interactable = avoidMouseToggle.isOn;

		attractMouseStrength.interactable = attractToMouseToggle.isOn;



		cohesionStrengthText.text = "Cohesion Strength: " + cohesionStrength.value.ToString ();

		separationStrengthText.text = "Separation Strength: " + separationStrength.value.ToString ();

		avoidanceRadiusText.text = "Avoidance Radius: " + avoidanceRadius.value.ToString ();

		alignmentStrengthText.text = "Alignment Strength: " + alignmentStrength.value.ToString ();

        queueingStrengthText.text = "Queueing Strength: " + queueingStrength.value.ToString();

		avoidMouseStrengthText.text = "Mouse Avoidance: " + avoidMouseStrength.value.ToString ();

		attractMouseStrengthText.text = "Mouse Attraction: " + attractMouseStrength.value.ToString ();

		UpdateValues ();
	}

	void UpdateValues(){

		FlockConstants.cohesion_On = cohesionToggle.isOn;
		FlockConstants.cohesion_Strength = cohesionStrength.value;

		FlockConstants.separation_On = separationToggle.isOn;
		FlockConstants.separation_Strength = separationStrength.value;

		FlockConstants.avoidance_Radius = avoidanceRadius.value;

		FlockConstants.alignment_On = alignmentToggle.isOn;
		FlockConstants.alignment_Strength = alignmentStrength.value;

        FlockConstants.queueing_On = queueingToggle.isOn;
        FlockConstants.queueing_Strength = queueingStrength.value;

		FlockConstants.avoid_Mouse = avoidMouseToggle.isOn;
		FlockConstants.mouse_Separation_Strength = avoidMouseStrength.value;

		FlockConstants.cohese_Mouse = attractToMouseToggle.isOn;
		FlockConstants.mouse_Cohesion_Strength = attractMouseStrength.value;
	}
}
