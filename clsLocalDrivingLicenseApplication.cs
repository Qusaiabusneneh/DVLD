using clsPeopleDataAccess;
using DVLD_Buisness;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_BuisnessLayer
{
    public class clsLocalDrivingLicenseApplication : clsApplication
    {
        public enum enMode { enAddNew=0,enUpdate=1};
        public enMode Mode=enMode.enAddNew;
         public int LocalDrivingLicenseApplicationID { set; get; }
        public int LicenseClassID { set; get; }
        public clsLicenseClass LicenseClassInfo;
        public string PersonFullName
        {
            get {
                // return clsPerson.Find(ApplicantFullName);
                return clsPerson.Find(ApplicantPersonID).FullName;
                }
        }
        public clsLocalDrivingLicenseApplication()
        {
            this.LocalDrivingLicenseApplicationID = -1;
            this.LicenseClassID = -1;

            Mode = enMode.enAddNew;
        }
        public clsLocalDrivingLicenseApplication(int LocalDrivingLicenseApplicationID,int ApplicationID,int ApplicantPersonID,
            DateTime ApplicaionDate,int ApplicationTypeID,enApplicationStatus ApplicationStatus,
            DateTime LastStatusDate,decimal PaidFees,int CreatedByUserID,int LicenseClassID)
        {
            this.LocalDrivingLicenseApplicationID= LocalDrivingLicenseApplicationID;
            this.ApplicationID= ApplicationID;
            this.ApplicantPersonID= ApplicantPersonID;
            this.ApplicationDate= ApplicaionDate;
            this.ApplicationTypeID= (int)ApplicationTypeID;
            this.ApplicationStatus= ApplicationStatus;
            this.LastStatusDate = LastStatusDate;
            this.PaidFees= PaidFees;
            this.CreatedByUserID= CreatedByUserID;
            this.LicenseClassID = LicenseClassID;
            this.LicenseClassInfo = clsLicenseClass.Find(LicenseClassID);

            Mode = enMode.enUpdate;
        }

        private bool _AddNewLocalDrivingLicenseApplication()
        {
            this.LocalDrivingLicenseApplicationID = clsLocalDrivingLicenseApplicationDataAccess.AddNewLocalDrivingLicense
                (this.ApplicationID, this.LicenseClassID);
            return (this.LocalDrivingLicenseApplicationID != -1);
        }
        private bool _UpdateLocalDrivingLicenseApplication()
        {
            return clsLocalDrivingLicenseApplicationDataAccess.UpdateLocalDrivingLicense
                (this.LocalDrivingLicenseApplicationID, this.ApplicationID, this.LicenseClassID);
        }
        public static clsLocalDrivingLicenseApplication FindByLocalDrivingLicenseApplicationID(int LocalDrivingLicenseApplicationID)
        {
            int ApplicationID=-1, LicenseCLassID=-1;
           bool isFound= clsLocalDrivingLicenseApplicationDataAccess.GetLocalDrivingLicenseApplicationInfoByID
                (LocalDrivingLicenseApplicationID, ref ApplicationID, ref LicenseCLassID);

            if (isFound)
            {
                //we find the base application
                clsApplication Application = clsApplication.FindBaseApplication(ApplicationID);

                return new clsLocalDrivingLicenseApplication
                    (LocalDrivingLicenseApplicationID, Application.ApplicationID, Application.ApplicantPersonID, Application.ApplicationDate,
                    Application.ApplicationTypeID, (enApplicationStatus)Application.ApplicationStatus, Application.LastStatusDate,
                    Application.PaidFees, Application.CreatedByUserID, LicenseCLassID);
            }

            else
                return null;
        }
        public static clsLocalDrivingLicenseApplication FindByApplicationID(int ApplicationID)
        {
            int LocalDrivingLicenseApplicationID=-1, LicenseCLassID=-1;
            bool isFound = clsLocalDrivingLicenseApplicationDataAccess.GetLocalDrivingLicenseInfoByApplicationID
                (ApplicationID, ref LocalDrivingLicenseApplicationID, ref LicenseCLassID);
            if (isFound)
            {
                clsApplication Application = clsApplication.FindBaseApplication(ApplicationID);

                return new clsLocalDrivingLicenseApplication
                    (LocalDrivingLicenseApplicationID, Application.ApplicationID, Application.ApplicantPersonID, Application.ApplicationDate,
                    Application.ApplicationTypeID, (enApplicationStatus)Application.ApplicationStatus, Application.LastStatusDate,
                    Application.PaidFees, Application.CreatedByUserID, LicenseCLassID);
            }

            else
                return null;
        }
        public bool Save()
        {
            //Because of inheritance first we call the save method in the base class,
            //it will take care of adding all information to the application table.
            base.Mode = (clsApplication.enMode)Mode;
            if (!base.Save())
                return false;

            switch(Mode)
            {
                case enMode.enAddNew:
                    if (_AddNewLocalDrivingLicenseApplication())
                    {
                        Mode = enMode.enUpdate;
                        return true;
                    }
                    else
                        return false;

                case enMode.enUpdate:
                    return _UpdateLocalDrivingLicenseApplication();
            }
            return false;
        }
        public static DataTable GetAllLocalDrivingLicneseApplications()
        {
            return clsLocalDrivingLicenseApplicationDataAccess.GetAllGetLocalDrivingLicenseApplications();
        }
        public bool Delete()
        {
            bool IsLocalDrivingApplicationDeleted = false;
            bool IsBaseApplicationDeleted = false;

            IsLocalDrivingApplicationDeleted = clsLocalDrivingLicenseApplicationDataAccess.DeleteLocalDrivingLicense
                (this.LocalDrivingLicenseApplicationID);
            if (!IsLocalDrivingApplicationDeleted)
                return false;

            IsBaseApplicationDeleted = base.Delete();
            return IsBaseApplicationDeleted;
        }
        public bool DoesPassTestType(clsTestTypes.enTestType TestTypeID)
        {
            return clsLocalDrivingLicenseApplicationDataAccess.DoesPassTestType(this.LocalDrivingLicenseApplicationID, (int)TestTypeID);
        }
        public static bool DoesPassTestType(int LocalDrivingLicenseApplicationID,clsTestTypes.enTestType TestTypeID)
        {
            return clsLocalDrivingLicenseApplicationDataAccess.DoesAttendTestType(LocalDrivingLicenseApplicationID, (int)TestTypeID);
        }
        public bool DoesPassPreviousTest(clsTestTypes.enTestType CurrentTestType)
        {
            switch(CurrentTestType)
            {
                case clsTestTypes.enTestType.VisionTest:
                    return true;

                case clsTestTypes.enTestType.WrittenTest:
                    return this.DoesPassTestType(clsTestTypes.enTestType.VisionTest);

                case clsTestTypes.enTestType.StreetTest:
                    return this.DoesPassTestType(clsTestTypes.enTestType.WrittenTest);

                default:
                    return false;
            }
        }
        public  bool DoesAttendTestType(clsTestTypes.enTestType TestTypeID)
        {
            return clsLocalDrivingLicenseApplicationDataAccess.DoesAttendTestType(this.LocalDrivingLicenseApplicationID, (int)TestTypeID);
        }
        public byte TotalTrialsPerTest(clsTestTypes.enTestType TestTypeID)
        {
            return clsLocalDrivingLicenseApplicationDataAccess.TotalTrialPerTest(this.LocalDrivingLicenseApplicationID, (int)TestTypeID);
        }
        public static byte TotalTrialsPerTest(int LocalDrivingLicneseApplicationID,clsTestTypes.enTestType TestTypeID)
        {
            return clsLocalDrivingLicenseApplicationDataAccess.TotalTrialPerTest(LocalDrivingLicneseApplicationID, (int)TestTypeID);
        }
        public static bool AttendedTest(int LocalDrivingLicenseApplicationID,clsTestTypes.enTestType TestTypeID)
        {
            return clsLocalDrivingLicenseApplicationDataAccess.TotalTrialPerTest(LocalDrivingLicenseApplicationID,(int)TestTypeID)>0;
        }
        public bool AttendedTest(clsTestTypes.enTestType TestTypeID)
        {
            return clsLocalDrivingLicenseApplicationDataAccess.TotalTrialPerTest(this.LocalDrivingLicenseApplicationID, (int)TestTypeID) > 0;
        }
        public static bool IsThereAnActiveScheduledTest(int LocalDrivingLicneseApplicationID,clsTestTypes.enTestType TestTypeID)
        {
            return clsLocalDrivingLicenseApplicationDataAccess.IsThereAnActiveScheduledTest(LocalDrivingLicneseApplicationID,(int)TestTypeID);
        }
        public bool IsThereAnActiveScheduledTest(clsTestTypes.enTestType TestTypeID)
        {
            return clsLocalDrivingLicenseApplication.IsThereAnActiveScheduledTest(this.LocalDrivingLicenseApplicationID, TestTypeID);
        }
        public clsTest GetLastTestPerTestType(clsTestTypes.enTestType TestTypeID)
        {
            return clsTest.FindLastTestPerPersonAndLicenseClass(this.ApplicantPersonID, this.LicenseClassID,TestTypeID);
        }
        public static byte GetPassedTestCount(int LocalDrivingLicenseApplicationID)
        {
            return clsTest.GetPassedTestCount(LocalDrivingLicenseApplicationID);
        }
        public byte GetPassedTestCount()
        {
            return clsTest.GetPassedTestCount(this.LocalDrivingLicenseApplicationID);
        }
        public bool PassedAllTest()
        {
            return clsTest.PassedAllTests(this.LocalDrivingLicenseApplicationID);
        }
        public static bool PassedAllTest(int LocalDrivingLicenseApplicationID)
        {
            return clsTest.PassedAllTests(LocalDrivingLicenseApplicationID);
        }
        public int IssueLicneseForFirstTime(string Notes,int CreatedByUserID)
        {
            int DriverID = -1;
            clsDriver Driver = clsDriver.FindByPersonID(this.ApplicantPersonID);
            if (Driver == null)
            {
                Driver = new clsDriver();
                Driver.PersonID = this.ApplicantPersonID;
                Driver.CreatedByUserID = CreatedByUserID;
                if (Driver.Save())
                {
                    DriverID = Driver.PersonID;
                }
                else
                    return -1;
            }

            else
                DriverID = Driver.DriverID;

            clsLicense License = new clsLicense();
            License.ApplicationID = this.ApplicationID;
            License.DriverID = DriverID;
            License.LicenseClass = this.LicenseClassID;
            License.IssueDate = DateTime.Now;
            License.ExpirationDate = DateTime.Now.AddYears(this.LicenseClassInfo.DefaultValidityLength);
            License.Notes= Notes;
            License.PaidFees = this.LicenseClassInfo.ClassFees;
            License.IsActive= true;
            License.IssueReason = clsLicense.enIssueReason.enFirstTime;
            License.CreatedByUserID= CreatedByUserID;

            if (License.Save())
            {
                //now we should set the application status to complete.
                this.SetComplete();
                return License.LicenseID;
            }
            else
                return -1;
           
        }
        public int GetActiveLicenseID()
        {
            //this will get the license id that belongs to this application
            return clsLicense.GetActiveLicenseIDByPersonID(this.ApplicantPersonID, this.LicenseClassID);
        }
        public bool IsLicenseIssued()
        {
            return ((GetActiveLicenseID()) != -1);
        }
    }
}
