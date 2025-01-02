using System.Text.Json;
using System.Text;
using System.Runtime.Serialization.Json;

namespace RabbitMQ.Shared.MessageBus.Helper
{
    public static class JsonSerialization
    {
        public static string SerializeMessage<T>(T message)
        {
            ArgumentNullException.ThrowIfNull(message);

            DataContractJsonSerializer dataContractJsonSerializer = new(message.GetType());
            MemoryStream memoryStream = new();
            dataContractJsonSerializer.WriteObject(memoryStream, message);
            return Encoding.UTF8.GetString(memoryStream.ToArray());
        }

        public static byte[] SerializeMessageInBytes<T>(T message)
        {
            ArgumentNullException.ThrowIfNull(message);

            return Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));
        }


        public static T? DeserializeMessage<T>(string json)
        {
            ArgumentNullException.ThrowIfNull(json);

            T objectInstance = Activator.CreateInstance<T>();
            MemoryStream memoryStream = new(Encoding.Unicode.GetBytes(json));

            ArgumentNullException.ThrowIfNull(objectInstance);

            DataContractJsonSerializer dataContractJsonSerializer = new(objectInstance.GetType());

            var data = dataContractJsonSerializer.ReadObject(memoryStream);

            ArgumentNullException.ThrowIfNull(data);

            objectInstance = (T)data;
            memoryStream.Close();
            return objectInstance;
        }
    }
}
