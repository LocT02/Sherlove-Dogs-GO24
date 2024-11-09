using Godot;
using Godot.Collections;
using IItems;
using InventoryManager;
using ItemTypes;
using System.IO;
using Result;

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
			} else {
				var attempt = LoadSaveData(SaveFilePath);
				GD.Print(attempt.Message);
			}
		}

		//Create base dictionary with default values
		public void CreateNewSave(string filePath) {
			Dictionary content = new()
			{
				{ "Hp", DEFAULT_HP },
				{ "Score", 0 },
				{ "Inventory", InventoryListToArray(new Inventory()) }
			};

			string json = Json.Stringify(content);

			WriteSaveData(json, filePath);

			// check if file was successfully created, return file location
		}

		private Result<bool> LoadSaveData(string filePath) {
			Dictionary content = JsonToDictionary(filePath);

			if (content == null) {
				return Result<bool>.Failure("LoadFile Returned Empty");
			}

			Hp = (int)content["Hp"];
			Score = (int)content["Score"];
			Array<string> tempInventory = (Array<string>)content["Inventory"];

			//Inventory -> System.Dictionary<IItemTypes,int> -> IItemTypes -> ItemA, ItemB, ItemC -> name
			foreach (string itemName in tempInventory){
				IItemTypes item = null;

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
			return Result<bool>.Success(true, "Successfully Loaded Save File!");
		}

		//Helper method to do actual writing
		private static void WriteSaveData(string json, string filePath) {
			if(!Directory.Exists(filePath)){
				Directory.CreateDirectory(filePath);
			}
			string path = Path.Join(filePath, "GameData.json");
			try{
				File.WriteAllText(path, json);
			}
			catch{
				throw;
			}
		}

		//Rewrite save file with current Data (at time of function call)
		public void SaveGame(string filePath) {
			// Data to be Saved
			Dictionary content = new()
			{
				{ "Hp", Hp },
				{ "Score", Score },
				{ "Inventory", InventoryListToArray(Inventory) }
			};

			string json = Json.Stringify(content);
			WriteSaveData(json, filePath);
		}

		private static string LoadDataFromFile(string filePath){
			string data = null;
			if(!File.Exists(filePath)) return null;
			try{
				data = File.ReadAllText(filePath);
			}
			catch{

			}
			return data;
		}

		//String containing json content -> Dictionary (without item types)
		private static Dictionary JsonToDictionary(string filePath){
			string content = LoadDataFromFile(Path.Join(filePath, "GameData.json"));
			Json jsonLoader = new Json();
			Error loadError = jsonLoader.Parse(content);
			if(loadError != Error.Ok) {GD.Print("put error code here"); return null;}
			return (Dictionary)jsonLoader.Data;
		}

		//Convert Inventory.Items -> Array of Strings for json serialization
		private static Array<string> InventoryListToArray(Inventory inventory){
			Array<string> temp = new Array<string>();
			foreach(IItemTypes items in inventory.Items){
				temp.Add(items.Name);
			}
			return temp;
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

		public Result<int> ChangeHp(int increment) {
			Hp += increment;
			return (Hp <= 0) 
			? GameManager.GameManager.Instance.EndGame()
			: Result<int>.Success(Hp);
		}
		
		public Result<int> ChangeScore(int increment) {
			Score += increment;
			return Result<int>.Success(Score);
		}
		
	}
}
