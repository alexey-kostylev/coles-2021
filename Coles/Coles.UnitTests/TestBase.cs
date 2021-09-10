using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Coles.Api.Mapping;

namespace Coles.UnitTests
{
    public class TestBase
    {
        protected IMapper Mapper { get; }

        public TestBase()
        {
            var config = new AutoMapper.MapperConfiguration(cfg => cfg.AddMaps(typeof(MapperProfile).Assembly));
            config.AssertConfigurationIsValid();

            Mapper = config.CreateMapper();
        }
    }
}
