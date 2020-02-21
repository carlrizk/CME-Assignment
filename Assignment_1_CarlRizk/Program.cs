using System;
using System.Collections.Generic;
using System.Linq;

namespace CarlRizk.Assignment_1
{
    class Program
    {
        static void Main(string[] args)
        {
            PolicyCollection policies = new PolicyCollection();

            //1.
            Console.WriteLine("1. Create 3 valid policies of different types.");
            policies.AddPolicy(new TravelPolicy(new DateTime(2019, 1, 1), new DateTime(2019, 1, 25), "Beirut", "Paris", false));
            policies.AddPolicy(new MotorPolicy(new DateTime(2019, 1, 1), new DateTime(2020, 1, 1), 20_000));
            policies.AddPolicy(new MedicalPolicy(new DateTime(2019, 1, 1), new DateTime(2020, 1, 1), new List<Beneficiary>{
                new Beneficiary("Carl", Gender.Male, Relationship.Self, new DateTime(1999,2,19)),
                new Beneficiary("John", Gender.Male, Relationship.Son, new DateTime(2019, 1,1)),
                new Beneficiary("Jane", Gender.Female, Relationship.Spouse, new DateTime(1918, 1,1))
            }));
            Console.WriteLine("-----------------------------------------------------------------------------");

            Console.WriteLine();

            //2.
            Console.WriteLine("2. Attempt adding 5 more policies. Ensure that one Travel and one Medical policies fail the validation.");
            policies.AddPolicy(new TravelPolicy(new DateTime(2019, 1, 1), new DateTime(2019, 3, 1), "Beirut", "London", true)); //Fails over 30 days
            policies.AddPolicy(new MedicalPolicy(new DateTime(2019, 1, 1), new DateTime(2020, 1, 1), new List<Beneficiary>()));  //Fails no beneficiaries
            policies.AddPolicy(new MedicalPolicy(new DateTime(2019, 1, 1), new DateTime(2020, 1, 1), new List<Beneficiary> {     //Fails multiple self
                new Beneficiary("Self 1",Gender.Female, Relationship.Self, DateTime.Now.AddYears(-64)),
                new Beneficiary("Self 2", Gender.Male, Relationship.Self, DateTime.Now.AddYears(-64))
            }));
            policies.AddPolicy(new MotorPolicy(new DateTime(2019, 1, 1), new DateTime(2020, 1, 1), 10_000));
            policies.AddPolicy(new TravelPolicy(new DateTime(2019, 2, 1), new DateTime(2019, 2, 25), "Beirut", "London", true));
            Console.WriteLine("-------------------------------------------------------------------------------------");

            Console.WriteLine();

            //4.
            Console.WriteLine("4. Display all available policies (following the Display Template).");
            foreach (Policy _policy in policies)
            {
                Console.WriteLine(_policy);
            }
            Console.WriteLine("-------------------------------------------------------------------------");

            Console.WriteLine();

            //5.
            Console.WriteLine("5. Display all policies whose premium is between 500 USD and 2000 USD.");
            foreach (Policy _policy in policies.Where(
                _pol =>
                {
                    float _premium = _pol.Premium;
                    return _premium >= 500 && _premium <= 2000;
                }))
            {
                Console.WriteLine(_policy);
            }
            Console.WriteLine("----------------------------------------------------------------------------------");

            Console.WriteLine();

            Console.WriteLine("6. Attempt adding 15 claims against the available policies.");
            policies.AddClaim(new Claim(new DateTime(2018, 1, 1), "2019-Travel-0", 250));   //Fail Not active Yet
            policies.AddClaim(new Claim(new DateTime(2020, 1, 1), "2019-Travel-0", 250));   //Fail Expired
            policies.AddClaim(new Claim(new DateTime(2019, 1, 2), "2019-Travel-1", 250));   //Fail 2019-Travel-1 doesnt exist

            policies.AddClaim(new Claim(new DateTime(2019, 1, 2), "2019-Travel-0", 15));
            policies.AddClaim(new Claim(new DateTime(2019, 1, 6), "2019-Travel-0", 55));
            policies.AddClaim(new Claim(new DateTime(2019, 3, 2), "2019-Motor-1", 100));
            policies.AddClaim(new Claim(new DateTime(2019, 7, 8), "2019-Motor-1", 25));
            policies.AddClaim(new Claim(new DateTime(2019, 2, 8), "2019-Motor-1", 125));
            policies.AddClaim(new Claim(new DateTime(2019, 6, 4), "2019-Medical-2", 245));
            policies.AddClaim(new Claim(new DateTime(2019, 9, 10), "2019-Medical-2", 35));
            policies.AddClaim(new Claim(new DateTime(2019, 7, 6), "2019-Medical-2", 375));
            policies.AddClaim(new Claim(new DateTime(2019, 3, 12), "2019-Motor-6", 125));
            policies.AddClaim(new Claim(new DateTime(2019, 11, 12), "2019-Motor-6", 65));
            policies.AddClaim(new Claim(new DateTime(2019, 2, 19), "2019-Travel-7", 450));
            policies.AddClaim(new Claim(new DateTime(2019, 2, 12), "2019-Travel-7", 75));
            Console.WriteLine("-------------------------------------------------------------------------------");

            Console.WriteLine();

            Console.WriteLine("7. Display the following information per policy number");
            foreach (Policy _policy in policies)
            {
                Console.WriteLine("Claim Statistics for {0}:", _policy.PolicyNumber);
                Console.WriteLine(_policy.CalculateClaimsStatistics());
                Console.WriteLine("-----------------------");
            }
            Console.WriteLine("----------------------------------------------------------------------------------");
        }
    }
}
