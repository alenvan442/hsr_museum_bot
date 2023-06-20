using System.Diagnostics.CodeAnalysis;
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
        [Description("Hires an available employee for your museum. Use .hire without any parameters to get a list of employees you do not currently own.")]
        public async Task hire(CommandContext ctx, 
                [Description("Select which employee to hire.")]
                int index = 0) {
            if (ctx.Member == null) {
                await Task.CompletedTask;
            } else {

                Player currPlayer = CommandsHelper.playerController.getObject(ctx.Member.Id);
                Employee[] currEmployee = currPlayer.getEmployees();
                Employee[] allEmployees = CommandsHelper.employeeController.getObjects();
                Employee[] freeEmployees = new Employee[allEmployees.Count() - currEmployee.Count()];
                string output = "";

                int inc = 0;
                foreach (Employee i in allEmployees) {
                    if (currEmployee.Contains(i)) {
                        continue;
                    } else {
                        freeEmployees[inc] = i;
                        inc++;
                    }
                }

                inc = 1;
                if (index == 0) {
                    foreach (Employee i in freeEmployees) {
                        output += inc + ". " + i.name + "\n";
                        inc++;
                    }
                    if (output == "")
                    {
                        output = "No other Employee can be hired";
                    }
                } else {
                    bool result = CommandsHelper.playerController.hireEmployee(currPlayer.id, index - 1, freeEmployees);

                    output = result ? "Hired successfully" : "Hired failed";
                }

                DiscordEmbed embed = CommandsHelper.createEmbed(output);

                //send message
                await ctx.Channel.SendMessageAsync(embed);

            }
        }

        [Command("release")]
        [Description("Releases an employee. Use .release without any parameters to get a list of your employees.")]
        public async Task release(CommandContext ctx, 
                [Description("Select which employee to remove.")]
                int index = 0) {
    
            if (ctx.Member == null) {
                await Task.CompletedTask;
            } else {
                Player currPlayer = CommandsHelper.playerController.getObject(ctx.Member.Id);
                Employee[] currEmployees = currPlayer.getEmployees();
                string output = "";

                int inc = 1;
                if (index == 0) {
                    foreach (Employee i in currEmployees) {
                        output += inc + ". " + i.name + "\n";
                    }
                    if (output == "")
                    {
                        output = "No other Employee can be released";
                    }
                } else {
                    bool result = CommandsHelper.playerController.removeEmployee(currPlayer.id, index - 1, currEmployees);
                    output = result ? "Release employee successfully" : "Failed to release employee";
                }

                DiscordEmbed embed = CommandsHelper.createEmbed(output);

                //send message
                await ctx.Channel.SendMessageAsync(embed);

            }
        }

        [Command("add")]
        [Description("Add an exhibition to your museum. Use .add without any parameters to get a list of exhibitions you do not currently own.")]
        public async Task add(CommandContext ctx, 
                [Description("Select which exhibition to add.")]
                int index = 0) { 

            if (ctx.Member == null) {
                await Task.CompletedTask;
            } else {
                Player currPlayer = CommandsHelper.playerController.getObject(ctx.Member.Id);
                Exhibition[] currExhibition = currPlayer.getExhibitions();
                Exhibition[] allExhibition = CommandsHelper.exhibitionController.getObjects();
                Exhibition[] freeExhibition = new Exhibition[allExhibition.Count() - currExhibition.Count()];
                string output = "";

                int inc = 0;
                foreach (Exhibition i in allExhibition)
                {
                    if (currExhibition.Contains(i))
                    {
                        continue;
                    }
                    else
                    {
                        freeExhibition[inc] = i;
                        inc++;
                    }
                }

                inc = 1;
                if (index == 0) {
                    foreach (Exhibition i in freeExhibition) {
                        output += inc + ". " + i.name + "\n";
                        inc++;
                    }
                    if (output == "") {
                        output = "No other Exhibition can be added";
                    }
                } else {
                    bool result = CommandsHelper.playerController.addExhibition(currPlayer.id, index - 1, freeExhibition);
                    output = result ? "Added exhibition successfully." : "Exhibition failed to be added.";
                }

                DiscordEmbed embed = CommandsHelper.createEmbed(output);

                //send message
                await ctx.Channel.SendMessageAsync(embed);

            }
        }

        [Command("remove")]
        [Description("Remove an exhibition from your museum. Use .remove without any parameters to get a list of your exhibitions.")]
        public async Task remove(CommandContext ctx, 
                [Description("Select which exhibition to remove.")]
                int index = 0) { 

            if (ctx.Member == null) {
                await Task.CompletedTask;
            } else {
                Player currPlayer = CommandsHelper.playerController.getObject(ctx.Member.Id);
                Exhibition[] currExhibitions = currPlayer.getExhibitions();
                string output = "";

                int inc = 1;
                if (index == 0) {
                    foreach (Exhibition i in currExhibitions) {
                        output += inc + ". " + i.name + "\n";
                        inc++;
                    }
                    if (output == "")
                    {
                        output = "No other Exhibition can be removed";
                    }
                } else {
                    bool result = CommandsHelper.playerController.removeExhibition(currPlayer.id, index - 1, currExhibitions);
                    output = result ? "Removed an exhibition successfully" : "Failed to remove target exhibition";
                }

                DiscordEmbed embed = CommandsHelper.createEmbed(output);

                //send message
                await ctx.Channel.SendMessageAsync(embed);
            }
        }

        [Command("upgrade")]
        [Description("Upgrade an exhibition. Use .upgrade with no parameters to get a list of your exhibitions.")]
        public async Task upgrade(CommandContext ctx,
                [Description("Select which exhibition to upgrade.")] 
                int index = 0,
                [Description("Select which parameter of the exhibition to upgrade.")] 
                int option = 0, 
                [Description("What level to set the parameter to.")]
                int level = 0) { 

            if (ctx.Member == null) {
                await Task.CompletedTask;
            } else {
                Player currPlayer = CommandsHelper.playerController.getObject(ctx.Member.Id);
                Exhibition[] currExhibitions = currPlayer.getExhibitions();
                string output = "";

                int inc = 1;
                if(index == 0) {
                    foreach (Exhibition i in currExhibitions) {
                        output += inc + ". " + i.name + "\n";
                    }
                    if (output == "")
                    {
                        output = "No other Exhibition can be upgraded";
                    }
                } else { 
                    if(option == 0 || level == 0) {
                        output = "Please specify both an option and a level you would like to set. (.upgrade (index) (option) (level)) Use .help upgrade for more information.";
                    } else {
                        bool result = CommandsHelper.playerController.upgradeExhibition(
                                        currPlayer.id, index - 1, currExhibitions, option, level);
                        output = result ? "Sucessfully upgrade your exhibition." : "Failed to upgrade your exhibition.";
                    }
                }

                DiscordEmbed embed = CommandsHelper.createEmbed(output);

                //send message
                await ctx.Channel.SendMessageAsync(embed);

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
                    i.clearEmployees();
                }

                if (output == "") {
                    output = "You have no exhibitions to optimize.";
                }

                DiscordEmbed embed = CommandsHelper.createEmbed(output);

                //send message
                await ctx.Channel.SendMessageAsync(embed);

            }
        }

        [Command("status")]
        [Description("View the current status of your museum.")]
        public async Task status(CommandContext ctx,
                [Description("Select which status you would like to see. (employees, exhibitions, all)")]
                string option = "") {

            if (ctx.Member == null) {
                await Task.CompletedTask;
            }
            else {
                Player currPlayer = CommandsHelper.playerController.getObject(ctx.Member.Id);
                string output = "";
                switch (option.ToLower()) {
                    case "employees":
                    case "employee":
                        foreach (Employee i in currPlayer.getEmployees()) {
                            output += i.ToString() + "\n";
                        }
                        break;
                    case "exhibitions":
                    case "exhibition":
                        foreach (Exhibition i in currPlayer.getExhibitions()) {
                            output += i.ToString() + "\n";
                        }
                        break;
                    case "all":
                    case "":
                        foreach (Exhibition i in currPlayer.getExhibitions()) {
                            output += i.ToString() + "\n";
                        }
                        foreach (Employee i in currPlayer.getEmployees()) {
                            output += i.ToString() + "\n";
                        }
                        break;
                    default:
                        output = "unknown error TODO";
                        break;
                }

                DiscordEmbed embed = CommandsHelper.createEmbed(output);
                await ctx.Channel.SendMessageAsync(embed);
            }
        }

    }
}