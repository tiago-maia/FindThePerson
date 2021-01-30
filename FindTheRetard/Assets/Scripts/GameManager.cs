using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	// Need to randomly create a number of persons ( range ? 20 - 30 ? Increase with difficulty ? ) and to randomize their clothes
	// Create instance of Correct Person

	[SerializeField] Transform peopleParent;

	[SerializeField] Mesh[] headAccessories;
	[SerializeField] Material[] maskMaterials;
	[SerializeField] Material[] shirtMaterials;
	[SerializeField] Material[] pantsMaterials;

	[SerializeField] Person personPrefab;

	List<Person> people = new List<Person>();

	void Start()
	{
		CreateNewGame(50);
	}

	void CreateNewGame(int nPeople)
	{
		for (int i = 0; i < nPeople; i++)
		{
			Person person = InstantiateRandomPerson();
			people.Add(person);
			RandomizePerson(person);
		}
	}

	Person InstantiateRandomPerson()
	{
		Person person = Instantiate(personPrefab, peopleParent);
		return person;
	}

	void RandomizePerson(Person person)
	{
		Mesh headAccessory = ChooseRandom(headAccessories);
		Material maskMaterial = ChooseRandom(maskMaterials);
		Material shirtMaterial = ChooseRandom(shirtMaterials);
		Material pantsMaterial = ChooseRandom(pantsMaterials);

		person.Setup(maskMaterial, shirtMaterial, pantsMaterial, headAccessory);
	}

	T ChooseRandom<T>(T[] array)
	{
		return array[Random.Range(0, array.Length)];
	}
}