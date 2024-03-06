using ArchiSteamFarm.IPC.Controllers.Api;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ASFEnhance.IPC.Controllers;

/// <summary>
/// 基础控制器
/// </summary>
[Route("/Api/[controller]/[action]", Name = nameof(ASFEnhance))]
[SwaggerTag(nameof(ASFEnhance))]
public abstract class ASFEController : ArchiController { }
