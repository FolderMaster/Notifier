using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp.Settings
{
    public class Settings
    {
        public DataBaseSettings DataBase { get; set; }

        public DiscordSettings Discord { get; set; }

        public EmailSettings Email { get; set; }

        public JiraSettings Jira { get; set; }
    }
}
