using Newtonsoft.Json;

namespace hsr_museum.src.main.model.structures.items.museum_event
{
    public class Employee
    {
        [JsonProperty("ID")]
        public uint id { get; private set; }
        [JsonProperty("Name")]
        public string name { get; private set; }
        [JsonProperty("TourDuration")]
        public uint tourDuration { get; private set; }
        [JsonProperty("EducationalValue")]
        public uint educationalValue { get; private set; }
        [JsonProperty("VisitorAppeal")]
        public uint visitorAppeal { get; private set; }

        /// <summary>
        /// Constructor for the Employee object
        /// </summary>
        /// <param name="id">the id of the employee</param>
        /// <param name="name">the name of the employee</param>
        /// <param name="tourDuration">parameter 1's value</param>
        /// <param name="educationalValue">parameter 2's value</param>
        /// <param name="visitorAppeal">parameter 3's value</param>
        [JsonConstructor]
        public Employee(uint id, string name, uint tourDuration,
                            uint educationalValue, uint visitorAppeal) {

            this.id = id;
            this.name = name;
            this.tourDuration = tourDuration;
            this.educationalValue = educationalValue;
            this.visitorAppeal = visitorAppeal;

        }

        /// <summary>
        /// constructor for the employee object
        /// this constructor is used when we add a new employee to a player's database
        /// caller searches for the inputted employee in the json database and imports it here
        /// </summary>
        /// <param name="employee">the newly added employee</param>
        public Employee(Employee employee)
        {
            this.id = employee.id;
            this.name = employee.name;
            this.tourDuration = employee.tourDuration;
            this.educationalValue = employee.educationalValue;
            this.visitorAppeal = employee.visitorAppeal;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj">the other object we are comparing to</param>
        /// <param name="option">what parameter we are comapring</param>
        /// <returns>
        /// Positive Integer: this is larger than the other
        /// 0               : the two are the exact same
        /// Negative Integer: this is lesser than the other
        /// </returns>
        public int compareTo(object obj, int option) {
            if (obj == null || GetType() != obj.GetType()) {
                return 0;
            } else { 
                Employee other = (Employee)obj;
                switch (option) { 
                    case 0: //total
                        uint thisTotal = this.tourDuration + this.educationalValue + this.visitorAppeal;
                        uint otherTotal = other.tourDuration + other.educationalValue + other.visitorAppeal;
                        return (int)thisTotal - (int)otherTotal;
                    case 1: //tourDuration
                        return (int)this.tourDuration - (int)other.tourDuration;
                    case 2: //educationalValue
                        return (int)this.educationalValue - (int)other.educationalValue;
                    case 3: //visitorAppeal
                        return (int)this.visitorAppeal - (int)other.visitorAppeal;
                    default:
                        return 0;
                }
            }
        }

        public override string ToString()
        {
            string result = this.name + " " + this.tourDuration + " " + this.educationalValue + " " + this.visitorAppeal;
            return result;
        }

        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType()) {
                return false;
            } else {
                Employee other = (Employee)obj;
                return this.id.Equals(other.id);
            }
        }

    }
}