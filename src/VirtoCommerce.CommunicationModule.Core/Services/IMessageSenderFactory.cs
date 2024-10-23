namespace VirtoCommerce.CommunicationModule.Core.Services;
public interface IMessageSenderFactory
{
    IMessageSender Create(string typeName);
}
