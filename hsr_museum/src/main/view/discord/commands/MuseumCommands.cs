using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using hsr_museum.src.main.controller;

namespace hsr_museum.src.main.view.discord.commands
{
    public class MuseumCommands: BaseCommandModule
    {
        [Command("hire")]
        [Description("Hires an available employee for your museum.")]
        public async Task hire(CommandContext ctx) { 
            
        }

        [Command("release")]
        [Description("Releases an employee.")]
        public async Task release(CommandContext ctx) { 

        }

        [Command("add")]
        [Description("Add an exhibition to your museum.")]
        public async Task add(CommandContext ctx) { 

        }

        [Command("remove")]
        [Description("Remove an exhibition from your museum.")]
        public async Task remove(CommandContext ctx) { 

        }

        [Command("upgrade")]
        [Description("Upgrade an exhibition.")]
        public async Task upgrade(CommandContext ctx) { 

        }

        [Command("calc")]
        [Description("Calculate the most optimal employee configuration.")]
        public async Task calc(CommandContext ctx) { 

        }

        private async Task employees() { 

        }

        private async Task exhibits() { 

        }

        [Command("status")]
        [Description("View the current status of your museum.")]
        public async Task status(CommandContext ctx) { 

        }

    }
}