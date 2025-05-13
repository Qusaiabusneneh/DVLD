using clsPeopleDataAccess;
using DVLD_Buisness;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_BuisnessLayer
{
    public class clsApplication
    {
        public enum enMode { enAddNew=0,enUpdate=1}
        public enum enApplicationType { enNewDrivingLicense=1,enRenewDrivingLicense=2,enReplaceLostDrivingLicense=3,
        enReplaceDamagedDrivingLicense=4,enReleaseDetainedDrivingLicense=5,enNewInternationalLicense=6,enRetakeTest=7}
        public enum enApplicationStatus { enNew=1,enCancelled=2,enCompleted=3}

        public enMode Mode;
        public int ApplicationID { set; get; }
        public int ApplicantPersonID { set; get; }
        public clsPerson PersonInfo;
        public string ApplicantFullName
        {
            get { return clsPerson.Find(ApplicantPersonID).FullName; }
        }
        public DateTime ApplicationDate { set; get; }
        public int ApplicationTypeID { set; get; }
        public clsApplicationTypes ApplicationTypeInfo;
        public enApplicationStatus ApplicationStatus { set; get; }
        public string StatusText
        {
            get
            {
                switch(ApplicationStatus)
                {
                    case enApplicationStatus.enNew:
                        return "New";
                    case enApplicationStatus.enCancelled:
                        return "Cancelled";
                    case enApplicationStatus.enCompleted:
                        return "Completed";
                    default:
                        return "UnKnown"; 
                }
            }
        }
        public DateTime LastStatusDate { set; get; }
        public decimal PaidFees {  set; get; }
        public int CreatedByUserID {  set; get; }
        public clsUser CreatedByUserInfo;
        public clsApplication()
        {
            this.ApplicationID = -1;
            this.ApplicantPersonID = -1;
            this.ApplicationDate = DateTime.Now;
            this.ApplicationTypeID = -1;
            this.ApplicationStatus = enApplicationStatus.enNew;
            this.LastStatusDate=DateTime.Now;
            this.PaidFees = 0;
            this.CreatedByUserID = -1;

            Mode = enMode.enAddNew;
        }
        public clsApplication(int ApplicationID,int ApplicantPersonID,DateTime ApplicationDate,int ApplicationTypeID,
            enApplicationStatus ApplicationStatus,DateTime LastStatusDate,decimal PaidFees,int CreatedByUserID)
        {
            this.ApplicationID=ApplicationID;
            this.ApplicantPersonID=ApplicantPersonID;
            PersonInfo = clsPerson.Find(ApplicantPersonID);
            this.ApplicationDate=ApplicationDate;
            this.ApplicationTypeID=ApplicationTypeID;
            this.ApplicationTypeInfo = clsApplicationTypes.Find(ApplicationTypeID);
            this.ApplicationStatus=ApplicationStatus;
            this.LastStatusDate=LastStatusDate;
            this.PaidFees=PaidFees;
            this.CreatedByUserID=CreatedByUserID;
            this.CreatedByUserInfo = clsUser.FindByUserID(CreatedByUserID);

            Mode = enMode.enUpdate;
        }
        private bool _AddNewApplication()
        {
            this.ApplicationID = clsApplicationDataAccess.AddNewApplication(this.ApplicantPersonID, this.ApplicationDate, this.ApplicationTypeID,
               (byte)this.ApplicationStatus, this.LastStatusDate, this.PaidFees, this.CreatedByUserID);
            return (this.ApplicationID != -1);
        }
        private bool _UpdateApplication()
        {
            return clsApplicationDataAccess.UpdateApplication(this.ApplicationID, this.ApplicantPersonID, this.ApplicationDate, this.ApplicationTypeID,
                (byte)this.ApplicationStatus, this.LastStatusDate, this.PaidFees, this.CreatedByUserID);
        }
        public static clsApplication FindBaseApplication(int ApplicationID)
        {
            int ApplicantPersonID = -1, ApplicationTypeID = -1, CreatedByUserID = -1;
            DateTime ApplicationDate = DateTime.Now, LastStatusDate = DateTime.Now;
            byte ApplicationStatus = 1;
            decimal PaidFees = 0;

            bool isFound = clsApplicationDataAccess.GetApplicationInfoByID(ApplicationID, ref ApplicantPersonID,ref ApplicationDate,
                ref ApplicationTypeID, ref ApplicationStatus, ref LastStatusDate, ref PaidFees,
                ref CreatedByUserID);

            if (isFound)
            {
                return new clsApplication(ApplicationID, ApplicantPersonID, ApplicationDate, ApplicationTypeID, (enApplicationStatus)ApplicationStatus,
                    LastStatusDate, PaidFees, CreatedByUserID);
            }
            else
                return null;

        }
        public  bool Cancel()
        {
            return clsApplicationDataAccess.UpdateSatus(this.ApplicationID, (int)enApplicationStatus.enCancelled);
        }
        public bool SetComplete()
        {
            return clsApplicationDataAccess.UpdateSatus(this.ApplicationID, (int)enApplicationStatus.enCompleted);
        }
        public bool Save()
        {
            switch(Mode)
            {
                case enMode.enAddNew:
                    {
                        if (_AddNewApplication())
                        {
                            Mode = enMode.enUpdate;
                            return true;
                        }
                        else
                            return false;
                    }
                case enMode.enUpdate:
                    _UpdateApplication();
                    break;
            }
            return false;
        }
        public bool Delete()
        {
            return clsApplicationDataAccess.DeleteApplication(this.ApplicationID);
        }
        public static bool IsApplicationExist(int ApplicationID)
        {
            return clsApplicationDataAccess.IsApplicationExist(ApplicationID);
        }
        public static bool DoesPersonHaveActiveApplication(int PersonID,int ApplicationTypeID)
        {
            return clsApplicationDataAccess.DoesPersonHaveActiveApplication(PersonID, ApplicationTypeID);
        }
        public  bool DoesPersonHaveActiveApplication(int ApplicationTypeID)
        {
            return clsApplicationDataAccess.DoesPersonHaveActiveApplication(this.ApplicantPersonID, ApplicationTypeID);
        }
        public static int GetActiveApplicationIDForLicenseClass(int PersonID,clsApplication.enApplicationType ApplicationTypeID,int LicenseClassID)
        {
            return clsApplicationDataAccess.GetActiveApplicationIDForLicenseClass(PersonID,(int)ApplicationTypeID, LicenseClassID);
        }
        public int GetActiveApplicationID(int PersonID,clsApplication.enApplicationStatus ApplicationTypeID)
        {
            return clsApplicationDataAccess.GetActiveApplicationID(PersonID,(int)ApplicationTypeID);
        }
        public int GetActiveApplicationID(clsApplication.enApplicationStatus ApplicationTypeID)
        {
            return GetActiveApplicationID(this.ApplicantPersonID, ApplicationTypeID);
        }

    }
}
