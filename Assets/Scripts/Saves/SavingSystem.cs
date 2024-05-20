using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SavingSystem
{
    public static void SaveGame(GameScript gameScript, GameCharacter enemy, GameCharacter ball)
    {
        BinaryFormatter formatter = new BinaryFormatter();

        string path = Path.Combine(Application.persistentDataPath, "game.data");
        FileStream stream = new FileStream(path, FileMode.Create);

        GameData data = new GameData(gameScript.Score, enemy.GetLocation(), ball.GetLocation(), gameScript.Lifes, gameScript.WaitForCoin, enemy.MovementSpeed, ball.MovementSpeed, gameScript.CurrentPowerup);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static GameData LoadGame()
    {
        string path = Path.Combine(Application.persistentDataPath, "game.data");

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();

            FileStream stream = new FileStream(path, FileMode.Open);

            GameData data = formatter.Deserialize(stream) as GameData;

            stream.Close();

            return data;
        }
        else
        {
            return null;
        }
    }

    public static void DeleteData()
    {
        string path = Path.Combine(Application.persistentDataPath, "game.data");
        if (File.Exists(path)) File.Delete(path);
    }

    public static void SaveBladesList(List<int> blades)
    {
        BinaryFormatter formatter = new BinaryFormatter();

        string path = Path.Combine(Application.persistentDataPath, "blades.data");
        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, blades);
        stream.Close();
    }

    public static List<int> LoadBlades()
    {
        string path = Path.Combine(Application.persistentDataPath, "blades.data");

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();

            FileStream stream = new FileStream(path, FileMode.Open);

            List<int> blades = formatter.Deserialize(stream) as List<int>;

            stream.Close();

            return blades;
        }
        else
        {
            return null;
        }
    }
}
