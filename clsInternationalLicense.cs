using clsPeopleDataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_BuisnessLayer
{
    public class clsInternationalLicense:clsApplication
    {
        public  enum enMode { enAddNew=0,enUpdate=1}
        public enMode Mode=enMode.enAddNew;
        public clsDriver DriverInfo;
        public int InternationalLicenseID { set; get; }
        public int DriverID { set; get; }
        public int IssueUsingLocalLicenseID { set; get; }
        public DateTime IssueDate { set; get; }
        public DateTime ExpirationDate { set; get; }
        public bool IsActive { set; get; }
        public clsInternationalLicense()
        {
            this.ApplicationTypeID = (int)clsApplication.enApplicationType.enNewInternationalLicense;
            this.InternationalLicenseID = -1;
            this.DriverID = -1;
            this.IssueUsingLocalLicenseID = -1;
            this.IssueDate = DateTime.Now;
            this.ExpirationDate= DateTime.Now;
            this.IsActive = true;
            Mode = enMode.enAddNew;
        }
        public clsInternationalLicense(int ApplicationID,int ApplicantPersonID,DateTime ApplicationDate,
            enApplicationStatus ApplicationStatus,DateTime LastStatusDate,
            decimal PaidFees,int CreatedByUserID,
            int InternationalLicenseID,int DriverID,int IssueUsingLocalLicenseID,DateTime IssueDate,DateTime ExpirationDate,bool IsActive)
        {
            base.ApplicationID = ApplicationID;
            base.ApplicantPersonID = ApplicantPersonID;
            base.ApplicationDate = ApplicationDate;
            base.ApplicationStatus = ApplicationStatus;
            base.LastStatusDate = LastStatusDate;
            base.PaidFees= PaidFees;
            base.CreatedByUserID = CreatedByUserID;

            this.InternationalLicenseID= InternationalLicenseID;
            this.ApplicationID= ApplicationID;
            this.DriverID = DriverID;
            this.IssueUsingLocalLicenseID= IssueUsingLocalLicenseID;
            this.IssueDate = IssueDate;
            this.ExpirationDate=ExpirationDate;
            this.IsActive=IsActive;
            this.CreatedByUserID= CreatedByUserID;

            this.DriverInfo = clsDriver.FindByDriverID(this.DriverID);

            Mode = enMode.enUpdate;
        }
        private bool _AddNewInternationalLicense()
        {
            this.InternationalLicenseID = clsInternationalLiceseDataAccess.AddNewInternationalLicense(this.ApplicationID,
                this.DriverID, this.IssueUsingLocalLicenseID, this.IssueDate, this.ExpirationDate, this.IsActive, this.CreatedByUserID);
            return (this.InternationalLicenseID != -1);
        }
        private bool _UpdateInternationalLicense()
        {
            return clsInternationalLiceseDataAccess.UpdateInternationalLicense(this.InternationalLicenseID,this.ApplicationID,
                this.DriverID,this.IssueUsingLocalLicenseID,this.IssueDate,this.ExpirationDate, this.IsActive,this.CreatedByUserID);
        }
        public bool Save()
        {
            base.Mode = (clsApplication.enMode)Mode;
            if (!base.Save())
                return false;

            switch(Mode)
            {
                case enMode.enAddNew:
                    if (_AddNewInternationalLicense())
                    {
                        Mode = enMode.enUpdate;
                        return true;
                    }
                    else
                        return false;

                    case enMode.enUpdate:
                   return _UpdateInternationalLicense();
            }
            return false;
        }
        public static clsInternationalLicense Find(int InternationalLicenseID)
        {
            int DriverID = -1, IssueUsingLocalLicenseID = -1,ApplicationID=-1,CreatedByUserID=-1;
            DateTime IssueDate=DateTime.Now,ExpirationDate=DateTime.Now;
            bool IsActive = false;
            bool isFound = clsInternationalLiceseDataAccess.GetInternationalLiceseInfoByID(InternationalLicenseID,ref ApplicationID,
                ref DriverID,ref IssueUsingLocalLicenseID, ref IssueDate, ref ExpirationDate, ref IsActive, ref CreatedByUserID);
            if (isFound)
            {
                clsApplication Applications = clsApplication.FindBaseApplication(ApplicationID);

                return new clsInternationalLicense(ApplicationID, Applications.ApplicantPersonID, Applications.ApplicationDate,
                    (enApplicationStatus)Applications.ApplicationStatus, Applications.LastStatusDate, Applications.PaidFees, 
                    Applications.CreatedByUserID,InternationalLicenseID, DriverID, IssueUsingLocalLicenseID, 
                    IssueDate, ExpirationDate, IsActive);
            }
            else
                return null;
        }
        public static DataTable GetAllInternationalLicenses()
        {
            return clsInternationalLiceseDataAccess.GetAllInternationalLiceses();
        }
        public static int GetActiveInternationalLicenseIDByDriverID(int DriverID)
        {
            return clsInternationalLiceseDataAccess.GetActiveInternationalLicenseIDByDriverID(DriverID);
        }
        public static DataTable GetDriverInternationalLicenses(int DriverID)
        {
            return clsInternationalLiceseDataAccess.GetDriverInternationalLicense(DriverID);
        }
    }
}
