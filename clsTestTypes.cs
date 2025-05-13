using clsPeopleDataAccess;
using DevExpress.XtraBars.Docking.Helpers;
using DevExpress.XtraSplashScreen;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_BuisnessLayer
{
    public class clsTestTypes
    {
        private enum enMode { enAddNew=0,enUpdate}
        enMode _Mode=enMode.enAddNew;

        public enum enTestType { VisionTest = 1, WrittenTest = 2, StreetTest = 3 }
        public clsTestTypes.enTestType TestTypeID { set; get; }
        public string TestTypeTitle { set; get; }
        public string TestTypeDescription { set; get;}
        public decimal TestTypeFees { set; get; }
        public clsTestTypes()
        {
            TestTypeID = clsTestTypes.enTestType.VisionTest;
            TestTypeTitle = string.Empty;
            TestTypeDescription = string.Empty;
            TestTypeFees = 0;
            _Mode= enMode.enAddNew;
        }
        public clsTestTypes(clsTestTypes.enTestType TestTypeID,string TestTypeTitle,string TestTypeDescription,decimal TestTypeFees)
        {
            this.TestTypeID= TestTypeID;
            this.TestTypeTitle= TestTypeTitle;
            this.TestTypeDescription= TestTypeDescription;
            this.TestTypeFees = TestTypeFees;
            _Mode = enMode.enUpdate;
        }
        public static clsTestTypes Find(clsTestTypes.enTestType TestTypeID)
        {
            string TestTypeTilte = "", TestTypeDescription = "";
            decimal TestTypeFees = 0;
            bool isFound = clsTestTypeDataAccess.GetTestTypeInfoByID((int)TestTypeID, ref TestTypeTilte, 
                ref TestTypeDescription, ref TestTypeFees);

            if (isFound)
                return new clsTestTypes(TestTypeID, TestTypeTilte, TestTypeDescription, TestTypeFees);
            else
                return null;
        }
        public static DataTable GetAllTestTypes()
        {
            return clsTestTypeDataAccess.GetAllTestTypes();
        }
        private bool _AddNewTestType()
        {
            this.TestTypeID =(clsTestTypes.enTestType)clsTestTypeDataAccess.AddNewTestType(this.TestTypeTitle, this.TestTypeDescription, this.TestTypeFees);
            return this.TestTypeTitle != "";
        }
        private bool _UpdateTestType()
        {
            return clsTestTypeDataAccess.UpdateTestType((int)this.TestTypeID,this.TestTypeTitle,this.TestTypeDescription,this.TestTypeFees);
        }
        public bool Save()
        {
            switch(_Mode)
            {
                case enMode.enAddNew:
                    {
                        if (_AddNewTestType())
                        {
                            _Mode = enMode.enUpdate;
                            return true;
                        }
                        else
                            return false;
                    }

                case enMode.enUpdate:
                    return _UpdateTestType();
            }
            return false;
        }
    }
}
