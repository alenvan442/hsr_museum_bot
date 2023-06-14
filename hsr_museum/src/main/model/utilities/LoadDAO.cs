using DSharpPlus.Entities;
using hsr_museum.src.main.controller;
using hsr_museum.src.main.model.persistence;
using hsr_museum.src.main.model.structures;
using hsr_museum.src.main.model.structures.items.museum_event;

namespace hsr_museum.src.main.model.utilities
{
    public static class LoadDAO
    {
        static PlayersFileDAO? playersFileDAO;
        static PlayerController? playerController;
        static ObjectFileDAO<Events>? eventsFileDAO;
        static ObjectFileDAO<Employee>? employeeFileDAO;
        static ObjectFileDAO<Exhibition>? exhibitionFileDAO;

        /// <summary>
        /// Loads the FileDAOs
        /// </summary>
        public static void load() {
            JsonUtilities json = new JsonUtilities();
            eventsFileDAO = new ObjectFileDAO<Events>(StaticUtil.eventsJson, json);
            employeeFileDAO = new ObjectFileDAO<Employee>(StaticUtil.employeeJson, json);
            exhibitionFileDAO = new ObjectFileDAO<Exhibition>(StaticUtil.exhibitionJson, json);
            playersFileDAO = new PlayersFileDAO(StaticUtil.playersJson, json);
            playerController = new PlayerController(playersFileDAO, eventsFileDAO, employeeFileDAO, exhibitionFileDAO);            
        }

        /// <summary>
        /// Adds every player in a discord guild to the database
        /// Creating an empty, or basic player account
        /// </summary>
        /// <param name="guild"> The guild that was recently connected to </param>
        public async static void addPlayers(DiscordGuild guild) {
            foreach(DiscordMember i in await guild.GetAllMembersAsync()) {
                playersFileDAO.addPlayer(i);
            }
        } 
    }
}