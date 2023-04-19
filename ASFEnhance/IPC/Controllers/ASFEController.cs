using ArchiSteamFarm.IPC.Controllers.Api;
using Microsoft.AspNetCore.Mvc;

namespace ASFEnhance.IPC.Controllers;

/// <summary>
/// 基础控制器
/// </summary>
[Route("/Api/ASFEnhance", Name = nameof(ASFEnhance))]
public class ASFEController : ArchiController { }
