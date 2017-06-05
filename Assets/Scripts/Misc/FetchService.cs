using System.IO;
using Newtonsoft.Json;
using UnityEngine;

namespace TicTacToe3D
{
    public interface IFetchService<T>
    {
        T Load(string fileName);
        void Save(T instance, string fileName);
    }
    
    public class FetchServiceBase<T> : IFetchService<T> where T : class
    {
        public virtual T Load(string fileName)
        {
            T entity;
            using (var streamReader = File.OpenText(Application.dataPath + "/" + fileName))
            using (JsonReader reader = new JsonTextReader(streamReader))
            {
                var serializer = new JsonSerializer();
                entity = serializer.Deserialize<T>(reader);
            }
            return entity;
        }

        public virtual void Save(T instance, string fileName)
        {
            using (var fileStream = File.CreateText(Application.dataPath + "/" + fileName))
            using (JsonWriter writer = new JsonTextWriter(fileStream))
            {
                var serializer = new JsonSerializer
                {
                    Formatting = Formatting.Indented,
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                };
                serializer.Serialize(writer, instance);
            }
        }
    }
    
    public class HistoryFetchService : FetchServiceBase<History>
    {
        public override History Load(string fileName)
        {
            return base.Load("Saves/" + fileName + ".json");
        }

        public override void Save(History instance, string fileName)
        {
            var dir = new DirectoryInfo(Application.dataPath + "/Saves/");
            if (dir.Exists == false)
            {
                dir.Create();
            }
            base.Save(instance, "Saves/" + fileName + ".json");
        }
    }
    
    public class StatsFetchService : FetchServiceBase<Stats>
    {
        public override Stats Load(string fileName)
        {
            return base.Load("Stats/" + fileName + ".json");
        }

        public override void Save(Stats instance, string fileName)
        {
            var dir = new DirectoryInfo(Application.dataPath + "/Stats/");
            if (dir.Exists == false)
            {
                dir.Create();
            }
            base.Save(instance, "Stats/" + fileName + ".json");
        }
    }
}