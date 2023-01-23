using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using AutoMapper;
using Humanizer;
using System.Linq;

namespace BeyondIT.MicroLoan.Application.Mappings
{
   
    public class AutoMapperConfig
    {
        private static IMapper _mapper;

        public static IMapper CreateMappers()
        {
            if (_mapper != null) return _mapper;

            var config = new MapperConfiguration(cfg =>
            {
                MicroLoanMappings(cfg);
            });

            _mapper = config.CreateMapper();

            return _mapper;

        }
       
      
        public static void MicroLoanMappings(IProfileExpression cfg)
        {
            
        }

    }
}
