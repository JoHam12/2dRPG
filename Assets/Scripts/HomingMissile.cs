using UnityEngine;
using System.Collections;

public class HomingMissile : MonoBehaviour
{
	#region PUBLIC_VARS

	[SerializeField] private Transform target;
	private Rigidbody2D rigidBody;
	[SerializeField] private float angleChangingSpeed;
	[SerializeField] private float movementSpeed;
	private float targetAngle;
	private Vector2 lastPosition;
	[SerializeField] private GameController gameController;
	[SerializeField] private ParticleSystem trailParticles, explosionParticles;

	[SerializeField] private int damage;
	#endregion

	#region UNITY_CALLBACKS

	void Start(){
		rigidBody = GetComponent<Rigidbody2D>();
		trailParticles = GetComponent<ParticleSystem>();
		trailParticles.Stop();
		explosionParticles.gameObject.SetActive(false);
		// if(gameController.GetTarget() == null){ return ; }
		// target = gameController.GetTarget();
	}

	void FixedUpdate ()
	{
		if(rigidBody.velocity == Vector2.zero){
			trailParticles.Stop();
		}
		else{
			trailParticles.Play();
		}
		Vector2 direction = (Vector2)target.position - (Vector2)transform.position;
		targetAngle = Mathf.Atan2 (direction.y, direction.x) * Mathf.Rad2Deg - 90f;
		float angle = Mathf.LerpAngle(transform.eulerAngles.z, targetAngle, angleChangingSpeed);
		transform.rotation = Quaternion.Euler (0f, 0f, angle);
		lastPosition = transform.position;
		if (Vector2.Distance (((Vector2)target.position), rigidBody.position) >= 10.0f)
		{
//			Vector2 direction = (Vector2)target.position - rigidBody.position;
//			direction.Normalize ();
//			float rotateAmount = Vector3.Cross (direction, transform.up).z;
//			rigidBody.angularVelocity = -angleChangingSpeed * rotateAmount;
			rigidBody.velocity = transform.up * movementSpeed;
		} else
		{
			rigidBody.velocity = 1 * transform.up * movementSpeed;
		}
	}

	private void OnTriggerEnter2D(Collider2D other) {
		if(other.CompareTag("Player")){
			other.GetComponent<Player>().GetHealth().HealthManager(-damage);
			explosionParticles.transform.position = other.transform.position;
			explosionParticles.gameObject.SetActive(true);
			explosionParticles.Play();
			Destroy(this.gameObject);
		}
	}

	#endregion

}