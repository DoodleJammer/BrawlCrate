﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;
using BrawlLib.IO;
using BrawlLib.SSBBTypes;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class CENode : ARCEntryNode
    {
        internal int unk1, unk2, unk3;
        internal CEHeader* Header => (CEHeader*) WorkingUncompressed.Address;

        public override ResourceType ResourceFileType => ResourceType.CE;

        [Category("Offensive AI Node")] public int NumEntries => Children.Count;

        public override bool OnInitialize()
        {
            if (_name == null) _name = "CE " + Parent.Name.Replace("ai_", "");

            unk1 = Header->_unk1;
            unk2 = Header->_unk2;
            unk3 = Header->_unk3;

            return Header->_numEntries > 0;
        }

        public override void OnPopulate()
        {
            new CEGroupNode("Events").Initialize(this, new DataSource(Header, 0x0));
            new CEGroupNode("Strings").Initialize(this, new DataSource(Header, 0x0));
        }

        internal static ResourceNode TryParse(DataSource source)
        {
            var header = (CEHeader*) source.Address;
            if (header->_numEntries <= 0 || header->_numEntries > 0x100 || header->_unk1 != 0 ||
                header->_unk2 != 0x1000000 || header->_unk3 != 0) return null;

            return new CENode();
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            var header = (CEHeader*) address;
            header->_unk1 = unk1;
            header->_unk2 = unk2;
            header->_unk3 = unk3;
            header->_numEntries = Children[0].Children.Count;
            Children[0].Rebuild(address, 0x0, true); //rebuild CEEntries
            var EntrySize = Children[0].CalculateSize(true); //Caluculate CEEntry's size
            var offset = EntrySize + header->entryOffsets[0]; //set first CEString offset
            for (var i = 0; i < Children[1].Children.Count; i++)
            {
                header->entryOffsets[Children[0].Children.Count + i] = offset;
                offset += Children[1].Children[i].CalculateSize(true);
            }

            Children[1].Rebuild(address + header->entryOffsets[0] + EntrySize, 0x0, true);
        }

        public CEEntryNode CreateCEEntryNode()
        {
            var n = new CEEntryNode();
            Children[0].AddChild(n);
            return n;
        }

        public CEStringNode CreateCEStringNode()
        {
            var n = new CEStringNode();
            Children[1].AddChild(n);
            return n;
        }

        public override int OnCalculateSize(bool force)
        {
            var size = 0x10 + Children[0].Children.Count * 0x4 + Children[1].Children.Count * 0x4; //CEheader size
            if (size % 0x10 != 0) size = size + 0x10 - size % 0x10;

            size += Children[0].CalculateSize(true);
            size += Children[1].CalculateSize(true);
            return size;
        }
    }

    public unsafe class CEGroupNode : ResourceNode
    {
        public CEGroupNode()
        {
        }

        public CEGroupNode(string name)
        {
            _name = name;
        }

        internal CEHeader* Header => (CEHeader*) WorkingUncompressed.Address;


        public override bool OnInitialize()
        {
            return true;
        }

        //when use this you must send first address or string address
        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            var header = (CEHeader*) address;
            VoidPtr entry = null;
            var offset = 0x10 + Children.Count * 0x4 + Parent.Children[1].Children.Count * 0x4;
            if (offset % 0x10 != 0)
                entry = address + offset + 0x10 - offset % 0x10;
            else
                entry = address + offset;

            var count = 0;
            header->entryOffsets[count] = entry - address; //write offset of first entry
            if (_name == "Events")
                foreach (CEEntryNode n in Children)
                {
                    var entrySize = n.CalculateSize(true);
                    n.Rebuild(entry, entrySize, true);
                    entry += entrySize;
                    count++;
                    header->entryOffsets[count] = entry - address; //write each offset of entries
                }

            if (_name == "Strings")
                foreach (CEStringNode n in Children) //"address" is first address of CEString
                {
                    var entrySize = n.CalculateSize(true);
                    n.Rebuild(address, entrySize, true);
                    address += entrySize;
                }
        }

        public override void OnPopulate()
        {
            Type t = null; //switch by name
            if (_name == "Events")
                t = typeof(CEEntryNode);
            else if (_name == "Strings")
                t = typeof(CEStringNode);
            else
                return;

            if (t == typeof(CEEntryNode)) //is this CEEntryNode?
            {
                for (var i = 0; i < Header->_numEntries; i++)
                {
                    var entry = (VoidPtr) Header + Header->entryOffsets[i];
                    var node = new CEEntryNode
                    {
                        NextAddress = (VoidPtr) Header + Header->entryOffsets[i + 1]
                    };
                    node.Initialize(this, new DataSource(entry, node.NextAddress - entry));
                }
            }
            else //this is CEStringNode
            {
                var offset = &Header->entryOffsets[Header->_numEntries];
                for (; offset < (VoidPtr) Header + Header->entryOffsets[0]; offset++)
                    if (*offset != 0x0)
                    {
                        var node = new CEStringNode();
                        node.Initialize(this, new DataSource((VoidPtr) Header + *offset, 0x0));
                    }
            }
        }

        public override int OnCalculateSize(bool force)
        {
            var size = 0;
            if (_name == "Events") //calculate size of all CEEntryNode
                foreach (CEEntryNode n in Children)
                    size += n.CalculateSize(true);
            else //calculate size of all CEStringNode
                foreach (CEStringNode n in Children)
                    size += n.CalculateSize(true);

            return size;
        }
    }

    public unsafe class CEEntryNode : ResourceNode
    {
        private readonly List<int> IndexList = new List<int>();
        private List<float> entries = new List<float>();
        private int id, EventsOffset, part2Offset, unknown;
        public VoidPtr NextAddress = 0;
        internal CEEntry* Header => (CEEntry*) WorkingUncompressed.Address;

        public override ResourceType ResourceFileType => ResourceType.CEEntry;

        [Category("CEEntry")]
        public string ID
        {
            get => id.ToString("X");
            set
            {
                id = Convert.ToInt32(value, 16);
                SignalPropertyChange();
            }
        }

        [Category("CEEntry")] public string TrueID => (id & 0x7FFF).ToString("X");

        [Category("CEEntry")]
        public int Unknown
        {
            get => unknown;
            set
            {
                unknown = value;
                SignalPropertyChange();
            }
        }

        [Category("CEEntry")] public int NumEntries => Children.Count;

        [Category("CEEntry")]
        public List<float> Part2Entries
        {
            get => entries;
            set
            {
                entries = value;
                SignalPropertyChange();
            }
        }


        public override bool OnInitialize()
        {
            entries.Clear();
            if (_name == null || _name == "") _name = ((int) Header->_ID).ToString("X");

            var part2 = Header->part2;
            while (part2 < (bfloat*) (VoidPtr) Header + WorkingUncompressed.Length)
            {
                entries.Add(*part2);
                part2++;
            }

            id = Header->_ID;
            EventsOffset = Header->_EventsOffset;
            part2Offset = Header->_part2Offset;
            unknown = Header->_unknown;
            _name = TrueID;
            return true;
        }


        public override void OnPopulate()
        {
            CEEvent* currentEvent = null;
            var current = Header->Event;
            while ((bfloat*) current < Header->part2)
            {
                currentEvent = (CEEvent*) current;
                var temp = new CEEventNode();
                temp.Initialize(this, new DataSource(current, currentEvent->_entrySize));
                current += 0x4 + 0x4 * temp.NumEntries;
            }

            for (var i = 0; i < Part2Entries.Count; i++) IndexList.Add(i);
        }

        public float? GetPart2(int id)
        {
            if (IndexList.IndexOf(id - 0x2000) >= 0)
                return Part2Entries[IndexList.IndexOf(id - 0x2000)];
            return null;
        }

        public int SetPart2(float value, int insertID) //returns id
        {
            if (GetPart2(insertID) != value && GetPart2(insertID) != null)
            {
                IndexList.Insert(insertID - 0x2000, Part2Entries.Count);
                Part2Entries.Insert(insertID - 0x2000, value);
                return IndexList[insertID - 0x2000] + 0x2000;
            }

            if (GetPart2(insertID) == null) //if insertID is out of range of Part2Entries
            {
                IndexList.Add(Part2Entries.Count);
                Part2Entries.Add(value);
                return IndexList[IndexList.Count - 1] + 0x2000;
            }

            return insertID; //if there's no change
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            var header = (CEEntry*) address;
            var eventSize = 0;
            header->_EventsOffset = EventsOffset;
            header->_ID = id;
#if DEBUG
            header->_ID = Convert.ToInt32(_name, 16);//Debug Code
#endif
            header->_unknown = unknown;
            var currentAddress = header->Event;
            foreach (CEEventNode n in Children)
            {
                var Size = n.CalculateSize(true);
                eventSize += Size;
                for (var i = 0; i < n.Entries.Count; i++)
                    if (n.Entries[i] >= 0x2000)
                        n.Entries[i] = IndexList.IndexOf(n.Entries[i] - 0x2000) + 0x2000;
                n.Rebuild(currentAddress, Size, true);
                currentAddress += Size;
            }

            header->_part2Offset = eventSize + 0x10; //header size
            for (var i = 0; i < Part2Entries.Count; i++) header->part2[i] = Part2Entries[i];
        }

        public override int OnCalculateSize(bool force)
        {
            var eventSize = 0;
            var part2Size = 0;
            var headerSize = 0x10;
            foreach (CEEventNode n in Children) eventSize += n.EntrySize;
            part2Size += Part2Entries.Count * 0x4;
            if ((eventSize + part2Size + headerSize) % 0x10 != 0)
                return eventSize + part2Size + headerSize + 0x10 - (eventSize + part2Size + headerSize) % 0x10;
            return eventSize + part2Size + headerSize;
        }
    }

    public unsafe class CEEventNode : ResourceNode
    {
        private readonly List<float> param = new List<float>();
        public List<int> Entries = new List<int>();
        private sbyte type;
        internal CEEvent* Header => (CEEvent*) WorkingUncompressed.Address;

        public override ResourceType ResourceFileType => ResourceType.CEEvent;

        [Category("CE Event")]
        [Description("Entry Type")]
        public sbyte Type
        {
            get => type;
            set
            {
                type = value;
                SignalPropertyChange();
            }
        }

        [Category("CE Event")] public int NumEntries => Entries.Count;

        [Category("CE Event")] public int EntrySize => Entries.Count * 0x4 + 0x4;

        [Category("CE Event")]
        public float[] Parameters
        {
            get
            {
                param.Clear();
                foreach (var i in Entries)
                    if (i >= 0x2000)
                        param.Add(((CEEntryNode) Parent).GetPart2(i) ?? 0);
                return param.ToArray();
            }
            set
            {
                param.Clear();
                foreach (var f in value) param.Add(f);

                for (int i = 0x1ffE + ((CEEntryNode) Parent).Part2Entries.Count, j = 0, k = 0; k < param.Count; j++)
                {
                    Action function = () => //sorry for bad code...
                    {
                        if (i >= 0x2000)
                        {
                            if (j < Entries.Count)
                            {
                                Entries[j] = ((CEEntryNode) Parent).SetPart2(param[k], i);
                                k++;
                            }
                            else
                            {
                                Entries.Add(((CEEntryNode) Parent).SetPart2(param[k], i));
                                k++;
                            }
                        }
                    };

                    if (j < Entries.Count)
                    {
                        if (Entries[j] >= 0x2000)
                        {
                            i = Entries[j];
                            function();
                        }
                    }
                    else
                    {
                        i++;
                        function();
                    }
                }

                SignalPropertyChange();
            }
        }


        public override bool OnInitialize()
        {
            if (_name == null || _name == "") _name = "Event " + ((int) Header->_type).ToString("X");

            for (var i = 0; i < Header->_numEntries; i++) Entries.Add(Header->Entries[i]);

            type = Header->_type;
            return false;
        }


        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            var e = (CEEvent*) address;
            e->_entrySize = (short) EntrySize;
            e->_numEntries = (sbyte) Entries.Count;
            e->_type = type;
            for (var i = 0; i < Entries.Count; i++) e->Entries[i] = Entries[i];
        }

        public override int OnCalculateSize(bool force)
        {
            return EntrySize;
        }
    }

    public unsafe class CEStringNode : ResourceNode
    {
        // internal int[,] entries;
        internal int[][] entries;
        internal string[] strings;
        internal int unk1, unk2, unk3;
        internal CEString* Header => (CEString*) WorkingUncompressed.Address;

        public override ResourceType ResourceFileType => ResourceType.CEString;

        [Category("AI StringNode Entry")]
        [Description("Each entries are related to Strings")]
        public int[][] Entries
        {
            get => entries;
            set
            {
                entries = value;
                SignalPropertyChange();
            }
        }

        [Category("AI StringNode Entry")]
        [Description("Unknown strings")]
        public string[] Strings
        {
            get => strings;
            set
            {
                strings = value;
                SignalPropertyChange();
            }
        }

        public override bool OnInitialize()
        {
            if (_name == null || _name == "") _name = "StringEntry" + Parent.Children.IndexOf(this);

            entries = new int[Header->_numEntries][];
            for (var i = 0; i < entries.Length; i++) entries[i] = new int[2];

            for (int i = 0, j = 0; j < Header->_numEntries; i += 0x2, j++)
            {
                entries[j][0] = Header->Entries[i];
                entries[j][1] = Header->Entries[i + 1];
            }

            strings = GetStrings();
            unk1 = Header->_unk1;
            unk2 = Header->_unk2;
            unk3 = Header->_unk3;
            return false;
        }

        internal string[] GetStrings()
        {
            var s = new List<byte>();
            var returnStrings = new string[Header->_numEntries];
            var word = (byte*) &Header->Entries[Header->_numEntries * 2];
            for (var i = 0; i < Header->_numEntries; i++)
            {
                for (; *word != 0x0; word++) s.Add(*word);
                var sjisEnc = Encoding.GetEncoding("Shift_JIS");
                returnStrings[i] = sjisEnc.GetString(s.ToArray());
                s.Clear();
                word++;
            }

            return returnStrings;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            var strings = (CEString*) address;
            strings->_numEntries = Strings.Length;
            strings->_unk1 = unk1;
            strings->_unk2 = unk2;
            strings->_unk3 = unk3;
            for (int i = 0, j = 0; j < Entries.Length; i += 0x2, j++)
            {
                strings->Entries[i] = Entries[j][0];
                strings->Entries[i + 1] = Entries[j][1];
            }

            var word = (byte*) &strings->Entries[strings->_numEntries * 2];
            var ByteStrLine = new List<byte[]>();
            var sjisEnc = Encoding.GetEncoding("Shift_JIS");
            foreach (var s in Strings) ByteStrLine.Add(sjisEnc.GetBytes(s));

            foreach (var bline in ByteStrLine)
            {
                foreach (var b in bline)
                {
                    *word = b;
                    word++;
                }

                *word = 0x0;
                word++;
            }
        }

        public override int OnCalculateSize(bool force)
        {
            var size = 0x10;
            foreach (var array in Entries)
            foreach (var entry in array)
                size += 0x4;

            foreach (var s in Strings) size += s.Length; // + 0x1;

            if (Strings.Length > 1) size += Strings.Length - 1;

            if (size % 0x10 != 0)
                return size + 0x10 - size % 0x10;
            return size;
        }

        public override void Export(string outPath)
        {
            var dataLen = (uint) OnCalculateSize(true);
            using (var stream = new FileStream(outPath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None, 8,
                FileOptions.RandomAccess))
            {
                stream.SetLength(dataLen);
                using (var map = FileMap.FromStream(stream))
                {
                    Rebuild(map.Address, (int) dataLen, true);
                }
            }
        }
    }
}