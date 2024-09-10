namespace Ardalis.GuardClauses;

public static class GuardExtention
{
        public static void AgainstForbidden(this IGuardClause guardClause, bool condition)
        {
            if (!condition)
            {
                throw new ForbiddenException();
            }
        }
}

