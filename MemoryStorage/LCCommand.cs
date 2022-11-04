using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoryStorage
{
    internal class LCCommand : iEntry
    {
        public int Id { get; set; }
        public string Command { get; set; }
        public CommandStatus? Status { get; set; }
        public string Observation { get; set; }
        public string ScannerName { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string Token { get; set; }

        public LCCommand(int id, string command, CommandStatus? status, string observation, string scannerName, DateTime? createdOn, string token)
        {
            Id = id;
            Command = command;
            Status = status;
            Observation = observation;
            ScannerName = scannerName;
            CreatedOn = createdOn;
            Token = token;
        }

        public bool Update(iEntry command)
        {
            if (command.GetType() != typeof(LCCommand))
            {
                throw new ArgumentException("Wrong argument in LCCommand.Uodate");
            }

            LCCommand cmd = (LCCommand)command;

            if (cmd.Id != Id)
                return false;

            if (cmd.Command != null)
                Command = cmd.Command;

            if (cmd.Status != null)
                Status = cmd.Status;

            if (cmd.Observation != null)
                Observation = cmd.Observation;

            if (cmd.ScannerName != null)
                ScannerName = cmd.ScannerName;

            if (cmd.CreatedOn != null)
                CreatedOn = cmd.CreatedOn;

            if (cmd.Token != null)
                Token = cmd.Token;

            return true;
        }
    }
}
