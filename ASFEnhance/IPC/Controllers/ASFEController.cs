using ArchiSteamFarm.IPC.Controllers.Api;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace ASFEnhance.IPC.Controllers;

/// <summary>
/// 基础控制器
/// </summary>
[Route("/Api/[controller]/[action]")]
public abstract class ASFEController : ArchiController { }
