namespace FakeRank.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    using Exiled.API.Features;
    using Exiled.API.Features.Pools;

    using YamlDotNet.Serialization;
    using YamlDotNet.Serialization.NamingConventions;

    using static FakeRank;

    /// <summary>
    /// Custom FakeRank Config (fakeRanks.yml) 
    /// </summary>
    public static class FakeRankStorage
    {
        private static readonly ISerializer Serializer = new SerializerBuilder()
            .WithNamingConvention(UnderscoredNamingConvention.Instance)
            .IgnoreFields()
            .Build();

        private static readonly IDeserializer Deserializer = new DeserializerBuilder()
            .WithNamingConvention(UnderscoredNamingConvention.Instance)
            .IgnoreFields()
            .IgnoreUnmatchedProperties()
            .Build();


        public static Dictionary<string, Dictionary<string, string>> Storage { get; internal set; } = new();


        public static void Create()
        {
            if (!Directory.Exists(Instance.Config.Folder))
            {
                Log.Warn($"FakeRank directory at {Instance.Config.Folder} is missing, creating.");
                Directory.CreateDirectory(Instance.Config.Folder);
            }

            if (!File.Exists(Instance.Config.FullPath))
            {
                Log.Warn($"FakeRank file at {Instance.Config.FullPath} is missing, creating.");

                File.WriteAllText(Instance.Config.FullPath, "");

                Storage.Add("ID@steam",
                        new Dictionary<string, string>()
                            {
                                { "color", "red" },
                                { "value", "The Administrator" }
                            }
                    );
                Save();
            }
        }

        public static void Reload()
        {
            try
            {
                Storage = Deserializer.Deserialize<Dictionary<string, Dictionary<string, string>>>(File.ReadAllText(Instance.Config.FullPath)) ?? DictionaryPool<string, Dictionary<string, string>>.Pool.Get();
            }
            catch (Exception e)
            {
                Log.Error($"Unable to parse config:\n{e}.\nMake sure your config file is setup correctly.");
            }
        }

        public static void Save() => File.WriteAllText(Instance.Config.FullPath, Serializer.Serialize(Storage));
    }
}