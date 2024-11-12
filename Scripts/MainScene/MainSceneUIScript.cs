using System;
using CustomButton;
using GameData;
using Godot;
using IItemsTypes;
using InventoryManager;
using ResultManager;

public partial class MainSceneUIScript : CanvasLayer
{
	private Label CategoryLabel;
	private Label FeedbackLabel;
	private LineEdit GuessInputField;
	private Button SubmitGuessButton;
	private ItemButton ItemSlot1;
	private ItemButton ItemSlot2;
	private ItemButton ItemSlot3;
	private MainScene MainScene;
	private GameManager.GameManager gameInstance;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		InitializeUI();

		GD.Print("MainSceneUI Ready");
	}

	private void InitializeUI() {
		gameInstance = GameManager.GameManager.Instance;
		MainScene = GetTree().Root.GetNode<Node2D>("MainSceneNode") as MainScene;
		CategoryLabel = GetNode<Label>("WordUI/WordGameContainer/CategoryLabel");
		FeedbackLabel = GetNode<Label>("WordUI/WordGameContainer/FeedbackLabel");
		GuessInputField = GetNode<LineEdit>("WordUI/WordGameContainer/GuessInputField");
		SubmitGuessButton = GetNode<Button>("WordUI/WordGameContainer/SubmitGuessButton");
		ItemSlot1 = GetNode<Button>("WordUI/ItemButtonContainer/ItemSlot1") as ItemButton;
		ItemSlot2 = GetNode<Button>("WordUI/ItemButtonContainer/ItemSlot2") as ItemButton;
		ItemSlot3 = GetNode<Button>("WordUI/ItemButtonContainer/ItemSlot3") as ItemButton;
		ItemSlot1.Disabled = true;
		ItemSlot2.Disabled = true;
		ItemSlot3.Disabled = true;
	}

	public Result UpdateCategoryLabel(string category) {
		if (category == null || category == "") {
			return Result.Failure("Input Category String is Null");
		}
		CategoryLabel.Text = category;
		return Result.Success();
	}

	public Result UpdateFeedbackLabel(string feedback) {
		if (feedback == null) {
			return Result.Failure("Input Feedback String is Null");
		}
		FeedbackLabel.Text = feedback;
		return Result.Success();
	}

	public Result<string> GetPlayerGuess() {
		if (GuessInputField.Text == null) {
			return Result.Failure<string>("Guess Input is Null or empty");
		}

		if (GuessInputField.MaxLength != GuessInputField.Text.Length ) {
			return Result.Failure<string>("Guess Input is Not Filled");
		}

		// Check guess characters

		return Result.Success(GuessInputField.Text);
	}

	public Result ClearInputField() {
		if (GuessInputField.Text == null || GuessInputField.Text == "") {
			return Result.Failure("Guess Input is Already Empty or Null");
		}
		GuessInputField.Text = "";
		return Result.Success();
	}

	public Result SetInputConstraints(int wordLength) {
		if (wordLength <= 0) {
			return Result.Failure("Word Length is <= 0");
		}
		GuessInputField.MaxLength = wordLength;
		return Result.Success();
	}

	public void OnSubmitGuessButtonPress() {
		GD.Print("Submit pressed");
		// disable button to prevent spam
		SubmitGuessButton.Disabled = true;

		// Grab guess input
		var guess = GetPlayerGuess();

		if (guess.IsFailure) {
			GD.Print(guess.Error);
			SubmitGuessButton.Disabled = false;
			ClearInputField();
			return;
		}

		// call mainscene with guesscheck
		var result = MainScene.GuessSubmit(guess.Value.ToUpper());

		// if something error crash game kekw
		if (result.IsFailure) {
			throw new InvalidProgramException(result.Error);
		}

		// else everything goochi clear input reenable button
		ClearInputField();
		SubmitGuessButton.Disabled = false;
	}

	public Result AttachItemsToButtons() {
		// Attaches Item and Enables Button
		var buttons = new ItemButton[] { ItemSlot1, ItemSlot2, ItemSlot3};
		var inventory = gameInstance.gameData.Inventory.Items;

		if (inventory == null) {
			return Result.Failure("Inventory Does Not Exist");
		}

		for (int i = 0; i < buttons.Length; i++) {
			
			if ( i < inventory.Count && inventory[i] != null) {
				buttons[i].AttachedItem = inventory[i];
				SetButtonAssets(buttons[i]);
			} else {
				buttons[i].AttachedItem = null;
			}
		}
		return Result.Success();
	}

	private void SetButtonAssets(ItemButton itemButton) {
		// Set button pictures here
		itemButton.Disabled = false;
	}

	public void OnItemSlot1ButtonPress() {
		GD.Print("Item 1 pressed");
		// grab button data which item it is
		IItem item = ItemSlot1.AttachedItem;
		var resultUseItem = gameInstance.gameData.Inventory.UseItem(item);

		if (resultUseItem.IsFailure) {
			throw new InvalidProgramException(resultUseItem.Error);
		}

		var resultAttachButtons = AttachItemsToButtons();

		if (resultAttachButtons.IsFailure) {
			throw new InvalidProgramException(resultAttachButtons.Error);
		}

	}

	public void OnItemSlot2ButtonPress() {
		GD.Print("Item 2 pressed");
		// grab button data which item it is
		IItem item = ItemSlot1.AttachedItem;
		var resultUseItem = gameInstance.gameData.Inventory.UseItem(item);

		if (resultUseItem.IsFailure) {
			throw new InvalidProgramException(resultUseItem.Error);
		}

		var resultAttachButtons = AttachItemsToButtons();

		if (resultAttachButtons.IsFailure) {
			throw new InvalidProgramException(resultAttachButtons.Error);
		}
	}

	public void OnItemSlot3ButtonPress() {
		GD.Print("Item 3 pressed");
		// grab button data which item it is
		IItem item = ItemSlot1.AttachedItem;

		var resultUseItem = gameInstance.gameData.Inventory.UseItem(item);

		if (resultUseItem.IsFailure) {
			throw new InvalidProgramException(resultUseItem.Error);
		}

		var resultAttachButtons = AttachItemsToButtons();

		if (resultAttachButtons.IsFailure) {
			throw new InvalidProgramException(resultAttachButtons.Error);
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
