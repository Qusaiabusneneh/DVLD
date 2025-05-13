using clsPeopleDataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_BuisnessLayer
{
    public class clsApplicationTypes
    {
        public enum enMode { AddNew=0,Update=1}
        public enMode Mode=enMode.AddNew;
        public int ApplicationTypeID { set; get; }
        public string ApplicationTypeTitle { set; get; }
        public decimal ApplicationTypeFees { set; get; }
        public clsApplicationTypes()
        {
            ApplicationTypeID = -1;
            ApplicationTypeTitle= string.Empty;
            ApplicationTypeFees = 0;
            Mode=enMode.AddNew; 
        }
        public clsApplicationTypes(int ApplicationTypeID,string ApplicationTypeTitle,decimal ApplicationTypeFees)
        {
            this.ApplicationTypeID = ApplicationTypeID;
            this.ApplicationTypeTitle= ApplicationTypeTitle;
            this.ApplicationTypeFees= ApplicationTypeFees;
            Mode = enMode.Update;
        }
        public static clsApplicationTypes Find(int ApplicationTypeID)
        {
            string ApplicationTypeTitle = "";
            decimal ApplicationTypeFees = 0;
            if (clsApplicationTypeDataAccess.GetApplicationTypeINFOByID(ApplicationTypeID, ref ApplicationTypeTitle,
                ref ApplicationTypeFees))
                return new clsApplicationTypes(ApplicationTypeID, ApplicationTypeTitle, ApplicationTypeFees);
            else
                return null;
        }
        public static DataTable GetAllApplicationTypes()
        {
            return clsApplicationTypeDataAccess.GetAllApplicationTypes();
        }
        private bool _UpdateApplicationType()
        {
            return clsApplicationTypeDataAccess.UpdateApplicationTypes(this.ApplicationTypeID,this.ApplicationTypeTitle,this.ApplicationTypeFees);
        }
        private bool _AddNewApplicationType()
        {
            this.ApplicationTypeID = clsApplicationTypeDataAccess.AddNewApplicationType(this.ApplicationTypeTitle, this.ApplicationTypeFees);
            return (this.ApplicationTypeID != -1);
        }
        public bool Save()
        {
            switch(Mode)
            {
                case enMode.AddNew:
                    {
                        if (_AddNewApplicationType())
                        {
                            Mode = enMode.Update;
                            return true;
                        }
                        else
                            return false;
                    }
                case enMode.Update:
                    {
                        return _UpdateApplicationType();
                    }
            }
            return false;
        }
    }
}
