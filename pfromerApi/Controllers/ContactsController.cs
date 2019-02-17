using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using pfromerApi.DataModels;
using pfromerApi.Services;

namespace pfromerApi.Controllers
{
    [Consumes("application/json", "multipart/form-data")]
    [Route("api/[controller]")]
    [ApiController]
    public class ContactsController : Controller
    {
        private readonly PersistanceInterface _persistanceLayer;

        public ContactsController(PersistanceInterface persistanceLayer) {

            _persistanceLayer = persistanceLayer;
        }

        /// <summary>
        /// GET contact record by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var result = _persistanceLayer.Load(id);

            if (!result.Success) {
                return NotFound(Json(result));
            }

            return Json(result.QueryResult);
        }

        /// <summary>
        /// GET all contacts for city and state
        /// </summary>
        /// <param name="state"></param>
        /// <param name="city"></param>
        /// <returns></returns>
        [Route("All")]
        [HttpGet]
        public IActionResult All([FromQuery] string state, [FromQuery] string city)
        {
            return Json(_persistanceLayer.GetAllBy(state, city).QueryResult);
        }

        /// <summary>
        /// GET a single contacts by email and/or phone number
        /// </summary>
        /// <param name="email"></param>
        /// <param name="phoneNumber"></param>
        /// <returns></returns>
        [Route("Search")]
        [HttpGet]
        public IActionResult Search([FromQuery] string email, [FromQuery] string phoneNumber)
        {
            var result = _persistanceLayer.Search(email, phoneNumber);

            if (!result.Success){
                return NotFound(Json(result));
            }

            return Json(result.QueryResult);
        }

        /// <summary>
        /// Create a new contact
        /// </summary>
        /// <param name="contact"></param>
        [HttpPost]
        public IActionResult Post([FromBody] Contact contact)
        {
            var result = _persistanceLayer.Save(contact);

            if (!result.Success)
                return BadRequest(Json(result));

            return Created(new Uri($"{Request.Path}/{((Contact)result.QueryResult).Id}", UriKind.Relative), result.QueryResult);
        }

        /// <summary>
        /// Update existing contact by Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="contact"></param>
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Contact contact)
        {
            var result = _persistanceLayer.Update(id, contact);

            if (!result.Success)
                return BadRequest(Json(result));

            return Created(new Uri($"{Request.Path}/{((Contact)result.QueryResult).Id}", UriKind.Relative), result.QueryResult);
        }

        /// <summary>
        /// Delete a contact by id
        /// </summary>
        /// <param name="id"></param>
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var result = _persistanceLayer.Delete(id);

            if (!result.Success)
            {
                return NotFound(Json(result));
            }

            return Json(result.QueryResult);
        }       
    }
}