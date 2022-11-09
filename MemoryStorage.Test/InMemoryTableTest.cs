using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace MemoryStorage.Test
{
    public class InMemoryTableTest
    {
        private readonly InMemoryTable<LCCommand> _tut;     // table under test

        public InMemoryTableTest()
        {
            /* Note:
             * The tests run cmpletely in parallel by default
             * and they do not affect each other because
             * each test is running on its own instance of InMemoryTableTest class 
             * so that _tut will not be reused for several tests */
            _tut = new InMemoryTable<LCCommand>("LCCommandQueueItems");
        }

        private void FillTable(int length)
        {
            for (int i = 0; i < length; i++)
            {
                IEntry entry = new LCCommand()
                {
                    Command = $"cmd {i}",
                    Status = (CommandStatus)(i % 5),
                    Observation = $"LCCommand observation {i}",
                    ScannerName = "SCNR26739856",
                    CreatedOn = DateTime.Now
                };

                if (_tut.Create(ref entry))
                {
#if DEBUG
                    Console.WriteLine($"Created LCCommand Id={entry.Id}");
#endif
                }
            }

        }

        [Theory]
        [InlineData(1, "cmd 0", CommandStatus.New, "LCCommand observation 0", "SCNR26739856")]
        [InlineData(7, "cmd 6", CommandStatus.Open, "LCCommand observation 6", "SCNR26739856")]
        [InlineData(13, "cmd 12", CommandStatus.Running, "LCCommand observation 12", "SCNR26739856")]
        [InlineData(19, "cmd 18", CommandStatus.Suscess, "LCCommand observation 18", "SCNR26739856")]
        public void ReadTableEtriesCorrectly(
            int id, string command, CommandStatus status, string observation, string ScannerName)
        {
            FillTable(20);

            LCCommand? cmd = (LCCommand?)_tut.Read(id);
            Assert.Equal(command, cmd?.Command);
            Assert.Equal(status, cmd?.Status);
            Assert.Equal(observation, cmd?.Observation);
            Assert.Equal(ScannerName, cmd?.ScannerName);
        }

        [Fact]
        public void FilterTableReturnsTheRightList()
        {
            FillTable(20);

            LCCommand filter = new LCCommand() { Status = CommandStatus.Running };

            List<LCCommand> openCmdsList = _tut.Read(filter).Cast<LCCommand>().ToList();
            List<int> indexList = new List<int> { 3, 8, 13, 18 };

            for (int i = 0; i < openCmdsList.Count; i++)
            {
                Assert.Equal(CommandStatus.Running, openCmdsList[i].Status);
                Assert.True(indexList?.Contains(openCmdsList[i].Id));

                Assert.True(indexList.Remove(openCmdsList[i].Id));
            }

            Assert.Empty(indexList);
        }

        [Fact]
        public void DeleteEntryThenReadEntry()
        {
            List<int> indexList = new List<int> { 1, 5, 9, 12, 16, 20 };

            FillTable(20);

            foreach(int i in indexList)
            {
                Assert.True(_tut.Read(i) != null);
                _tut.Delete(i);
                Assert.True(_tut.Read(i) == null);
            }
        }

        [Fact]
        public void DeleteIsIdempotent()
        {
            FillTable(10);

            // delete unexisting Id
            Assert.True(_tut.Delete(100));

            // delete same entry several times
            Assert.True(_tut.Read(3) != null);
            Assert.True(_tut.Delete(3));
            Assert.True(_tut.Read(3) == null);
            Assert.True(_tut.Delete(3));
            Assert.True(_tut.Delete(3));
        }

        [Theory]
        [MemberData(nameof(TestData))]
        public void CreateReadCompareSeveralEntries(IEntry _, IEntry[] operands)
        {
            IEntry entry = operands[0];

            for (int i = 1; i <= 100_000; i++)   // 100 000 size table
            {
                Assert.True(_tut.Create(ref entry));

                LCCommand? cmd = (LCCommand?)_tut.Read(i);

                Assert.NotNull(cmd);
                Assert.True(cmd.Match(entry));
            }
        }

        [Theory]
        [MemberData(nameof(TestData))]
        public void CreateUpdateReadCompareEntry(IEntry result, IEntry[] operands)
        {
            IEntry entry = operands[0];
            IEntry update = operands[1];

            Assert.True(_tut.Create(ref entry));
            update.Id = entry.Id;

            LCCommand? cmd = (LCCommand?)_tut.Copy(entry.Id);

            Assert.NotNull(cmd);
            Assert.True(cmd.Match(entry));

            Assert.True(_tut.Update(update));
            LCCommand? cmdUpdated = (LCCommand?)_tut.Read(entry.Id);
            Assert.False(cmdUpdated.Match(cmd));
        }


        // create 100 test data-sets
        public static IEnumerable<object[]> TestData()
        {
            Random rnd = new();

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
    }
}