using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeedbackSoftware.Classes.Dtos
{
	public class UserDto
	{
		public int UserID { get; set; }
		public string Name { get; set; }
		public string Passwort { get; set; }
		public string Rolle { get; set; }
	}
}
