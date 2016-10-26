using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SomeKit.ESENT.TypeInfoHelper;

namespace SomeKit.ESENT.Repository
{
    public abstract class RepositoryGroupBase : IRepositoryGroup
    {
        protected RepositoryGroupBase()
        {
            TypeInfoHelper=new TypeInfoHelper.TypeInfoHelper();
        }
        internal ITypeInfoHelper TypeInfoHelper { get; }
    }
}
