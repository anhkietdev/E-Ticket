using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Presentation.Extension
{
    public static class BaseControllerExtension
    {
        public static Guid GetAuthorizedUserId(this ControllerBase controllerBase)
        {
            var userId = controllerBase.User.FindFirst(ClaimTypes.NameIdentifier);

            if (userId == null)
                throw new UnauthorizedAccessException("User is not authorized");

            return new Guid(userId.Value);
        }
    }
}
