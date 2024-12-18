using System;
using System.Collections.Generic;
using CustomButton;
using GameData;
using Godot;
using GodotPlugins.Game;
using IItemsTypes;
using InventoryManager;
using ResultManager;

public partial class MainSceneUIScript : CanvasLayer
{
	private Label CategoryLabel;
	private RichTextLabel FeedbackLabel;
	private LineEdit GuessInputField;
	private Button SubmitGuessButton;
	private ItemButton ItemSlot1;
	private ItemButton ItemSlot2;
	private ItemButton ItemSlot3;
	private MainScene MainScene;
	private GameManager.GameManager gameInstance;
	private Dictionary<string, Texture2D> textureCache = new();
	private Camera2D MainCamera;
	private Camera2D MinigameCamera;
	private PlayerMovement player;
	private DogBed dogBed;
	private CanvasLayer HowToOverlay;
	private CanvasLayer HowToItemOverlay;
	public Label ExitBedText;
	private Label ItemSlotText1;
	private Label ItemSlotText2;
	private Label ItemSlotText3;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		InitializeUI();

		GD.Print("MainSceneUI Ready");
	}

	private void InitializeUI() {
		gameInstance = GameManager.GameManager.Instance;
		MainScene = GetTree().Root.GetNode<Node2D>("MainSceneNode") as MainScene;
		CategoryLabel = GetNode<Label>("WordUI/CategoryLabel");
		FeedbackLabel = GetNode<RichTextLabel>("WordUI/FeedbackLabel");
		GuessInputField = GetNode<LineEdit>("WordUI/GuessInputField");
		GuessInputField.Editable = false;
		SubmitGuessButton = GetNode<Button>("WordUI/SubmitGuessButton");
		ItemSlot1 = GetNode<Button>("ItemButtonContainer/ItemSlot1") as ItemButton;
		ItemSlot2 = GetNode<Button>("ItemButtonContainer/ItemSlot2") as ItemButton;
		ItemSlot3 = GetNode<Button>("ItemButtonContainer/ItemSlot3") as ItemButton;
		MainCamera = GetNode<Camera2D>("/root/MainSceneNode/MainCamera");
		MinigameCamera = GetNode<Camera2D>("/root/MainSceneNode/MinigameCamera");
		player = GetNode<PlayerMovement>("/root/MainSceneNode/Player");
		dogBed = GetNode<DogBed>("/root/MainSceneNode/GamePlay/DogBed");
		HowToOverlay = GetNode<CanvasLayer>("HowToUI/HowToOverlay");
		HowToItemOverlay = GetNode<CanvasLayer>("ItemButtonContainer/HowToItemUI/HowToItemOverlay");
		ExitBedText = GetNode<Label>("ExitBedUI/ExitBedText");
		ItemSlotText1 = GetNode<Label>("ItemButtonContainer/ItemSlot1/ItemText1");
		ItemSlotText2 = GetNode<Label>("ItemButtonContainer/ItemSlot2/ItemText2");
		ItemSlotText3 = GetNode<Label>("ItemButtonContainer/ItemSlot3/ItemText3");
		
		ItemSlotText1.Visible = false;
		ItemSlotText2.Visible = false;
		ItemSlotText3.Visible = false;
		ExitBedText.Visible = false;

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

	public Result UpdateFeedbackLabel(char[] feedback) {
		if (feedback == null) {
			return Result.Failure("Input Feedback is Null");
		}
		string spacedLetters = string.Join(" ", feedback);
		string newLined = spacedLetters.Replace("   ", "\n");
		FeedbackLabel.Text = $"[center][color=black]{newLined}[/color][/center]";
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
		string filePath = item.Upgraded ? item.ImgFilePath[1] : item.ImgFilePath[0];
		Texture2D texture;
		StyleBoxTexture styleBox = new();
		
		// Check if Texture is cached
		if (!textureCache.TryGetValue(filePath, out texture)) {
			// Not Cached = Load and Cache Texture
			texture = (Texture2D)GD.Load(filePath);
			if (texture == null) {
				return Result.Failure($"Unable To Load Texture: {filePath}");
			}
			textureCache[filePath] = texture;
		}

		if (texture == null) {
			return Result.Failure($"Unable To Load Cached Texture: {filePath}");
		}

		// Set Texture and Enable button
		styleBox.Texture = texture;
		itemButton.AddThemeStyleboxOverride("normal", styleBox);
		itemButton.AddThemeStyleboxOverride("pressed", styleBox);
		itemButton.AddThemeStyleboxOverride("hover", styleBox);
		itemButton.AddThemeStyleboxOverride("focused", styleBox);
		itemButton.Disabled = false;
		itemButton.Visible = true;
		return Result.Success();
	}

	public void OnItemSlot1ButtonPress() {
		GD.Print("Item 1 pressed");
		gameInstance.PlaySFX("MainSceneNode", gameInstance.sfxPaths["BUTTON_CLICKED"]);
		// grab button data which item it is
		IItem item = ItemSlot1.AttachedItem;
		OnItemButtonPress(item);
	}

	private void OnItemSlot1ButtonHover(){
		gameInstance.PlaySFX("MainSceneNode", gameInstance.sfxPaths["BUTTON_HOVER"]);
		ItemSlotText1.Text = ItemSlot1.AttachedItem.Name;
		ItemSlotText1.Visible = true;
	}

	private void OnItemSlot1ButtonHoverExit(){
		ItemSlotText1.Visible = false;
	}

	public void OnItemSlot2ButtonPress() {
		GD.Print("Item 2 pressed");
		gameInstance.PlaySFX("MainSceneNode", gameInstance.sfxPaths["BUTTON_CLICKED"]);
		// grab button data which item it is
		IItem item = ItemSlot2.AttachedItem;
		OnItemButtonPress(item);
	}

	private void OnItemSlot2ButtonHover(){
		gameInstance.PlaySFX("MainSceneNode", gameInstance.sfxPaths["BUTTON_HOVER"]);
		ItemSlotText2.Text = ItemSlot2.AttachedItem.Name;
		ItemSlotText2.Visible = true;
	}

	private void OnItemSlot2ButtonHoverExit(){
		ItemSlotText2.Visible = false;
	}

	public void OnItemSlot3ButtonPress() {
		GD.Print("Item 3 pressed");
		gameInstance.PlaySFX("MainSceneNode", gameInstance.sfxPaths["BUTTON_CLICKED"]);
		// grab button data which item it is
		IItem item = ItemSlot3.AttachedItem;
		OnItemButtonPress(item);
	}

	private void OnItemSlot3ButtonHover(){
		gameInstance.PlaySFX("MainSceneNode", gameInstance.sfxPaths["BUTTON_HOVER"]);
		ItemSlotText3.Text = ItemSlot3.AttachedItem.Name;
		ItemSlotText3.Visible = true;
	}

	private void OnItemSlot3ButtonHoverExit(){
		ItemSlotText3.Visible = false;
	}

	private void OnItemButtonPress(IItem item) {
		var resultUseItem = gameInstance.gameData.Inventory.UseItem(item);

		if (resultUseItem.IsFailure) {
			throw new InvalidProgramException(resultUseItem.Error);
		}

		var resultAttachButtons = MainScene.SetGameUI();

		if (resultAttachButtons.IsFailure) {
			throw new InvalidProgramException(resultAttachButtons.Error);
		}
	}

	public void ChangeCamera() {
		// Switches between the cameras
		if (MainCamera.IsCurrent()) {
			MinigameCamera.MakeCurrent();
		} else {
			MainCamera.MakeCurrent();
		}
	}
	private void _on_player_input_event(Node viewport, InputEvent events, int shape_idx){
		if(events is InputEventMouseButton){
			if (events.IsPressed()){
				gameInstance.PlaySFX("MainSceneNode", gameInstance.sfxPaths["BUTTON_CLICKED"]);
				ExitBedText.Visible = false;
				player.controllable = true;
				GuessInputField.Editable = false;
				dogBed.interactable = true;
			}
		}
	}

	private void OnEnterKeySubmit() {
		if (Input.IsActionJustPressed("EnterKey") && GuessInputField.Editable && !player.controllable) {
			OnSubmitGuessButtonPress();
		}
	}

	private void OnHowToButtonPressed(){
		gameInstance.PlaySFX("MainSceneNode", gameInstance.sfxPaths["BUTTON_CLICKED"]);
		HowToOverlay.Visible = true;
		GetTree().Paused = true;
	}
	private void OnHowToButtonHover(){
		gameInstance.PlaySFX("MainSceneNode", gameInstance.sfxPaths["BUTTON_HOVER"]);

	}
	private void OnHowToExitButtonPressed(){
		gameInstance.PlaySFX("MainSceneNode", gameInstance.sfxPaths["BUTTON_CLICKED"]);
		HowToOverlay.Visible = false;
		GetTree().Paused = false;
	}
	private void OnHowToExitButtonHover(){
		gameInstance.PlaySFX("MainSceneNode", gameInstance.sfxPaths["BUTTON_HOVER"]);
	}
	private void OnHowToItemButtonPressed(){
		gameInstance.PlaySFX("MainSceneNode", gameInstance.sfxPaths["BUTTON_CLICKED"]);
		HowToItemOverlay.Visible = true;
		GetTree().Paused = true;
	}
	private void OnHowToItemExitButtonPressed(){
		gameInstance.PlaySFX("MainSceneNode", gameInstance.sfxPaths["BUTTON_CLICKED"]);
		HowToItemOverlay.Visible = false;
		GetTree().Paused = false;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		OnEnterKeySubmit();
	}
}
