using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode2016.Day11
{
    enum DeviceType
    {
        Generator,
        Microchip
    }

    class Device
    {
        public Device(string name, DeviceType type)
        {
            Name = name;
            Type = type;
        }

        public string Name { get; set; }
        public DeviceType Type { get; private set; }
    }

    struct DevicePairState
    {
        public Device Generator { get; set; }
        public int GeneratorFloor { get; set; }
        public Device Microchip { get; set; }
        public int MicrochipFloor { get; set; }
    }

    class Node
    {
        private int _elevatorFloor;
        private List<DevicePairState> _deviceStates = new List<DevicePairState>();
        private static int NumberOfFloors;

        public Node(List<Device>[] initialDevicesPerFloor)
        {
            Steps = 0;
            _elevatorFloor = 1;
            NumberOfFloors = initialDevicesPerFloor.Length;

            var devicesWithFloor = initialDevicesPerFloor.SelectMany((list, floor) => list.Select(d => new Tuple<Device, int>(d, floor + 1)));
            var devicePairs =
                from deviceWithFloor in devicesWithFloor
                group deviceWithFloor by deviceWithFloor.Item1.Name into devicePair
                select devicePair;
            
            foreach (var pair in devicePairs)
            {
               _deviceStates.Add(new DevicePairState() {
                    Generator = pair.First(d => d.Item1.Type == DeviceType.Generator).Item1,
                    GeneratorFloor = pair.First(d => d.Item1.Type == DeviceType.Generator).Item2,
                    Microchip = pair.First(d => d.Item1.Type == DeviceType.Microchip).Item1,
                    MicrochipFloor = pair.First(d => d.Item1.Type == DeviceType.Microchip).Item2
                });
            }
        }

        private Node(Node parent, int newElevatorFloor, Device firstMovedDevice, Device secondMovedDevice)
        {
            _elevatorFloor = newElevatorFloor;
            Steps = parent.Steps + 1;
            _deviceStates = new List<DevicePairState>(parent._deviceStates);
            for (int i = 0; i < _deviceStates.Count; i++)
            {
                if (_deviceStates[i].Generator == firstMovedDevice)
                {
                    _deviceStates[i] = new DevicePairState() { Generator = firstMovedDevice, GeneratorFloor = newElevatorFloor, Microchip = _deviceStates[i].Microchip, MicrochipFloor = _deviceStates[i].MicrochipFloor };
                }
                if (_deviceStates[i].Microchip == firstMovedDevice)
                {
                    _deviceStates[i] = new DevicePairState() { Generator = _deviceStates[i].Generator, GeneratorFloor = _deviceStates[i].GeneratorFloor, Microchip = firstMovedDevice, MicrochipFloor = newElevatorFloor };
                }
                if (secondMovedDevice != null)
                { 
                    if (_deviceStates[i].Generator == secondMovedDevice)
                    {
                        _deviceStates[i] = new DevicePairState() { Generator = secondMovedDevice, GeneratorFloor = newElevatorFloor, Microchip = _deviceStates[i].Microchip, MicrochipFloor = _deviceStates[i].MicrochipFloor };
                    }
                    if (_deviceStates[i].Microchip == secondMovedDevice)
                    {
                        _deviceStates[i] = new DevicePairState() { Generator = _deviceStates[i].Generator, GeneratorFloor = _deviceStates[i].GeneratorFloor, Microchip = secondMovedDevice, MicrochipFloor = newElevatorFloor };
                    }
                }
            }
        }

        public int Steps { get; private set; }

        public bool IsSolution
        {
            get
            {
                return _deviceStates.All(ds => ds.GeneratorFloor == NumberOfFloors && ds.MicrochipFloor == NumberOfFloors);
            }
        }

        public bool IsValidState
        {
            get
            {
                for (int floor = 1; floor <= NumberOfFloors; floor++)
                {
                    var allDevicesOnFloor = DevicesOnFloor(floor);
                    var generators = allDevicesOnFloor.Where(d => d.Type == DeviceType.Generator);
                    var chips = allDevicesOnFloor.Where(d => d.Type == DeviceType.Microchip);

                    var connectedChips =
                        from c in chips
                        join g in generators on c.Name equals g.Name
                        select c;

                    if (generators.Any() && chips.Except(connectedChips).Any())
                    {
                        return false;
                    }
                }

                return true;
            }
        }

        public string GetStateDescription()
        {
            var result =
                from ds in _deviceStates
                orderby ds.GeneratorFloor descending, ds.MicrochipFloor descending
                select $"({ds.GeneratorFloor},{ds.MicrochipFloor})";

            return _elevatorFloor.ToString() + "," + String.Join<string>(",", result);
        }

        private IEnumerable<Device> DevicesOnCurrentFloor()
        {
            return DevicesOnFloor(_elevatorFloor);
        }

        private IEnumerable<Device> DevicesOnFloor(int floor)
        {
            var generators = from ds in _deviceStates where ds.GeneratorFloor == floor select ds.Generator;
            foreach (var generator in generators) yield return generator;
            var chips = from ds in _deviceStates where ds.MicrochipFloor == floor select ds.Microchip;
            foreach (var chip in chips) yield return chip;
        }

        public IEnumerable<Node> GetPotentialMoves()
        {
            var devicesOnCurrentFloor = DevicesOnCurrentFloor();

            foreach (var singleDeviceToMove in devicesOnCurrentFloor)
            {
                if (_elevatorFloor < NumberOfFloors) yield return new Node(this, _elevatorFloor + 1, singleDeviceToMove, null);
                if (_elevatorFloor > 1) yield return new Node(this, _elevatorFloor - 1, singleDeviceToMove, null);
            }

            List<Device> paired = new List<Device>();
            foreach (var firstDeviceInPair in devicesOnCurrentFloor)
            {
                paired.Add(firstDeviceInPair);
                foreach (var secondDeviceInPair in devicesOnCurrentFloor.Except(paired))
                {
                    if (_elevatorFloor < NumberOfFloors) yield return new Node(this, _elevatorFloor + 1, firstDeviceInPair, secondDeviceInPair);
                    if (_elevatorFloor > 1) yield return new Node(this, _elevatorFloor - 1, firstDeviceInPair, secondDeviceInPair);
                }
            }
        }
    }

    static class Solve11
    {
        public static void Solve()
        {
            const bool Part2 = true;
            string[] input = File.ReadAllLines("Day11\\input11.txt");
            List<Device>[] initialFloorState = new List<Device>[input.Length];
            for (int i = 0; i < input.Length; i++)
            {
                initialFloorState[i] = new List<Device>();
                if (input[i].Contains("nothing relevant")) continue;
                foreach (string deviceDescription in input[i].Split(new[] { " a " }, StringSplitOptions.None).Skip(1))
                {
                    string name = deviceDescription.Substring(0, deviceDescription.IndexOfAny(new[] { ' ', '-' }));
                    DeviceType type = deviceDescription.Contains("microchip") ? DeviceType.Microchip : DeviceType.Generator;

                    initialFloorState[i].Add(new Device(name, type));
                }
            }

            if (Part2)
            {
                initialFloorState[0].Add(new Device("elerium", DeviceType.Generator));
                initialFloorState[0].Add(new Device("elerium", DeviceType.Microchip));
                initialFloorState[0].Add(new Device("dilithium", DeviceType.Generator));
                initialFloorState[0].Add(new Device("dilithium", DeviceType.Microchip));
            }

            Node startNode = new Node(initialFloorState);
            Queue<Node> nodesToVisit = new Queue<Node>();
            Dictionary<string, Node> visitedStates = new Dictionary<string, Node>();

            nodesToVisit.Enqueue(startNode);

            while (nodesToVisit.Count > 0)
            {
                Node currentNode = nodesToVisit.Dequeue();
                
                if (visitedStates.ContainsKey(currentNode.GetStateDescription()))
                {
                    continue;
                }
                else
                {
                    visitedStates.Add(currentNode.GetStateDescription(), currentNode);
                }

                if (currentNode.IsSolution)
                {
                    Console.WriteLine(currentNode.Steps);
                    break;
                }

                foreach (var nextNode in currentNode.GetPotentialMoves())
                {
                    if (nextNode.IsValidState && !visitedStates.ContainsKey(nextNode.GetStateDescription()))
                    {
                        nodesToVisit.Enqueue(nextNode);
                    }
                }
            }
        }
    }
}
