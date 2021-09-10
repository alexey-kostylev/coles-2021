using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Coles.Api.Models;
using Coles.Api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Coles.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArtistController : ControllerBase
    {
        private readonly IMusicService _musicService;

        public ArtistController(IMusicService musicService)
        {
            _musicService = musicService;
        }

        [HttpGet]
        public Task<ArtistResponse> GetArtists([Required]string artistOrBandName)
        {
            return _musicService.GetArtistsOrReleases(artistOrBandName);
        }
    }
}
