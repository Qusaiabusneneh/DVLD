﻿using System;
using System.Data;
namespace DVLD_Buisness
{
    public class clsCountry
    {
        public int ID { set; get; }
        public string CountryName { set; get; }
        public clsCountry()

        {
            this.ID = -1;
            this.CountryName = "";

        }
        private clsCountry(int ID, string CountryName)

        {
            this.ID = ID;
            this.CountryName = CountryName;
        }
        public static clsCountry Find(int ID)
        {
            string CountryName = "";

            if (clsCountriesDataAccess.clsCountriesDataAccess.GetCountryInfoByCountryID(ID, ref CountryName))

                return new clsCountry(ID, CountryName);
            else
                return null;

        }
        public static clsCountry Find(string CountryName)
        {
            int ID = -1;
            if (clsCountriesDataAccess.clsCountriesDataAccess.GetCountryInfoByCountryName(CountryName, ref ID))
                return new clsCountry(ID, CountryName);
            else
                return null;
        }
        public static DataTable GetAllCountries()
        {
            return clsCountriesDataAccess.clsCountriesDataAccess.GetAllCountries();
        }

    }
}
