using System.Globalization;
using DSharpPlus.Entities;
using hsr_museum.src.main.model.structures.items.museum_event;
using hsr_museum.src.main.model.utilities;
using Newtonsoft.Json;

namespace hsr_museum.src.main.model.structures
{
    /// <summary>
    /// 
    /// </summary>
    public class Player
    {

        [JsonProperty("ID")]
        public ulong UID { get; private set; }
        [JsonProperty("Name")]
        public string name { get; private set; }
        [JsonProperty("Employees")]
        public Dictionary<uint, string> employeesId { get; private set; }
        [JsonProperty("Exhibitions")]
        public Dictionary<uint, int[]> exhibitionsId { get; private set; }

        private Dictionary<uint, Employee> employees = new Dictionary<uint, Employee>();
        private Dictionary<uint, Exhibition> exhibitions = new Dictionary<uint, Exhibition>();

        /// <summary>
        /// constructor that json uses for a new player
        /// </summary>
        /// <param name="ID">the id of the player</param>
        /// <param name="Name">the associated name of the player</param>       
        [JsonConstructor]
        public Player(ulong ID, string Name, Dictionary<uint, string> Employees, Dictionary<uint, int[]> Exhibitions) {
            this.UID = ID;
            this.name = Name;
            this.employeesId = Employees;
            this.exhibitionsId = Exhibitions;

            loadEvent();
        }

        /// <summary>
        /// Constructor for a new player utilizing an already existing member
        /// Give them the first plant pot for free
        /// </summary>
        public Player(DiscordMember member) {
            this.UID = member.Id;
            this.name = member.Username;
        }

        private void loadEvent() {
            foreach (uint i in employeesId.Keys) {
                this.employees[i] = IdentificationSearch.employeeSearch(i);
            }

            foreach (uint i in exhibitionsId.Keys) {
                Exhibition temp = IdentificationSearch.exhibitionSearch(i);
                this.exhibitions[i] = new Exhibition(temp, exhibitionsId[i]);
            }
        }

        /*
        public void switchEvent(Events newEvent) {

            loadEvent();
        }
        */

        public Boolean addExhibition(Exhibition toAdd) {
            if(this.exhibitions.ContainsKey(toAdd.id)) {
                return false;
            } else {
                this.exhibitions[toAdd.id] = toAdd;
                this.exhibitionsId[toAdd.id] =
                    new int[4] { toAdd.level, toAdd.tourDurationLevel, 
                                    toAdd.educationalValueLevel, toAdd.visitorAppealLevel};
                return true;
            }
        }

        public Boolean removeExhibition(Exhibition toRemove) { 
            if(this.exhibitions.ContainsKey(toRemove.id)) {
                return false;
            } else {
                this.exhibitions.Remove(toRemove.id);
                this.exhibitionsId.Remove(toRemove.id);
                return true;
            }
        }

        public Boolean hireEmployee(Employee toHire) {
            if(this.employees.ContainsKey(toHire.id)) {
                return false;
            } else {
                this.employees[toHire.id] = toHire;
                this.employeesId[toHire.id] = toHire.name;
                return true;
            }
        }

        public Boolean removeEmployee(Employee toRemove) { 
            if (this.employees.ContainsKey(toRemove.id)) {
                return false;
            } else {
                this.employees.Remove(toRemove.id);
                this.employees.Remove(toRemove.id);
                return true;
            }
        }

        /// <summary>
        /// The toString method of the player class
        /// Format: 
        ///         (Player's Name)
        /// 
        ///         (Player's Balance)
        /// </summary>
        /// <returns></returns>
        public override string ToString() {

            String result = "";
            result += "\nUsername: " + this.name;

            return result;
        }

    }
}