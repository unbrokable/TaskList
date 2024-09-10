namespace TaskList.Domain.Exceptions;

public class ForbiddenException : Exception
{
    public ForbiddenException() : base("Access is forbidden.")
    {
    }
}