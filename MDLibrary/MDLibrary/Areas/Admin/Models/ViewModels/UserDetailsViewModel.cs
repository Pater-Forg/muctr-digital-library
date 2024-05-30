using System.Collections.Generic;

namespace MDLibrary.Areas.Admin.Models.ViewModels
{
	public class UserDetailsViewModel
	{
		public string Id { get; set; }
		public string Name { get; set; }
		public string Email { get; set; }
		public bool EmailConfirmed { get; set; }
		public string? PhoneNumber { get; set; }
		public bool PhoneNumberConfirmed { get; set; }
		public IList<RoleCheckboxesViewModel> RoleCheckboxes { get; set; }
	}
}
