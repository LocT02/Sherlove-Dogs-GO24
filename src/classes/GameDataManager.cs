using Godot;
using Godot.Collections;
using IItems;
using InventoryManager;
using ItemTypes;
using System;
using System.Collections.Generic;
using System.IO;

namespace GameData {
	public class GameDataManager
	{
		const int DEFAULT_HP = 100;
		private int HP, Score;
		public Inventory Inventory = new Inventory();
		public GameDataManager(string SaveFilePath) {
			if (!Godot.FileAccess.FileExists(Path.Join(SaveFilePath, "GameData.json"))){
				GD.Print("Save file does not exist, creating new save");
				CreateNewSave(SaveFilePath);
			} else {
				LoadSaveData(SaveFilePath);
			}
		}
		//Create base dictionary with default values
		public void CreateNewSave(string filePath) {
			Godot.Collections.Dictionary content = new Godot.Collections.Dictionary();
			content.Add("HP", DEFAULT_HP);
			content.Add("Score", 0);
			content.Add("Inventory", InventoryListToArray(new Inventory()));

			string json = Json.Stringify(content);

			WriteSaveData(json, filePath);
		}

		private void LoadSaveData(string filePath) {
			Godot.Collections.Dictionary content = JsonToDictionary(filePath);
			HP = (int)content["HP"];
			Score = (int)content["Score"];
			Array<String> tempInventory = (Array<String>)content["Inventory"];
			//Inventory -> System.Dictionary<IItemTypes,int> -> IItemTypes -> ItemA, ItemB, ItemC -> name
			foreach (string item in tempInventory){
				switch(item){
					case "ItemA":
						Inventory.Items.Add(new ItemA(item));
						break;
					case "ItemB":
						Inventory.Items.Add(new ItemB(item));
						break;
					case "ItemC":
						Inventory.Items.Add(new ItemC(item));
						break;
					default: 
						GD.Print("Unknown Item Loaded");
						break;
				}
			}
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
			Godot.Collections.Dictionary content = new Godot.Collections.Dictionary();
			content.Add("HP", this.HP);
			content.Add("Score", this.Score);
			content.Add("Inventory", InventoryListToArray(this.Inventory));

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
		private static Godot.Collections.Dictionary JsonToDictionary(string filePath){
			string content = LoadDataFromFile(Path.Join(filePath, "GameData.json"));
			Godot.Json jsonLoader = new Json();
			Error loadError = jsonLoader.Parse(content);
			if(loadError != Error.Ok) {GD.Print("put error code here"); return null;}
			return (Godot.Collections.Dictionary)jsonLoader.Data;
		}
		//Convert Inventory.Items -> Array of Strings for json serialization
		private static Array<String> InventoryListToArray(Inventory inventory){
			Array<String> temp = new Array<string>();
			foreach(IItemTypes items in inventory.Items){
				temp.Add(items.Name);
			}
			return temp;
		}
		public void SetHP(int hp){
			HP=hp;
		}
		public int GetHP(){
			return HP;
		}
		public void SetScore(int score){
			Score = score;
		}
		public int GetScore(){
			return Score;
		}
		
	}
}