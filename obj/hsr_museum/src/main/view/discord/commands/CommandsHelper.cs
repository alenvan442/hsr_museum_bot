using System.Globalization;
using hsr_museum.src.main.controller;
using hsr_museum.src.main.model.utilities;

namespace hsr_museum.src.main.view.discord.commands
{
    public static class CommandsHelper
    {
        public static PlayerController playerController;

        /// <summary>
        /// Constructor of the Farming commands class
        /// </summary>
        /// <param name="plantPotController"> The controller that will be handling the delegation of plant pot interactions </param>
        public static void setup(PlayerController playerController1)
        {

            playerController = playerController1;

        }
    }
}