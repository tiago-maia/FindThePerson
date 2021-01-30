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

	private Material maskMaterial;
	private Material shirtMaterial;
	private Material pantsMaterial;

	private Vector3 targetPoint;
	private NavMeshAgent navMeshAgent;
    private const float TARGET_REACHED_DISTANCE = 1f;

	void Start()
	{
		navMeshAgent = GetComponent<NavMeshAgent>();
        SetRandomTarget();
	}

    void Update()
    {
        if ((targetPoint - transform.position).magnitude < TARGET_REACHED_DISTANCE) {
				SetRandomTarget();
			}
    }

	public void Setup(Material maskMaterial, Material shirtMaterial, Material pantsMaterial, Mesh headAccessory)
	{
		meshRenderer.materials[0] = maskMaterial;
		meshRenderer.materials[1] = shirtMaterial;
		meshRenderer.materials[2] = pantsMaterial;
		headMeshFilter.mesh = headAccessory;
	}

	private void SetRandomTarget()
	{
		targetPoint = new Vector3(Random.Range(-30f, 30f), 0, Random.Range(-30f, 30f));
		navMeshAgent.destination = targetPoint;
	}
}
