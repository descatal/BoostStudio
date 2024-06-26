
//----------------------
// <auto-generated>
//     This is a generated file! Please edit source .ksy file and use kaitai-struct-compiler to rebuild
// </auto-generated>
//----------------------
using Kaitai;
using System.Collections.Generic;

namespace BoostStudio.Formats
{
    public partial class StatsBinaryFormat : KaitaiStruct
    {
        public static StatsBinaryFormat FromFile(string fileName)
        {
            return new StatsBinaryFormat(new KaitaiStream(fileName));
        }


        public enum PropertyTypeEnum
        {
            Float = 0,
            Integer = 3,
            Unknown6 = 6,
        }

        public enum PropertiesEnum : uint
        {
            AssaultBurstDownValueDealtMultiplier = 26730313,
            BoostInitialConsumption = 35703725,
            Unk324 = 91189775,
            ThirdBurstDamageDealtBurstGaugeIncreaseMultiplier = 112188836,
            Unk384 = 167388454,
            FourthBurstDamageDealtMultiplier = 283495003,
            AssaultBurstRedLock = 284157589,
            FourthBurstDamageTakenMultiplier = 356108876,
            Unk292 = 366248313,
            Unk496 = 397765295,
            Unk196 = 406321436,
            Unk328 = 477381454,
            MaxHp = 477439873,
            ThirdBurstDamageTakenBurstGaugeIncreaseMultiplier = 481663324,
            Unk256 = 493747660,
            Unk116 = 520039182,
            ThirdBurstRedLock = 584680471,
            SizeMultiplier = 633880592,
            Unk512 = 665513126,
            UnitCost2 = 678270836,
            Unk268 = 692127964,
            Unk28 = 738048464,
            Unk300 = 749862671,
            Unk320 = 776085964,
            Unk184 = 791262326,
            ThirdBurstDownValueDealtMultiplier = 801576649,
            Unk448 = 813860966,
            BoostFuwaInitialConsumption = 828831337,
            BoostFlyConsumption = 895807738,
            FourthBurstBoostConsumptionMultiplier = 925119923,
            Unk316 = 928583821,
            BlastBurstDownValueDealtMultiplier = 955216393,
            Unk24 = 995969827,
            BlastBurstRedLock = 1002616150,
            BoostTransformConsumption = 1014622134,
            Unk460 = 1031066006,
            BoostPostActionConsumption = 1041821406,
            Unk168 = 1088266336,
            BoostBdConsumption = 1181984799,
            Unk264 = 1223460413,
            Unk304 = 1244587208,
            Unk576 = 1258717641,
            BoostGroundStepInitialConsumption = 1289953086,
            Unk128 = 1301630992,
            Unk72 = 1319393177,
            RedLockRange = 1383035672,
            Unk124 = 1418617169,
            Unk524 = 1477700304,
            Unk164 = 1506185505,
            Unk592 = 1585873014,
            Unk308 = 1627633419,
            Unk120 = 1674073936,
            BoostTransformInitialConsumption = 1685408730,
            Unk132 = 1723335635,
            YorukeValueThreshold = 1774716527,
            Unk388 = 1796246959,
            Unk56 = 1799814114,
            ThirdBurstBoostConsumptionMultiplier = 1805115093,
            FourthBurstDamageTakenBurstGaugeIncreaseMultiplier = 1837290380,
            Unk428 = 1917233204,
            Unk160 = 1928053474,
            FourthBurstMobilityMultiplier = 2005345955,
            Unk312 = 2014873162,
            Unk596 = 2050602675,
            Unk76 = 2146557220,
            Unk364 = 2148615628,
            Unk108 = 2155592119,
            BoostAirStepConsumption = 2193836675,
            AssaultBurstRedLockMelee = 2208341459,
            Unk604 = 2213848896,
            AssaultBurstDamageTakenBurstGaugeIncreaseMultiplier = 2243401409,
            BoostAirStepInitialConsumption = 2251143049,
            AssaultBurstDamageTakenMultiplier = 2251300981,
            BlastBurstDamageDealtMultiplier = 2259621275,
            Unk600 = 2264703969,
            Unk144 = 2286333178,
            Unk156 = 2340516805,
            Unk96 = 2368293133,
            Unk572 = 2435098049,
            Unk140 = 2438831547,
            ThirdBurstDamageDealtMultiplier = 2441572699,
            Unk152 = 2459583108,
            Unk396 = 2463997020,
            BlastBurstDamageDealtBurstGaugeIncreaseMultiplier = 2480867633,
            Unk492 = 2505772707,
            Unk432 = 2526023560,
            FourthBurstRedLockMelee = 2557651057,
            Unk332 = 2615959425,
            ThirdBurstMobilityMultiplier = 2622425738,
            BlastBurstMobilityMultiplier = 2687494530,
            Unk516 = 2715745059,
            BoostRainbowStepInitialConsumption = 2756301960,
            Unk216 = 2772128046,
            Unk588 = 2772497681,
            Unk44 = 2779597778,
            UnitCost = 2785325993,
            Unk204 = 2824392278,
            BlastBurstRedLockMelee = 2827865616,
            Unk104 = 2874600052,
            GravityMultiplierAir = 2909963034,
            Unk36 = 2926888403,
            FourthBurstDownValueDealtMultiplier = 2930093513,
            FourthBurstDamageDealtBurstGaugeIncreaseMultiplier = 2932498545,
            Unk296 = 2933526153,
            ThirdBurstRedLockMelee = 2979462993,
            Unk100 = 2991446837,
            AssaultBurstBoostConsumptionMultiplier = 2997621466,
            BoostBdInitialConsumption = 3055470982,
            Unk148 = 3115782471,
            Unk136 = 3127955064,
            Unk68 = 3145015401,
            Unk64 = 3160943787,
            BlastBurstDamageTakenBurstGaugeIncreaseMultiplier = 3180076466,
            AssaultBurstDamageDealtMultiplier = 3218585819,
            Unk200 = 3239797796,
            BlastBurstDamageTakenMultiplier = 3264284013,
            Unk40 = 3276144880,
            Unk92 = 3277063457,
            Unk452 = 3300507749,
            Unk180 = 3326726862,
            Unk20 = 3328576160,
            MaxBoost = 3335446209,
            Unk608 = 3343888302,
            Unk88 = 3355680210,
            Unk444 = 3410296065,
            Unk260 = 3433087472,
            Unk368 = 3465455264,
            Unk392 = 3514828465,
            DownValueThreshold = 3563541733,
            BoostNonVernierActionConsumption = 3610736632,
            CameraZoomMultiplier = 3624583084,
            RedLockRangeMelee = 3645845992,
            GravityMultiplierLand = 3686727461,
            Unk508 = 3698104769,
            Unk176 = 3746751375,
            AssaultBurstMobilityMultiplier = 3834563738,
            Unk80 = 3878263424,
            Unk456 = 3908611057,
            BlastBurstBoostConsumptionMultiplier = 3937582066,
            Unk60 = 4015895192,
            BoostReplenish = 4063051613,
            Unk380 = 4064038977,
            Unk172 = 4102008908,
            AssaultBurstDamageDealtBurstGaugeIncreaseMultiplier = 4148130511,
            FourthBurstRedLock = 4186340105,
            BoostGroundStepConsumption = 4274080117,
            ThirdBurstDamageTakenMultiplier = 4277266021,
            Unk208 = 4283342885,
            Unk520 = 4291874609,
        }
        public StatsBinaryFormat(KaitaiStream p__io, KaitaiStruct p__parent = null, StatsBinaryFormat p__root = null, bool write = false) : base(p__io)
        {
            m_parent = p__parent;
            m_root = p__root ?? this;
            f_ammoHashes = write;
            f_propertyCount = write;
            f_setCount = write;
            f_sets = write;
            if (!write)
                _read();
        }
        private void _read()
        {
            _magic = m_io.ReadU4be();
            _unitId = m_io.ReadU4be();
            _unk8 = m_io.ReadBytes(4);
            if (!((KaitaiStream.ByteArrayCompare(Unk8, new byte[] { 29, 119, 38, 188 }) == 0)))
            {
                throw new ValidationNotEqualError(new byte[] { 29, 119, 38, 188 }, Unk8, M_Io, "/seq/2");
            }
            _ammoSlot5 = new AmmoInfoBody(48, m_io, this, m_root);
            _ammoHashChunkPointer = m_io.ReadS4be();
            _propertyMetadataChunkPointer = m_io.ReadS4be();
            _setInfoChunkPointer = m_io.ReadS4be();
            _valueChunkPointer = m_io.ReadS4be();
            _ammoSlot1 = new AmmoInfoBody(AmmoHashChunkPointer, m_io, this, m_root);
            _ammoSlot2 = new AmmoInfoBody(AmmoHashChunkPointer, m_io, this, m_root);
            _ammoSlot3 = new AmmoInfoBody(AmmoHashChunkPointer, m_io, this, m_root);
            _ammoSlot4 = new AmmoInfoBody(AmmoHashChunkPointer, m_io, this, m_root);
        }
        public partial class AmmoInfoBody : KaitaiStruct
        {
            public AmmoInfoBody(int p_ammoHashChunkPointer, KaitaiStream p__io, StatsBinaryFormat p__parent = null, StatsBinaryFormat p__root = null, bool write = false) : base(p__io)
            {
                m_parent = p__parent;
                m_root = p__root;
                _ammoHashChunkPointer = p_ammoHashChunkPointer;
                f_ammoHash = write;
                if (!write)
                    _read();
            }
            private void _read()
            {
                _slotIndex = m_io.ReadS4be();
            }
            private bool f_ammoHash;
            private uint? _ammoHash;
            public uint? AmmoHash
            {
                get
                {
                    if (f_ammoHash)
                        return _ammoHash;
                    if (SlotIndex != -1)
                    {
                        long _pos = m_io.Pos;
                        m_io.Seek(((AmmoHashChunkPointer + 4) + (SlotIndex * 4)));
                        _ammoHash = m_io.ReadU4be();
                        m_io.Seek(_pos);
                        f_ammoHash = true;
                    }
                    return _ammoHash;
                }

                set
                {
                    _ammoHash = value;
                }
            }
            private int _slotIndex;
            private int _ammoHashChunkPointer;
            private StatsBinaryFormat m_root;
            private StatsBinaryFormat m_parent;
            public int SlotIndex
            {
                get { return _slotIndex; }

                set
                {
                    _slotIndex = value;
                }
            }
            public int AmmoHashChunkPointer
            {
                get { return _ammoHashChunkPointer; }

                set
                {
                    _ammoHashChunkPointer = value;
                }
            }
            public StatsBinaryFormat M_Root
            {
                get { return m_root; }

                set
                {
                    m_root = value;
                }
            }
            public StatsBinaryFormat M_Parent
            {
                get { return m_parent; }

                set
                {
                    m_parent = value;
                }
            }
        }
        public partial class AmmoHashesBody : KaitaiStruct
        {
            public static AmmoHashesBody FromFile(string fileName)
            {
                return new AmmoHashesBody(new KaitaiStream(fileName));
            }

            public AmmoHashesBody(KaitaiStream p__io, StatsBinaryFormat p__parent = null, StatsBinaryFormat p__root = null, bool write = false) : base(p__io)
            {
                m_parent = p__parent;
                m_root = p__root;
                if (!write)
                    _read();
            }
            private void _read()
            {
                _count = m_io.ReadU4be();
                _hashes = new List<uint>();
                for (var i = 0; i < Count; i++)
                {
                    _hashes.Add(m_io.ReadU4be());
                }
            }
            private uint _count;
            private List<uint> _hashes;
            private StatsBinaryFormat m_root;
            private StatsBinaryFormat m_parent;
            public uint Count
            {
                get { return _count; }

                set
                {
                    _count = value;
                }
            }
            public List<uint> Hashes
            {
                get { return _hashes; }

                set
                {
                    _hashes = value;
                }
            }
            public StatsBinaryFormat M_Root
            {
                get { return m_root; }

                set
                {
                    m_root = value;
                }
            }
            public StatsBinaryFormat M_Parent
            {
                get { return m_parent; }

                set
                {
                    m_parent = value;
                }
            }
        }
        public partial class SetBody : KaitaiStruct
        {
            public SetBody(int p_setIndex, int p_propertyCount, int p_propertyMetadataChunkPointer, int p_valueChunkPointer, KaitaiStream p__io, StatsBinaryFormat p__parent = null, StatsBinaryFormat p__root = null, bool write = false) : base(p__io)
            {
                m_parent = p__parent;
                m_root = p__root;
                _setIndex = p_setIndex;
                _propertyCount = p_propertyCount;
                _propertyMetadataChunkPointer = p_propertyMetadataChunkPointer;
                _valueChunkPointer = p_valueChunkPointer;
                if (!write)
                    _read();
            }
            private void _read()
            {
                _stats = new List<StatsBody>();
                for (var i = 0; i < PropertyCount; i++)
                {
                    _stats.Add(new StatsBody(i, SetIndex, PropertyCount, PropertyMetadataChunkPointer, ValueChunkPointer, m_io, this, m_root));
                }
            }
            private List<StatsBody> _stats;
            private int _setIndex;
            private int _propertyCount;
            private int _propertyMetadataChunkPointer;
            private int _valueChunkPointer;
            private StatsBinaryFormat m_root;
            private StatsBinaryFormat m_parent;
            public List<StatsBody> Stats
            {
                get { return _stats; }

                set
                {
                    _stats = value;
                }
            }
            public int SetIndex
            {
                get { return _setIndex; }

                set
                {
                    _setIndex = value;
                }
            }
            public int PropertyCount
            {
                get { return _propertyCount; }

                set
                {
                    _propertyCount = value;
                }
            }
            public int PropertyMetadataChunkPointer
            {
                get { return _propertyMetadataChunkPointer; }

                set
                {
                    _propertyMetadataChunkPointer = value;
                }
            }
            public int ValueChunkPointer
            {
                get { return _valueChunkPointer; }

                set
                {
                    _valueChunkPointer = value;
                }
            }
            public StatsBinaryFormat M_Root
            {
                get { return m_root; }

                set
                {
                    m_root = value;
                }
            }
            public StatsBinaryFormat M_Parent
            {
                get { return m_parent; }

                set
                {
                    m_parent = value;
                }
            }
        }
        public partial class StatsBody : KaitaiStruct
        {
            public StatsBody(int p_index, int p_setIndex, int p_propertyCount, int p_propertyMetadataChunkPointer, int p_valueChunkPointer, KaitaiStream p__io, StatsBinaryFormat.SetBody p__parent = null, StatsBinaryFormat p__root = null, bool write = false) : base(p__io)
            {
                m_parent = p__parent;
                m_root = p__root;
                _index = p_index;
                _setIndex = p_setIndex;
                _propertyCount = p_propertyCount;
                _propertyMetadataChunkPointer = p_propertyMetadataChunkPointer;
                _valueChunkPointer = p_valueChunkPointer;
                f_propertyType = write;
                f_propertyName = write;
                f_propertyValue = write;
                if (!write)
                    _read();
            }
            private void _read()
            {
            }
            private bool f_propertyType;
            private PropertyTypeEnum _propertyType;
            public PropertyTypeEnum PropertyType
            {
                get
                {
                    if (f_propertyType)
                        return _propertyType;
                    long _pos = m_io.Pos;
                    m_io.Seek((((PropertyMetadataChunkPointer + 4) + (PropertyCount * 4)) + (Index * 4)));
                    _propertyType = ((StatsBinaryFormat.PropertyTypeEnum)m_io.ReadS4be());
                    m_io.Seek(_pos);
                    f_propertyType = true;
                    return _propertyType;
                }

                set
                {
                    _propertyType = value;
                }
            }
            private bool f_propertyName;
            private PropertiesEnum _propertyName;
            public PropertiesEnum PropertyName
            {
                get
                {
                    if (f_propertyName)
                        return _propertyName;
                    long _pos = m_io.Pos;
                    m_io.Seek(((PropertyMetadataChunkPointer + 4) + (Index * 4)));
                    _propertyName = ((StatsBinaryFormat.PropertiesEnum)m_io.ReadU4be());
                    m_io.Seek(_pos);
                    f_propertyName = true;
                    return _propertyName;
                }

                set
                {
                    _propertyName = value;
                }
            }
            private bool f_propertyValue;
            private double _propertyValue;
            public double PropertyValue
            {
                get
                {
                    if (f_propertyValue)
                        return _propertyValue;
                    long _pos = m_io.Pos;
                    m_io.Seek(((ValueChunkPointer + ((SetIndex * PropertyCount) * 4)) + (Index * 4)));
                    switch (PropertyType)
                    {
                        case StatsBinaryFormat.PropertyTypeEnum.Float:
                            {
                                _propertyValue = m_io.ReadF4be();
                                break;
                            }
                        case StatsBinaryFormat.PropertyTypeEnum.Integer:
                            {
                                _propertyValue = m_io.ReadS4be();
                                break;
                            }
                        default:
                            {
                                _propertyValue = m_io.ReadS4be();
                                break;
                            }
                    }
                    m_io.Seek(_pos);
                    f_propertyValue = true;
                    return _propertyValue;
                }

                set
                {
                    _propertyValue = value;
                }
            }
            private int _index;
            private int _setIndex;
            private int _propertyCount;
            private int _propertyMetadataChunkPointer;
            private int _valueChunkPointer;
            private StatsBinaryFormat m_root;
            private StatsBinaryFormat.SetBody m_parent;
            public int Index
            {
                get { return _index; }

                set
                {
                    _index = value;
                }
            }
            public int SetIndex
            {
                get { return _setIndex; }

                set
                {
                    _setIndex = value;
                }
            }
            public int PropertyCount
            {
                get { return _propertyCount; }

                set
                {
                    _propertyCount = value;
                }
            }
            public int PropertyMetadataChunkPointer
            {
                get { return _propertyMetadataChunkPointer; }

                set
                {
                    _propertyMetadataChunkPointer = value;
                }
            }
            public int ValueChunkPointer
            {
                get { return _valueChunkPointer; }

                set
                {
                    _valueChunkPointer = value;
                }
            }
            public StatsBinaryFormat M_Root
            {
                get { return m_root; }

                set
                {
                    m_root = value;
                }
            }
            public StatsBinaryFormat.SetBody M_Parent
            {
                get { return m_parent; }

                set
                {
                    m_parent = value;
                }
            }
        }
        private bool f_ammoHashes;
        private AmmoHashesBody _ammoHashes;

        /// <summary>
        /// All ammo hashes that's loaded by this unit
        /// </summary>
        public AmmoHashesBody AmmoHashes
        {
            get
            {
                if (f_ammoHashes)
                    return _ammoHashes;
                long _pos = m_io.Pos;
                m_io.Seek(AmmoHashChunkPointer);
                _ammoHashes = new AmmoHashesBody(m_io, this, m_root);
                m_io.Seek(_pos);
                f_ammoHashes = true;
                return _ammoHashes;
            }

            set
            {
                _ammoHashes = value;
            }
        }
        private bool f_propertyCount;
        private int _propertyCount;

        /// <summary>
        /// The number of property on each set
        /// </summary>
        public int PropertyCount
        {
            get
            {
                if (f_propertyCount)
                    return _propertyCount;
                long _pos = m_io.Pos;
                m_io.Seek(PropertyMetadataChunkPointer);
                _propertyCount = m_io.ReadS4be();
                m_io.Seek(_pos);
                f_propertyCount = true;
                return _propertyCount;
            }

            set
            {
                _propertyCount = value;
            }
        }
        private bool f_setCount;
        private int _setCount;

        /// <summary>
        /// The number of sets for this unit
        /// </summary>
        public int SetCount
        {
            get
            {
                if (f_setCount)
                    return _setCount;
                long _pos = m_io.Pos;
                m_io.Seek(SetInfoChunkPointer);
                _setCount = m_io.ReadS4be();
                m_io.Seek(_pos);
                f_setCount = true;
                return _setCount;
            }

            set
            {
                _setCount = value;
            }
        }
        private bool f_sets;
        private List<SetBody> _sets;

        /// <summary>
        /// List of stats set
        /// </summary>
        public List<SetBody> Sets
        {
            get
            {
                if (f_sets)
                    return _sets;
                long _pos = m_io.Pos;
                m_io.Seek(ValueChunkPointer);
                _sets = new List<SetBody>();
                for (var i = 0; i < SetCount; i++)
                {
                    _sets.Add(new SetBody(i, PropertyCount, PropertyMetadataChunkPointer, ValueChunkPointer, m_io, this, m_root));
                }
                m_io.Seek(_pos);
                f_sets = true;
                return _sets;
            }

            set
            {
                _sets = value;
            }
        }
        private uint _magic;
        private uint _unitId;
        private byte[] _unk8;
        private AmmoInfoBody _ammoSlot5;
        private int _ammoHashChunkPointer;
        private int _propertyMetadataChunkPointer;
        private int _setInfoChunkPointer;
        private int _valueChunkPointer;
        private AmmoInfoBody _ammoSlot1;
        private AmmoInfoBody _ammoSlot2;
        private AmmoInfoBody _ammoSlot3;
        private AmmoInfoBody _ammoSlot4;
        private StatsBinaryFormat m_root;
        private KaitaiStruct m_parent;
        public uint Magic
        {
            get { return _magic; }

            set
            {
                _magic = value;
            }
        }
        public uint UnitId
        {
            get { return _unitId; }

            set
            {
                _unitId = value;
            }
        }
        public byte[] Unk8
        {
            get { return _unk8; }

            set
            {
                _unk8 = value;
            }
        }

        /// <summary>
        /// Usually is zero, but repurposed as the initial spawn fifth index slot.
        /// Since the design of ammo_info_body is to be parsed after the ammo_hash_chunk_pointer is read
        /// Make a special case for this and directly put the pointer, which is usually 0x30
        /// </summary>
        public AmmoInfoBody AmmoSlot5
        {
            get { return _ammoSlot5; }

            set
            {
                _ammoSlot5 = value;
            }
        }

        /// <summary>
        /// Pointer to the start of the ammo hash chunk, which contains all of the ammo hashes to be loaded by this unit
        /// </summary>
        public int AmmoHashChunkPointer
        {
            get { return _ammoHashChunkPointer; }

            set
            {
                _ammoHashChunkPointer = value;
            }
        }

        /// <summary>
        /// Pointer to the start of property chunk, which contains the property name hash and the data type
        /// </summary>
        public int PropertyMetadataChunkPointer
        {
            get { return _propertyMetadataChunkPointer; }

            set
            {
                _propertyMetadataChunkPointer = value;
            }
        }

        /// <summary>
        /// Pointer to the start of set info chunk, which indicates the number of sets and the ordering index
        /// </summary>
        public int SetInfoChunkPointer
        {
            get { return _setInfoChunkPointer; }

            set
            {
                _setInfoChunkPointer = value;
            }
        }

        /// <summary>
        /// Pointer to the values chunk, which contains one value per property per set
        /// </summary>
        public int ValueChunkPointer
        {
            get { return _valueChunkPointer; }

            set
            {
                _valueChunkPointer = value;
            }
        }

        /// <summary>
        /// The initial spawn first ammo slot
        /// </summary>
        public AmmoInfoBody AmmoSlot1
        {
            get { return _ammoSlot1; }

            set
            {
                _ammoSlot1 = value;
            }
        }

        /// <summary>
        /// The initial spawn second ammo slot
        /// </summary>
        public AmmoInfoBody AmmoSlot2
        {
            get { return _ammoSlot2; }

            set
            {
                _ammoSlot2 = value;
            }
        }

        /// <summary>
        /// The initial spawn third ammo slot
        /// </summary>
        public AmmoInfoBody AmmoSlot3
        {
            get { return _ammoSlot3; }

            set
            {
                _ammoSlot3 = value;
            }
        }

        /// <summary>
        /// The initial spawn fourth ammo slot
        /// </summary>
        public AmmoInfoBody AmmoSlot4
        {
            get { return _ammoSlot4; }

            set
            {
                _ammoSlot4 = value;
            }
        }
        public StatsBinaryFormat M_Root
        {
            get { return m_root; }

            set
            {
                m_root = value;
            }
        }
        public KaitaiStruct M_Parent
        {
            get { return m_parent; }

            set
            {
                m_parent = value;
            }
        }
    }
}
