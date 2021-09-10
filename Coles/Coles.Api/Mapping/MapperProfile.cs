using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using apiModels = Coles.Api.Models;
using musicModels = Coles.Api.MusicBrainz.Models;

namespace Coles.Api.Mapping
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<musicModels.Alias, apiModels.Alias>();
            CreateMap<musicModels.Area, apiModels.Area>();
            CreateMap<musicModels.Artist, apiModels.Artist>();
            CreateMap<musicModels.CoverArtArchive, apiModels.CoverArtArchive>();
            CreateMap<musicModels.LifeSpan, apiModels.LifeSpan>();
            CreateMap<musicModels.TextRepresentation, apiModels.TextRepresentation>();
            CreateMap<musicModels.Release, apiModels.Release>();
            CreateMap<musicModels.ReleaseArea, apiModels.ReleaseArea>();
            CreateMap<musicModels.ReleaseEvent, apiModels.ReleaseEvent>();
            CreateMap<musicModels.Tag, apiModels.Tag>();
        }
    }
}
