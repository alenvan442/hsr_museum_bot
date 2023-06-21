using Newtonsoft.Json;

namespace hsr_museum.src.main.model.structures.items.museum_event
{
    public class Exhibition
    {
        [JsonProperty("ID")]
        public uint id { get; private set; }
        [JsonProperty("Name")]
        public string name { get; private set; }
        [JsonProperty("BaseStats")]
        public uint[][] baseStats { get; private set; }
        [JsonProperty("Level")]
        public int level { get; private set; }
        [JsonProperty("TourDurations")]
        public uint[] tourDurations { get; private set; }
        [JsonProperty("TourDurationLevel")]
        public int tourDurationLevel { get; private set; }
        [JsonProperty("EducationalValues")]
        public uint[] educationalValues { get; private set; }
        [JsonProperty("EducationalValueLevel")]
        public int educationalValueLevel { get; private set; }
        [JsonProperty("VisitorAppeals")]
        public uint[] visitorAppeals { get; private set; }
        [JsonProperty("VisitorAppealLevel")]
        public int visitorAppealLevel { get; private set; }
        public List<Employee> employees { get; private set; }

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
        public Exhibition(uint ID, string Name, uint[][] BaseStats, int Level, uint[] TourDurations, int TourDurationLevel,
                            uint[] EducationalValues, int EducationalValueLevel, uint[] VisitorAppeals,
                            int VisitorAppealLevel) {

            this.id = ID;
            this.name = Name;
            this.baseStats = BaseStats;
            this.level = Level;
            this.tourDurations = TourDurations;
            this.tourDurationLevel = TourDurationLevel;
            this.educationalValues = EducationalValues;
            this.educationalValueLevel = EducationalValueLevel;
            this.visitorAppeals = VisitorAppeals;
            this.visitorAppealLevel = VisitorAppealLevel;
            this.employees = new List<Employee>();
        }

        /// <summary>
        /// This is a constructor to add a newly gained exhibition to the player,
        /// before calling this constructor, the caller will search for the exhibition
        /// and obtain it's information from the DAO, then pass in the return to this
        /// constructor where it initializes the exhibition
        /// </summary>
        /// <param name="exhibition">the level 1 exhibition to create</param>
        public Exhibition(Exhibition exhibition, int[]? parameters = null) {
            this.id = exhibition.id;
            this.name = exhibition.name;
            this.baseStats = exhibition.baseStats;
            this.level = (parameters != null && parameters[0] != 0 ? parameters[0] : 1);
            this.tourDurations = exhibition.tourDurations;
            this.tourDurationLevel = (parameters != null && parameters[1] != 0 ? parameters[1] : 1);
            this.educationalValues = exhibition.educationalValues;
            this.educationalValueLevel = (parameters != null && parameters[2] != 0 ? parameters[2] : 1);
            this.visitorAppeals = exhibition.visitorAppeals;
            this.visitorAppealLevel = (parameters != null && parameters[3] != 0 ? parameters[3] : 1);
            this.employees = new List<Employee>();
        }

        /// <summary>
        /// sets the level of one of the "level" parameters
        /// of the exhibition. We will check if the level is appropriate
        /// or not in the caller.
        /// </summary>
        /// <param name="option">the select of which level to set</param>
        /// <param name="level">what level to set the parameter to</param>
        /// <returns>boolean to indicate if the passed in option was appropriate</returns>
        public bool setLevel(int option, int level) {
            //TODO check for max levels
            //TODO offset levels by -1 due to the starting level being 1
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
            uint[] capVals = this.exValues();

            offsets[0] = (int)currVals[0] - (int)capVals[0];
            offsets[1] = (int)currVals[1] - (int)capVals[1];
            offsets[2] = (int)currVals[2] - (int)capVals[2];

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
            uint[] capVals = this.exValues();

            if (currVals[0] < capVals[0]) { numberLess += 1; }
            if (currVals[1] < capVals[1]) { numberLess += 1; }
            if (currVals[2] < capVals[2]) { numberLess += 1; }

            return -1 * Math.Pow(numberLess, 50);
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

        public uint[] getEmployeeValue(int option) {
            uint[] result = new uint[3];
            if (option < 1 || option > 3) { return null; }

            foreach (Employee i in this.employees) {
                switch (option) {
                    case 1:
                        result[result.Count()] = i.tourDuration;
                        break;
                    case 2:
                        result[result.Count()] = i.educationalValue;
                        break;
                    case 3:
                        result[result.Count()] = i.visitorAppeal;
                        break;
                }
            }
            return result;
        }

        public uint[] exValues() {
            uint[] result = new uint[3];
            uint[] currBaseStat = this.baseStats[this.level - 1];

            result[0] = currBaseStat[0] + this.tourDurations[this.tourDurationLevel - 1];
            result[1] = currBaseStat[1] + this.educationalValues[this.educationalValueLevel - 1];
            result[2] = currBaseStat[2] + this.visitorAppeals[this.visitorAppealLevel - 1];

            return result;
        }

        public uint totalBaseValue() {
            uint[] baseStats = this.exValues();
            return baseStats[0] + baseStats[1] + baseStats[2];
        }

        public Boolean addEmployee(Employee employee) { 
            if(this.employees.Count() >= 3) {
                return false;
            } else {
                this.employees.Add(employee);
                return true;
            }
        }

        public Employee removeEmployee(Employee employee) {
            if (!employees.Contains(employee)) {
                return null;
            } else {
                employees.Remove(employee);
                return employee;
            }
        }

        public void clearEmployees() {
            this.employees.RemoveRange(0, this.employees.Count());
        }

        public override bool Equals(object? obj)
        {
            if (obj is Exhibition) {
                Exhibition other = (Exhibition)obj;
                return this.id == other.id;
            } else {
                return false;
            }
        }

        public override string ToString()
        {
            string result = this.name + "\n";
            result += "Employees:\n";
            foreach (Employee i in this.employees) {
                if (i is null) {
                    continue;
                }
                result += "\t" + i.ToString() + "\n";
            }

            return result;
        }

        public int CompareTo(object? obj)
        {
            if (obj == null || GetType() != obj.GetType()) {
                return 1;
            }
            Exhibition other = (Exhibition)obj;
            return (int)this.totalBaseValue() - (int)other.totalBaseValue();
        }
    }
}