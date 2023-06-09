using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHealth
{

    interface IDeleteable<T>
    {
        public event Action<T>? OnDelete;
    }
}
