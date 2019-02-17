using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using pfromerApi.Services;

namespace pfromerApi.Controllers
{

    [Consumes("application/json", "multipart/form-data")]
    [Route("api/[controller]")]
    [ApiController]
    public class PicturesController : Controller
    {

        private readonly PersistanceInterface _persistanceLayer;

        public PicturesController(PersistanceInterface persistanceLayer)
        {

            _persistanceLayer = persistanceLayer;
        }

        /// <summary>
        /// Upload profile picture for a contact
        /// </summary>
        /// <param name="id"></param>
        [HttpPost]
        public IActionResult Post([FromForm]IFormFile file, [FromForm] int contactId)
        {
            var result = _persistanceLayer.SavePicture(contactId, file);

            if (!result.Success)
            {
                return NotFound(Json(result));
            }

            return Json(result.QueryResult);
        }


        /// <summary>
        /// Download a profile picture for a contact
        /// </summary>
        /// <param name="id"></param>
        [HttpGet("{contactId}")]
        public IActionResult Get(int contactId)
        {
            var result = _persistanceLayer.LoadPicture(contactId);

            if (!result.Success)
            {
                return NotFound(Json(result));
            }

            return Json(result.QueryResult);
        }
    }
}