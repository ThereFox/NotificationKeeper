using CSharpFunctionalExtensions;
using Domain.Entitys;

namespace App.Stores;

public interface IBlueprintStore
{
    public Task<Result<Blueprint>> Get(Guid id);
}