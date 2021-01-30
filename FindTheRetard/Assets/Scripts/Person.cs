using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Person : MonoBehaviour
{
	[SerializeField]
	private MeshRenderer meshRenderer;
	[SerializeField]
	private MeshFilter headMeshFilter;
	[SerializeField]
	private MeshRenderer headMeshRenderer;
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
		if ((targetPoint - transform.position).magnitude < TARGET_REACHED_DISTANCE) {
			SetRandomTarget();
		}
	}

	public void Setup(PersonAssets personAssets, Vector3 mapSize)
	{
		this.PersonAssets = personAssets;
		this.mapSize = mapSize;

		headMeshFilter.mesh = personAssets.HeadAccessory?.Mesh;

		// needs to be done like this because the array Unity returns is a copy
		Material[] materials = meshRenderer.materials;
		materials[0] = personAssets.MaskMaterial.Material;
		materials[1] = personAssets.ShirtMaterial.Material;
		materials[2] = personAssets.PantsMaterial.Material;
		meshRenderer.materials = materials;

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
		if (enabled) {
			outline.OutlineWidth = 1f;
		} else {
			outline.OutlineWidth = 0f;
		}
	}
}
