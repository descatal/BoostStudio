namespace BoostStudio.Domain.Common.Interfaces;

public interface IHasDomainEvents
{
    public IReadOnlyCollection<BaseEvent> DomainEvents { get; }

    public void ClearDomainEvents();
}
