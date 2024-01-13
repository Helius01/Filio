using Microsoft.EntityFrameworkCore;

namespace Filio.Api.Data;

/// <summary>
/// 
/// </summary>
public class ResilientTransaction
{
    private readonly DataContext _context;
    private ResilientTransaction(DataContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    /// <summary>
    /// Creates a new resilient transaction
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public static ResilientTransaction Create(DataContext context) => new(context);

    /// <summary>
    /// Executes the action
    /// </summary>
    /// <param name="action"></param>
    /// <returns></returns>
    public async Task ExecuteAsync(Func<Task> action)
    {
        var strategy = _context.Database.CreateExecutionStrategy();

        await strategy.ExecuteAsync(async () =>
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                await action();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }

            await transaction.CommitAsync();
        });
    }
}