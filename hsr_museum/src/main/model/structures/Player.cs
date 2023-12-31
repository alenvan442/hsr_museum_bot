using System.Xml.Linq;
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
        public ulong id { get; private set; }
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
            this.id = ID;
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
            this.id = member.Id;
            this.name = member.Username;
            this.employeesId = new Dictionary<uint, string>();
            this.exhibitionsId = new Dictionary<uint, int[]>();
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
            if(!this.exhibitions.ContainsKey(toRemove.id)) {
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
            if (!this.employees.ContainsKey(toRemove.id)) {
                return false;
            } else {
                this.employees.Remove(toRemove.id);
                this.employeesId.Remove(toRemove.id);
                return true;
            }
        }

        public Boolean setExhibitionLevel(Exhibition exhibit, int option, int level) {
            if (!this.exhibitions.ContainsKey(exhibit.id)) {
                return false;
            } else {
                Exhibition myExhibit = this.exhibitions[exhibit.id];
                bool result = myExhibit.setLevel(option, level);

                this.exhibitionsId[exhibit.id][0] = myExhibit.level;
                this.exhibitionsId[exhibit.id][1] = myExhibit.tourDurationLevel;
                this.exhibitionsId[exhibit.id][2] = myExhibit.educationalValueLevel;
                this.exhibitionsId[exhibit.id][3] = myExhibit.visitorAppealLevel;

                return result;
            }
        }

        public Employee[] getEmployees() {
            return this.employees.Values.ToArray();
        }

        public Exhibition[] getExhibitions() {
            return this.sortExhibition();
        }

        public Exhibition[] sortExhibition() {
            Exhibition[] result = new Exhibition[this.exhibitions.Count()];
            List<Exhibition> current = this.exhibitions.Values.ToList();
            for (int i = 0; i < this.exhibitions.Count(); i++) {
                uint idLow = 100;
                foreach (Exhibition ex in current) {
                    if (ex.id < idLow) { 
                        idLow = ex.id; 
                    }
                }
                result[i] = this.exhibitions[idLow];
                current.Remove(this.exhibitions[idLow]);
            }
            return result;
        }


        /// <summary>
        /// Sorts the array of employees based on 4 aspects
        /// chosen by the option parameter
        /// 0 (default): based on total sum of the employee's 3 parameters
        /// 1: sorted based on tourDuration
        /// 2: sorted based on educationalValue
        /// 3: sorted based on visitorAppeal
        /// /// </summary>
        /// <param name="employees">the list of employees to sort</param>
        /// <param name="option">param to choose how we sort</param>
        /// <param name="desc">which direction we sort, defaulted to descending</param>
        /// <returns>the sorted list</returns>
        private Employee[] sortEmployee(Employee[] employees, int option = 0, Boolean desc = true) {
            if (employees.Count() <= 1) {
                return employees;
            }

            Employee[] firstHalf = new Employee[employees.Count() / 2];
            Array.Copy(
                employees,
                0, 
                firstHalf, 0, 
                employees.Count() / 2);
            firstHalf = this.sortEmployee(firstHalf, option, desc);

            Employee[] secondHalf = new Employee[employees.Count() - (employees.Count() / 2)];
            Array.Copy(
                employees,
                (employees.Count() / 2),
                secondHalf, 0, 
                (employees.Count() - (employees.Count() / 2)));
            secondHalf = this.sortEmployee(secondHalf, option, desc);

            Employee[] sorted = new Employee[employees.Count()];

            int firstIndex = 0;
            int secondIndex = 0;
            int descending = desc ? 1 : -1;
            for (int i = 0; i < employees.Count(); i++) {

                //if the first half has been used up, add the rest of the second half
                if (firstIndex >= firstHalf.Count()) {
                    while (secondIndex < secondHalf.Count()) {
                        sorted[i] = secondHalf[secondIndex];
                        secondIndex++;
                        i++;
                    }
                    break;
                }

                //if the second half has been used up, add the rest of the first half
                if (secondIndex >= secondHalf.Count()) {
                    while (firstIndex < firstHalf.Count()) {
                        sorted[i] = firstHalf[firstIndex];
                        firstIndex++;
                        i++;
                    }
                    break;
                }

                //compare and add
                int compareResult = (descending) * firstHalf[firstIndex].compareTo(secondHalf[secondIndex], option);
                if (compareResult > 0) {
                    sorted[i] = firstHalf[firstIndex];
                    firstIndex++;
                } else if (compareResult < 0) {
                    sorted[i] = secondHalf[secondIndex];
                    secondIndex++;
                } else {
                    sorted[i] = firstHalf[firstIndex];
                    firstIndex++;
                    i++;
                    sorted[i] = secondHalf[secondIndex];
                    secondIndex++;
                }
            }

            return sorted;
        }

        public Exhibition[] calc() {
            List<Employee> currEmployees = this.employees.Values.ToList();
            List<Exhibition> currExhibition = this.sortExhibition().ToList();

            //sort based on total
            currEmployees = sortEmployee(currEmployees.ToArray()).ToList();

            //append greatest total to each
            foreach (Exhibition i in currExhibition) {
                //reset exhibition and start
                i.clearEmployees();
                i.addEmployee(currEmployees[0]);
                currEmployees.RemoveAt(0);
            }

            //foreach exhibition that does not have a weight of 0
            //sort employees based on the first parameter that is still under
            //append the next greatest
            //continue until each exhibition has 3 employees or no employees are lef tot be assigned
            foreach (Exhibition i in currExhibition) {
                while (i.employees.Count() < 3 && i.weight() != 0 && currEmployees.Count() > 0) {
                    int[] offsets = i.offsets();
                    for (int o = 0; o < offsets.Count(); o++) {
                        if (offsets[o] < 0) {
                            currEmployees = sortEmployee(currEmployees.ToArray(), o + 1).ToList();
                            i.addEmployee(currEmployees[0]);
                            currEmployees.RemoveAt(0);
                            break;
                        }
                    }
                }
            }

            bool swapped = false;
            do {
                swapped = false;
                if (currEmployees.Count() == 0) { break; }
                //determine which parameters of each exhibition has not been reached
                //determine the offsets of each of the parameters
                foreach (Exhibition i in currExhibition) {
                    if (i.weight() != 0) {
                        int[] offsets = i.offsets();

                        //foreach exhibition sort the list of employees based on the missing parameters
                        //find the first employee that can reduce the negative offset, without reducing
                        //the positive offset below 0
                        //swap the employees if fouond
                        for (int o = 0; o < offsets.Count(); o++)
                        {
                            if (offsets[o] < 0)
                            {
                                currEmployees = sortEmployee(currEmployees.ToArray(), o + 1).ToList();
                                List<Employee> exEmployees = sortEmployee(i.employees.ToArray(), o + 1, false).ToList();

                                foreach (Employee j in currEmployees)
                                {
                                    foreach (Employee p in exEmployees)
                                    {
                                        int goodCount = 0;
                                        //diff in terms of how much current exhibition wuld gain if we swap
                                        int tourDiff = (int)j.tourDuration - (int)p.tourDuration;
                                        int eduDiff = (int)j.educationalValue - (int)p.educationalValue;
                                        int visiDiff = (int)j.visitorAppeal - (int)p.visitorAppeal;

                                        //check if the new employee would benefit the exhibition more
                                        //than hurt it, if so, swap them
                                        switch (o)
                                        {
                                            case 0:
                                                if (tourDiff > 0)
                                                {

                                                    if ((offsets[1] + eduDiff) > offsets[1] ||
                                                            (offsets[1] + eduDiff) > 0) { goodCount += 1; }

                                                    if ((offsets[2] + visiDiff) > offsets[2] ||
                                                            (offsets[2] + visiDiff) > 0) { goodCount += 1; }

                                                    if (goodCount == 2)
                                                    {
                                                        currEmployees.Add(i.removeEmployee(p));
                                                        i.addEmployee(j);
                                                        currEmployees.Remove(j);
                                                    }

                                                }
                                                else
                                                {
                                                    //our lowest value is larger than the unassigned
                                                    //employee's largest value, so we skip this 
                                                    //swap with unassinged employees
                                                    goodCount = -1;
                                                }
                                                break;
                                            case 1:
                                                if (eduDiff > 0)
                                                {

                                                    if ((offsets[0] + tourDiff) > offsets[0] ||
                                                            (offsets[0] + tourDiff) > 0) { goodCount += 1; }

                                                    if ((offsets[2] + visiDiff) > offsets[2] ||
                                                            (offsets[2] + visiDiff) > 0) { goodCount += 1; }

                                                    if (goodCount == 2)
                                                    {
                                                        currEmployees.Add(i.removeEmployee(p));
                                                        i.addEmployee(j);
                                                        currEmployees.Remove(j);
                                                    }

                                                }
                                                else
                                                {
                                                    //our lowest value is larger than the unassigned
                                                    //employee's largest value, so we skip this 
                                                    //swap with unassinged employees
                                                    goodCount = -1;
                                                }
                                                break;
                                            case 2:
                                                if (visiDiff > 0)
                                                {

                                                    if ((offsets[0] + tourDiff) > offsets[0] ||
                                                            (offsets[0] + tourDiff) > 0) { goodCount += 1; }

                                                    if ((offsets[1] + eduDiff) > offsets[1] ||
                                                            (offsets[1] + eduDiff) > 0) { goodCount += 1; }

                                                    if (goodCount == 2)
                                                    {
                                                        currEmployees.Add(i.removeEmployee(p));
                                                        i.addEmployee(j);
                                                        currEmployees.Remove(j);
                                                    }

                                                }
                                                else
                                                {
                                                    //our lowest value is larger than the unassigned
                                                    //employee's largest value, so we skip this 
                                                    //swap with unassinged employees
                                                    goodCount = -1;
                                                }
                                                break;
                                        }

                                        if (goodCount == 2)
                                        {
                                            swapped = true;
                                            break;
                                        }
                                        else if (goodCount == -1)
                                        {
                                            break;
                                        }
                                    }
                                    if (swapped == true) { break; }

                                }

                                if (swapped) { break; }

                                //if not found in unassigned, look in the other exhibitions, determine if:
                                // 1. benefits both exhibitions
                                // 2. benefits one without hurting the other
                                foreach (Exhibition p in currExhibition) {
                                    if (i == p) { continue; }
                                    List<Employee> otherExEmployees = sortEmployee(p.employees.ToArray(), o + 1).ToList();
                                    int[] otherOffsets = p.offsets();

                                    foreach (Employee e in otherExEmployees) {
                                        foreach (Employee f in exEmployees)
                                        {
                                            int goodCount = 0;

                                            //diff in terms of how much current exhibition wuld gain if we swap
                                            //diff in terms of how much othe rexhibition would lose if we swap
                                            int tourDiff = (int)e.tourDuration - (int)f.tourDuration;
                                            int eduDiff = (int)e.educationalValue - (int)f.educationalValue;
                                            int visiDiff = (int)e.visitorAppeal - (int)f.visitorAppeal;
                                            switch (o)
                                            {
                                                case 0:
                                                    if (tourDiff > 0)
                                                    {

                                                        //if we swap, current exhibition benefits, but check to see
                                                        //if other exhibition does not lose too much from it
                                                        if ((otherOffsets[0] - tourDiff) > 0) { goodCount += 1; }

                                                        if (
                                                            ((offsets[1] + eduDiff) > offsets[1] ||
                                                             (offsets[1] + eduDiff) > 0) &&
                                                            ((otherOffsets[1] - eduDiff) > otherOffsets[1] ||
                                                             (otherOffsets[1] - eduDiff > 0))
                                                            ) { goodCount += 1; }

                                                        if (
                                                            ((offsets[2] + visiDiff) > offsets[2] ||
                                                             (offsets[2] + visiDiff) > 0) &&
                                                            ((otherOffsets[2] - visiDiff) > otherOffsets[2] ||
                                                             (otherOffsets[2] - visiDiff) > 0)
                                                            ) { goodCount += 1; }

                                                        if (goodCount == 3)
                                                        {
                                                            i.removeEmployee(f);
                                                            i.addEmployee(e);
                                                            p.removeEmployee(e);
                                                            p.addEmployee(f);
                                                        }

                                                    }
                                                    else
                                                    {
                                                        //our lowest value is larger than the unassigned
                                                        //employee's largest value, so we skip this 
                                                        //swap with unassinged employees
                                                        goodCount = -1;
                                                    }
                                                    break;
                                                case 1:
                                                    if (eduDiff > 0)
                                                    {

                                                        if ((otherOffsets[1] - eduDiff) > 0) { goodCount += 1; }

                                                        if (
                                                            ((offsets[0] + tourDiff) > offsets[0] ||
                                                             (offsets[0] + tourDiff) > 0) &&
                                                            ((otherOffsets[0] - tourDiff) > otherOffsets[0] ||
                                                             (otherOffsets[0] - tourDiff) > 0)
                                                            ) { goodCount += 1; }

                                                        if (
                                                            ((offsets[2] + visiDiff) > offsets[2] ||
                                                             (offsets[2] + visiDiff) > 0) &&
                                                            ((otherOffsets[2] - visiDiff) > otherOffsets[2] ||
                                                             (otherOffsets[2] - visiDiff) > 0)
                                                            ) { goodCount += 1; }

                                                        if (goodCount == 3)
                                                        {
                                                            i.removeEmployee(f);
                                                            i.addEmployee(e);
                                                            p.removeEmployee(e);
                                                            p.addEmployee(f);
                                                        }

                                                    }
                                                    else
                                                    {
                                                        //our lowest value is larger than the unassigned
                                                        //employee's largest value, so we skip this 
                                                        //swap with unassinged employees
                                                        goodCount = -1;
                                                    }
                                                    break;
                                                case 2:
                                                    if (visiDiff > 0)
                                                    {

                                                        if ((otherOffsets[2] - visiDiff) > 0) { goodCount += 1; }

                                                        if (
                                                            ((offsets[0] + tourDiff) > offsets[0] ||
                                                             (offsets[0] + tourDiff) > 0) &&
                                                            ((otherOffsets[0] - tourDiff > otherOffsets[0]) ||
                                                             (otherOffsets[0] - tourDiff) > 0)
                                                            ) { goodCount += 1; }

                                                        if (
                                                            ((offsets[1] + eduDiff) > offsets[1] ||
                                                             (offsets[1] + eduDiff) > 0) &&
                                                             ((otherOffsets[1] - eduDiff) > otherOffsets[1]) ||
                                                             (otherOffsets[1] - eduDiff) > 0
                                                            ) { goodCount += 1; }

                                                        if (goodCount == 3)
                                                        {
                                                            i.removeEmployee(f);
                                                            i.addEmployee(e);
                                                            p.removeEmployee(e);
                                                            p.addEmployee(f);
                                                        }

                                                    }
                                                    else
                                                    {
                                                        //our lowest value is larger than the unassigned
                                                        //employee's largest value, so we skip this 
                                                        //swap with unassinged employees
                                                        goodCount = -1;
                                                    }
                                                    break;
                                            }

                                            if (goodCount == 3)
                                            {
                                                swapped = true;
                                                break;
                                            }
                                            else if (goodCount == -1)
                                            {
                                                break;
                                            }
                                        }
                                        if (swapped == true) { break; }
                                    }

                                    //end loop once no other swaps can be made
                                    //end loop once all exhibitions weights are 0
                                    //end loop if there are no more employees to assign
                                }

                            }

                        }
                        
                    }
                }

            } while (swapped == true);

            return currExhibition.ToArray();
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