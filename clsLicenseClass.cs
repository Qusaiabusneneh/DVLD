using clsPeopleDataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_BuisnessLayer
{
    public class clsLicenseClass
    {
        public enum enMode { enAddNew=0,enUpdate=1}
        public enMode Mode = enMode.enAddNew;
        public int LicesneClassID { set; get; }
        public string ClassName { set; get; }
        public string ClassDescription { set; get; }
        public byte MinimumAllowedAge { set; get; }
        public byte DefaultValidityLength { set; get; }
        public decimal ClassFees { set; get; }
        public clsLicenseClass()
        {
            this.LicesneClassID = -1;
            this.ClassName = string.Empty;
            this.ClassDescription = string.Empty;
            this.MinimumAllowedAge = 18;
            this.DefaultValidityLength = 10;
            this.ClassFees = 0;
            Mode = enMode.enAddNew;
        }
        public clsLicenseClass(int LicesneClassID, string ClassName,string ClassDescription,
            byte MinimumAllowedAge,byte DefaultValidityLength,decimal ClassFees)
        {
            this.LicesneClassID= LicesneClassID;
            this.ClassName= ClassName;
            this.ClassDescription= ClassDescription;
            this.MinimumAllowedAge= MinimumAllowedAge;
            this.DefaultValidityLength= DefaultValidityLength;
            this.ClassFees= ClassFees;
            Mode = enMode.enUpdate;
        }
        private bool _AddNewLicenseClass()
        {
            this.LicesneClassID = clsLicenseClassDataAccess.AddNewLicenseClass(this.ClassName, this.ClassDescription, this.MinimumAllowedAge,
                            this.DefaultValidityLength, this.ClassFees);
            return (this.LicesneClassID != -1);
        }
        private bool _UpdateLicenseClass()
        {
            return clsLicenseClassDataAccess.UpdateLicenseClass(this.LicesneClassID, this.ClassName, this.ClassDescription,
                    this.MinimumAllowedAge, this.DefaultValidityLength, this.ClassFees);
        }
        public static clsLicenseClass Find(int LicenseClassID)
        {
            string ClassName = ""; string ClassDescription = "";
            byte MinimumAllowedAge = 18; byte DefaultValidityLength = 10; decimal ClassFees = 0;
            bool isFound = clsLicenseClassDataAccess.GetLicenseClassInfoByID(LicenseClassID, ref ClassName, ref ClassDescription,
                    ref MinimumAllowedAge, ref DefaultValidityLength, ref ClassFees);
            if(isFound)
                return new clsLicenseClass(LicenseClassID, ClassName, ClassDescription,
                        MinimumAllowedAge, DefaultValidityLength, ClassFees);
            else
                return null;
        }
        public static clsLicenseClass Find(string ClassName)
        {
            int LicenseClassID = -1; string ClassDescription = "";
            byte MinimumAllowedAge = 18; byte DefaultValidityLength = 10; decimal ClassFees = 0;
            bool isFound = clsLicenseClassDataAccess.GetLicenseClassInfoByClassName(ClassName, ref LicenseClassID, ref ClassDescription,
                ref MinimumAllowedAge, ref DefaultValidityLength, ref ClassFees);
            if (isFound)
            {
                return new clsLicenseClass(LicenseClassID, ClassName, ClassDescription,
                 MinimumAllowedAge, DefaultValidityLength, ClassFees);
            }
            else
                return null;
        }
        public static DataTable GetAllLicenseClasses()
        {
            return clsLicenseClassDataAccess.GetAllLicenseClasses();
        }
        public bool Save()
        {
            switch (Mode)
            {
                case enMode.enAddNew:
                    if (_AddNewLicenseClass())
                    {
                        Mode = enMode.enUpdate;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                case enMode.enUpdate:
                    return _UpdateLicenseClass();
            }
            return false;
        }
    }
}
