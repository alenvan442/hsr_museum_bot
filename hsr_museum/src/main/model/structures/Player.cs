using System.Runtime.CompilerServices;
using System.Collections;
using DSharpPlus.Entities;
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

        /// <summary>
        /// constructor that json uses for a new player
        /// </summary>
        /// <param name="ID">the id of the player</param>
        /// <param name="Name">the associated name of the player</param>       
        [JsonConstructor]
        public Player(ulong ID, string Name) {
            this.UID = ID;
            this.name = Name;

            loadInventory();
        }

        /// <summary>
        /// Constructor for a new player utilizing an already existing member
        /// Give them the first plant pot for free
        /// </summary>
        public Player(DiscordMember member) {
            this.UID = member.Id;
            this.name = member.Username;
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