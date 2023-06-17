using System.Runtime.InteropServices;
using System.Xml;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using hsr_museum.src.main.model.structures;
using hsr_museum.src.main.model.structures.items.museum_event;

namespace hsr_museum.src.main.view.discord.commands
{
    public class MuseumCommands: BaseCommandModule
    {
        [Command("hire")]
        [Description("Hires an available employee for your museum.")]
        public async Task hire(CommandContext ctx, int index = 0) {
            if (ctx.Member == null) {
                await Task.CompletedTask;
            } else {

                Player currPlayer = CommandsHelper.playerController.getObject(ctx.Member.Id);
                Employee[] currEmployee = currPlayer.getEmployees();
                Employee[] allEmployees = CommandsHelper.employeeController.getObjects();
                Employee[] freeEmployees = new Employee[allEmployees.Count() - currEmployee.Count()];
                string output = "";

                foreach (Employee i in allEmployees) {
                    if (currEmployee.Contains(i)) {
                        continue;
                    } else {
                        freeEmployees[freeEmployees.Count()] = i;
                    }
                }

                if (index == 0) {
                    foreach (Employee i in freeEmployees) {
                        output += i.ToString();
                    }
                } else {
                    bool result = CommandsHelper.playerController.hireEmployee(currPlayer.id, index - 1, freeEmployees);

                    output = result ? "" : "";
                }

                DiscordEmbed embed = CommandsHelper.createEmbed(output);

                //send message

            }
        }

        [Command("release")]
        [Description("Releases an employee.")]
        public async Task release(CommandContext ctx, int index = 0) {
            if (ctx.Member == null) {
                await Task.CompletedTask;
            } else {
                Player currPlayer = CommandsHelper.playerController.getObject(ctx.Member.Id);
                Employee[] currEmployees = currPlayer.getEmployees();
                string output = "";

                if (index == 0) {
                    foreach (Employee i in currEmployees) {
                        output += i.ToString();
                    }
                } else {
                    bool result = CommandsHelper.playerController.removeEmployee(currPlayer.id, index - 1, currEmployees);
                    output = result ? "" : "";
                }

                DiscordEmbed embed = CommandsHelper.createEmbed(output);

                //send message

            }
        }

        [Command("add")]
        [Description("Add an exhibition to your museum.")]
        public async Task add(CommandContext ctx, int index = 0) { 
            if (ctx.Member == null) {
                await Task.CompletedTask;
            } else {
                Player currPlayer = CommandsHelper.playerController.getObject(ctx.Member.Id);
                Exhibition[] currExhibition = currPlayer.getExhibitions();
                Exhibition[] allExhibition = CommandsHelper.exhibitionController.getObjects();
                Exhibition[] freeExhibition = new Exhibition[allExhibition.Count() - currExhibition.Count()];
                string output = "";

                if (index == 0) {
                    foreach (Exhibition i in allExhibition) {
                        if (currExhibition.Contains(i)) {
                            continue;
                        } else {
                            freeExhibition[freeExhibition.Count()] = i;
                        }
                    }
                } else {
                    bool result = CommandsHelper.playerController.addExhibition(currPlayer.id, index - 1, freeExhibition);
                    output = result ? "" : "";
                }

                DiscordEmbed embed = CommandsHelper.createEmbed(output);

                //send message

            }
        }

        [Command("remove")]
        [Description("Remove an exhibition from your museum.")]
        public async Task remove(CommandContext ctx, int index = 0) { 
            if (ctx.Member == null) {
                await Task.CompletedTask;
            } else {
                Player currPlayer = CommandsHelper.playerController.getObject(ctx.Member.Id);
                Exhibition[] currExhibitions = currPlayer.getExhibitions();
                string output = "";

                if (index == 0) {
                    foreach (Exhibition i in currExhibitions) {
                        output += i.ToString();
                    }
                } else {
                    bool result = CommandsHelper.playerController.removeExhibition(currPlayer.id, index - 1, currExhibitions);
                    output = result ? "" : "";
                }

                DiscordEmbed embed = CommandsHelper.createEmbed(output);

                //send message
            }
        }

        [Command("upgrade")]
        [Description("Upgrade an exhibition.")]
        public async Task upgrade(CommandContext ctx, int index = 0, int option = 0, int level = 0) { 
            if (ctx.Member == null) {
                await Task.CompletedTask;
            } else {
                Player currPlayer = CommandsHelper.playerController.getObject(ctx.Member.Id);
                Exhibition[] currExhibitions = currPlayer.getExhibitions();
                string output = "";

                if(index == 0) {
                    foreach (Exhibition i in currExhibitions) {
                        output += i.ToString();
                    }
                } else { 
                    if(option == 0) {
                        //must specify an option and a level
                    } else {
                        bool result = CommandsHelper.playerController.upgradeExhibition(
                                        currPlayer.id, index - 1, currExhibitions, option, level);
                        output = result ? "" : "";
                    }
                }

                DiscordEmbed embed = CommandsHelper.createEmbed(output);

                //send message

            }
        }

        [Command("calc")]
        [Description("Calculate the most optimal employee configuration.")]
        public async Task calc(CommandContext ctx) { 
            if (ctx.Member == null) {
                await Task.CompletedTask;
            } else {
                Player currPlayer = CommandsHelper.playerController.getObject(ctx.Member.Id);
                Exhibition[] optExhibitions = currPlayer.calc();
                string output = "";

                foreach (Exhibition i in optExhibitions) {
                    output += i.ToString();
                }

                DiscordEmbed embed = CommandsHelper.createEmbed(output);

                //send message

            }
        }

        private async Task employees() { 

        }

        private async Task exhibits() { 

        }

        [Command("status")]
        [Description("View the current status of your museum.")]
        public async Task status(CommandContext ctx) { 
            if (ctx.Member == null) {
                await Task.CompletedTask;
            } else {
                Player currPlayer = CommandsHelper.playerController.getObject(ctx.Member.Id);
            }
        }

    }
}