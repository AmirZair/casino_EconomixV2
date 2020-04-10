using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Valve.VR.InteractionSystem.Sample
{
	public class MachineController : MonoBehaviour
	{
		public HoverButton ButtonLessMoney;
		public HoverButton ButtonMoreMoney;
		public HoverButton ButtonGameA;
		public HoverButton ButtonGameB;
		public HoverButton ButtonStart;
		public HoverButton ButtonPower;

		// Player starts with 9 coins and a bet of 0 and has to bet at least 0
		private int Coins = 20;
		private int bet = 0;
		private int minimumBet = 0;

		// 1 = gameA, 2 = gameB
		public int gameMode = 1; 

		// isOn = the machine is powered on
		public bool isOn = true;

		// we need to retrieve the screen GameObject to power it on and off
		public GameObject screen;

		// All the UI texts
		public Text TextApplicationName;
		public Text TextApplicationInfo;
		public Text TextCurrentCoins;
		public Text TextCurrentBet;
		public Text TextCurrentGameMode;

		// We retrieve Panel gameobjects in order to change the panels color regarding to the gameMode
		public GameObject PanelApplicationName;
		public GameObject PanelApplicationInfo;
		public GameObject PanelCurrentCoins;
		public GameObject PanelCurrentBet;
		public GameObject PanelCurrentGameMode;


		// Game over
		private bool gameOver = false;

		private void Start() {
			ButtonLessMoney.onButtonDown.AddListener(removeBet);
			ButtonMoreMoney.onButtonDown.AddListener(addBet);
			ButtonGameA.onButtonDown.AddListener(switchToA);
			ButtonGameB.onButtonDown.AddListener(switchToB);
			ButtonStart.onButtonDown.AddListener(startGame);
			ButtonPower.onButtonDown.AddListener(powerMachine);
			//i.color = Color.red;
		}

		void Update () {

			screen.SetActive (isOn);

			if (isOn) {
				updateUI ();
				adjustBet ();
			}

		}

		private void removeBet (Hand hand) {
			if(bet > 0 && !gameOver)
				bet -= 1;
		}

		private void addBet (Hand hand) {
			if (bet < Coins && !gameOver)
				bet += 1;
		}

		private void switchToA(Hand hand) {
			if (gameOver)
				return;
			gameMode = 1;
		}

		private void switchToB(Hand hand) {
			if (gameOver)
				return;
			gameMode = 2;
		}

		private void startGame(Hand hand) {
			if (!isOn || gameOver)
				return;

			if (bet < minimumBet) {
				TextApplicationInfo.text = "Bet higher !";
				return;
			}

			//TextApplicationInfo.text = "Drawing...";
			Coins -= bet;

			int result = -1;

			if (gameMode == 1) 
				result = Random.Range (0, 2);

			if (gameMode == 2) 
				result = Random.Range (0, 6);


			if (result == 0) {
				if (gameMode == 1) {
					TextApplicationInfo.text = "Win ! +" + bet + "*3 coins !";
					Coins += bet * 3;
				}
				if (gameMode == 2) {
					TextApplicationInfo.text = "Win ! +" + bet + "*9 coins !";
					Coins += bet * 9;
				}

			} else {
				TextApplicationInfo.text = "Lose... -" + bet + " coins !";
				if (Coins == 0) {
					gameOver = true;
					TextCurrentCoins.text = "$$$ : " + Coins;
				}
			}
			Debug.Log (result);
		}

		private void powerMachine(Hand hand) {
			isOn = !isOn;
			if (!isOn)
				resetMachine ();
		}

		private void resetMachine() {
			Coins = 20;
			bet = 1;
			gameMode = 1;
			gameOver = false;
			TextApplicationInfo.text = "Place your bets !";
		}

		private void updateUI() {
			TextApplicationName.text = "Economix";

			if (gameOver) {
				TextApplicationInfo.text = "Game over...";
				return;
			}

			TextCurrentCoins.text = "Credit : " + Coins;
			TextCurrentBet.text = "Bet : " + bet;

			if (gameMode == 1) {
				TextCurrentGameMode.text = "Gamemode A : 1 in 2 chances \nof winning 3 times your bet !";

				PanelApplicationName.GetComponent<Image>().color = new Color(0, 143, 228);
				PanelApplicationInfo.GetComponent<Image>().color = new Color(0, 143, 228);
				PanelCurrentCoins.GetComponent<Image>().color = new Color(0, 143, 228);
				PanelCurrentBet.GetComponent<Image>().color = new Color(0, 143, 228);
				PanelCurrentGameMode.GetComponent<Image>().color = new Color(0, 143, 228);
			}
			if (gameMode == 2) {
				TextCurrentGameMode.text = "Gamemode B : 1 in 6 chances \nof winning 9 times your bet !";

				PanelApplicationName.GetComponent<Image>().color = new Color(255, 234, 0);
				PanelApplicationInfo.GetComponent<Image>().color = new Color(255, 234, 0);
				PanelCurrentCoins.GetComponent<Image>().color = new Color(255, 234, 0);
				PanelCurrentBet.GetComponent<Image>().color = new Color(255, 234, 0);
				PanelCurrentGameMode.GetComponent<Image>().color = new Color(255, 234, 0);
			}
		}

		// If bet is higher than current coins, set bet to current coins to avoid having a negative amount of coins in case we lose
		private void adjustBet() {
			if (bet > Coins)
				bet = Coins;
		}
	}
}
