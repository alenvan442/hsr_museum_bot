using DSharpPlus.Entities;
using hsr_museum.src.main.model.persistence;
using hsr_museum.src.main.model.structures;
using hsr_museum.src.main.model.structures.items.museum_event;
using hsr_museum_bot.hsr_museum.src.main.controller;

namespace hsr_museum.src.main.controller
{
    /// <summary>
    /// This controller will receive information from the view and either add, delete, or update a player accordingly
    /// The main purpose of this controller is to receive and delegate tasks that involves the creation or deletion of a player
    /// </summary>
    public class PlayerController: ObjectController<Player>
    {
        PlayersFileDAO playersFileDAO;
        ObjectFileDAO<Events> eventsFileDAO;
        ObjectFileDAO<Employee> employeeFileDAO;
        ObjectFileDAO<Exhibition> exhibitionFileDAO;

        /// <summary>
        /// Constructor for the player controller
        /// Utilizes the player DAO
        /// </summary>
        /// <param name="playersFileDAO"> A class that holds methods that correspond with the manipulation of data with players </param>
        public PlayerController(PlayersFileDAO playersFileDAO, ObjectFileDAO<Events> eventsDAO,
                                    ObjectFileDAO<Employee> employeeDAO, ObjectFileDAO<Exhibition> exDAO):
                                    base(playersFileDAO) {
            this.playersFileDAO = playersFileDAO;
            this.eventsFileDAO = eventsDAO;
            this.employeeFileDAO = employeeDAO;
            this.exhibitionFileDAO = exDAO;
        }

        /// <summary>
        /// Adds a new player to the database
        /// </summary>
        /// <param name="member"> The discord member that will gain a new acconut </param>
        /// <returns> A boolean indicating whether or not the action was successful </returns>
        public Boolean addPlayer(DiscordMember member) {
            return playersFileDAO.addPlayer(member);
        }

        /*
        public void switchEvent(ulong id, int option) {
            Events newEvent = this.eventsFileDAO.getObject((ulong)option);
            Player player = this.playersFileDAO.getObject(id);
            player.switchEvent(newEvent);
            this.playersFileDAO.addObject(player, player.UID);
        }
        */

        public Boolean hireEmployee(uint id, int index, Employee[] employees) {
            Player player = getObject(id);
            Boolean result = player.hireEmployee(employees[index]);
            if(result) { addObject(player, id); } //saves
            return result;
        }

        public Boolean removeEmployee(uint id, int index, Employee[] employees) {
            Player player = getObject(id);
            Boolean result = player.removeEmployee(employees[index]);
            if (result) { addObject(player, id); } //saves
            return result;
        }

        public Boolean addExhibition(uint id, int index, Exhibition[] exhibitions) {
            Player player = getObject(id);
            Boolean result = player.addExhibition(exhibitions[index]);
            if (result) { addObject(player, id); } //saves
            return result;
        }

        public Boolean removeExhibition(uint id, int index, Exhibition[] exhibitions) {
            Player player = getObject(id);
            Boolean result = player.removeExhibition(exhibitions[index]);
            if (result) { addObject(player, id); } //saves
            return result;
        }

    }
}