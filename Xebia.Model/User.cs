using System;
using System.Collections.Generic;
using System.Text;

namespace Xebia.Model
{
    public class User
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public UserRole UserRole { get; set; }
        public bool IsActive { get; set; }
		public DateTime CreatedDate { get; set; }
		public DateTime UpdatedDate { get; set; }
	}

  	public enum UserRole
	{
		Employee = 0,
		Admin = 1
	}

}
