using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;
using System.ComponentModel;

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
	const int TIMER_DURATION = 5;
	const int START_LEVEL = 4;
	const int NUM_OF_LEVELS = 12;
    private List<ArrowKey> arrowSequence;
    private List<ArrowKey> playerInput;
    private int currentLevel = START_LEVEL;
    private Timer displayTimer;
	private Timer transitionTimer;
	private HBoxContainer arrowContainer;
    private Godot.Collections.Dictionary<ArrowKey,Texture2D> arrowMap;

	public override void _Ready()
	{
		//Load Arrow Textures
		arrowMap = new Godot.Collections.Dictionary<ArrowKey,Texture2D>
		{
			{ArrowKey.Left, (Texture2D)GD.Load("res://Assets/Icons/left_arrow.svg")},
			{ArrowKey.Right, (Texture2D)GD.Load("res://Assets/Icons/right_arrow.svg")},
			{ArrowKey.Up, (Texture2D)GD.Load("res://Assets/Icons/up_arrow.svg")},
			{ArrowKey.Down, (Texture2D)GD.Load("res://Assets/Icons/down_arrow.svg")}
		};

		//Get reference to HBoxContainer for arrow display
		arrowContainer = GetNode<HBoxContainer>("PanelContainer/VBoxContainer/HBoxContainer");

		//Generate arrow sequence (all 12)
		arrowSequence = GenerateNewSequence(NUM_OF_LEVELS);
		//Empty list to store player input
		playerInput = new List<ArrowKey>();
		//Initialize display timer 
		displayTimer = new Timer();
		displayTimer.WaitTime = TIMER_DURATION;
		displayTimer.Timeout += OnDisplayTimerTimeout;
		//Initialize transition timer
		transitionTimer = new Timer();
		transitionTimer.WaitTime = TIMER_DURATION;
		transitionTimer.Timeout += OnTransitionTimerTimeout;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if(!displayTimer.IsStopped() || playerInput.Count >= currentLevel){
			return;
		}
		if (Input.IsActionJustPressed("ui_left"))
		{
			playerInput.Add(ArrowKey.Left);
		}
		else if (Input.IsActionJustPressed("ui_right"))
		{
			playerInput.Add(ArrowKey.Right);
		}
		else if (Input.IsActionJustPressed("ui_up"))
		{
			playerInput.Add(ArrowKey.Up);
		}
		else if (Input.IsActionJustPressed("ui_down"))
		{
			playerInput.Add(ArrowKey.Down);
		}
	}
	private void StartNewLevel(){
		//Add elements up til currentLevel
		GD.Print("Starting level: " + currentLevel);
		playerInput.Clear();
		ShowSequence();
		displayTimer.Start();
	}
	private static List<ArrowKey> GenerateNewSequence(int length){
		var random = new Random();
		var newSequence = new List<ArrowKey>();
		for(int i = 0; i < length; i++){
			int idx = random.Next(Enum.GetValues(typeof(ArrowKey)).Length);
			newSequence.Add((ArrowKey)idx);
		}
		return newSequence;
	}
	private void CheckPlayerInput(){
		for (int i = 0; i < playerInput.Count; i++){
			if(playerInput[i] != arrowSequence[i]){
				GameOver();
				return;
			}
		}
		GD.Print("Level Completed");
		transitionTimer.Start();
	}

	private void NextLevel(){
		currentLevel += 1;
		
		if(currentLevel > NUM_OF_LEVELS){
			GD.Print("All levels finished");
			GameWin();
		}
		else{
			StartNewLevel();
		}
	}
	private void OnDisplayTimerTimeout(){
		arrowContainer.Visible=false;
		GD.Print("Put in the sequence");
	}
	private void ShowSequence(){
		ClearChildren(arrowContainer);
		for (var i = 0; i < currentLevel; i++){
			AddBox(i);
		}
		arrowContainer.Visible = true;
	}
	private void AddBox(int idx){
		var arrow = new TextureRect();
		arrow.Texture = arrowMap[arrowSequence[idx]];
		arrow.SetSize(new Vector2(64,64));
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
		NextLevel();
	}

	private void GameOver(){
		//TODO
	}
	private void GameWin(){
		//TODO
	}
}
