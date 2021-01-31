using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Random = UnityEngine.Random;
using UnityEngine.Video;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
	[Header("Project references")]
	[SerializeField] Assets assets;
	[SerializeField] Person personPrefab;

	[Header("Scene references")]
	[SerializeField] Transform peopleParent;
	[SerializeField] Camera cameraMain;
	[SerializeField] AudioSource themeAudioSource;

	[Header("UI menus")]
	[SerializeField] GameObject startMenuUIObject;
	[SerializeField] GameObject gameUIObject;
	[SerializeField] GameObject gameEndUIObject;

	[Header("Start Menu references")]
	[SerializeField] TextMeshProUGUI titleText;
	[SerializeField] Button startButton;

	[Header("Game UI references")]
	[SerializeField] TextMeshProUGUI timeText;
	[SerializeField] Button restartButton;

	[SerializeField] Image headImage;
	[SerializeField] Image maskImage;
	[SerializeField] Image shirtImage;
	[SerializeField] Image pantsImage;

	[Header("Game End UI references")]
	[SerializeField] TextMeshProUGUI endScreenText;
	[SerializeField] TextMeshProUGUI timeLeftEndScreenText;
	[SerializeField] Button restartButtonEndGame;
	[SerializeField] Button mainMenuButtonEndGame;
	[SerializeField] VideoPlayer endGameVideoPlayer;

	[SerializeField] Person[] randomPeopleWithJOBS;

	enum Menu
	{
		Start,
		Game,
		End,
	}

	List<Person> people = new List<Person>();

	readonly Vector3 mapSize = new Vector3(157.891f, 0, 128.126f);
	Person targetPerson;
	Person lastHoveredPerson;

	const float StartingTime = 60 * 2;
	float timeLeft;

	void Start()
	{
		RegisterUICallbacks();
		PrepareTweens();
		CreateNewGame();
		OpenMenu(Menu.Start);
	}

	void Update()
	{
		if (IsActiveMenu(Menu.Start)) return;
		if (timeLeft <= 0) return;

		timeLeft -= Time.deltaTime;
		DrawTime(timeLeft);

		if (timeLeft <= 0) {
			EndGame(false);
		}

		CheckForMousePositionAndClick();
	}

	private void RegisterUICallbacks()
	{
		startButton.onClick.AddListener(() => {
			OpenMenu(Menu.Game);
		});

		restartButton.onClick.AddListener(() => {
			CreateNewGame();
		});

		restartButtonEndGame.onClick.AddListener(() => {
			CreateNewGame();
			OpenMenu(Menu.Game);
		});

		mainMenuButtonEndGame.onClick.AddListener(() => {
			CreateNewGame();	// for start menu
			OpenMenu(Menu.Start);
		});

		endGameVideoPlayer.prepareCompleted += player => {
			player.time = 9f;
			player.Play();
			themeAudioSource.DOFade(0.05f, 0.25f);
		};

		endGameVideoPlayer.loopPointReached += player => {
			player.Stop();
			themeAudioSource.DOFade(0.5f, 0.25f);
		};
	}

	private void PrepareTweens()
	{
		startButton.targetGraphic.DOFade(0.25f, 1.5f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);

		DOTween.Sequence()
			.Append(titleText.transform.DOLocalMoveY(50f, 0.1f).SetLoops(2, LoopType.Yoyo).SetEase(Ease.Linear))
			.AppendInterval(0.26154f)
			.SetLoops(-1, LoopType.Restart)
			.SetEase(Ease.Linear);
	}

	private void OpenMenu(Menu menu)
	{
		// clear any pending things
		endGameVideoPlayer.Stop();
		themeAudioSource.DOKill();
		themeAudioSource.volume = 1f;

		// disable all menus
		startMenuUIObject.SetActive(false);
		gameUIObject.SetActive(false);
		gameEndUIObject.SetActive(false);

		// enable the only one that matters
		switch (menu)
		{
			case Menu.Start: startMenuUIObject.SetActive(true); break;
			case Menu.Game:  gameUIObject.SetActive(true);      break;
			case Menu.End:   gameEndUIObject.SetActive(true);   break;
		}
	}

	private bool IsActiveMenu(Menu menu)
	{
		switch (menu)
		{
			case Menu.Start: return startMenuUIObject.activeSelf;
			case Menu.Game:  return gameUIObject.activeSelf;
			case Menu.End:   return gameEndUIObject.activeSelf;
		}

		return false;
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

		OpenMenu(Menu.End);

		bool isTópe = timeLeft > StartingTime - 5f;	// finished game in less than 5s

		endScreenText.text = isVictory
			? (isTópe ? "TÓPE!" : " You WON ")
			: " You LOSE ";

		timeLeftEndScreenText.text = timeText.text;
		timeLeftEndScreenText.enabled = isVictory;

		if (isTópe) {
			endGameVideoPlayer.Prepare();
		}
	}

	private void CreateNewGame()
	{
		foreach (Transform child in peopleParent) {
			Destroy(child.gameObject);
		}
		people.Clear();

		const int nPeople = 1000;

		this.targetPerson = InstantiatePerson();
		// this.targetPerson.transform.localScale = new Vector3(2, 2, 2);
		this.targetPerson.Setup(GetRandomPersonAssets(), mapSize);
		people.Add(this.targetPerson);

		headImage.sprite = targetPerson.PersonAssets.HeadAccessory.UIImage;
		maskImage.sprite = targetPerson.PersonAssets.MaskMaterial.UIImage;
		shirtImage.sprite = targetPerson.PersonAssets.ShirtMaterial.UIImage;
		pantsImage.sprite = targetPerson.PersonAssets.PantsMaterial.UIImage;

		for (int i = 0; i < nPeople-1; i++)	// -1 since target was already chosen
		{
			Person person;
			PersonAssets personAssets;
			// int value = Random.Range(0,100);
			// if (value <= 20) {
			// 	person = Instantiate(ChooseRandom(randomPeopleWithJOBS), peopleParent);
			// 	personAssets = null;
			// } else {
				person = InstantiatePerson();
				personAssets = GetRandomPersonAssetsDifferentFrom(targetPerson.PersonAssets);
			// }

			person.Setup(personAssets, mapSize);
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
			HeadAccessory = ChooseRandom(assets.HeadAccessories),
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
				HeadAccessory = ChooseRandom(assets.HeadAccessories),
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