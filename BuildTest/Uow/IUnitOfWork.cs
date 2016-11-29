using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildTest.Uow
{
    public interface IUnitOfWork:IDisposable
    {
        void Cancel();
        void Commit();

    }
}
