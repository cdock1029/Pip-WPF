namespace Pip.Model;

public class Treasury
{
    public string? Cusip { get; set; }
    public DateOnly IssueDate { get; set; }
    public DateOnly MaturityDate { get; set; }
}