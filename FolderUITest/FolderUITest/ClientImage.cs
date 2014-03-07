using System;
using System.Collections.Generic;
using System.Text;

namespace FolderUITest
{
    public class ClientImage
    {
        public int Id;
        public string Name;
        public string Provider;
        public int ClientId;
        public int UserId;
        public int OrgId;
        public Category Category;
        public DateTime DateCreated;
        public string FileName;

        public ClientImage(int iId, string iName, string iProvider, int iClientId, int iUserId, int iOrgId, Category iCategory, DateTime iDateCreated, string iFileName)
        {
            Id = iId;
            Name = iName;
            Provider = iProvider;
            ClientId = iClientId;
            UserId = iUserId;
            OrgId = iOrgId;
            Category = iCategory;
            DateCreated = iDateCreated;
            FileName = iFileName;
        }
    }
}
