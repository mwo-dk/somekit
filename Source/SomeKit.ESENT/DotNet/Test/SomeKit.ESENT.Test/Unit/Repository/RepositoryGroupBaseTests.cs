using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using SomeKit.ESENT.Repository;

namespace SomeKit.ESENT.Test.Unit
{
    public class RepositoryGroupBaseTests
    {
        #region Members
        #endregion

        #region Test initialization
        #endregion

        #region Type test
        [Fact]
        [Trait("Category", "Unit")]
        public void RepositoryGroupBase_implements_IRepositoryGroup()
        {
            Assert.True(typeof(RepositoryGroupBase).GetInterfaces().Contains(typeof(IRepositoryGroup)));
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void RepositoryGroupBase_is_abstract()
        {
            Assert.True(typeof(RepositoryGroupBase).IsAbstract);
        }
        #endregion

        #region Initialization test

        [Fact]
        [Trait("Category", "Unit")]
        public void Initializing_RepositoryBase_initializes_TypeInfoHelper()
        {
            var sut = new RepositoryMock();

            Assert.NotNull(sut.TypeInfoHelper);
        }
        #endregion

        #region Utility

        public class RepositoryMock : RepositoryGroupBase
        {
            
        }
        #endregion
    }
}
