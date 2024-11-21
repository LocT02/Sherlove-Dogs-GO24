using System;
using System.Collections.Generic;
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
	private Dictionary<string, Texture2D> textureCache = new();
	
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
		ItemSlot1 = GetNode<Button>("ItemButtonContainer/ItemSlot1") as ItemButton;
		ItemSlot2 = GetNode<Button>("ItemButtonContainer/ItemSlot2") as ItemButton;
		ItemSlot3 = GetNode<Button>("ItemButtonContainer/ItemSlot3") as ItemButton;
		List<ItemButton> buttonList = new List<ItemButton>{ ItemSlot1, ItemSlot2, ItemSlot3 };
		foreach (ItemButton button in buttonList) {
			button.Disabled = true;
			button.Visible = false;
		}
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
				var resultSetButtonUI = SetButtonAssets(buttons[i]);
				if (resultSetButtonUI.IsFailure) {
					return resultSetButtonUI;
				}
			} else {
				// Disable Button
				buttons[i].AttachedItem = null;
				buttons[i].Disabled = true;
				buttons[i].Visible = false;
			}
		}
		return Result.Success();
	}

	private Result SetButtonAssets(ItemButton itemButton) {
		// Set button Texture
		var item = itemButton.AttachedItem;
		Texture2D texture;
		StyleBoxTexture styleBox = new();
		
		// Check if Texture is cached
		if (!textureCache.TryGetValue(item.ImgFilePath, out texture)) {
			// Not Cached = Load and Cache Texture
			texture = (Texture2D)GD.Load(item.ImgFilePath);
			if (texture == null) {
				return Result.Failure($"Unable To Load Texture: {item.ImgFilePath}");
			}
			textureCache[item.ImgFilePath] = texture;
		}

		if (texture == null) {
			return Result.Failure($"Unable To Load Cached Texture: {item.ImgFilePath}");
		}

		// Set Texture and Enable button
		styleBox.Texture = texture;
		itemButton.AddThemeStyleboxOverride("normal", styleBox);
		itemButton.Disabled = false;
		itemButton.Visible = true;
		return Result.Success();
	}

	public void OnItemSlot1ButtonPress() {
		GD.Print("Item 1 pressed");
		// grab button data which item it is
		IItem item = ItemSlot1.AttachedItem;
		OnItemButtonPress(item);
	}

	public void OnItemSlot2ButtonPress() {
		GD.Print("Item 2 pressed");
		// grab button data which item it is
		IItem item = ItemSlot2.AttachedItem;
		OnItemButtonPress(item);
	}

	public void OnItemSlot3ButtonPress() {
		GD.Print("Item 3 pressed");
		// grab button data which item it is
		IItem item = ItemSlot3.AttachedItem;
		OnItemButtonPress(item);
	}

	private void OnItemButtonPress(IItem item) {
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
