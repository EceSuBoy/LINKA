using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Linka.WebUI.Areas.Admin.Controllers
{
    [Authorize(Policy = "AdminOrManager")]
    public abstract class AdminControllerBase
        : Controller
    {
    }
}