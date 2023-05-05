using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireflyParticleSystem : MonoBehaviour
{
	public int fireflyCount = 100;
	public float minSpeed = 0.5f;
	public float maxSpeed = 2.0f;
	public Vector3 spawnRange = new Vector3(10, 5, 10);
	public GameObject fireflyPrefab;

	private List<Firefly> fireflies;

	private void Start()
	{
		fireflies = new List<Firefly>();
		for (int i = 0; i < fireflyCount; i++)
		{
			SpawnFirefly();
		}
	}

	private void Update()
	{
		foreach (Firefly firefly in fireflies)
		{
			firefly.Move();
		}
	}

	private void SpawnFirefly()
	{
		Vector3 spawnPosition = new Vector3(
			Random.Range(-spawnRange.x / 2, spawnRange.x / 2),
			Random.Range(-spawnRange.y / 2, spawnRange.y / 2),
			Random.Range(-spawnRange.z / 2, spawnRange.z / 2)
		);
		GameObject fireflyObject = Instantiate(fireflyPrefab, transform.position + spawnPosition, Quaternion.identity, transform);
		Firefly firefly = new Firefly(fireflyObject.transform, minSpeed, maxSpeed);
		fireflies.Add(firefly);
	}

	private class Firefly
	{
		private Transform transform;
		private float speed;
		private Vector3 targetPosition;

		public Firefly(Transform transform, float minSpeed, float maxSpeed)
		{
			this.transform = transform;
			this.speed = Random.Range(minSpeed, maxSpeed);
			SetNewTargetPosition();
		}

		public void Move()
		{
			transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
			float distanceToTarget = Vector3.Distance(transform.position, targetPosition);

			if (distanceToTarget < 0.1f)
			{
				SetNewTargetPosition();
			}
		}

		private void SetNewTargetPosition()
		{
			targetPosition = transform.position + new Vector3(
				Random.Range(-1f, 1f),
				Random.Range(-1f, 1f),
				Random.Range(-1f, 1f)
			).normalized * Random.Range(1f, 5f);
		}
	}
}
