using System;

namespace CarlRizk.Assignment_1
{
    public class Beneficiary
    {
        public string Name { get; private set; }
        public Gender Gender { get; private set; }
        public Relationship Relationship { get; private set; }
        public DateTime DateOfBirth { get; private set; }

        public Beneficiary(string name, Gender gender, Relationship relationship, DateTime dateOfBirth)
        {
            Name = name;
            Gender = gender;
            Relationship = relationship;
            DateOfBirth = dateOfBirth;
        }

        public int GetAge()
        {
            int age = DateTime.Now.Year - DateOfBirth.Year;
            if (DateTime.Now.DayOfYear < DateOfBirth.DayOfYear)
                age -= 1;
            return age;
        }
    }
}
