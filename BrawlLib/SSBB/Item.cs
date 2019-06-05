﻿namespace BrawlLib.SSBB
{
    public class Item
    {
        public static readonly Item[] Items =
        {
            //        ID    Display Name     
            new Item(0x00, "Assist Trophy"),
            new Item(0x01, "Franklin Badge"),
            new Item(0x02, "Banana Peel"),
            new Item(0x03, "Barrel"),
            new Item(0x04, "Beam Sword"),
            new Item(0x05, "Bill [Coin mode]"),
            new Item(0x06, "Bob-Omb"),
            new Item(0x07, "Crate"),
            new Item(0x08, "Bumper"),
            new Item(0x09, "Capsule"),
            new Item(0x0A, "Rolling Crate"),
            new Item(0x0B, "CD"),
            new Item(0x0C, "Gooey Bomb"),
            new Item(0x0D, "Cracker Launcher"),
            new Item(0x0E, "Cracker Launcher Shot"),
            new Item(0x0F, "Coin"),
            new Item(0x10, "Superspicy Curry"),
            new Item(0x11, "Superspice Curry Shot"),
            new Item(0x12, "Deku Nut"),
            new Item(0x13, "Mr. Saturn"),
            new Item(0x14, "Dragoon Part"),
            new Item(0x15, "Dragoon Set"),
            new Item(0x16, "Dragoon Sight"),
            new Item(0x17, "Trophy"),
            new Item(0x18, "Fire Flower"),
            new Item(0x19, "Fire Flower Shot"),
            new Item(0x1A, "Freezie"),
            new Item(0x1B, "Golden Hammer"),
            new Item(0x1C, "Green Shell"),
            new Item(0x1D, "Hammer"),
            new Item(0x1E, "Hammer Head"),
            new Item(0x1F, "Fan"),
            new Item(0x20, "Heart Container"),
            new Item(0x21, "Homerun Bat"),
            new Item(0x22, "Party Ball"),
            new Item(0x23, "Manaphy Heart"),
            new Item(0x24, "Maxim Tomato"),
            new Item(0x25, "Poison Mushroom"),
            new Item(0x26, "Super Mushroom"),
            new Item(0x27, "Metal Box"),
            new Item(0x28, "Hothead"),
            new Item(0x29, "Pitfall"),
            new Item(0x2A, "Pokéball"),
            new Item(0x2B, "Blast Box"),
            new Item(0x2C, "Ray Gun"),
            new Item(0x2D, "Ray Gun Shot"),
            new Item(0x2E, "Lipstick"),
            new Item(0x2F, "Lipstick Flower"),
            new Item(0x30, "Lipstick Shot [Dust/Powder]"),
            new Item(0x31, "Sandbag"),
            new Item(0x32, "Screw Attack"),
            new Item(0x33, "Sticker"),
            new Item(0x34, "Motion-Sensor Bomb"),
            new Item(0x35, "Timer"),
            new Item(0x36, "Smart Bomb"),
            new Item(0x37, "Smash Ball"),
            new Item(0x38, "Smoke Screen"),
            new Item(0x39, "Spring"),
            new Item(0x3A, "Star Rod"),
            new Item(0x3B, "Star Rod Shot"),
            new Item(0x3C, "Soccer Ball"),
            new Item(0x3D, "Super Scope"),
            new Item(0x3E, "Super Scope shot"),
            new Item(0x3F, "Star"),
            new Item(0x40, "Food"),
            new Item(0x41, "Team Healer"),
            new Item(0x42, "Lightning"),
            new Item(0x43, "Unira"),
            new Item(0x44, "Bunny Hood"),
            new Item(0x45, "Warpstar"),
            new Item(0x46, "Trophy [SSE]"),
            new Item(0x47, "Key"),
            new Item(0x48, "Trophy Stand"),
            new Item(0x49, "Stock Ball"),
            new Item(0x4A, "Apple [Green Greens]"),
            new Item(0x4B, "Sidestepper"),
            new Item(0x4C, "Shellcreeper"),
            new Item(0x4D, "Pellet"),
            new Item(0x4E, "Vegetable [Summit]"),
            new Item(0x4F, "Sandbag [HRC]"),
            new Item(0x50, "Auroros"),
            new Item(0x51, "Koopa"),
            new Item(0x52, "Koopa"),
            new Item(0x53, "Snake's Box"),
            new Item(0x54, "Diddy's Peanut"),
            new Item(0x55, "Link's Bomb"),
            new Item(0x56, "Peach's Turnup"),
            new Item(0x57, "R.O.B.'s Gyro"),
            new Item(0x58, "Seed [edible peanut]"),
            new Item(0x59, "Snake's Grenade"),
            new Item(0x5A, "Samus' Armor piece"),
            new Item(0x5B, "Toon Link's Bomb"),
            new Item(0x5C, "Wario's Bike"),
            new Item(0x5D, "Wario's Bike A"),
            new Item(0x5E, "Wario's Bike B"),
            new Item(0x5F, "Wario's Bike C"),
            new Item(0x60, "Wario's Bike D"),
            new Item(0x61, "Wario's Bike E"),
            new Item(0x62, "Torchic"),
            new Item(0x63, "Cerebi"),
            new Item(0x64, "Chickorita"),
            new Item(0x65, "Chickorita's Shot"),
            new Item(0x66, "Entei"),
            new Item(0x67, "Moltres"),
            new Item(0x68, "Munchlax"),
            new Item(0x69, "Deoxys"),
            new Item(0x6A, "Groudon"),
            new Item(0x6B, "Gulpin"),
            new Item(0x6C, "Staryu"),
            new Item(0x6D, "Staryu's Shot"),
            new Item(0x6E, "Ho-oh"),
            new Item(0x6F, "Ho-oh's Shot"),
            new Item(0x70, "Jirachi"),
            new Item(0x71, "Snorlax"),
            new Item(0x72, "Bellossom"),
            new Item(0x73, "Kyogre"),
            new Item(0x74, "Kyogre's Shot"),
            new Item(0x75, "Latias/Latios"),
            new Item(0x76, "Lugia"),
            new Item(0x77, "Lugia's Shot"),
            new Item(0x78, "Manaphy"),
            new Item(0x79, "Weavile"),
            new Item(0x7A, "Electrode"),
            new Item(0x7B, "Metagross"),
            new Item(0x7C, "Mew"),
            new Item(0x7D, "Meowth"),
            new Item(0x7E, "Meowth's Shot"),
            new Item(0x7F, "Piplup"),
            new Item(0x80, "Togepi"),
            new Item(0x81, "Goldeen"),
            new Item(0x82, "Gardevoir"),
            new Item(0x83, "Wobuffet"),
            new Item(0x84, "Suicune"),
            new Item(0x85, "Bonsly"),
            new Item(0x86, "Andross"),
            new Item(0x87, "Andross Shot"),
            new Item(0x88, "Barbara"),
            new Item(0x89, "Gray Fox"),
            new Item(0x8A, "Ray MKII [Custom Robo]"),
            new Item(0x8B, "Ray MKII Bomb"),
            new Item(0x8C, "Ray MKII Gun Shot"),
            new Item(0x8D, "Samurai Goroh"),
            new Item(0x8E, "Devil"),
            new Item(0x8F, "Excitebike"),
            new Item(0x90, "Jeff Andonuts"),
            new Item(0x91, "Jeff Pencil Bullet"),
            new Item(0x92, "Jeff Pencil Rocket"),
            new Item(0x93, "Lakitu"),
            new Item(0x94, "Knuckle Joe"),
            new Item(0x95, "Knuckle Joe Shot"),
            new Item(0x96, "Hammer Bro."),
            new Item(0x97, "Hammer Bro. Hammer"),
            new Item(0x98, "Helirin"),
            new Item(0x99, "Kat & Ana"),
            new Item(0x9A, "Ana"),
            new Item(0x9B, "Jill & Drill Dozer"),
            new Item(0x9C, "Lyn"),
            new Item(0x9D, "Little Mac"),
            new Item(0x9E, "Metroid"),
            new Item(0x9F, "Nintendog"),
            new Item(0xA0, "NintendogFull"),
            new Item(0xA1, "Mr. Resetti"),
            new Item(0xA2, "Isaac"),
            new Item(0xA3, "Isaac Shot"),
            new Item(0xA4, "Saki Amamiya"),
            new Item(0xA5, "Saki Shot 1"),
            new Item(0xA6, "Saki Shot 2"),
            new Item(0xA7, "Shadow the Hedgehog"),
            new Item(0xA8, "Infantry"),
            new Item(0xA9, "Infantry Shot"),
            new Item(0xAA, "Stafy"),
            new Item(0xAB, "Tank [+Infantry]"),
            new Item(0xAC, "Tank Shot"),
            new Item(0xAD, "Tingle"),
            new Item(0xAE, "togezo [?]"),
            new Item(0xAF, "Waluigi"),
            new Item(0xB0, "Dr. Wright"),
            new Item(0xB1, "Wright Buildings"),
            new Item(0x7D1, "Unknown"),
            new Item(0x7D2, "Unknown"),
            new Item(0x7D3, "Unknown"),
            new Item(0x7D4, "Unknown"),
            new Item(0x7D5, "Unknown")
        };

        public Item(int id, string name)
        {
            ID = id;
            Name = name;
        }

        /// <summary>
        ///     The Item ID, as used by the module files, PSA script, and the item frequency tables.
        /// </summary>
        public int ID { get; }

        /// <summary>
        ///     The Item name (e.g. "Heart Container").
        /// </summary>
        public string Name { get; }

        public override string ToString()
        {
            return Name;
        }
    }
}