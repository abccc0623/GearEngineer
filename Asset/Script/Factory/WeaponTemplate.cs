using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Godot;
public partial class WeaponTemplate : RefCounted
{
    private List<List<WeaponPiece>> pieceList;

    public List<List<WeaponPiece>> Get() => pieceList;
    
    public void Create(int sizeX, int sizeY)
    {
        if(pieceList != null)  pieceList.Clear();
        pieceList = new List<List<WeaponPiece>>(sizeX);
        for (var i = 0; i < sizeX; i++)
        {
            var innerList = new List<WeaponPiece>(sizeY);
            for (var j = 0; j < sizeY; j++)
            {
                innerList.Add(new WeaponPiece());
            }
            pieceList.Add(innerList);
        }
    }
    
    public void LoadToJson(string filePath)
    {
        if (!File.Exists(filePath)) return;
        string json = File.ReadAllText(filePath);
        var options = new JsonSerializerOptions {  WriteIndented = true ,  IncludeFields = true };
        pieceList = JsonSerializer.Deserialize<List<List<WeaponPiece>>>(json,options);
    }

    public void SaveToJson(string filePath)
    {
        var options = new JsonSerializerOptions { WriteIndented = true ,  IncludeFields = true};
        string json = JsonSerializer.Serialize(pieceList, options);
        File.WriteAllText(filePath, json);
    }
}