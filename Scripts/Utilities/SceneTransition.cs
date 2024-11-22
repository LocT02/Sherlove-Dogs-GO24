using System;
using System.Threading.Tasks;
using Godot;
using ResultManager;

namespace SceneTransitionManager {

	public partial class SceneTransition : CanvasLayer {
		private AnimationPlayer AnimationPlayer;
		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			AnimationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
			if (AnimationPlayer == null) {
				throw new InvalidProgramException("Unable to GetNode AnimationPlayer");
			}

			GD.Print("Scene Transitions Ready");
		}

		// Called every frame. 'delta' is the elapsed time since the previous frame.
		public override void _Process(double delta)
		{
		}

		public async Task<Result> StartFadeInSlow() {
			if (AnimationPlayer == null) {
				return Result.Failure("AnimationPlayer null");
			}
			AnimationPlayer.Play("FadeInSlow");
			await ToSignal(AnimationPlayer, "animation_finished");
			return Result.Success();
		}

		public async Task<Result> ReverseFadeInSlow() {
			if (AnimationPlayer == null) {
				return Result.Failure("AnimationPlayer null");
			}
			AnimationPlayer.PlayBackwards("FadeInSlow");
			await ToSignal(AnimationPlayer, "animation_finished");
			return Result.Success();
		}

		public async Task<Result> StartFadeInFast() {
		if (AnimationPlayer == null) {
			return Result.Failure("AnimationPlayer null");
		}
		AnimationPlayer.Play("FadeInFast");
		await ToSignal(AnimationPlayer, "animation_finished");
		return Result.Success();
		}

		public async Task<Result> ReverseFadeInFast() {
			if (AnimationPlayer == null) {
				return Result.Failure("AnimationPlayer null");
			}
			AnimationPlayer.PlayBackwards("FadeInFast");
			await ToSignal(AnimationPlayer, "animation_finished");
			return Result.Success();
		}
	}
}
