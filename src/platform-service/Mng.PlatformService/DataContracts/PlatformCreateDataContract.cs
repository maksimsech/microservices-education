﻿namespace Mng.PlatformService.DataContracts;

public class PlatformCreateDataContract
{
    public string Name { get; set; } = null!;

    public string Publisher { get; set; } = null!;

    public string Cost { get; set; } = null!;
}