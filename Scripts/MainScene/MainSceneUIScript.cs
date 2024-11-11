using Godot;
using ResultManager;

public partial class MainSceneUIScript : CanvasLayer
{
	private Label CategoryLabel;
	private Label FeedbackLabel;
	private LineEdit GuessInputField;
	private Button SubmitGuessButton;
	private Button ItemSlot1;
	private Button ItemSlot2;
	private Button ItemSlot3;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		CategoryLabel = GetNode<Label>("CategoryLabel");
		FeedbackLabel = GetNode<Label>("FeedbackLabel");
		GuessInputField = GetNode<LineEdit>("GuessInputField");
		SubmitGuessButton = GetNode<Button>("SubmitGuessButton");
		ItemSlot1 = GetNode<Button>("ItemSlot1");
		ItemSlot2 = GetNode<Button>("ItemSlot2");
		ItemSlot3 = GetNode<Button>("ItemSlot3");

		GD.Print("Successfully Loaded Main Scene UI?");
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
		return Result.Success(GuessInputField.Text);
	}

	public Result ClearPlayerGuess() {
		if (GuessInputField.Text == null || GuessInputField.Text == "") {
			return Result.Failure("Guess Input is Already Empty or Null");
		}
		GuessInputField.Text = "";
		return Result.Success();
	}

	public void OnSubmitGuessButtonPress() {
		GD.Print("Submit pressed");
		// disable button
		// Grab guess input
		// check guess input
		// grab result
		// set result to ui stuff
		// enable button

	}

	public void OnItemSlot1ButtonPress() {
		// grab button data which item it is
		// use item
		// adjust score
		// set feedback to ui
		// move items to the left most unused button?

	}

	public void OnItemSlot2ButtonPress() {

	}

	public void OnItemSlot3ButtonPress() {

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
