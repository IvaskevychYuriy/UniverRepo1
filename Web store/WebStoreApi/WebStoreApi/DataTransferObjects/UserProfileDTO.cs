﻿using WebStore.Api.Contracts;

namespace WebStore.Api.DataTransferObjects
{
    public class UserProfileDTO : IDataTransferObject
    {
        public string Email { get; set; }

        public string UserName { get; set; }

        public bool IsAdministrator { get; set; }
    }
}
