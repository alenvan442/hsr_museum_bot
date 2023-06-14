using DSharpPlus.Entities;
using hsr_museum.src.main.model.persistence;
using hsr_museum.src.main.model.structures;

namespace hsr_museum.src.main.controller
{
    /// <summary>
    /// This controller will receive information from the view and either add, delete, or update a player accordingly
    /// The main purpose of this controller is to receive and delegate tasks that involves the creation or deletion of a player
    /// </summary>
    public class PlayerController
    {
        PlayersFileDAO playersFileDAO;

        /// <summary>
        /// Constructor for the player controller
        /// Utilizes the player DAO
        /// </summary>
        /// <param name="playersFileDAO"> A class that holds methods that correspond with the manipulation of data with players </param>
        public PlayerController(PlayersFileDAO playersFileDAO) {
            this.playersFileDAO = playersFileDAO;
        }

        /// <summary>
        /// Retrieves a player from the database given an ID
        /// </summary>
        /// <param name="UID"> The ID of the player to look for </param>
        /// <returns> The retrieved player </returns>
        public Player getPlayer(ulong UID) {
            return playersFileDAO.getPlayer(UID);
        }

        /// <summary>
        /// Retrieves all of the players in the database
        /// </summary>
        /// <returns> An array of all of the players </returns>
        public Player[] getPlayers() {
            return playersFileDAO.getPlayers();
        }

        /// <summary>
        /// Adds a new player to the database
        /// </summary>
        /// <param name="member"> The discord member that will gain a new acconut </param>
        /// <returns> A boolean indicating whether or not the action was successful </returns>
        public Boolean addPlayer(DiscordMember member) {
            return playersFileDAO.addPlayer(member);
        } 

        /// <summary>
        /// Deletes a user's data based on the given ID
        /// </summary>
        /// <param name="UID"> The ID of the player to delete </param>
        /// <returns> A boolean indicating whether or not the action was successful </returns>
        public Boolean deletePlayer(ulong UID) {
            return playersFileDAO.deletePlayer(UID);
        }

    }
}