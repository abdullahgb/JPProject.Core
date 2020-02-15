using AutoMapper;
using System.Collections.Generic;

namespace JPProject.Admin.Application.AutoMapper
{
    public class AdminUiMapperConfiguration
    {
        public static List<Profile> RegisterMappings()
        {
            var cfg = new List<Profile>
            {
                new DomainToViewModelMappingProfile(),
                new ViewModelToDomainMappingProfile()
            };

            return cfg;
        }
    }
}
