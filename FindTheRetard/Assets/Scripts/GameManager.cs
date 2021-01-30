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

	List<Person> people = new List<Person>();

	readonly Vector3 mapSize = new Vector3(24, 0, 24);
	Person targetPerson;

	void Start()
	{
		CreateNewGame(50);
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