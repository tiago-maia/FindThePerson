using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using OutlineNamespace;

public class Person : MonoBehaviour
{
	[SerializeField]
	private SkinnedMeshRenderer meshRenderer;
	[SerializeField]
	private MeshFilter headMeshFilter;
	[SerializeField]
	private Outline outline;

	public PersonAssets PersonAssets;
	private Vector3 mapSize;

	private Vector3 targetPoint;
	private NavMeshAgent navMeshAgent;
	private const float TARGET_REACHED_DISTANCE = 1f;

	void Awake()
	{
		navMeshAgent = GetComponent<NavMeshAgent>();
	}

	void Update()
	{
		if (!navMeshAgent.enabled) return;

		if ((targetPoint.x - transform.position.x) < TARGET_REACHED_DISTANCE && (targetPoint.z - transform.position.z) < TARGET_REACHED_DISTANCE) {
			SetRandomTarget();
		}
	}

	void OnDrawGizmos()
	{
		Gizmos.DrawSphere(targetPoint, 0.3f);
	}

	public void Disable()
	{
		navMeshAgent.enabled = false;
	}

	public void Setup(PersonAssets personAssets, Vector3 mapSize)
	{
		this.PersonAssets = personAssets;
		this.mapSize = mapSize;

		if (personAssets != null)
		{
			headMeshFilter.mesh = personAssets.HeadAccessory?.Mesh;

			// needs to be done like this because the array Unity returns is a copy
			Material[] materials = meshRenderer.materials;
			materials[1] = personAssets.PantsMaterial.Material;
			materials[2] = personAssets.ShirtMaterial.Material;
			materials[3] = personAssets.MaskMaterial.Material;
			meshRenderer.materials = materials;
		}

		transform.position = GetRandomMapPosition();
		SetRandomTarget();
		SetHightlight(false);
	}

	private void SetRandomTarget()
	{
		targetPoint = GetRandomMapPosition();
		navMeshAgent.destination = targetPoint;
	}

	private Vector3 GetRandomMapPosition()
	{
		return new Vector3(
			Random.Range(-mapSize.x/2, mapSize.x/2),
			0,
			Random.Range(-mapSize.z/2, mapSize.z/2)
		);
	}

	public void SetHightlight(bool enabled)
	{
		outline.enabled = enabled;
	}
}
