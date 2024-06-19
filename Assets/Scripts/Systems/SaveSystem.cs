using Core;
using System.IO;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;

namespace Systems
{
    public class SaveSystem
    {
        private readonly string _filePath = Application.persistentDataPath + "/player.data";

        public void SavePlayer(Player player)
        {
            var dataStream = new FileStream(_filePath, FileMode.Create);
            var converter = new BinaryFormatter();

            converter.Serialize(dataStream, player);
            dataStream.Close();
        }
        
        public Player LoadPlayer()
        {
            var player = LoadPlayerFromDisk();

            if (player == null)
                player = new Player();
            
            return player;
        }

        private Player LoadPlayerFromDisk()
        {
            if (File.Exists(_filePath))
            {
                FileStream dataStream = new FileStream(_filePath, FileMode.Open);
                BinaryFormatter converter = new BinaryFormatter();
                Player player = converter.Deserialize(dataStream) as Player;

                dataStream.Close();
               
                return player;
            }

            return null;
        }
        
        public void DeleteSave()
        {
            if (File.Exists(_filePath)) 
                File.Delete(_filePath);
        }
    }
}