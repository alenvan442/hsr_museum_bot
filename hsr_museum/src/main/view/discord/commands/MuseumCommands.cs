using System.Collections.Immutable;
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
    public class MuseumCommands : BaseCommandModule
    {
        [Command("hire")]
        [Description("Hires an available employee for your museum. Use .hire without any parameters to get a list of employees you do not currently own.")]
        public async Task hire(CommandContext ctx,
                [Description("Select which employee to hire.")]
                int index = 0)
        {
            if (ctx.Member == null)
            {
                await Task.CompletedTask;
            }
            else
            {

                Player currPlayer = CommandsHelper.playerController.getObject(ctx.Member.Id);
                Employee[] currEmployee = currPlayer.getEmployees();
                Employee[] allEmployees = CommandsHelper.employeeController.getObjects();
                Employee[] freeEmployees = new Employee[allEmployees.Count() - currEmployee.Count()];
                string output = "";

                if (freeEmployees.Count() == 0)
                {
                    DiscordEmbed baseCase = CommandsHelper.createEmbed("No other Employee can be hired");
                    await ctx.Channel.SendMessageAsync(baseCase);
                    return;
                }

                int inc = 0;
                foreach (Employee i in allEmployees)
                {
                    if (currEmployee.Contains(i))
                    {
                        continue;
                    }
                    else
                    {
                        freeEmployees[inc] = i;
                        inc++;
                    }
                }

                inc = 1;
                if (index == 0)
                {
                    foreach (Employee i in freeEmployees)
                    {
                        output += inc + ". " + i.name + "\n";
                        inc++;
                    }
                }
                else
                {
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
                int index = 0)
        {

            if (ctx.Member == null)
            {
                await Task.CompletedTask;
            }
            else
            {
                Player currPlayer = CommandsHelper.playerController.getObject(ctx.Member.Id);
                Employee[] currEmployees = currPlayer.getEmployees();
                string output = "";

                int inc = 1;
                if (index == 0)
                {
                    foreach (Employee i in currEmployees)
                    {
                        output += inc + ". " + i.name + "\n";
                    }
                    if (output == "")
                    {
                        output = "No other Employee can be released";
                    }
                }
                else
                {
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
                int index = 0)
        {

            if (ctx.Member == null)
            {
                await Task.CompletedTask;
            }
            else
            {
                Player currPlayer = CommandsHelper.playerController.getObject(ctx.Member.Id);
                Exhibition[] currExhibition = currPlayer.getExhibitions();
                Exhibition[] allExhibition = CommandsHelper.exhibitionController.getObjects();
                Exhibition[] freeExhibition = new Exhibition[allExhibition.Count() - currExhibition.Count()];
                string output = "";

                if (freeExhibition.Count() == 0)
                {
                    DiscordEmbed baseCase = CommandsHelper.createEmbed("No other Exhibition can be added");
                    await ctx.Channel.SendMessageAsync(baseCase);
                    return;
                }

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
                if (index == 0)
                {
                    foreach (Exhibition i in freeExhibition)
                    {
                        output += inc + ". " + i.name + "\n";
                        inc++;
                    }
                }
                else
                {
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
                int index = 0)
        {

            if (ctx.Member == null)
            {
                await Task.CompletedTask;
            }
            else
            {
                Player currPlayer = CommandsHelper.playerController.getObject(ctx.Member.Id);
                Exhibition[] currExhibitions = currPlayer.getExhibitions();
                string output = "";

                int inc = 1;
                if (index == 0)
                {
                    foreach (Exhibition i in currExhibitions)
                    {
                        output += inc + ". " + i.name + "\n";
                        inc++;
                    }
                    if (output == "")
                    {
                        output = "No other Exhibition can be removed";
                    }
                }
                else
                {
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
                int level = 0)
        {

            //TODO better output embed strings
            if (ctx.Member == null)
            {
                await Task.CompletedTask;
            }
            else
            {
                Player currPlayer = CommandsHelper.playerController.getObject(ctx.Member.Id);
                Exhibition[] currExhibitions = currPlayer.getExhibitions();

                if (currExhibitions.Count() == 0)
                {
                    DiscordEmbed baseCase = CommandsHelper.createEmbed("No other Exhibition can be upgraded");
                    await ctx.Channel.SendMessageAsync(baseCase);
                    return;
                }

                DiscordEmbedBuilder embed = CommandsHelper.createEmbed("");
                if (index == 0)
                {
                    foreach (Exhibition i in currPlayer.getExhibitions())
                    {
                        uint[] exVals = i.exValues();

                        embed.AddField(
                            i.name + "  " + exVals[0] + " " +
                                            exVals[1] + " " +
                                            exVals[2],
                            "Level: " + i.level + "\n" +
                            "Tour Level: " + i.tourDurationLevel + "\n" +
                            "Educational Level: " + i.educationalValueLevel + "\n" +
                            "Visitor Level: " + i.visitorAppealLevel);
                    }
                }
                else
                {
                    if (option == 0 || level == 0)
                    {
                        embed.WithDescription(
                            "Please specify both an option and a level you would like to set." +
                            "(.upgrade (index) (option) (level)) Use .help upgrade for more information.");
                    }
                    else
                    {
                        bool result = CommandsHelper.playerController.upgradeExhibition(
                                        currPlayer.id, index - 1, currExhibitions, option, level);
                        embed.WithDescription(result ? 
                            "Sucessfully upgrade your exhibition." : "Failed to upgrade your exhibition.");
                    }
                }

                //send message
                await ctx.Channel.SendMessageAsync(embed);

            }
        }

        [Command("calc")]
        [Description("Calculate the most optimal employee configuration.")]
        public async Task calc(CommandContext ctx)
        {
            //TODO DEBUG
            if (ctx.Member == null)
            {

                await Task.CompletedTask;
            }
            else
            {
                Player currPlayer = CommandsHelper.playerController.getObject(ctx.Member.Id);

                if (currPlayer.getExhibitions().Count() < 0)
                {
                    DiscordEmbed embed = CommandsHelper.createEmbed("You need exhibitions in order to run calc");
                    await ctx.Channel.SendMessageAsync(embed);
                    return;
                }

                Exhibition[] optExhibitions = currPlayer.calc();
                DiscordEmbedBuilder builder = new DiscordEmbedBuilder
                {
                    Title = "Optimzation for " + ctx.Member.DisplayName
                };

                foreach (Exhibition i in optExhibitions)
                {
                    Employee[] employees = i.employees.ToArray();
                    string employed = "";
                    foreach (Employee j in employees)
                    {
                        employed += j.name + "    ";
                    }

                    int[] offsets = i.offsets();
                    uint[] exVals = i.exValues();

                    builder.AddField(
                        i.name + "  " + (offsets[0] + exVals[0]) + "/" + exVals[0] + " " +
                                        (offsets[1] + exVals[1]) + "/" + exVals[1] + " " +
                                        (offsets[2] + exVals[2] + "/" + exVals[2]),
                        employed);
                    i.clearEmployees();
                }

                //send message
                await ctx.Channel.SendMessageAsync(builder.Build());

            }
        }

        [Command("status")]
        [Description("View the current status of your museum.")]
        public async Task status(CommandContext ctx,
                [Description("Select which status you would like to see. (employees, exhibitions, all)")]
                string option = "")
        {

            //TODO better output strings, use embed fields
            if (ctx.Member == null)
            {
                await Task.CompletedTask;
            }
            else
            {
                Player currPlayer = CommandsHelper.playerController.getObject(ctx.Member.Id);
                DiscordEmbedBuilder embed = CommandsHelper.createEmbed("");
                string title;
                string description = "";
                switch (option.ToLower())
                {
                    case "employees":
                    case "employee":
                        title = "Employees";
                        foreach (Employee i in currPlayer.getEmployees())
                        {
                            description += i.ToString() + "\n";
                        }
                        break;
                    case "exhibitions":
                    case "exhibition":
                        title = "Exhibitions";
                        foreach (Exhibition i in currPlayer.getExhibitions())
                        {
                            uint[] exVals = i.exValues();

                            embed.AddField(
                                i.name + "  " + exVals[0] + " " +
                                                exVals[1] + " " +
                                                exVals[2],
                                "Level: " + i.level + "\n" + 
                                "Tour Level: " + i.tourDurationLevel + "\n" + 
                                "Educational Level: " + i.educationalValueLevel + "\n" + 
                                "Visitor Level: " + i.visitorAppealLevel);
                        }
                        description = "n/a";
                        break;
                    case "all":
                    case "":
                        title = "Exhibitions and Employees";
                        foreach (Exhibition i in currPlayer.getExhibitions())
                        {
                            uint[] exVals = i.exValues();

                            embed.AddField(
                                i.name + "  " + exVals[0] + " " +
                                                exVals[1] + " " +
                                                exVals[2],
                                "Level: " + i.level + "\n" +
                                "Tour Level: " + i.tourDurationLevel + "\n" +
                                "Educational Level: " + i.educationalValueLevel + "\n" +
                                "Visitor Level: " + i.visitorAppealLevel);
                        }
                        foreach (Employee i in currPlayer.getEmployees())
                        {
                            description += i.ToString() + "\n";
                        }
                        break;
                    default:
                        title = "unknown error TODO";
                        break;
                }

                if (description == "") { description = "You do not have any, add some then try again."; }

                embed.WithTitle(title);
                if (description != "n/a") {
                    embed.WithDescription(description);
                }

                await ctx.Channel.SendMessageAsync(embed);
            }
        }

    }
}