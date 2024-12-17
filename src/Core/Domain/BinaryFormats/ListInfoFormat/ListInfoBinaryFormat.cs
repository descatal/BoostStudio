// This is a generated file! Please edit source .ksy file and use kaitai-struct-compiler to rebuild

using Kaitai;
using System.Collections.Generic;

namespace BoostStudio.Formats
{
    public partial class ListInfoBinaryFormat : KaitaiStruct
    {
        public static ListInfoBinaryFormat FromFile(string fileName)
        {
            return new ListInfoBinaryFormat(new KaitaiStream(fileName));
        }

        public ListInfoBinaryFormat(KaitaiStream p__io, KaitaiStruct p__parent = null, ListInfoBinaryFormat p__root = null) : base(p__io)
        {
            m_parent = p__parent;
            m_root = p__root ?? this;
            f_listInfoName = false;
            f_body = false;
            _read();
        }
        private void _read()
        {
            _listInfoNameStringOffset = m_io.ReadU4be();
            _count = m_io.ReadU2be();
            _unk6 = m_io.ReadBytes(2);
            if (!((KaitaiStream.ByteArrayCompare(Unk6, new byte[] { 0, 0 }) == 0)))
            {
                throw new ValidationNotEqualError(new byte[] { 0, 0 }, Unk6, M_Io, "/seq/2");
            }
        }

        /// <summary>
        /// Playable characters (unit) metadata info
        /// </summary>
        public partial class CharacterInfo : KaitaiStruct
        {
            public static CharacterInfo FromFile(string fileName)
            {
                return new CharacterInfo(new KaitaiStream(fileName));
            }

            public CharacterInfo(KaitaiStream p__io, ListInfoBinaryFormat p__parent = null, ListInfoBinaryFormat p__root = null) : base(p__io)
            {
                m_parent = p__parent;
                m_root = p__root;
                f_catalogStorePilotCostume3String = false;
                f_catalogStorePilotCostume3TString = false;
                f_catalogStorePilotCostume2String = false;
                f_fOutString = false;
                f_catalogStorePilotCostume2TString = false;
                f_fString = false;
                f_releaseString = false;
                f_pString = false;
                _read();
            }
            private void _read()
            {
                _unitIndex = m_io.ReadU1();
                _seriesId = m_io.ReadU1();
                _unk2 = m_io.ReadU2be();
                _unitId = m_io.ReadU4be();
                _releaseStringOffset = m_io.ReadU4be();
                _fStringOffset = m_io.ReadU4be();
                _fOutStringOffset = m_io.ReadU4be();
                _pStringOffset = m_io.ReadU4be();
                _unitSelectOrder = m_io.ReadU1();
                _arcadeSmallSpriteIndex = m_io.ReadU1();
                _arcadeUnitNameSpriteIndex = m_io.ReadU1();
                _unk27 = m_io.ReadU1();
                _arcadeSelectionCostume1SpriteAssetHash = m_io.ReadU4be();
                _arcadeSelectionCostume2SpriteAssetHash = m_io.ReadU4be();
                _arcadeSelectionCostume3SpriteAssetHash = m_io.ReadU4be();
                _loadingLeftCostume1SpriteAssetHash = m_io.ReadU4be();
                _loadingLeftCostume2SpriteAssetHash = m_io.ReadU4be();
                _loadingLeftPilotCostume3SpriteAssetHash = m_io.ReadU4be();
                _loadingRightCostume1SpriteAssetHash = m_io.ReadU4be();
                _loadingRighCostume2SpriteAssetHash = m_io.ReadU4be();
                _loadingRightCostume3SpriteAssetHash = m_io.ReadU4be();
                _genericSelectionCostume1SpriteAssetHash = m_io.ReadU4be();
                _genericSelectionCostume2SpriteAssetHash = m_io.ReadU4be();
                _genericSelectionCostume3SpriteAssetHash = m_io.ReadU4be();
                _loadingTargetUnitSpriteAssetHash = m_io.ReadU4be();
                _loadingTargetPilotCostume1SpriteAssetHash = m_io.ReadU4be();
                _loadingTargetPilotCostume2SpriteAssetHash = m_io.ReadU4be();
                _loadingTargetPilotCostume3SpriteAssetHash = m_io.ReadU4be();
                _inGameSortieAndAwakeningPilotCostume1SpriteAssetHash = m_io.ReadU4be();
                _inGameSortieAndAwakeningPilotCostume2SpriteAssetHash = m_io.ReadU4be();
                _inGameSortieAndAwakeningPilotCostume3SpriteAssetHash = m_io.ReadU4be();
                _spriteFramesAssetHash = m_io.ReadU4be();
                _resultSmallUnitSpriteHash = m_io.ReadU4be();
                _unk112 = m_io.ReadU1();
                _figurineSpriteIndex = m_io.ReadU1();
                _unk114 = m_io.ReadU2be();
                _figurineSpriteAssetHash = m_io.ReadU4be();
                _loadingTargetUnitSmallSpriteAssetHash = m_io.ReadU4be();
                _unk124 = m_io.ReadU4be();
                _unk128 = m_io.ReadU4be();
                _catalogStorePilotCostume2SpriteAssetHash = m_io.ReadU4be();
                _catalogStorePilotCostume2TStringOffset = m_io.ReadU4be();
                _catalogStorePilotCostume2StringOffset = m_io.ReadU4be();
                _catalogStorePilotCostume3SpriteAssetHash = m_io.ReadU4be();
                _catalogStorePilotCostume3TStringOffset = m_io.ReadU4be();
                _catalogStorePilotCostume3StringOffset = m_io.ReadU4be();
                _unk156 = m_io.ReadU4be();
            }
            private bool f_catalogStorePilotCostume3String;
            private string _catalogStorePilotCostume3String;
            public string CatalogStorePilotCostume3String
            {
                get
                {
                    if (f_catalogStorePilotCostume3String)
                        return _catalogStorePilotCostume3String;
                    long _pos = m_io.Pos;
                    m_io.Seek(CatalogStorePilotCostume3StringOffset);
                    _catalogStorePilotCostume3String = System.Text.Encoding.GetEncoding("UTF-8").GetString(m_io.ReadBytesTerm(0, false, true, true));
                    m_io.Seek(_pos);
                    f_catalogStorePilotCostume3String = true;
                    return _catalogStorePilotCostume3String;
                }
            }
            private bool f_catalogStorePilotCostume3TString;
            private string _catalogStorePilotCostume3TString;
            public string CatalogStorePilotCostume3TString
            {
                get
                {
                    if (f_catalogStorePilotCostume3TString)
                        return _catalogStorePilotCostume3TString;
                    long _pos = m_io.Pos;
                    m_io.Seek(CatalogStorePilotCostume3TStringOffset);
                    _catalogStorePilotCostume3TString = System.Text.Encoding.GetEncoding("UTF-8").GetString(m_io.ReadBytesTerm(0, false, true, true));
                    m_io.Seek(_pos);
                    f_catalogStorePilotCostume3TString = true;
                    return _catalogStorePilotCostume3TString;
                }
            }
            private bool f_catalogStorePilotCostume2String;
            private string _catalogStorePilotCostume2String;
            public string CatalogStorePilotCostume2String
            {
                get
                {
                    if (f_catalogStorePilotCostume2String)
                        return _catalogStorePilotCostume2String;
                    long _pos = m_io.Pos;
                    m_io.Seek(CatalogStorePilotCostume2StringOffset);
                    _catalogStorePilotCostume2String = System.Text.Encoding.GetEncoding("UTF-8").GetString(m_io.ReadBytesTerm(0, false, true, true));
                    m_io.Seek(_pos);
                    f_catalogStorePilotCostume2String = true;
                    return _catalogStorePilotCostume2String;
                }
            }
            private bool f_fOutString;
            private string _fOutString;
            public string FOutString
            {
                get
                {
                    if (f_fOutString)
                        return _fOutString;
                    long _pos = m_io.Pos;
                    m_io.Seek(FOutStringOffset);
                    _fOutString = System.Text.Encoding.GetEncoding("UTF-8").GetString(m_io.ReadBytesTerm(0, false, true, true));
                    m_io.Seek(_pos);
                    f_fOutString = true;
                    return _fOutString;
                }
            }
            private bool f_catalogStorePilotCostume2TString;
            private string _catalogStorePilotCostume2TString;
            public string CatalogStorePilotCostume2TString
            {
                get
                {
                    if (f_catalogStorePilotCostume2TString)
                        return _catalogStorePilotCostume2TString;
                    long _pos = m_io.Pos;
                    m_io.Seek(CatalogStorePilotCostume2TStringOffset);
                    _catalogStorePilotCostume2TString = System.Text.Encoding.GetEncoding("UTF-8").GetString(m_io.ReadBytesTerm(0, false, true, true));
                    m_io.Seek(_pos);
                    f_catalogStorePilotCostume2TString = true;
                    return _catalogStorePilotCostume2TString;
                }
            }
            private bool f_fString;
            private string _fString;
            public string FString
            {
                get
                {
                    if (f_fString)
                        return _fString;
                    long _pos = m_io.Pos;
                    m_io.Seek(FStringOffset);
                    _fString = System.Text.Encoding.GetEncoding("UTF-8").GetString(m_io.ReadBytesTerm(0, false, true, true));
                    m_io.Seek(_pos);
                    f_fString = true;
                    return _fString;
                }
            }
            private bool f_releaseString;
            private string _releaseString;
            public string ReleaseString
            {
                get
                {
                    if (f_releaseString)
                        return _releaseString;
                    long _pos = m_io.Pos;
                    m_io.Seek(ReleaseStringOffset);
                    _releaseString = System.Text.Encoding.GetEncoding("UTF-8").GetString(m_io.ReadBytesTerm(0, false, true, true));
                    m_io.Seek(_pos);
                    f_releaseString = true;
                    return _releaseString;
                }
            }
            private bool f_pString;
            private string _pString;
            public string PString
            {
                get
                {
                    if (f_pString)
                        return _pString;
                    long _pos = m_io.Pos;
                    m_io.Seek(PStringOffset);
                    _pString = System.Text.Encoding.GetEncoding("UTF-8").GetString(m_io.ReadBytesTerm(0, false, true, true));
                    m_io.Seek(_pos);
                    f_pString = true;
                    return _pString;
                }
            }
            private byte _unitIndex;
            private byte _seriesId;
            private ushort _unk2;
            private uint _unitId;
            private uint _releaseStringOffset;
            private uint _fStringOffset;
            private uint _fOutStringOffset;
            private uint _pStringOffset;
            private byte _unitSelectOrder;
            private byte _arcadeSmallSpriteIndex;
            private byte _arcadeUnitNameSpriteIndex;
            private byte _unk27;
            private uint _arcadeSelectionCostume1SpriteAssetHash;
            private uint _arcadeSelectionCostume2SpriteAssetHash;
            private uint _arcadeSelectionCostume3SpriteAssetHash;
            private uint _loadingLeftCostume1SpriteAssetHash;
            private uint _loadingLeftCostume2SpriteAssetHash;
            private uint _loadingLeftPilotCostume3SpriteAssetHash;
            private uint _loadingRightCostume1SpriteAssetHash;
            private uint _loadingRighCostume2SpriteAssetHash;
            private uint _loadingRightCostume3SpriteAssetHash;
            private uint _genericSelectionCostume1SpriteAssetHash;
            private uint _genericSelectionCostume2SpriteAssetHash;
            private uint _genericSelectionCostume3SpriteAssetHash;
            private uint _loadingTargetUnitSpriteAssetHash;
            private uint _loadingTargetPilotCostume1SpriteAssetHash;
            private uint _loadingTargetPilotCostume2SpriteAssetHash;
            private uint _loadingTargetPilotCostume3SpriteAssetHash;
            private uint _inGameSortieAndAwakeningPilotCostume1SpriteAssetHash;
            private uint _inGameSortieAndAwakeningPilotCostume2SpriteAssetHash;
            private uint _inGameSortieAndAwakeningPilotCostume3SpriteAssetHash;
            private uint _spriteFramesAssetHash;
            private uint _resultSmallUnitSpriteHash;
            private byte _unk112;
            private byte _figurineSpriteIndex;
            private ushort _unk114;
            private uint _figurineSpriteAssetHash;
            private uint _loadingTargetUnitSmallSpriteAssetHash;
            private uint _unk124;
            private uint _unk128;
            private uint _catalogStorePilotCostume2SpriteAssetHash;
            private uint _catalogStorePilotCostume2TStringOffset;
            private uint _catalogStorePilotCostume2StringOffset;
            private uint _catalogStorePilotCostume3SpriteAssetHash;
            private uint _catalogStorePilotCostume3TStringOffset;
            private uint _catalogStorePilotCostume3StringOffset;
            private uint _unk156;
            private ListInfoBinaryFormat m_root;
            private ListInfoBinaryFormat m_parent;
            public byte UnitIndex { get { return _unitIndex; } }
            public byte SeriesId { get { return _seriesId; } }

            /// <summary>
            /// Always double 0xFF from observed patterns
            /// </summary>
            public ushort Unk2 { get { return _unk2; } }
            public uint UnitId { get { return _unitId; } }

            /// <summary>
            /// Always after 'SCharacterList.' which is 'Release' in Japanese 'リリース'
            /// </summary>
            public uint ReleaseStringOffset { get { return _releaseStringOffset; } }

            /// <summary>
            /// Format: F_{{unit_id}}
            /// </summary>
            public uint FStringOffset { get { return _fStringOffset; } }

            /// <summary>
            /// Format: F_OUT_{{unit_id}}
            /// </summary>
            public uint FOutStringOffset { get { return _fOutStringOffset; } }

            /// <summary>
            /// Format: P_{{unit_id}}
            /// </summary>
            public uint PStringOffset { get { return _pStringOffset; } }

            /// <summary>
            /// Placement of unit's selection order in its series, starts from 0
            /// </summary>
            public byte UnitSelectOrder { get { return _unitSelectOrder; } }

            /// <summary>
            /// Placement of unit's arcade small select sprite texture in the 'ArcadeSelectSmallSprites' asset file
            /// </summary>
            public byte ArcadeSmallSpriteIndex { get { return _arcadeSmallSpriteIndex; } }

            /// <summary>
            /// Placement of unit's arcade name select texture in the 'ArcadeSelectUnitNameSprites' asset file
            /// </summary>
            public byte ArcadeUnitNameSpriteIndex { get { return _arcadeUnitNameSpriteIndex; } }
            public byte Unk27 { get { return _unk27; } }

            /// <summary>
            /// Asset hash for arcade selection srpties
            /// Used when selecting unit in arcade mode
            /// Asset contains both unit and pilot sprites (costume 1)
            /// </summary>
            public uint ArcadeSelectionCostume1SpriteAssetHash { get { return _arcadeSelectionCostume1SpriteAssetHash; } }

            /// <summary>
            /// Asset hash for arcade selection srpties (optional)
            /// Used when selecting unit in arcade mode
            /// Asset contains both unit and pilot sprites (costume 2)
            /// </summary>
            public uint ArcadeSelectionCostume2SpriteAssetHash { get { return _arcadeSelectionCostume2SpriteAssetHash; } }

            /// <summary>
            /// Asset hash for arcade selection srpties (optional)
            /// Used when selecting unit in arcade mode
            /// Asset contains both unit and pilot sprites (costume 3)
            /// </summary>
            public uint ArcadeSelectionCostume3SpriteAssetHash { get { return _arcadeSelectionCostume3SpriteAssetHash; } }

            /// <summary>
            /// Asset hash for loading screen (left) srpties 
            /// Used during VS loading screen when the unit is on the left side
            /// Asset contains both unit and pilot sprites (costume 1)
            /// </summary>
            public uint LoadingLeftCostume1SpriteAssetHash { get { return _loadingLeftCostume1SpriteAssetHash; } }

            /// <summary>
            /// Asset hash for loading screen (left) srpties (optional)
            /// Used during VS loading screen when the unit is on the left side
            /// Asset contains both unit and pilot sprites (costume 2)
            /// </summary>
            public uint LoadingLeftCostume2SpriteAssetHash { get { return _loadingLeftCostume2SpriteAssetHash; } }

            /// <summary>
            /// Asset hash for loading screen (left) srpties (optional)
            /// Used during VS loading screen when the unit is on the left side
            /// Asset contains both unit and pilot sprite (costume 3)
            /// </summary>
            public uint LoadingLeftPilotCostume3SpriteAssetHash { get { return _loadingLeftPilotCostume3SpriteAssetHash; } }

            /// <summary>
            /// Asset hash for loading screen (right) srpties
            /// Used during VS loading screen when the unit is on the right side
            /// Asset contains both unit and pilot sprites (costume 1)
            /// </summary>
            public uint LoadingRightCostume1SpriteAssetHash { get { return _loadingRightCostume1SpriteAssetHash; } }

            /// <summary>
            /// Asset hash for loading screen (right) srpties (optional)
            /// Used during VS loading screen when the unit is on the right side
            /// Asset contains both unit and pilot sprites (costume 2)
            /// </summary>
            public uint LoadingRighCostume2SpriteAssetHash { get { return _loadingRighCostume2SpriteAssetHash; } }

            /// <summary>
            /// Asset hash for loading screen (right) srpties (optional)
            /// Used during VS loading screen when the unit is on the right side
            /// Asset contains both unit and pilot sprites (costume 3)
            /// </summary>
            public uint LoadingRightCostume3SpriteAssetHash { get { return _loadingRightCostume3SpriteAssetHash; } }

            /// <summary>
            /// Asset hash for generic selection srpties
            /// Mostly used in vertical unit selection menu e.g. Free Battle / FB Missions
            /// Asset contains both unit and pilot sprite (costume 1)
            /// </summary>
            public uint GenericSelectionCostume1SpriteAssetHash { get { return _genericSelectionCostume1SpriteAssetHash; } }

            /// <summary>
            /// Asset hash for generic selection srpties (optional)
            /// Mostly used in vertical unit selection menu e.g. Free Battle / FB Missions
            /// Asset contains both unit and pilot sprites (costume 2)
            /// </summary>
            public uint GenericSelectionCostume2SpriteAssetHash { get { return _genericSelectionCostume2SpriteAssetHash; } }

            /// <summary>
            /// Asset hash for generic selection srpties (optional)
            /// Mostly used in vertical unit selection menu e.g. Free Battle / FB Missions
            /// Asset contains both unit and pilot sprites (costume 3)
            /// </summary>
            public uint GenericSelectionCostume3SpriteAssetHash { get { return _genericSelectionCostume3SpriteAssetHash; } }

            /// <summary>
            /// Asset hash for loading screen target unit sprite
            /// Mainly used in Arcade / CPU battles where this unit is the designated target, usually is the same sprite as loading right but bigger
            /// Asset only contains unit sprite
            /// </summary>
            public uint LoadingTargetUnitSpriteAssetHash { get { return _loadingTargetUnitSpriteAssetHash; } }

            /// <summary>
            /// Asset hash for loading screen pilot target pilot sprite
            /// Mainly used in Arcade / CPU battles where this unit is the designated target, usually is the same sprite as loading right but bigger
            /// Asset only contains pilot costume 1 sprite
            /// </summary>
            public uint LoadingTargetPilotCostume1SpriteAssetHash { get { return _loadingTargetPilotCostume1SpriteAssetHash; } }

            /// <summary>
            /// Asset hash for loading screen pilot target pilot sprite (optional)
            /// Mainly used in Arcade / CPU battles where this unit is the designated target, usually is the same sprite as loading right but bigger
            /// Asset only contains pilot costume 2 sprite
            /// </summary>
            public uint LoadingTargetPilotCostume2SpriteAssetHash { get { return _loadingTargetPilotCostume2SpriteAssetHash; } }

            /// <summary>
            /// Asset hash for loading screen pilot target pilot sprite (optional)
            /// Mainly used in Arcade / CPU battles where this unit is the designated target, usually is the same sprite as loading right but bigger
            /// Asset only contains pilot costume 3 sprite
            /// </summary>
            public uint LoadingTargetPilotCostume3SpriteAssetHash { get { return _loadingTargetPilotCostume3SpriteAssetHash; } }

            /// <summary>
            /// Asset hash for in game sortie and awakening sprites
            /// Used during initial sortie bottom left pilot costume 1 speaking and awakening cut-in
            /// Asset contains two folders: 
            ///   1. Bottom left pilot costume 1 speaking sprite with mouth piece frame sprites, alongside sprite placement / script file LMB
            ///   2. Awakening cut-in pilot costume 1 sprite with background / effects etc, alongside sprite placement / script file LMB
            /// </summary>
            public uint InGameSortieAndAwakeningPilotCostume1SpriteAssetHash { get { return _inGameSortieAndAwakeningPilotCostume1SpriteAssetHash; } }

            /// <summary>
            /// Asset hash for in game sortie and awakening sprites (optional)
            /// Used during initial sortie bottom left pilot costume 2 speaking and awakening cut-in
            /// Asset contains two folders: 
            ///   1. Bottom left pilot costume 2 speaking sprite with mouth piece frame sprites, alongside sprite placement / script file LMB
            ///   2. Awakening cut-in pilot costume 2 sprite with background / effects etc, alongside sprite placement / script file LMB
            /// </summary>
            public uint InGameSortieAndAwakeningPilotCostume2SpriteAssetHash { get { return _inGameSortieAndAwakeningPilotCostume2SpriteAssetHash; } }

            /// <summary>
            /// Asset hash for in game sortie and awakening sprites (optional)
            /// Used during initial sortie bottom left pilot costume 3 speaking and awakening cut-in
            /// Asset contains two folders: 
            ///   1. Bottom left pilot costume 3 speaking sprite with mouth piece frame sprites, alongside sprite placement / script file LMB
            ///   2. Awakening cut-in pilot costume 3 sprite with background / effects etc, alongside sprite placement / script file LMB
            /// </summary>
            public uint InGameSortieAndAwakeningPilotCostume3SpriteAssetHash { get { return _inGameSortieAndAwakeningPilotCostume3SpriteAssetHash; } }

            /// <summary>
            /// Asset hash for sprite frame data, also known as KPKP format
            /// In game sortie sprite's mouth piece sprite &quot;movement&quot; is controlled by this file
            /// </summary>
            public uint SpriteFramesAssetHash { get { return _spriteFramesAssetHash; } }

            /// <summary>
            /// Asset hash for result screen sidebar scoreboard's unit sprite
            /// Asset only contains unit sprite
            /// </summary>
            public uint ResultSmallUnitSpriteHash { get { return _resultSmallUnitSpriteHash; } }

            /// <summary>
            /// Always single 0 from observed patterns
            /// </summary>
            public byte Unk112 { get { return _unk112; } }

            /// <summary>
            /// Placement of unit's figure sprite texture in the 'FigurineSprites' asset file
            /// </summary>
            public byte FigurineSpriteIndex { get { return _figurineSpriteIndex; } }

            /// <summary>
            /// Always double 0xFF from observed patterns
            /// </summary>
            public ushort Unk114 { get { return _unk114; } }

            /// <summary>
            /// Asset hash for unit's standalone figurine sprite
            /// Unused / deprecated in game, the game respects the figurine index instead of this
            /// </summary>
            public uint FigurineSpriteAssetHash { get { return _figurineSpriteAssetHash; } }

            /// <summary>
            /// Asset hash for loading screen small target unit sprite
            /// More compact version of `loading_target_unit_sprite_asset_hash`, probably used in similiar scenarios
            /// Asset only contains unit sprite
            /// </summary>
            public uint LoadingTargetUnitSmallSpriteAssetHash { get { return _loadingTargetUnitSmallSpriteAssetHash; } }
            public uint Unk124 { get { return _unk124; } }
            public uint Unk128 { get { return _unk128; } }

            /// <summary>
            /// Asset hash for catalog store pilot costume 2 sprite (optional)
            /// Used as a preview on the online catalog store for users to purchase these
            /// </summary>
            public uint CatalogStorePilotCostume2SpriteAssetHash { get { return _catalogStorePilotCostume2SpriteAssetHash; } }

            /// <summary>
            /// Format: IS_COSTUME_{{costume_id}}_T
            /// </summary>
            public uint CatalogStorePilotCostume2TStringOffset { get { return _catalogStorePilotCostume2TStringOffset; } }

            /// <summary>
            /// Format: IS_COSTUME_{{costume_id}}
            /// </summary>
            public uint CatalogStorePilotCostume2StringOffset { get { return _catalogStorePilotCostume2StringOffset; } }

            /// <summary>
            /// Asset hash for catalog store pilot costume 3 sprite (optional)
            /// Used as a preview on the online catalog store for users to purchase these
            /// </summary>
            public uint CatalogStorePilotCostume3SpriteAssetHash { get { return _catalogStorePilotCostume3SpriteAssetHash; } }

            /// <summary>
            /// Format: IS_COSTUME_{{costume_id}}_T
            /// </summary>
            public uint CatalogStorePilotCostume3TStringOffset { get { return _catalogStorePilotCostume3TStringOffset; } }

            /// <summary>
            /// Format: IS_COSTUME_{{costume_id}}
            /// </summary>
            public uint CatalogStorePilotCostume3StringOffset { get { return _catalogStorePilotCostume3StringOffset; } }
            public uint Unk156 { get { return _unk156; } }
            public ListInfoBinaryFormat M_Root { get { return m_root; } }
            public ListInfoBinaryFormat M_Parent { get { return m_parent; } }
        }

        /// <summary>
        /// Series metadata info
        /// </summary>
        public partial class SeriesInfo : KaitaiStruct
        {
            public static SeriesInfo FromFile(string fileName)
            {
                return new SeriesInfo(new KaitaiStream(fileName));
            }

            public SeriesInfo(KaitaiStream p__io, ListInfoBinaryFormat p__parent = null, ListInfoBinaryFormat p__root = null) : base(p__io)
            {
                m_parent = p__parent;
                m_root = p__root;
                f_releaseString = false;
                _read();
            }
            private void _read()
            {
                _seriesId = m_io.ReadU1();
                _unk2 = m_io.ReadU1();
                _unk3 = m_io.ReadU1();
                _unk4 = m_io.ReadU1();
                _releaseStringOffset = m_io.ReadU4be();
                _selectOrder = m_io.ReadU1();
                _logoSpriteIndex = m_io.ReadU1();
                _logoSprite2Index = m_io.ReadU1();
                _unk11 = m_io.ReadU1();
                _movieAssetHash = m_io.ReadU4be();
            }
            private bool f_releaseString;
            private string _releaseString;
            public string ReleaseString
            {
                get
                {
                    if (f_releaseString)
                        return _releaseString;
                    long _pos = m_io.Pos;
                    m_io.Seek(ReleaseStringOffset);
                    _releaseString = System.Text.Encoding.GetEncoding("UTF-8").GetString(m_io.ReadBytesTerm(0, false, true, true));
                    m_io.Seek(_pos);
                    f_releaseString = true;
                    return _releaseString;
                }
            }
            private byte _seriesId;
            private byte _unk2;
            private byte _unk3;
            private byte _unk4;
            private uint _releaseStringOffset;
            private byte _selectOrder;
            private byte _logoSpriteIndex;
            private byte _logoSprite2Index;
            private byte _unk11;
            private uint _movieAssetHash;
            private ListInfoBinaryFormat m_root;
            private ListInfoBinaryFormat m_parent;
            public byte SeriesId { get { return _seriesId; } }

            /// <summary>
            /// Not sure what this is, but closely related to the series_id
            /// </summary>
            public byte Unk2 { get { return _unk2; } }

            /// <summary>
            /// Always 0x80 from observed patterns
            /// </summary>
            public byte Unk3 { get { return _unk3; } }

            /// <summary>
            /// Always 0xFF from observed patterns
            /// </summary>
            public byte Unk4 { get { return _unk4; } }

            /// <summary>
            /// Always after 'SSeriesList.' which is 'Release' in Japanese 'リリース'
            /// </summary>
            public uint ReleaseStringOffset { get { return _releaseStringOffset; } }

            /// <summary>
            /// Placement of series's selection order, starts from 0
            /// </summary>
            public byte SelectOrder { get { return _selectOrder; } }

            /// <summary>
            /// Placement of series's select sprite texture in the 'SeriesLogoSprites' asset file
            /// </summary>
            public byte LogoSpriteIndex { get { return _logoSpriteIndex; } }

            /// <summary>
            /// Placement of series's select sprite texture in the 'SeriesLogoSprites2' asset file
            /// </summary>
            public byte LogoSprite2Index { get { return _logoSprite2Index; } }

            /// <summary>
            /// Always 0xFF from observed patterns
            /// </summary>
            public byte Unk11 { get { return _unk11; } }

            /// <summary>
            /// Asset hash for the series movie / pv
            /// Played after selection of unit in arcade mode
            /// </summary>
            public uint MovieAssetHash { get { return _movieAssetHash; } }
            public ListInfoBinaryFormat M_Root { get { return m_root; } }
            public ListInfoBinaryFormat M_Parent { get { return m_parent; } }
        }
        private bool f_listInfoName;
        private string _listInfoName;
        public string ListInfoName
        {
            get
            {
                if (f_listInfoName)
                    return _listInfoName;
                long _pos = m_io.Pos;
                m_io.Seek(ListInfoNameStringOffset);
                _listInfoName = System.Text.Encoding.GetEncoding("UTF-8").GetString(m_io.ReadBytesTerm(0, false, true, true));
                m_io.Seek(_pos);
                f_listInfoName = true;
                return _listInfoName;
            }
        }
        private bool f_body;
        private List<KaitaiStruct> _body;
        public List<KaitaiStruct> Body
        {
            get
            {
                if (f_body)
                    return _body;
                _body = new List<KaitaiStruct>();
                for (var i = 0; i < Count; i++)
                {
                    switch (ListInfoName) {
                    case "SCharacterList": {
                        _body.Add(new CharacterInfo(m_io, this, m_root));
                        break;
                    }
                    case "SSeriesList": {
                        _body.Add(new SeriesInfo(m_io, this, m_root));
                        break;
                    }
                    }
                }
                f_body = true;
                return _body;
            }
        }
        private uint _listInfoNameStringOffset;
        private ushort _count;
        private byte[] _unk6;
        private ListInfoBinaryFormat m_root;
        private KaitaiStruct m_parent;

        /// <summary>
        /// Name of the list info, will determine which list info schema to use
        /// </summary>
        public uint ListInfoNameStringOffset { get { return _listInfoNameStringOffset; } }

        /// <summary>
        /// Number of info items in the list
        /// </summary>
        public ushort Count { get { return _count; } }

        /// <summary>
        /// Always 0 from observed patterns
        /// </summary>
        public byte[] Unk6 { get { return _unk6; } }
        public ListInfoBinaryFormat M_Root { get { return m_root; } }
        public KaitaiStruct M_Parent { get { return m_parent; } }
    }
}
