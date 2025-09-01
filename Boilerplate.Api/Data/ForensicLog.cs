using System;

public class ForensicLog
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    public string Method { get; set; }
    public string Path { get; set; }
    public string QueryString { get; set; }

    public string RequestHeaders { get; set; }
    public string RequestBody { get; set; }

    public int ResponseStatusCode { get; set; }
    public string ResponseHeaders { get; set; }
    public string ResponseBody { get; set; }

    public string? RemoteIp { get; set; }
    public string? UserAgent { get; set; }
    public string? UserId { get; set; }
}