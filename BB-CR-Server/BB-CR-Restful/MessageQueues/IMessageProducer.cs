namespace BB.CR.Rest.MessageQueues
{
    public interface IMessageProducer
    {
        void Send<T>(T message, string nameAction, ILogger logger);
    }
}
