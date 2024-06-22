using Microsoft.AspNetCore.Identity;

namespace MDLibrary.Domain.Entities
{
	public class LibraryUser : IdentityUser
	{
        public LibraryUser()
        {
            
        }

        public LibraryUser(string name) : base(name)
        {
            
        }
    }
}
