using Microsoft.AspNetCore.Mvc;

namespace ASFEnhance.IPC.Controllers;

/// <summary>
/// 基础控制器
/// </summary>
[ApiController]
[Produces("application/json")]
public abstract class AbstractController : ControllerBase { }
