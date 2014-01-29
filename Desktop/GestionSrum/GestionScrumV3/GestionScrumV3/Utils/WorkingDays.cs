using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GestionScrumV3.Utils
{
    public class WorkingDays
    {
        static public DateTime GetEasterDay(int year)
        {
            // Calcul du jour de pâques (algorithme de Oudin (1940))
            //Calcul du nombre d'or - 1
            int intGoldNumber = (int)(year % 19);
            // Année divisé par cent
            int intAnneeDiv100 = (int)(year / 100);
            // intEpacte est = 23 - Epacte (modulo 30)
            int intEpacte = (int)((intAnneeDiv100 - intAnneeDiv100 / 4 - (8 * intAnneeDiv100 + 13) / 25 + (
            19 * intGoldNumber) + 15) % 30);
            //Le nombre de jours à partir du 21 mars pour atteindre la pleine lune Pascale
            int intDaysEquinoxeToMoonFull = (int)(intEpacte - (intEpacte / 28) * (1 - (intEpacte / 28) * (29 / (intEpacte + 1)) * ((21 - intGoldNumber) / 11)));
            //Jour de la semaine pour la pleine lune Pascale (0=dimanche)
            int intWeekDayMoonFull = (int)((year + year / 4 + intDaysEquinoxeToMoonFull +
                  2 - intAnneeDiv100 + intAnneeDiv100 / 4) % 7);
            // Nombre de jours du 21 mars jusqu'au dimanche de ou 
            // avant la pleine lune Pascale (un nombre entre -6 et 28)
            int intDaysEquinoxeBeforeFullMoon = intDaysEquinoxeToMoonFull - intWeekDayMoonFull;
            // mois de pâques
            int intMonthPaques = (int)(3 + (intDaysEquinoxeBeforeFullMoon + 40) / 44);
            // jour de pâques
            int intDayPaques = (int)(intDaysEquinoxeBeforeFullMoon + 28 - 31 * (intMonthPaques / 4));
            // lundi de pâques
            return new DateTime(year, intMonthPaques, intDayPaques).AddDays(1);
        }

        static public List<DateTime> GetNationalHolidays(int year)
        {
            List<DateTime> NationalHoliday = new List<DateTime>();

            // 01 Janvier
            NationalHoliday.Add(new DateTime(year, 1, 1));
            // 01 Mai
            NationalHoliday.Add(new DateTime(year, 5, 1));
            // 08 Mai
            NationalHoliday.Add(new DateTime(year, 5, 8));
            // 14 Juillet
            NationalHoliday.Add(new DateTime(year, 7, 14));
            // 15 Aout
            NationalHoliday.Add(new DateTime(year, 8, 15));
            // 01 Novembre
            NationalHoliday.Add(new DateTime(year, 11, 1));
            // 11 Novembre
            NationalHoliday.Add(new DateTime(year, 11, 11));
            // Noël
            NationalHoliday.Add(new DateTime(year, 12, 25));

            DateTime EasterDay = GetEasterDay(year);
            // Lundi de pâque
            NationalHoliday.Add(EasterDay);
            // Ascension
            NationalHoliday.Add(EasterDay.AddDays(38));
            //Pentecote
            NationalHoliday.Add(EasterDay.AddDays(49));

            return NationalHoliday;
        }

        static public List<DateTime> GetNationalHolidaysBetweenTwoDate(DateTime startDate, DateTime endDate)
        {
            List<DateTime> notWorkingDay = new List<DateTime>();
            while (startDate <= endDate)
            {
                if(!IsWorkingDay(startDate))
                {
                    notWorkingDay.Add(startDate);
                }
                startDate = startDate.AddDays(1);
            }
            return notWorkingDay;
        }

        static public bool IsWorkingDay(DateTime date)
        {
            bool workingDay = true;

            List<DateTime> nationalHolidays = GetNationalHolidays(date.Year);
            if (nationalHolidays.Where(x=>x.ToShortDateString() == date.ToShortDateString()).Count() > 0 
                || date.DayOfWeek == DayOfWeek.Sunday || date.DayOfWeek == DayOfWeek.Saturday)
            {
                workingDay = false;
            }

            return workingDay;
        }

        static public DateTime AddWorkingDays(DateTime startDate, int numberOfDays)
        {
            int nbNotWorkingDays = 0;
            for (int i = 0; i < numberOfDays + nbNotWorkingDays; i++)
            {
                if (!IsWorkingDay(startDate.AddDays(i)))
                {
                    nbNotWorkingDays++;
                }
            }
            return startDate.AddDays((numberOfDays + nbNotWorkingDays) - 1);
        }
    }
}