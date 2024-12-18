using Godot;
using Godot.Collections;
using InventoryManager;
using ResultManager;
using System;
using System.Collections.Generic;

/*
Game flow:
Upon creation of Memory class:
	Load/Cache arrow textures
	Grab ref to HBoxContainer (where we put our arrows)
	Generate Arrow Sequence
	Initialize timers
Upon game start:
	Display sequence for 5 seconds
	Player inputs sequence until correct amnt of keys are pressed
	Input is blocked until displayTimer stops
	DisplayTimer stops
	Instant feedback
	Wait 5 seconds to perform animation (can change this later)
	Repeat from top if game continues

Upon game win:
	Display feedback and receive item upon success, move back to main scene

Upon game over
	Display feedback and move back to main scene
*/
public partial class Memory : Control
{
	public enum ArrowKey {Left, Right, Up, Down};

	public enum GameStates {Entry, Memorize, Input, Transition, Win, Lose};
	private GameStates gameState = GameStates.Entry;
	const int DISPLAY_DURATION = 5;
	const int TRANSITION_DURATION = 3;
	const int START_LEVEL = 3;
	const int STEP_LEVEL = 2;
	const int NUM_OF_ROUNDS = 4;
	private int MAX_LEVEL = START_LEVEL + STEP_LEVEL * (NUM_OF_ROUNDS-1);
	private int currRound = 0;
	private int roundsWon = 0;
	private string playerInputString = "";
	private List<ArrowKey> arrowSequence, playerInput;
	private int currentLevel = START_LEVEL;
	private Godot.Timer displayTimer, transitionTimer;
	private HBoxContainer arrowContainer;
	private ProgressBar progressBar;
	private RichTextLabel progressBarText;
	private Label topText, roundsWonText, currRoundText;
	private Godot.Collections.Dictionary<ArrowKey,Texture2D> arrowMap;
	private GameManager.GameManager gameManagerInstance;
	private Inventory inventoryInstance;
	private AnimatedSprite2D dog;
	private int ItemTextureValue;
	private PanelContainer ItemGetPanel;
	private TextureRect ItemDisplay;
	private Label ItemNameText;

	public override void _Ready()
	{
		//Initialize references to GameManager
		gameManagerInstance = GameManager.GameManager.Instance;
		inventoryInstance = gameManagerInstance.gameData.Inventory;
		//Load Arrow Textures
		arrowMap = new Godot.Collections.Dictionary<ArrowKey,Texture2D>
		{
			{ArrowKey.Left, (Texture2D)GD.Load("res://Assets/Icons/Left.svg")},
			{ArrowKey.Right, (Texture2D)GD.Load("res://Assets/Icons/Right.svg")},
			{ArrowKey.Up, (Texture2D)GD.Load("res://Assets/Icons/Up.svg")},
			{ArrowKey.Down, (Texture2D)GD.Load("res://Assets/Icons/Down.svg")}
		};

		//Get references
		topText = GetNode<Label>("GameContainer/VBoxContainer/TopText");
		arrowContainer = GetNode<HBoxContainer>("GameContainer/VBoxContainer/ArrowContainer");
		progressBar = GetNode<ProgressBar>("GameContainer/VBoxContainer/ProgressBar");
		progressBarText = GetNode<RichTextLabel>("GameContainer/VBoxContainer/ProgressBar/BarText");
		roundsWonText = GetNode<Label>("PanelContainer2/HBoxContainer/Panel/RoundsWon");
		currRoundText = GetNode<Label>("PanelContainer2/HBoxContainer/Panel2/CurrRound");
		dog = GetNode<AnimatedSprite2D>("Player/AnimatedSprite2D");
		ItemGetPanel = GetNode<PanelContainer>("ItemGetPanel");
		ItemDisplay = GetNode<TextureRect>("ItemGetPanel/ItemDisplay");
		ItemNameText = GetNode<Label>("ItemGetPanel/ItemDisplay/ItemNameText");
		ItemGetPanel.Visible = false;
		progressBarText.BbcodeEnabled = true;
		progressBar.Value = 100;

		//Generate arrow sequence (all 12)
		arrowSequence = GenerateNewSequence(MAX_LEVEL);
		//Empty list to store player input
		playerInput = new List<ArrowKey>();
		//Initialize display timer 
		displayTimer = GetNode<Godot.Timer>("DisplayTimer");
		displayTimer.OneShot = true;
		displayTimer.WaitTime = DISPLAY_DURATION;
		displayTimer.Timeout += OnDisplayTimerTimeout;
		//Initialize transition timer
		transitionTimer = GetNode<Godot.Timer>("TransitionTimer");
		transitionTimer.OneShot = true;
		transitionTimer.WaitTime = TRANSITION_DURATION;
		transitionTimer.Timeout += OnTransitionTimerTimeout;

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		//Start when player exit the how-to overlay.
		if(gameManagerInstance.allowMinigameStart){
			StartLevel();
			gameManagerInstance.allowMinigameStart = false;
		}
		
		//(int)(displayTimer.TimeLeft/displayTimer.WaitTime*100) : (int)(1-(transitionTimer.TimeLeft/transitionTimer.WaitTime)*100);
		if(gameState == GameStates.Memorize){
			progressBar.Value = (int)(displayTimer.TimeLeft/displayTimer.WaitTime*100);
			progressBarText.Text = $"[center]{displayTimer.TimeLeft.ToString("0.0") + 's'}[/center]";
			topText.Text = "Memorize the sequence!";
		}
		else if(gameState == GameStates.Transition){
			progressBar.Value = (int)((1-transitionTimer.TimeLeft/transitionTimer.WaitTime)*100);
			progressBarText.Text = $"[center]{transitionTimer.TimeLeft.ToString("0.0") + 's'}[/center]";
		}
		else if(gameState == GameStates.Input){
			progressBarText.Text = $"[center]{playerInputString}[/center]";
			if(playerInput.Count >= currentLevel)
			{
				progressBarText.Clear();
				if(CheckPlayerInput()){
					gameManagerInstance.PlaySFX("MainSceneNode", gameManagerInstance.sfxPaths["MEMORY_CORRECT"]); //SFX for round won
					topText.Text = "Good job! You got it right!";
					NextLevel(true);
				}
				else{
					gameManagerInstance.PlaySFX("MainSceneNode", gameManagerInstance.sfxPaths["MEMORY_INCORRECT"]); //SFX for round lost
					topText.Text = "Uh oh, try again!";
					NextLevel(false);
				}
			}
			if (Input.IsActionJustPressed("ui_left"))
			{
				gameManagerInstance.PlaySFX("MainSceneNode", gameManagerInstance.sfxPaths["BUTTON_CLICKED"]); //SFX for input
				playerInput.Add(ArrowKey.Left);
				dog.Play("Left");
				playerInputString += "←";
			}
			else if (Input.IsActionJustPressed("ui_right"))
			{
				gameManagerInstance.PlaySFX("MainSceneNode", gameManagerInstance.sfxPaths["BUTTON_CLICKED"]); //SFX for input
				playerInput.Add(ArrowKey.Right);
				dog.Play("Right");
				playerInputString += "→";
			}
			else if (Input.IsActionJustPressed("ui_up"))
			{
				gameManagerInstance.PlaySFX("MainSceneNode", gameManagerInstance.sfxPaths["BUTTON_CLICKED"]); //SFX for input
				playerInput.Add(ArrowKey.Up);
				dog.Play("Up");
				playerInputString += "↑";
			}
			else if (Input.IsActionJustPressed("ui_down"))
			{
				gameManagerInstance.PlaySFX("MainSceneNode", gameManagerInstance.sfxPaths["BUTTON_CLICKED"]); //SFX for input
				playerInput.Add(ArrowKey.Down);
				dog.Play("Down");
				playerInputString += "↓";
			}
		}
	}
	//Starts the current level
	private void StartLevel(){
		currRound++;
		currRoundText.Text = currRound.ToString();
		roundsWonText.Text = roundsWon.ToString();
		dog.RotationDegrees = 0.0f;
		playerInput.Clear();
		playerInputString ="";
		ShowSequence();
		gameState = GameStates.Memorize;
		displayTimer.Start(DISPLAY_DURATION);
	}
	//Generates a new sequence and returns it
	private static List<ArrowKey> GenerateNewSequence(int length){
		var random = new Random();
		var newSequence = new List<ArrowKey>();
		for(int i = 0; i < length; i++){
			int idx = random.Next(Enum.GetValues(typeof(ArrowKey)).Length);
			newSequence.Add((ArrowKey)idx);
		}
		return newSequence;
	}
	//Check players input against sequence
	private bool CheckPlayerInput(){
		for (int i = 0; i < playerInput.Count; i++){
			if(playerInput[i] != arrowSequence[i]){
				return false;
			}
		}
		roundsWon++;
		currRoundText.Text = currRound.ToString();
		roundsWonText.Text = roundsWon.ToString();
		return true;
	}
	//Continue to next level upon win
	private void NextLevel(bool localWin){
		dog.Play("IdleLeft");
		if(localWin) currentLevel += STEP_LEVEL;
		if(currRound >= NUM_OF_ROUNDS){
			if(roundsWon >= NUM_OF_ROUNDS-1){
				topText.Text = "You win!";
				gameState = GameStates.Win;
				GameWin(roundsWon == 4);
			}else {
				topText.Text = "You Lost!";
				gameState = GameStates.Lose;
				GameOver();
			}
		}
		else{
			gameState = GameStates.Transition;
			transitionTimer.Start(TRANSITION_DURATION);
		}
	}
	//Waits for Display Timer to end. Changes game state to "input"
	private void OnDisplayTimerTimeout(){
		arrowContainer.Visible=false;
		topText.Text = "Input the previous sequence!";
		gameState = GameStates.Input;
		progressBarText.Text = "";
	}
	//Show the sequence
	private void ShowSequence(){
		ClearChildren(arrowContainer);
		for (var i = 0; i < currentLevel; i++){
			AddBox(i);
		}
		arrowContainer.Visible = true;
	}
	//Add arrows by index from arrowSequence
	private void AddBox(int idx){
		var arrow = new TextureRect();
		arrow.Texture = arrowMap[arrowSequence[idx]];
		arrow.StretchMode = TextureRect.StretchModeEnum.KeepAspectCentered;
		arrowContainer.AddChild(arrow);
	}
	private static void ClearChildren(Node node){
		foreach (Node child in node.GetChildren()){
			node.RemoveChild(child);
			child.QueueFree();
		}
	}
	private void OnTransitionTimerTimeout(){
		StartLevel();
	}
	private void printList(List<ArrowKey> list){
		foreach(ArrowKey key in list){
			GD.Print(key);
		}
	}
	private async void GameOver(){
		gameManagerInstance.PlaySFX("MainSceneNode", gameManagerInstance.sfxPaths["MINIGAME_LOSS"]);
		//scene switch
		await gameManagerInstance.SceneChanger(gameManagerInstance.scenePaths["MAIN_SCENE"]);
	}
	
	private async void GameWin(bool upgraded){
		gameManagerInstance.PlaySFX("MainSceneNode", gameManagerInstance.sfxPaths["MINIGAME_WIN"]);
		if(!upgraded){
			ItemTextureValue = 0;
		} else{
			ItemTextureValue = 1;
		}

		var item = inventoryInstance.SelectRandomItem(upgraded);

		if(item.IsFailure) {
			GD.PushError(item.Error);
		}

		ItemGetPanel.Visible = true;
		ItemDisplay.Texture = (Texture2D)GD.Load(item.Value.ImgFilePath[ItemTextureValue]);
		ItemNameText.Text = item.Value.Name;

		var result = inventoryInstance.AddItem(item.Value);
		if(result.IsFailure) {
			GD.PushError(result.Error);
		}
		//Show item get screen first before scene switch
		
		await gameManagerInstance.SceneChanger(gameManagerInstance.scenePaths["MAIN_SCENE"]);

	}
}
