using Newtonsoft.Json;

namespace hsr_museum_bot.hsr_museum.src.main.model.structures.items.museum_event
{
    public class Employee
    {
        [JsonProperty("ID")]
        public ulong id { get; private set; }
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
        public Employee(ulong id, string name, uint tourDuration,
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

    }
}