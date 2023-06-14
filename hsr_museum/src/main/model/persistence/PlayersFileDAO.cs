using DSharpPlus.Entities;
using hsr_museum.src.main.model.structures;
using hsr_museum.src.main.model.utilities;

namespace hsr_museum.src.main.model.persistence
{
    /// <summary>
    /// This DAO file will handle all player data in terms of loading, saving, creating, and deleting player data
    /// </summary>
    public class PlayersFileDAO
    {

        Dictionary<ulong, Player> playersData;
        string playersJson;
        JsonUtilities jsonUtilities;

        /// <summary>
        /// Constructor for the PlayersFileDAO object
        /// setups the DAO object
        /// loads the player datas
        /// </summary>
        /// <param name="playersJson"> The path to the json file </param>
        /// <param name="jsonUtilities"> An object that helps with json manipulations </param>
        public PlayersFileDAO(string playersJson, JsonUtilities jsonUtilities, SeedsFileDAO seedsFileDAO) {
            
            this.playersJson = playersJson;
            this.jsonUtilities = jsonUtilities;
            playersData = new Dictionary<ulong, Player>();
            load();
        }

        /// <summary>
        /// Deserializes the players json file into a collection of players and their data
        /// The data is then saved into a local collection for easier access
        /// </summary>
        private void load() {
            var tempData = jsonUtilities.JsonDeserializeAsync<Dictionary<ulong, Player>>(playersJson).Result;
            if(tempData != null) {
                this.playersData = tempData;
            }
        }

        /// <summary>
        /// Serializes the local collection of player data into the json file
        /// This acts as saving player data
        /// </summary>
        public void save() {
            jsonUtilities.JsonSerialize(playersData, playersJson);
        }

        /// <summary>
        /// Retrieves a specific player based on their UID
        /// </summary>
        /// <param name="UID"> The UID of the player to be receieved </param>
        /// <returns> The player associated with the UID, null if not found </returns>
        public Player getPlayer(ulong UID) {
            Player result;
            playersData.TryGetValue(UID, out result);
            return result;
        }

        /// <summary>
        /// Gets a collection of all players currently in the database
        /// </summary>
        /// <returns> An array of all players that are saved in the database </returns>
        public Player[] getPlayers() {
            return playersData.Values.ToArray();
        }

        /// <summary>
        /// Adds a new player
        /// Creates a new player and adds them to the local collection then saves
        /// </summary>
        /// <param name="member"> The discord member to add </param>
        /// <returns> A boolean to indicate whether or not the player was successfully created </returns>
        public Boolean addPlayer(DiscordMember member) {
            //Checks to see if the UID is already in the system
            if(playersData.TryGetValue(member.Id, out _)) {
                return false;
            } else
            {
                Player newPlayer = new Player(member);
                playersData.Add(member.Id, newPlayer);
                save();
                return true;
            }
        }

        /// <summary>
        /// Deletes a player from the database, erasing their data
        /// </summary>
        /// <param name="UID"> The UID of the player to delete </param>
        /// <returns> Returns a boolean indicating whether or not the deletion was successful </returns>
        public Boolean deletePlayer(ulong UID) {
            playersData.Remove(UID);
            save();
            return true;
        }

        /// <summary>
        /// Retrieves the requested player's status information
        /// </summary>
        /// <param name="UID"> the id of the player to search for </param>
        /// <returns> the player's information in a string </returns>
        public String getStatus(ulong UID) {
            Player currPlayer = this.getPlayer(UID);
            return currPlayer.ToString();
        }

    }
}