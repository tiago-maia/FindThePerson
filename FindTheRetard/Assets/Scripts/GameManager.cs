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
		CreateNewGame(1000);
	}

	void CreateNewGame(int nPeople)
	{
		this.targetPerson = InstantiateRandomPerson();
		this.targetPerson.Setup(GetRandomPersonAssets(), mapSize);
		people.Add(this.targetPerson);

		for (int i = 0; i < nPeople-1; i++)	// -1 since target was already chosen
		{
			Person person = InstantiateRandomPerson();
			person.Setup(GetRandomPersonAssetsDifferentFrom(targetPerson.PersonAssets), mapSize);
			people.Add(person);
		}
	}

	Person InstantiateRandomPerson()
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
		return new PersonAssets {
			// HeadAccessory = ChooseRandomButDifferent(assets.HeadAccessories, personAssets.HeadAccessory),
			MaskMaterial = ChooseRandomButDifferent(assets.MaskMaterials, personAssets.MaskMaterial),
			ShirtMaterial = ChooseRandomButDifferent(assets.ShirtMaterials, personAssets.ShirtMaterial),
			PantsMaterial = ChooseRandomButDifferent(assets.PantsMaterials, personAssets.PantsMaterial)
		};
	}

	T ChooseRandom<T>(T[] array)
	{
		return array[Random.Range(0, array.Length)];
	}

	T ChooseRandomButDifferent<T>(T[] array, T exclusion) where T : class
	{
		T choice;

		do {
			choice = array[Random.Range(0, array.Length)];
		} while (choice == exclusion);

		return choice;
	}
}