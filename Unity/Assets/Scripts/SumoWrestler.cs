using UnityEngine;
using System.Collections;

public class SumoWrestler : MonoBehaviour
{
	[SerializeField]
	float _walkSpeed = 10;
	[SerializeField]
	float _maxWalkSpeed = 20;

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

	SumoWrestler lastContact;

	System.Action currentState;
	System.Action currentWalkState;

	Animator animator;

	Vector3 walkDirection;
	Vector3 chargeForce;

	Vector3 startPosition;

	// Use this for initialization
	void Start()
	{
		id = GameState.Instance.NextPlayerId;
		currentState = Idle;
		currentWalkState = Stand;

		startPosition = transform.position;

		_walkStepParticles.enableEmission = false;
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
		}
		else if (ChainJam.GetButtonJustPressed(id, ChainJam.BUTTON.B))
		{
			currentState = Jump;
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
		}
	}

	void Walk()
	{
		_walkStepParticles.enableEmission = true;

		walkDirection = Vector3.zero;

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
			currentWalkState = Stand;
	}

	void Jump()
	{

	}

	void OnCollisionEnter(Collision collision)
	{
		SumoWrestler w;
		if ((w = collision.collider.GetComponent<SumoWrestler>()) != null)
		{
			lastContact = w;
			_chargeImpactParticles.Play();
		}
	}
}
