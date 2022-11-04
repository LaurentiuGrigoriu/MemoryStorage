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
        public string? Command { get; set; }
        public CommandStatus? Status { get; set; }
        public string? Observation { get; set; }
        public string? ScannerName { get; set; }
        public DateTime? CreatedOn { get; set; }

        public bool UpdatePartial(iEntry command)
        {
            if (command.GetType() != typeof(LCCommand))
            {
                throw new ArgumentException("Wrong argument in LCCommand.Uodate");
            }

            LCCommand cmd = (LCCommand)command;

            if (cmd.Id != 0 && cmd.Id != Id)
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

            return true;
        }

        public bool Match(iEntry filter)
        {
            if (filter.GetType() != typeof(LCCommand))
            {
                throw new ArgumentException("Wrong argument in LCCommand.Match");
            }

            LCCommand f = (LCCommand)filter;

            if (f.Id != 0 && f.Id != Id)
                return false;

            if (f.Command != null && f.Command != Command)
                return false;

            if (f.Status != null && f.Status != Status)
                return false;

            if (f.Observation != null && f.Observation != Observation)
                return false;

            if (f.ScannerName != null && f.ScannerName != ScannerName)
                return false;

            if (f.CreatedOn != null && f.CreatedOn != CreatedOn)
                return false;

            return true;
        }

        public void Print()
        {
            Console.WriteLine($"\tId={Id}");
            Console.WriteLine($"\tCommand={Command}");
            Console.WriteLine($"\tStatus={Status}");
            Console.WriteLine($"\tObservation={Observation}");
            Console.WriteLine($"\tScannerName={ScannerName}");
            Console.WriteLine($"\tCreatedOn={CreatedOn}");
        }
    }
}
