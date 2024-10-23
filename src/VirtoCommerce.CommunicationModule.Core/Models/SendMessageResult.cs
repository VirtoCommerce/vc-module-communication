using System.Collections.Generic;

namespace VirtoCommerce.CommunicationModule.Core.Models;
public class SendMessageResult
{
    public string Status { get; set; }
    public IList<string> Errors { get; set; }

    public int ErrorsCount => Errors.Count;
}
