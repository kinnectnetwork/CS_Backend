using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.OData;
using Microsoft.Azure.Mobile.Server;
using Kinnect01Service.DataObjects;
using Kinnect01Service.Models;

namespace Kinnect01Service.Controllers
{
    public class UserProfileController : TableController<UserProfile>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            Kinnect01Context context = new Kinnect01Context();
            DomainManager = new EntityDomainManager<UserProfile>(context, Request);
        }

        // GET tables/UserProfile
        public IQueryable<UserProfile> GetAllUserProfile()
        {
            return Query(); 
        }

        // GET tables/UserProfile/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<UserProfile> GetUserProfile(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/UserProfile/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task<UserProfile> PatchUserProfile(string id, Delta<UserProfile> patch)
        {
             return UpdateAsync(id, patch);
        }

        // POST tables/UserProfile
        public async Task<IHttpActionResult> PostUserProfile(UserProfile item)
        {
            UserProfile current = await InsertAsync(item);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        // DELETE tables/UserProfile/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeleteUserProfile(string id)
        {
             return DeleteAsync(id);
        }
    }
}
