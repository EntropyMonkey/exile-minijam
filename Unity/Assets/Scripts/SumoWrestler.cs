using UnityEngine;
using System.Collections;
using XInputDotNetPure;

public class SumoWrestler : MonoBehaviour
{
	[SerializeField]
	float _walkSpeed = 10;
	[SerializeField]
	float _maxWalkSpeed = 20;
	[SerializeField]
	float _stepTimeout = 0.1f;

	float lastStepTimer;

	[SerializeField]
	float _chargeSpeed = 10;

	[SerializeField]
	float _chargeTimeout = 2;

	[SerializeField]
	ParticleSystem _chargeImpactParticles;

	[SerializeField]
	ParticleSystem _chargeStartParticles;

	[SerializeField]
	ParticleSystem _walkStepParticles;

	float chargeTimer;

	ChainJam.PLAYER id;
	public ChainJam.PLAYER ID { get { return id; } }
	PlayerIndex controllerId;

	SumoWrestler lastContact;

	System.Action currentState;
	System.Action currentWalkState;

	Animator animator;

	Vector3 walkDirection;
	Vector3 chargeForce;

	Vector3 startPosition;

	Transform meshTransform;

	// Use this for initialization
	void Start()
	{
		id = GameState.Instance.NextPlayerId;
		controllerId = GameState.GetControllerFromPlayerId(id);

		currentState = Idle;
		currentWalkState = Stand;

		startPosition = transform.position;

		_walkStepParticles.enableEmission = false;

		animator = GetComponentInChildren<Animator>();
		meshTransform = transform.FindChild("Mesh");
	}

	public void Restart()
	{
		transform.position = startPosition;
		rigidbody.velocity = Vector3.zero;
		rigidbody.angularVelocity = Vector3.zero;
	}

	// Update is called once per frame
	void Update()
	{
		currentWalkState();
		currentState();

		if (transform.position.sqrMagnitude > GameState.Instance.SumoRingRadius * GameState.Instance.SumoRingRadius)
		{
			GameState.Instance.PlayerPushedOut(this, lastContact);
		}

		animator.SetFloat("RunSpeed", walkDirection.magnitude);
	}

	void Idle()
	{
		chargeTimer -= Time.deltaTime;

		if (chargeTimer <= 0 && ChainJam.GetButtonJustPressed(id, ChainJam.BUTTON.A))
		{
			chargeForce = transform.forward * _chargeSpeed;
			rigidbody.AddForce(chargeForce, ForceMode.Impulse);
			chargeTimer = _chargeTimeout;
			_chargeStartParticles.Play();
			animator.SetBool("Charge", true);

			StartCoroutine(Vibrate(0.3f, 1, 1));
		}
	}

	void Stand()
	{
		_walkStepParticles.enableEmission = false;

		// play animation
		if (ChainJam.GetButtonJustPressed(id, ChainJam.BUTTON.DOWN) || ChainJam.GetButtonJustPressed(id, ChainJam.BUTTON.UP) ||
			ChainJam.GetButtonJustPressed(id, ChainJam.BUTTON.LEFT) || ChainJam.GetButtonJustPressed(id, ChainJam.BUTTON.RIGHT))
		{
			// switch to walking
			currentWalkState = Walk;
			lastStepTimer = _stepTimeout;
		}
	}

	void Walk()
	{
		_walkStepParticles.enableEmission = true;

		walkDirection = Vector3.zero;

		if (ChainJam.GetButtonJustPressed(id, ChainJam.BUTTON.A))
		{
			animator.SetBool("Charge", true);
		}

		if (!ChainJam.GetButtonPressed(id, ChainJam.BUTTON.A))
		{
			animator.SetBool("Charge", false);
		}

		//lastStepTimer -= Time.deltaTime;
		//if (lastStepTimer <= 0)
		//{
		//	lastStepTimer = _stepTimeout;
		//	StartCoroutine(VibrateStep(0.1f));
		//}

		if (ChainJam.GetButtonPressed(id, ChainJam.BUTTON.LEFT))
			walkDirection.x = -1;
		else if (ChainJam.GetButtonPressed(id, ChainJam.BUTTON.RIGHT))
			walkDirection.x = 1;

		if (ChainJam.GetButtonPressed(id, ChainJam.BUTTON.DOWN))
			walkDirection.z = -1;
		else if (ChainJam.GetButtonPressed(id, ChainJam.BUTTON.UP))
			walkDirection.z = 1;

		rigidbody.AddForce(walkDirection.normalized * _walkSpeed);
		rigidbody.velocity = Vector3.ClampMagnitude(rigidbody.velocity, _maxWalkSpeed);

		transform.LookAt(transform.position + walkDirection);

		// switch back to standing
		if (walkDirection == Vector3.zero)
		{
			lastStepTimer = 0;
			currentWalkState = Stand;
		}
	}

	IEnumerator Vibrate(float t, float lstrength, float rstrength)
	{
		while (t > 0)
		{
			t -= Time.deltaTime;
			GamePad.SetVibration(PlayerIndex.One, lstrength, rstrength);
			GamePad.SetVibration(PlayerIndex.Two, lstrength, rstrength);
			GamePad.SetVibration(PlayerIndex.Three, lstrength, rstrength);
			GamePad.SetVibration(PlayerIndex.Four, lstrength, rstrength);
			yield return new WaitForEndOfFrame();
		}
	}

	void OnCollisionEnter(Collision collision)
	{
		SumoWrestler w;
		if ((w = collision.collider.GetComponent<SumoWrestler>()) != null)
		{
			lastContact = w;
			_chargeImpactParticles.Play();
		}

		StartCoroutine(Vibrate(0.2f, 0.5f, 0.5f));
	}

	//void OnCollisionStay(Collision collision)
	//{
	//	SumoWrestler w;
	//	if ((w = collision.collider.GetComponent<SumoWrestler>()) != null)
	//	{
	//		StartCoroutine(Vibrate(0.1f, 0.1f, 0.1f));
	//	}
	//}
}
