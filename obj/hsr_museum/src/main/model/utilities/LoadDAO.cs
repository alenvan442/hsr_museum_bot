using DSharpPlus.Entities;
using hsr_museum.src.main.controller;
using hsr_museum.src.main.model.persistence;
using hsr_museum.src.main.view.discord.commands;

namespace hsr_museum.src.main.model.utilities
{
    public static class LoadDAO
    {

        static PlayersFileDAO? playersFileDAO;
        static PlayerController? playerController;

        /// <summary>
        /// Loads the FileDAOs
        /// </summary>
        public static void load() {
            JsonUtilities json = new JsonUtilities();
            playersFileDAO = new PlayersFileDAO(StaticUtil.playersJson, json, seedsFileDAO);
            playerController = new PlayerController(playersFileDAO);            
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