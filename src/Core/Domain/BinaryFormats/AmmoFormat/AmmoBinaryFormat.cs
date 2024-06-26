
//----------------------
// <auto-generated>
//     This is a generated file! Please edit source .ksy file and use kaitai-struct-compiler to rebuild
// </auto-generated>
//----------------------
using Kaitai;
using System.Collections.Generic;

namespace BoostStudio.Formats
{
    public partial class AmmoBinaryFormat : KaitaiStruct
    {
        public static AmmoBinaryFormat FromFile(string fileName)
        {
            return new AmmoBinaryFormat(new KaitaiStream(fileName));
        }

        public AmmoBinaryFormat(KaitaiStream p__io, KaitaiStruct p__parent = null, AmmoBinaryFormat p__root = null, bool write = false) : base(p__io)
        {
            m_parent = p__parent;
            m_root = p__root ?? this;
            f_ammo = write;
            if (!write)
                _read();
        }
        private void _read()
        {
            _fileMagic = m_io.ReadBytes(4);
            if (!((KaitaiStream.ByteArrayCompare(FileMagic, new byte[] { 29, 37, 143, 247 }) == 0)))
            {
                throw new ValidationNotEqualError(new byte[] { 29, 37, 143, 247 }, FileMagic, M_Io, "/seq/0");
            }
            _propertyCount = m_io.ReadS4be();
            _unk8 = m_io.ReadU4be();
            _unk12 = m_io.ReadU4be();
            _ammoCount = m_io.ReadS4be();
        }
        public partial class AmmoBody : KaitaiStruct
        {
            public AmmoBody(int p_index, int p_propertyCount, int p_ammoCount, KaitaiStream p__io, AmmoBinaryFormat p__parent = null, AmmoBinaryFormat p__root = null, bool write = false) : base(p__io)
            {
                m_parent = p__parent;
                m_root = p__root;
                _index = p_index;
                _propertyCount = p_propertyCount;
                _ammoCount = p_ammoCount;
                f_offset = write;
                f_ammoProperties = write;
                if (!write)
                    _read();
            }
            private void _read()
            {
                _hash = m_io.ReadU4be();
            }
            private bool f_offset;
            private int _offset;
            public int Offset
            {
                get
                {
                    if (f_offset)
                        return _offset;
                    _offset = (int)((((AmmoCount * 4) + ((Index * (PropertyCount - 1)) * 4)) + 20));
                    f_offset = true;
                    return _offset;
                }

                set
                {
                    _offset = value;
                }
            }
            private bool f_ammoProperties;
            private AmmoPropertiesBody _ammoProperties;
            public AmmoPropertiesBody AmmoProperties
            {
                get
                {
                    if (f_ammoProperties)
                        return _ammoProperties;
                    long _pos = m_io.Pos;
                    m_io.Seek(Offset);
                    _ammoProperties = new AmmoPropertiesBody(m_io, this, m_root);
                    m_io.Seek(_pos);
                    f_ammoProperties = true;
                    return _ammoProperties;
                }

                set
                {
                    _ammoProperties = value;
                }
            }
            private uint _hash;
            private int _index;
            private int _propertyCount;
            private int _ammoCount;
            private AmmoBinaryFormat m_root;
            private AmmoBinaryFormat m_parent;
            public uint Hash
            {
                get { return _hash; }

                set
                {
                    _hash = value;
                }
            }
            public int Index
            {
                get { return _index; }

                set
                {
                    _index = value;
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
            public int AmmoCount
            {
                get { return _ammoCount; }

                set
                {
                    _ammoCount = value;
                }
            }
            public AmmoBinaryFormat M_Root
            {
                get { return m_root; }

                set
                {
                    m_root = value;
                }
            }
            public AmmoBinaryFormat M_Parent
            {
                get { return m_parent; }

                set
                {
                    m_parent = value;
                }
            }
        }
        public partial class AmmoPropertiesBody : KaitaiStruct
        {
            public static AmmoPropertiesBody FromFile(string fileName)
            {
                return new AmmoPropertiesBody(new KaitaiStream(fileName));
            }

            public AmmoPropertiesBody(KaitaiStream p__io, AmmoBinaryFormat.AmmoBody p__parent = null, AmmoBinaryFormat p__root = null, bool write = false) : base(p__io)
            {
                m_parent = p__parent;
                m_root = p__root;
                if (!write)
                    _read();
            }
            private void _read()
            {
                _ammoType = m_io.ReadU4be();
                _maxAmmo = m_io.ReadU4be();
                _initialAmmo = m_io.ReadU4be();
                _timedDurationFrame = m_io.ReadU4be();
                _unk16 = m_io.ReadU4be();
                _reloadType = m_io.ReadU4be();
                _cooldownDurationFrame = m_io.ReadU4be();
                _reloadDurationFrame = m_io.ReadU4be();
                _assaultBurstReloadDurationFrame = m_io.ReadU4be();
                _blastBurstReloadDurationFrame = m_io.ReadU4be();
                _unk40 = m_io.ReadU4be();
                _unk44 = m_io.ReadU4be();
                _inactiveUnk48 = m_io.ReadU4be();
                _inactiveCooldownDurationFrame = m_io.ReadU4be();
                _inactiveReloadDurationFrame = m_io.ReadU4be();
                _inactiveAssaultBurstReloadDurationFrame = m_io.ReadU4be();
                _inactiveBlastBurstReloadDurationFrame = m_io.ReadU4be();
                _inactiveUnk68 = m_io.ReadU4be();
                _inactiveUnk72 = m_io.ReadU4be();
                _burstReplenish = m_io.ReadU4be();
                _unk80 = m_io.ReadU4be();
                _unk84 = m_io.ReadU4be();
                _unk88 = m_io.ReadU4be();
                _chargeInput = m_io.ReadU4be();
                _chargeDurationFrame = m_io.ReadU4be();
                _assaultBurstChargeDurationFrame = m_io.ReadU4be();
                _blastBurstChargeDurationFrame = m_io.ReadU4be();
                _unk108 = m_io.ReadU4be();
                _unk112 = m_io.ReadU4be();
                _releaseChargeLingerDurationFrame = m_io.ReadU4be();
                _maxChargeLevel = m_io.ReadU4be();
                _unk124 = m_io.ReadU4be();
                _unk128 = m_io.ReadU4be();
            }
            private uint _ammoType;
            private uint _maxAmmo;
            private uint _initialAmmo;
            private uint _timedDurationFrame;
            private uint _unk16;
            private uint _reloadType;
            private uint _cooldownDurationFrame;
            private uint _reloadDurationFrame;
            private uint _assaultBurstReloadDurationFrame;
            private uint _blastBurstReloadDurationFrame;
            private uint _unk40;
            private uint _unk44;
            private uint _inactiveUnk48;
            private uint _inactiveCooldownDurationFrame;
            private uint _inactiveReloadDurationFrame;
            private uint _inactiveAssaultBurstReloadDurationFrame;
            private uint _inactiveBlastBurstReloadDurationFrame;
            private uint _inactiveUnk68;
            private uint _inactiveUnk72;
            private uint _burstReplenish;
            private uint _unk80;
            private uint _unk84;
            private uint _unk88;
            private uint _chargeInput;
            private uint _chargeDurationFrame;
            private uint _assaultBurstChargeDurationFrame;
            private uint _blastBurstChargeDurationFrame;
            private uint _unk108;
            private uint _unk112;
            private uint _releaseChargeLingerDurationFrame;
            private uint _maxChargeLevel;
            private uint _unk124;
            private uint _unk128;
            private AmmoBinaryFormat m_root;
            private AmmoBinaryFormat.AmmoBody m_parent;
            public uint AmmoType
            {
                get { return _ammoType; }

                set
                {
                    _ammoType = value;
                }
            }
            public uint MaxAmmo
            {
                get { return _maxAmmo; }

                set
                {
                    _maxAmmo = value;
                }
            }
            public uint InitialAmmo
            {
                get { return _initialAmmo; }

                set
                {
                    _initialAmmo = value;
                }
            }
            public uint TimedDurationFrame
            {
                get { return _timedDurationFrame; }

                set
                {
                    _timedDurationFrame = value;
                }
            }
            public uint Unk16
            {
                get { return _unk16; }

                set
                {
                    _unk16 = value;
                }
            }
            public uint ReloadType
            {
                get { return _reloadType; }

                set
                {
                    _reloadType = value;
                }
            }
            public uint CooldownDurationFrame
            {
                get { return _cooldownDurationFrame; }

                set
                {
                    _cooldownDurationFrame = value;
                }
            }
            public uint ReloadDurationFrame
            {
                get { return _reloadDurationFrame; }

                set
                {
                    _reloadDurationFrame = value;
                }
            }
            public uint AssaultBurstReloadDurationFrame
            {
                get { return _assaultBurstReloadDurationFrame; }

                set
                {
                    _assaultBurstReloadDurationFrame = value;
                }
            }
            public uint BlastBurstReloadDurationFrame
            {
                get { return _blastBurstReloadDurationFrame; }

                set
                {
                    _blastBurstReloadDurationFrame = value;
                }
            }
            public uint Unk40
            {
                get { return _unk40; }

                set
                {
                    _unk40 = value;
                }
            }
            public uint Unk44
            {
                get { return _unk44; }

                set
                {
                    _unk44 = value;
                }
            }
            public uint InactiveUnk48
            {
                get { return _inactiveUnk48; }

                set
                {
                    _inactiveUnk48 = value;
                }
            }
            public uint InactiveCooldownDurationFrame
            {
                get { return _inactiveCooldownDurationFrame; }

                set
                {
                    _inactiveCooldownDurationFrame = value;
                }
            }
            public uint InactiveReloadDurationFrame
            {
                get { return _inactiveReloadDurationFrame; }

                set
                {
                    _inactiveReloadDurationFrame = value;
                }
            }
            public uint InactiveAssaultBurstReloadDurationFrame
            {
                get { return _inactiveAssaultBurstReloadDurationFrame; }

                set
                {
                    _inactiveAssaultBurstReloadDurationFrame = value;
                }
            }
            public uint InactiveBlastBurstReloadDurationFrame
            {
                get { return _inactiveBlastBurstReloadDurationFrame; }

                set
                {
                    _inactiveBlastBurstReloadDurationFrame = value;
                }
            }
            public uint InactiveUnk68
            {
                get { return _inactiveUnk68; }

                set
                {
                    _inactiveUnk68 = value;
                }
            }
            public uint InactiveUnk72
            {
                get { return _inactiveUnk72; }

                set
                {
                    _inactiveUnk72 = value;
                }
            }
            public uint BurstReplenish
            {
                get { return _burstReplenish; }

                set
                {
                    _burstReplenish = value;
                }
            }
            public uint Unk80
            {
                get { return _unk80; }

                set
                {
                    _unk80 = value;
                }
            }
            public uint Unk84
            {
                get { return _unk84; }

                set
                {
                    _unk84 = value;
                }
            }
            public uint Unk88
            {
                get { return _unk88; }

                set
                {
                    _unk88 = value;
                }
            }
            public uint ChargeInput
            {
                get { return _chargeInput; }

                set
                {
                    _chargeInput = value;
                }
            }
            public uint ChargeDurationFrame
            {
                get { return _chargeDurationFrame; }

                set
                {
                    _chargeDurationFrame = value;
                }
            }
            public uint AssaultBurstChargeDurationFrame
            {
                get { return _assaultBurstChargeDurationFrame; }

                set
                {
                    _assaultBurstChargeDurationFrame = value;
                }
            }
            public uint BlastBurstChargeDurationFrame
            {
                get { return _blastBurstChargeDurationFrame; }

                set
                {
                    _blastBurstChargeDurationFrame = value;
                }
            }
            public uint Unk108
            {
                get { return _unk108; }

                set
                {
                    _unk108 = value;
                }
            }
            public uint Unk112
            {
                get { return _unk112; }

                set
                {
                    _unk112 = value;
                }
            }
            public uint ReleaseChargeLingerDurationFrame
            {
                get { return _releaseChargeLingerDurationFrame; }

                set
                {
                    _releaseChargeLingerDurationFrame = value;
                }
            }
            public uint MaxChargeLevel
            {
                get { return _maxChargeLevel; }

                set
                {
                    _maxChargeLevel = value;
                }
            }
            public uint Unk124
            {
                get { return _unk124; }

                set
                {
                    _unk124 = value;
                }
            }
            public uint Unk128
            {
                get { return _unk128; }

                set
                {
                    _unk128 = value;
                }
            }
            public AmmoBinaryFormat M_Root
            {
                get { return m_root; }

                set
                {
                    m_root = value;
                }
            }
            public AmmoBinaryFormat.AmmoBody M_Parent
            {
                get { return m_parent; }

                set
                {
                    m_parent = value;
                }
            }
        }
        private bool f_ammo;
        private List<AmmoBody> _ammo;
        public List<AmmoBody> Ammo
        {
            get
            {
                if (f_ammo)
                    return _ammo;
                _ammo = new List<AmmoBody>();
                for (var i = 0; i < AmmoCount; i++)
                {
                    _ammo.Add(new AmmoBody(i, PropertyCount, AmmoCount, m_io, this, m_root));
                }
                f_ammo = true;
                return _ammo;
            }

            set
            {
                _ammo = value;
            }
        }
        private byte[] _fileMagic;
        private int _propertyCount;
        private uint _unk8;
        private uint _unk12;
        private int _ammoCount;
        private AmmoBinaryFormat m_root;
        private KaitaiStruct m_parent;
        public byte[] FileMagic
        {
            get { return _fileMagic; }

            set
            {
                _fileMagic = value;
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
        public uint Unk8
        {
            get { return _unk8; }

            set
            {
                _unk8 = value;
            }
        }
        public uint Unk12
        {
            get { return _unk12; }

            set
            {
                _unk12 = value;
            }
        }
        public int AmmoCount
        {
            get { return _ammoCount; }

            set
            {
                _ammoCount = value;
            }
        }
        public AmmoBinaryFormat M_Root
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
