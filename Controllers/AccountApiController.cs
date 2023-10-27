using AccountRoleApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AccountRoleApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountApiController : ControllerBase
    {
        AccountDbContext ctx;
        public AccountApiController(AccountDbContext ctx)
        {
            this.ctx = ctx;
        }
        [HttpGet]
        [Route("login/{email}/{password}")]
        public async Task<ActionResult<Account?>> Login(string email, string password)
        {
            Account? acc = await ctx.Accounts!.SingleOrDefaultAsync(x => x.Email == email && x.Password == password);
            return Ok(acc);
        }
        [HttpPost]
        [Route("create")]
        public async Task<ActionResult<bool>> Create(Account acc)
        {
            try {
                ctx.Accounts!.Add(acc);
                await ctx.SaveChangesAsync();
            }catch(Exception ex) { return BadRequest(); }
            return Ok(true);
        }
        [HttpGet]
        [Route("list")]
        public async Task<ActionResult<List<Account>?>> List()
        {
            return Ok(await ctx.Accounts!.ToListAsync());
        }
        [HttpGet]
        [Route("detail/{email}")]
        public async Task<ActionResult<Account?>> Detail(string? email)
        {
            return Ok(await ctx.Accounts!.FirstOrDefaultAsync(x => x.Email == email));
        }
        [HttpPost]
        [Route("update/{email}")]
        public async Task<ActionResult<bool>> Update(Account acc)
        {
            try { 
                ctx.Entry(acc).State = EntityState.Modified;
                await ctx.SaveChangesAsync();
            }catch(Exception ex) { return BadRequest();} return Ok(true);
        }
        [HttpPost]
        [Route("delete")]
        public async Task<ActionResult<bool>> Delete(Account acc)
        {
            try { 
                if (acc != null)
                {
                    ctx.Entry(acc).State = EntityState.Deleted;
                    await ctx.SaveChangesAsync();
                }else
                {
                    return BadRequest();
                }
            }catch(Exception ex) { return BadRequest(); 
            }
            return Ok(true);
        }
    }
}
