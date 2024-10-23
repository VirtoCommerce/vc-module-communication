using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VirtoCommerce.CommunicationModule.Core;

namespace VirtoCommerce.CommunicationModule.Web.Controllers.Api
{
    [Route("api/communication-module")]
    public class CommunicationModuleController : Controller
    {
        // GET: api/communication-module
        /// <summary>
        /// Get message
        /// </summary>
        /// <remarks>Return "Hello world!" message</remarks>
        [HttpGet]
        [Route("")]
        [Authorize(ModuleConstants.Security.Permissions.Read)]
        public ActionResult<string> Get()
        {
            return Ok(new { result = "Hello world!" });
        }
    }
}
