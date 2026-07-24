using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaleOrigin.Domain.Persons
{
    public sealed class Person
    {
        public int PersonID { get; private set; }
        public long NationalNum { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Phone { get; private set; }
        public string Email { get; private set; }
        public string Address { get; private set; }
        public char Gender { get; private set; }
        public DateTime DoB { get; private set; }

        public string FullName => $"{FirstName} {LastName}".Trim();

        public Person(
            int PersonID,
            int NationalNum,
            string FirstName,
            string LastName,
            string Phone,
            string Email,
            string Address, 
            char Gender,
            DateTime DoB)
        {
            this.PersonID = PersonID;
            this.FirstName = FirstName;
            this.LastName = LastName;
            this.Phone = Phone;
            this.Email = Email;
            this.Address = Address;
            this.Gender = Gender;
            this.DoB = DoB;
        }

    }
}
