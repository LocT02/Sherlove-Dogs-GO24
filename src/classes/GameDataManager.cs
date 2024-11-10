using Godot;
using Godot.Collections;
using IItemsTypes;
using InventoryManager;
using ItemTypes;
using System.IO;
using ResultManager;
using System.Reflection.Metadata.Ecma335;
using System;

namespace GameData {
	public partial class GameDataManager : Node
	{
		const int DEFAULT_HP = 100;
		private string SaveFilePath = ProjectSettings.GlobalizePath("user://");
		private int Hp, Score;
		public Inventory Inventory = new();

		public GameDataManager() {
			if (!Godot.FileAccess.FileExists(Path.Join(SaveFilePath, "GameData.json"))){
				GD.Print("Save file does not exist, creating new save");
				CreateNewSave(SaveFilePath);
			}
		}

		//Create base dictionary with default values
		public Result CreateNewSave(string filePath) {
			Dictionary content = new()
			{
				{ "Hp", DEFAULT_HP },
				{ "Score", 0 },
				{ "Inventory", InventoryListToArray(new Inventory()).Value }
			};

			string json = Json.Stringify(content);
			try {
				WriteSaveData(json, filePath);
				return Result.Success();
			} catch (Exception e) {
				return Result.Failure(e.Message);
			}
		}

		private Result LoadSaveData(string filePath) {
			Dictionary content = JsonToDictionary(filePath).Value;

			if (content == null) {
				return Result.Failure("LoadFile Returned Empty");
			}

			Hp = (int)content["Hp"];
			Score = (int)content["Score"];
			Array<string> tempInventory = (Array<string>)content["Inventory"];

			//Inventory -> System.Dictionary<IItemTypes,int> -> IItemTypes -> ItemA, ItemB, ItemC -> name
			foreach (string itemName in tempInventory){
				IItem item = null;

				switch(itemName){
					case "ItemA":
						item = new ItemA();
						break;
					case "ItemB":
						item = new ItemB();
						break;
					case "ItemC":
						item = new ItemC();
						break;
					default: 
						GD.Print("Unknown Item Loaded");
						break;
				}

				if (item != null) {
					Inventory.AddItem(item);
				}
			}
			return Result.Success();
		}

		//Helper method to do actual writing
		private static Result WriteSaveData(string json, string filePath) {
			if(!Directory.Exists(filePath)){
				Directory.CreateDirectory(filePath);
			}
			string path = Path.Join(filePath, "GameData.json");
			try{
				File.WriteAllText(path, json);
				return Result.Success();
			}
			catch (Exception e) {
				return Result.Failure(e.Message);
			}
		}

		//Rewrite save file with current Data (at time of function call)
		public void SaveGame(string filePath) {
			// Data to be Saved
			Dictionary content = new()
			{
				{ "Hp", Hp },
				{ "Score", Score },
				{ "Inventory", InventoryListToArray(Inventory).Value }
			};

			string json = Json.Stringify(content);
			WriteSaveData(json, filePath);
		}

		private static Result<string> LoadDataFromFile(string filePath){
			string data;
			try{
				data = File.ReadAllText(filePath);
			}
			catch (Exception e) {
				return Result.Failure<string>(e.Message);
			}
			return Result.Success(data);
		}

		//String containing json content -> Dictionary (without item types)
		private static Result<Dictionary> JsonToDictionary(string filePath){
			string content = LoadDataFromFile(Path.Join(filePath, "GameData.json")).Value;
			Json jsonLoader = new();
			Error loadError = jsonLoader.Parse(content);

			return (loadError != Error.Ok)
			? Result.Failure<Dictionary>(loadError.ToString()) 
			: Result.Success((Dictionary)jsonLoader.Data);
		}

		//Convert Inventory.Items -> Array of Strings for json serialization
		private static Result<Array<string>> InventoryListToArray(Inventory inventory){
			Array<string> temp = new Array<string>();
			foreach(IItem items in inventory.Items){
				temp.Add(items.Name);
			}
			return Result.Success(temp);
		}

		public void SetHp(int hp){
			Hp=hp;
		}

		public int GetHp(){
			return Hp;
		}

		public void SetScore(int score){
			Score = score;
		}
		
		public int GetScore(){
			return Score;
		}

		public Result ChangeHp(int increment) {
			Hp += increment;
			if (Hp <= 0) {
				GameManager.GameManager.Instance.EndGame();
			}
			return Result.Success();
		}
		
		public Result<int> ChangeScore(int increment) {
			Score += increment;
			return Result.Success(Score);
		}
		
	}
}
