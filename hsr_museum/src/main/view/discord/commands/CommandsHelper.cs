using System.Globalization;
using System.Net.NetworkInformation;
using DSharpPlus.Entities;
using hsr_museum.src.main.controller;
using hsr_museum.src.main.model.structures.items.museum_event;
using hsr_museum.src.main.model.utilities;
using hsr_museum_bot.hsr_museum.src.main.controller;
using Newtonsoft.Json.Serialization;

namespace hsr_museum.src.main.view.discord.commands
{
    public static class CommandsHelper
    {
        public static PlayerController playerController;
        public static ObjectController<Employee> employeeController;
        public static ObjectController<Exhibition> exhibitionController;

        /// <summary>
        /// Constructor of the Farming commands class
        /// </summary>
        /// <param name="plantPotController"> The controller that will be handling the delegation of plant pot interactions </param>
        public static void setup(PlayerController player, ObjectController<Employee> employee,
                                    ObjectController<Exhibition> exhibitions) {

            playerController = player;
            employeeController = employee;
            exhibitionController = exhibitions;

        }

        public static DiscordEmbed createEmbed(string message) {
            return null;
        }

    }
}