using System.Collections.Generic;

namespace Interfaces
{
    public interface IChairManager
    {
        IReadOnlyList<IChair> Chairs { get; }

        void Register(IChair chair);
    }
}