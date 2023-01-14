using System;

namespace Identity.API;

public class TokenValidationResult
{
    public bool IsValid { get; set; }
    
    public Exception Exception { get; set; }
}