﻿using GloriaFestasCatalogo.Server.Services.ConfigService;
using GloriaFestasCatalogo.Shared.Dtos.Config;
using GloriaFestasCatalogo.Shared.Utils;
using Microsoft.AspNetCore.Mvc;

namespace GloriaFestasCatalogo.Server.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ConfigController : ControllerBase
	{

		private readonly IConfigService _configService;

		public ConfigController(IConfigService configService)
		{
			_configService = configService;
		}

		[HttpGet]
		public async Task<ActionResult<ServiceResponse<AppConfigDto>>> GetConfig()
		{

			var result = await _configService.GetConfig();

			if (!result.Success)
			{
				return NotFound(result);
			}

			return Ok(result);

		}

	}
}
