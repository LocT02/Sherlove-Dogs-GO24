using Godot;
using Godot.Collections;
using IItemsTypes;
using InventoryManager;
using ItemTypes;
using System.IO;
using ResultManager;
using System;
using IGameDataManager;

namespace GameData {
	public partial class GameDataManager : Node, _IGameDataManager
	{
		const int DEFAULT_HP = 100;
		private string SaveFileDirPath = ProjectSettings.GlobalizePath("user://");
		private string SaveFilePath = Path.Join(ProjectSettings.GlobalizePath("user://"), "GameData.json");
		private int _Hp, _Score;
		public Inventory Inventory { get; }= new();

		public GameDataManager() {
			if (!Godot.FileAccess.FileExists(SaveFilePath)) {
				GD.Print("Save file does not exist, creating new save");
				CreateNewSave();
			}
		}

		//Create base dictionary with default values
		public Result CreateNewSave() {
			Dictionary content = new()
			{
				{ "Hp", DEFAULT_HP },
				{ "Score", 0 },
				{ "Inventory", InventoryListToArray(new Inventory()).Value }
			};

			string json = Json.Stringify(content);
			try {
				WriteSaveData(json);
				return Result.Success();
			} catch (Exception e) {
				return Result.Failure(e.Message);
			}
		}

		public Result LoadSaveData() {
			Dictionary content = JsonToDictionary(SaveFilePath).Value;

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
		private Result WriteSaveData(string json) {

			if(!Directory.Exists(SaveFileDirPath)){
				Directory.CreateDirectory(SaveFileDirPath);
			}

			try{
				File.WriteAllText(SaveFilePath, json);
				return Result.Success();
			}
			catch (Exception e) {
				return Result.Failure(e.Message);
			}
		}

		//Rewrite save file with current Data (at time of function call)
		public Result SaveGame() {
			// Data to be Saved
			Dictionary content = new()
			{
				{ "Hp", Hp },
				{ "Score", Score },
				{ "Inventory", InventoryListToArray(Inventory).Value }
			};

			string json = Json.Stringify(content);
			WriteSaveData(json);
			return Result.Success();
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
		public static Result<Dictionary> JsonToDictionary(string filePath) {
			string content = LoadDataFromFile(filePath).Value;
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

		public int Hp {
			get { return _Hp; }
			set { _Hp = value; }
		}

		public int Score {
			get { return _Score; }
			set { _Score = value; }
		}

		public Result<int> ChangeHp(int increment) {
			Hp += increment;
			return Result.Success(Hp);
		}
		
		public Result ChangeScore(int increment) {
			Score = Math.Max(0, Score + increment);
            return Result.Success();
		}
	}
}
