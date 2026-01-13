namespace BB.CR.Providers.Extensions
{
    public static class JsonConvertExtension
    {
        public static T? ToObject<T>(string json)
        {
            if (string.IsNullOrEmpty(Settings.KeySecurity))
                return default;

            string key = Settings.KeySecurity;
            var content = Algoris.Decrypt(key, json);
            var deserialize = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(content);

            return deserialize;
        }

        public static string? ToJson<T>(T? obj)
        {
            if (string.IsNullOrEmpty(Settings.KeySecurity))
                return null;

            string key = Settings.KeySecurity;
            var serialize = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
            var hashed = Algoris.Encrypt(key, serialize);

            return hashed;
        }
    }
}
