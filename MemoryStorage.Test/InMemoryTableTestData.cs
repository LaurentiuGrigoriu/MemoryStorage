using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoryStorage.Test
{
    internal class InMemoryTableTestData : IEnumerable<Object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            Random rnd = new();

            // create data for 100 tests
            for (int i = 1; i <= 100; i++)
            {
                string command = $"cmd {i}";
                CommandStatus status = (CommandStatus)(i % 5);
                string observation = $"Observation {i}";
                string scannerName = $"SCNR00{i}";
                DateTime createdOn = DateTime.Now;

                IEntry entry = new LCCommand()
                {
                    Command = command,
                    Status = status,
                    Observation = observation,
                    ScannerName = scannerName,
                    CreatedOn = createdOn
                };

                int mask = rnd.Next(1, 32);     // 5 bits random number
                IEntry update = new LCCommand()
                {
                    Command = ((mask >> 0) & 0x1) == 0x1 ? command + "01" : null,
                    Status = ((mask >> 1) & 0x1) == 0x1 ? (CommandStatus)((i + 1) % 5) : null,
                    Observation = ((mask >> 2) & 0x1) == 0x1 ? observation + "__" : null,
                    ScannerName = ((mask >> 3) & 0x1) == 0x1 ? scannerName + "00" : null,
                    CreatedOn = ((mask >> 4) & 0x1) == 0x1 ? createdOn.AddMinutes(1) : null
                };

                IEntry result = new LCCommand()
                {
                    Command = ((mask >> 0) & 0x1) == 0x1 ? command + "01" : command,
                    Status = ((mask >> 1) & 0x1) == 0x1 ? (CommandStatus)((i + 1) % 5) : status,
                    Observation = ((mask >> 2) & 0x1) == 0x1 ? observation + "__" : observation,
                    ScannerName = ((mask >> 3) & 0x1) == 0x1 ? scannerName + "00" : scannerName,
                    CreatedOn = ((mask >> 4) & 0x1) == 0x1 ? createdOn.AddMinutes(1) : createdOn
                };

                yield return new object[] { result, new IEntry[] { entry, update } };
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
