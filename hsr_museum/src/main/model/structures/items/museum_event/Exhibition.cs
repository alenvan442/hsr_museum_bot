using hsr_museum_bot.hsr_museum.src.main.model.structures.items.museum_event;
using Newtonsoft.Json;

namespace hsr_museum.src.main.model.structures.items
{
    public class Exhibition
    {
        [JsonProperty("ID")]
        public ulong id { get; private set; }
        [JsonProperty("Name")]
        public string name { get; private set; }
        [JsonProperty("Level")]
        public uint level { get; private set; }
        [JsonProperty("TourDuration")]
        public uint tourDuration { get; private set; }
        [JsonProperty("TourDurationLevel")]
        public uint tourDurationLevel { get; private set; }
        [JsonProperty("EducationalValue")]
        public uint educationalValue { get; private set; }
        [JsonProperty("EducationalValueLevel")]
        public uint educationalValueLevel { get; private set; }
        [JsonProperty("VisitorAppeal")]
        public uint visitorAppeal { get; private set; }
        [JsonProperty("VisitorAppealLevel")]
        public uint visitorAppealLevel { get; private set; }
        public Employee[] employees { get; private set; }

        /// <summary>
        /// json constructor for the exhibition object
        /// </summary>
        /// <param name="id">the id of the exhibition</param>
        /// <param name="name">name of the exhibition</param>
        /// <param name="level">current level of the exhibition</param>
        /// <param name="tourDuration">parameter 1's capacity</param>
        /// <param name="tourDurationLevel">parameter 1's level</param>
        /// <param name="educationalValue">parameter 2's capacity</param>
        /// <param name="educationalValueLevel">parameter 2's level</param>
        /// <param name="visitorAppeal">parameter 3's capacity</param>
        /// <param name="visitorAppealLevel">parameter 3's level</param>
        [JsonConstructor]
        public Exhibition(ulong id, string name, uint level, uint tourDuration, uint tourDurationLevel,
                            uint educationalValue, uint educationalValueLevel, uint visitorAppeal,
                            uint visitorAppealLevel) {

            this.id = id;
            this.name = name;
            this.level = level;
            this.tourDuration = tourDuration;
            this.tourDurationLevel = tourDurationLevel;
            this.educationalValue = educationalValue;
            this.educationalValueLevel = educationalValueLevel;
            this.visitorAppeal = visitorAppeal;
            this.visitorAppealLevel = visitorAppealLevel;
            this.employees = new Employee[3];
        }

        /// <summary>
        /// This is a constructor to add a newly gained exhibition to the player,
        /// before calling this constructor, the caller will search for the exhibition
        /// and obtain it's information from the DAO, then pass in the return to this
        /// constructor where it initializes the exhibition
        /// </summary>
        /// <param name="exhibition">the level 1 exhibition to create</param>
        public Exhibition(Exhibition exhibition) {
            this.id = exhibition.id;
            this.name = exhibition.name;
            this.level = 1;
            this.tourDuration = exhibition.tourDuration;
            this.tourDurationLevel = 1;
            this.educationalValue = exhibition.educationalValue;
            this.educationalValueLevel = 1;
            this.visitorAppeal = exhibition.visitorAppeal;
            this.visitorAppealLevel = 1;
            this.employees = new Employee[3];
        }

        /// <summary>
        /// sets the level of one of the "level" parameters
        /// of the exhibition. We will check if the level is appropriate
        /// or not in the caller.
        /// </summary>
        /// <param name="option">the select of which level to set</param>
        /// <param name="level">what level to set the parameter to</param>
        /// <returns>boolean to indicate if the passed in option was appropriate</returns>
        public bool setLevel(int option, uint level) {
            switch (option) { 
                case 1:
                    this.level = level;
                    break;
                case 2:
                    this.tourDurationLevel = level;
                    break;
                case 3:
                    this.educationalValueLevel = level;
                    break;
                case 4:
                    this.visitorAppealLevel = level;
                    break;
                default:
                    return false;
            }
            return true;
        }

        /// <summary>
        /// retrieves the 3 offsets in the order:
        /// [0] tourDuration
        /// [1] educationValue
        /// [2] visitorAppeal
        /// 
        /// if the offset < 0, we have not reached the threshold yet
        /// if the offset > 0, we have more than the threshold
        /// </summary>
        /// <returns>an array of 3 ints</returns>
        public int[] offsets() {
            int[] offsets = new int[3];
            uint[] currVals = this.totalValues();

            offsets[0] = (int)currVals[0] - (int)this.tourDuration;
            offsets[1] = (int)currVals[1] - (int)this.educationalValue;
            offsets[2] = (int)currVals[2] - (int)this.visitorAppeal;

            return offsets;

        }

        /// <summary>
        /// obtains the weight of the exhibition given
        /// the current employees currently assigned here
        /// the more number of parameters NOT met
        /// the lower the weight
        /// highest weight is 0
        /// </summary>
        /// <returns>the weight of this exhibition</returns>
        public double weight() {
            int numberLess = 0;

            uint[] currVals = this.totalValues();

            if (currVals[0] < this.tourDuration) { numberLess += 1; }
            if (currVals[1] < this.educationalValue) { numberLess += 1; }
            if (currVals[2] < this.visitorAppeal) { numberLess += 1; }

            return Math.Pow(numberLess, 50);
        }

        /// <summary>
        /// returns the current total value of the 3 employees
        /// assigned to this exhibition in the order:
        /// [0] tourDuration
        /// [1] educationValue
        /// [2] visitorAppeal
        /// 
        /// </summary>
        /// <returns>an array of 3 uint</returns>
        private uint[] totalValues() {
            uint[] result = new uint[3];

            foreach (Employee employee in this.employees)
            {
                result[0] += employee.tourDuration;
                result[1] += employee.educationalValue;
                result[2] += employee.visitorAppeal;
            }
            return result;
        }

    }
}