using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Random = UnityEngine.Random;

/*
- Start Menu UI
- Game UI
	- Restart button
	- Pause w/ black bg
	- Time left/elapsed
- Restrict game to 16:9
- Game UI - target person
	- For now draw colors??
	- Draw sprites
- Game end screen
	- Restart, time, you win/you lose
- Improve game "skybox"
- Implement police/other weird people(?)
*/

public class GameManager : MonoBehaviour
{
	// Need to randomly create a number of persons ( range ? 20 - 30 ? Increase with difficulty ? ) and to randomize their clothes
	// Create instance of Correct Person

	[Header("Project references")]
	[SerializeField] Assets assets;
	[SerializeField] Person personPrefab;

	[Header("Scene references")]
	[SerializeField] Transform peopleParent;
	[SerializeField] Camera cameraMain;

	[Header("UI menus")]
	[SerializeField] GameObject startMenuUIObject;
	[SerializeField] GameObject gameUIObject;
	[SerializeField] GameObject gameEndUIObject;

	[Header("Game UI references")]
	[SerializeField] TextMeshProUGUI timeText;

	[SerializeField] Button restartButton;
	[SerializeField] Button pauseButton;
	
	[SerializeField] Image headImage;
	[SerializeField] Image maskImage;
	[SerializeField] Image shirtImage;
	[SerializeField] Image pantsImage;

	[Header("Game End UI references")]
	[SerializeField] TextMeshProUGUI endScreenText;
	[SerializeField] TextMeshProUGUI timeLeftEndScreenText;
	[SerializeField] Button restartButtonEndGame;
	[SerializeField] Button mainMenuButtonEndGame;

	List<Person> people = new List<Person>();

	readonly Vector3 mapSize = new Vector3(24, 0, 24);
	Person targetPerson;
	Person lastHoveredPerson;

	const float StartingTime = 60 * 2;
	float timeLeft;

	void Start()
	{
		RegisterUICallbacks();
		CreateNewGame(200);
	}

	void Update()
	{
		if (timeLeft <= 0) {
			return;
		}

		timeLeft -= Time.deltaTime;
		DrawTime(timeLeft);

		if (timeLeft <= 0) {
			EndGame(false);
		}

		CheckForMousePositionAndClick();
	}

	private void RegisterUICallbacks()
	{
		restartButton.onClick.AddListener(() => {
			CreateNewGame(200);
		});

		pauseButton.onClick.AddListener(() => {
			/*  */
		});

		restartButtonEndGame.onClick.AddListener(() => {
			CreateNewGame(200);
			gameEndUIObject.SetActive(false);
			gameUIObject.SetActive(true);
		});

		mainMenuButtonEndGame.onClick.AddListener(() => {
			/*  */
		});
	}

	private void DrawTime(float time)
	{
		int minutes = (int) (time / 60);
		time -= minutes * 60;
		int seconds = (int) time;
		time -= seconds;
		int milliseconds = (int) (time * 1000);
		timeText.text = $"{minutes:00}:{seconds:00}.{milliseconds:000}";
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
				EndGame(true);
			}
		}
		else
		{
			if (lastHoveredPerson != null) {
				lastHoveredPerson.SetHightlight(false);
			}
		}
	}

	private void EndGame(bool isVictory)
	{
		foreach (Person person in people) {
			person.Disable();
		}
		gameUIObject.SetActive(false);
		gameEndUIObject.SetActive(true);

		endScreenText.text = isVictory ? " You WON " : " You LOSE ";
	
		timeLeftEndScreenText.text = timeText.text;
		timeLeftEndScreenText.enabled = isVictory;
	}

	private void CreateNewGame(int nPeople)
	{
		foreach (Transform child in peopleParent) {
			Destroy(child.gameObject);
		}
		people.Clear();

		this.targetPerson = InstantiatePerson();
		this.targetPerson.transform.localScale = new Vector3(2, 2, 2);
		this.targetPerson.Setup(GetRandomPersonAssets(), mapSize);
		people.Add(this.targetPerson);

		// headImage.sprite = targetPerson.PersonAssets.HeadAccessory.UIImage;
		maskImage.sprite = targetPerson.PersonAssets.MaskMaterial.UIImage;
		shirtImage.sprite = targetPerson.PersonAssets.ShirtMaterial.UIImage;
		pantsImage.sprite = targetPerson.PersonAssets.PantsMaterial.UIImage;

		for (int i = 0; i < nPeople-1; i++)	// -1 since target was already chosen
		{
			Person person = InstantiatePerson();
			person.Setup(GetRandomPersonAssetsDifferentFrom(targetPerson.PersonAssets), mapSize);
			people.Add(person);
		}

		timeLeft = StartingTime;
	}

	private Person InstantiatePerson()
	{
		Person person = Instantiate(personPrefab, peopleParent);
		return person;
	}

	private PersonAssets GetRandomPersonAssets()
	{
		return new PersonAssets {
			// HeadAccessory = ChooseRandom(assets.HeadAccessories),
			MaskMaterial = ChooseRandom(assets.MaskMaterials),
			ShirtMaterial = ChooseRandom(assets.ShirtMaterials),
			PantsMaterial = ChooseRandom(assets.PantsMaterials)
		};
	}

	private PersonAssets GetRandomPersonAssetsDifferentFrom(PersonAssets personAssets)
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

	private T ChooseRandom<T>(T[] array)
	{
		return array[Random.Range(0, array.Length)];
	}
}