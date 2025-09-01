using System.ComponentModel.DataAnnotations;

public class Permission
{
    public Guid Id { get; set; }

    [Required]
    public string Resource { get; set; } = string.Empty;

    [Required]
    public string Action { get; set; } = string.Empty;

    public string Name => $"{Resource}:{Action}";
}