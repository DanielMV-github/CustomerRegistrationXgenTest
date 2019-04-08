using System;
using System.Collections.Generic;

namespace CustomerRegistration.Infrastructure
{
    public interface IDao<T>
    {
        bool Create(T objectDomain);

        T Read(Guid id);

        ICollection<T> ReadList();

        bool Update(T objectDomain);

        bool Delete(Guid id);
    }
}
