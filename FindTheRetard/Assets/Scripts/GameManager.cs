using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
	// Need to randomly create a number of persons ( range ? 20 - 30 ? Increase with difficulty ? ) and to randomize their clothes
	// Create instance of Correct Person

	[SerializeField] Assets assets;
	[SerializeField] Transform peopleParent;

	[SerializeField] Person personPrefab;
	[SerializeField] Camera cameraMain;

	List<Person> people = new List<Person>();

	readonly Vector3 mapSize = new Vector3(24, 0, 24);
	Person targetPerson;
	Person lastHoveredPerson;

	void Start()
	{
		CreateNewGame(200);
	}

	void Update()
	{
		CheckForMousePositionAndClick();
	}

	private void CheckForMousePositionAndClick()
	{
		RaycastHit raycastHit;
		Ray ray = cameraMain.ScreenPointToRay(Input.mousePosition);

		if (Physics.Raycast(ray, out raycastHit))
		{
			Person person = raycastHit.transform.GetComponent<Person>();
			if (person != null)
			{
				person.SetHightlight(true);
			}

			if (person != lastHoveredPerson)
			{
				if (lastHoveredPerson != null) {
					lastHoveredPerson.SetHightlight(false);
				}

				lastHoveredPerson = person;
			}

			if (Input.GetMouseButtonDown(0) && person == targetPerson) {
				Debug.Log("Acertaste , T O P");
			}
		}
		else
		{
			if (lastHoveredPerson != null) {
				lastHoveredPerson.SetHightlight(false);
			}
		}
	}

	void CreateNewGame(int nPeople)
	{
		this.targetPerson = InstantiatePerson();
		this.targetPerson.transform.localScale = new Vector3(2, 2, 2);
		this.targetPerson.Setup(GetRandomPersonAssets(), mapSize);
		people.Add(this.targetPerson);

		for (int i = 0; i < nPeople-1; i++)	// -1 since target was already chosen
		{
			Person person = InstantiatePerson();
			person.Setup(GetRandomPersonAssetsDifferentFrom(targetPerson.PersonAssets), mapSize);
			people.Add(person);
		}
	}

	Person InstantiatePerson()
	{
		Person person = Instantiate(personPrefab, peopleParent);
		return person;
	}

	PersonAssets GetRandomPersonAssets()
	{
		return new PersonAssets {
			// HeadAccessory = ChooseRandom(assets.HeadAccessories),
			MaskMaterial = ChooseRandom(assets.MaskMaterials),
			ShirtMaterial = ChooseRandom(assets.ShirtMaterials),
			PantsMaterial = ChooseRandom(assets.PantsMaterials)
		};
	}

	PersonAssets GetRandomPersonAssetsDifferentFrom(PersonAssets personAssets)
	{
		PersonAssets newPersonAssets;

		do {
			newPersonAssets = new PersonAssets {
				// HeadAccessory = ChooseRandom(assets.HeadAccessories),
				MaskMaterial = ChooseRandom(assets.MaskMaterials),
				ShirtMaterial = ChooseRandom(assets.ShirtMaterials),
				PantsMaterial = ChooseRandom(assets.PantsMaterials)
			};
		} while (personAssets.Equals(newPersonAssets));

		return newPersonAssets;
	}

	T ChooseRandom<T>(T[] array)
	{
		return array[Random.Range(0, array.Length)];
	}
}