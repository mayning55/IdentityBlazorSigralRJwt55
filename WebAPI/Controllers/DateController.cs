using DateClassLibrary;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    /// <summary>
    /// DI依赖注入的方式实现
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="dateInterface"></param>
    [Route("[controller]/[action]")]
    [ApiController]
    public class DateController<T>(IWebApiDataInterface<T> dateInterface) : Controller where T : class
    {
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetItemsAsync() => Ok(await dateInterface.GetItems());

        [HttpDelete]
        [Authorize(Roles = "AdminRole,NormalUser")]
        public async Task<IActionResult> DeleteItemAsync(long id)
        {
            if (id > 0)
            {
                return Ok(await dateInterface.DeleteItem(id));
            }
            return BadRequest();
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetByIdAsync(long id)
        {
            if (id > 0)
            {
                return Ok(await dateInterface.GetById(id));
            }
            return BadRequest();
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> AddItemAsync(T model)
        {
            if (model is not null)
            {
                return Ok(await dateInterface.AddItem(model));
            }
            return BadRequest();
        }
        [HttpPut]
        [Authorize]
        public async Task<IActionResult> EditItemAsync(T model)
        {
            if (model is not null)
            {
                return Ok(await dateInterface.EditItem(model));
            }
            return BadRequest();
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetItemByIdAsync(long id)
        {
            if (id > 0)
            {
                return Ok(await dateInterface.GetItemById(id));
            }
            return BadRequest();
        }

    }

}
