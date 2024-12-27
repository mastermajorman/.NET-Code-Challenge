using System;

namespace CodeChallenge.Models
{
    public class Compensation
    {
        public Employee Employee { get; set; }
        public int Salary { get; set; }
        public DateTime EffectiveDate { get; set; }
        // not requested, but I added sinces it allows compensations to be identified
        public String CompensationId { get; set; }
    }
}
