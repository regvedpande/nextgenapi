using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StoreApi.Models;
using StoreApi.Services.Interfaces;
using System;
using System.Linq;
using System.Security.Claims;

namespace StoreApi.Controllers
{
    [Authorize]  // Only requires the user to be authenticated
    [ApiController]
    [Route("api/[controller]")]
    public class StoresController : ControllerBase
    {
        private readonly IStoreService _storeService;

        public StoresController(IStoreService storeService)
        {
            _storeService = storeService;
        }

        [HttpGet]
        public IActionResult GetStores(int pageNumber = 1, int pageSize = 10)
        {
            var stores = _storeService.GetStores(pageNumber, pageSize);
            return Ok(stores);
        }

        [HttpGet("{id}")]
        public IActionResult GetStore(int id)
        {
            var store = _storeService.GetStoreById(id);
            if (store == null)
            {
                return NotFound();
            }
            return Ok(store);
        }

        [HttpPost]
        public IActionResult CreateStore(Store store)
        {
            // Automatically set CreatedBy from the authenticated user's id
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized();
            }
            store.CreatedBy = Convert.ToInt32(userIdClaim.Value);

            _storeService.CreateStore(store);
            return Ok();
        }

        [HttpPut("{id}")]
        public IActionResult UpdateStore(int id, Store store)
        {
            _storeService.UpdateStore(id, store);
            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteStore(int id)
        {
            _storeService.DeleteStore(id);
            return Ok();
        }
    }
}
