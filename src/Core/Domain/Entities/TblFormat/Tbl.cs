//----------------------
// <auto-generated>
//     This is a generated file! Please edit source .ksy file and use kaitai-struct-compiler to rebuild
// </auto-generated>
//----------------------
using Kaitai;
using System.Collections.Generic;

namespace BoostStudio.Formats
{
    public partial class Tbl : KaitaiStruct
    {
        public Tbl(ushort p_totalFileSize, KaitaiStream p__io, KaitaiStruct p__parent = null, Tbl p__root = null, bool write = false) : base(p__io)
        {
            m_parent = p__parent;
            m_root = p__root ?? this;
            _totalFileSize = p_totalFileSize;
            f_filePaths = write;
            f_fileInfos = write;
            if (!write)
                _read();
        }
        private void _read()
        {
            _fileMagic = m_io.ReadBytes(4);
            if (!((KaitaiStream.ByteArrayCompare(FileMagic, new byte[]
                   {
                       84, 66, 76, 32
                   }) ==
                   0)))
            {
                throw new ValidationNotEqualError(new byte[]
                {
                    84, 66, 76, 32
                }, FileMagic, M_Io, "/seq/0");
            }
            _flag1 = m_io.ReadBytes(2);
            if (!((KaitaiStream.ByteArrayCompare(Flag1, new byte[]
                   {
                       1, 1
                   }) ==
                   0)))
            {
                throw new ValidationNotEqualError(new byte[]
                {
                    1, 1
                }, Flag1, M_Io, "/seq/1");
            }
            _flag2 = m_io.ReadBytes(2);
            if (!((KaitaiStream.ByteArrayCompare(Flag2, new byte[]
                   {
                       0, 0
                   }) ==
                   0)))
            {
                throw new ValidationNotEqualError(new byte[]
                {
                    0, 0
                }, Flag2, M_Io, "/seq/2");
            }
            _filePathCount = m_io.ReadS4be();
            _cumulativeFileCount = m_io.ReadU4be();
            _filePathOffsets = new List<FilePathOffsetBody>();
            for (var i = 0; i < FilePathCount; i++)
            {
                _filePathOffsets.Add(new FilePathOffsetBody(m_io, this, m_root));
            }
            _fileInfoOffsets = new List<uint>();
            for (var i = 0; i < CumulativeFileCount; i++)
            {
                _fileInfoOffsets.Add(m_io.ReadU4be());
            }
        }

        public partial class FilePathOffsetBody : KaitaiStruct
        {
            public static FilePathOffsetBody FromFile(string fileName)
            {
                return new FilePathOffsetBody(new KaitaiStream(fileName));
            }

            public FilePathOffsetBody(KaitaiStream p__io, Tbl p__parent = null, Tbl p__root = null, bool write = false) : base(p__io)
            {
                m_parent = p__parent;
                m_root = p__root;
                if (!write)
                    _read();
            }
            private void _read()
            {
                _subfolderFlag = m_io.ReadU2be();
                _pathPointer = m_io.ReadU2be();
            }
            private ushort _subfolderFlag;
            private ushort _pathPointer;
            private Tbl m_root;
            private Tbl m_parent;
            public ushort SubfolderFlag
            {
                get { return _subfolderFlag; }

                set
                {
                    _subfolderFlag = value;
                }
            }
            public ushort PathPointer
            {
                get { return _pathPointer; }

                set
                {
                    _pathPointer = value;
                }
            }
            public Tbl M_Root
            {
                get { return m_root; }

                set
                {
                    m_root = value;
                }
            }
            public Tbl M_Parent
            {
                get { return m_parent; }

                set
                {
                    m_parent = value;
                }
            }
        }

        public partial class FilePathBody : KaitaiStruct
        {
            public FilePathBody(int p_index, KaitaiStream p__io, Tbl p__parent = null, Tbl p__root = null, bool write = false) : base(p__io)
            {
                m_parent = p__parent;
                m_root = p__root;
                _index = p_index;
                f_pointer = write;
                f_size = write;
                f_path = write;
                if (!write)
                    _read();
            }
            private void _read()
            {
            }
            private bool f_pointer;
            private ushort _pointer;
            public ushort Pointer
            {
                get
                {
                    if (f_pointer)
                        return _pointer;
                    _pointer = (ushort)(M_Parent.FilePathOffsets[Index].PathPointer);
                    f_pointer = true;
                    return _pointer;
                }

                set
                {
                    _pointer = value;
                }
            }
            private bool f_size;
            private int _size;
            public int Size
            {
                get
                {
                    if (f_size)
                        return _size;
                    _size = (int)((M_Parent.FilePathCount == (Index + 1) ? (M_Parent.TotalFileSize - Pointer) : (M_Parent.FilePathOffsets[(Index + 1)].PathPointer - Pointer)));
                    f_size = true;
                    return _size;
                }

                set
                {
                    _size = value;
                }
            }
            private bool f_path;
            private string _path;
            public string Path
            {
                get
                {
                    if (f_path)
                        return _path;
                    long _pos = m_io.Pos;
                    m_io.Seek(Pointer);
                    _path = System.Text.Encoding.GetEncoding("UTF-8").GetString(KaitaiStream.BytesTerminate(m_io.ReadBytes(Size), 0, false));
                    m_io.Seek(_pos);
                    f_path = true;
                    return _path;
                }

                set
                {
                    _path = value;
                }
            }
            private int _index;
            private Tbl m_root;
            private Tbl m_parent;
            public int Index
            {
                get { return _index; }

                set
                {
                    _index = value;
                }
            }
            public Tbl M_Root
            {
                get { return m_root; }

                set
                {
                    m_root = value;
                }
            }
            public Tbl M_Parent
            {
                get { return m_parent; }

                set
                {
                    m_parent = value;
                }
            }
        }

        public partial class FileInfoBody : KaitaiStruct
        {
            public FileInfoBody(int p_index, KaitaiStream p__io, Tbl p__parent = null, Tbl p__root = null, bool write = false) : base(p__io)
            {
                m_parent = p__parent;
                m_root = p__root;
                _index = p_index;
                f_offset = write;
                f_fileInfo = write;
                if (!write)
                    _read();
            }
            private void _read()
            {
            }
            private bool f_offset;
            private uint _offset;
            public uint Offset
            {
                get
                {
                    if (f_offset)
                        return _offset;
                    _offset = (uint)(M_Parent.FileInfoOffsets[Index]);
                    f_offset = true;
                    return _offset;
                }

                set
                {
                    _offset = value;
                }
            }
            private bool f_fileInfo;
            private FileInfo _fileInfo;
            public FileInfo FileInfo
            {
                get
                {
                    if (f_fileInfo)
                        return _fileInfo;
                    if (Offset != 0)
                    {
                        long _pos = m_io.Pos;
                        m_io.Seek(Offset);
                        _fileInfo = new FileInfo(m_io, this, m_root);
                        m_io.Seek(_pos);
                        f_fileInfo = true;
                    }
                    return _fileInfo;
                }

                set
                {
                    _fileInfo = value;
                }
            }
            private int _index;
            private Tbl m_root;
            private Tbl m_parent;
            public int Index
            {
                get { return _index; }

                set
                {
                    _index = value;
                }
            }
            public Tbl M_Root
            {
                get { return m_root; }

                set
                {
                    m_root = value;
                }
            }
            public Tbl M_Parent
            {
                get { return m_parent; }

                set
                {
                    m_parent = value;
                }
            }
        }

        public partial class FileInfo : KaitaiStruct
        {
            public static FileInfo FromFile(string fileName)
            {
                return new FileInfo(new KaitaiStream(fileName));
            }

            public FileInfo(KaitaiStream p__io, Tbl.FileInfoBody p__parent = null, Tbl p__root = null, bool write = false) : base(p__io)
            {
                m_parent = p__parent;
                m_root = p__root;
                f_pathBody = write;
                if (!write)
                    _read();
            }
            private void _read()
            {
                _patchNumber = m_io.ReadU4be();
                _pathIndex = m_io.ReadS4be();
                _unk8 = m_io.ReadBytes(4);
                if (!((KaitaiStream.ByteArrayCompare(Unk8, new byte[]
                       {
                           0, 4, 0, 0
                       }) ==
                       0)))
                {
                    throw new ValidationNotEqualError(new byte[]
                    {
                        0, 4, 0, 0
                    }, Unk8, M_Io, "/types/file_info/seq/2");
                }
                _size1 = m_io.ReadU4be();
                _size2 = m_io.ReadU4be();
                _size3 = m_io.ReadU4be();
                _unk28 = m_io.ReadBytes(4);
                if (!((KaitaiStream.ByteArrayCompare(Unk28, new byte[]
                       {
                           0, 0, 0, 0
                       }) ==
                       0)))
                {
                    throw new ValidationNotEqualError(new byte[]
                    {
                        0, 0, 0, 0
                    }, Unk28, M_Io, "/types/file_info/seq/6");
                }
                _hashName = m_io.ReadU4be();
            }
            private bool f_pathBody;
            private FilePathBody _pathBody;
            public FilePathBody PathBody
            {
                get
                {
                    if (f_pathBody)
                        return _pathBody;
                    if (((PathIndex == 0) && (Size1 == 0) && (Size2 == 0) && (Size3 == 0)) != true)
                    {
                        _pathBody = (FilePathBody)(M_Parent.M_Parent.FilePaths[PathIndex]);
                    }
                    f_pathBody = true;
                    return _pathBody;
                }

                set
                {
                    _pathBody = value;
                }
            }
            private uint _patchNumber;
            private int _pathIndex;
            private byte[] _unk8;
            private uint _size1;
            private uint _size2;
            private uint _size3;
            private byte[] _unk28;
            private uint _hashName;
            private Tbl m_root;
            private Tbl.FileInfoBody m_parent;
            public uint PatchNumber
            {
                get { return _patchNumber; }

                set
                {
                    _patchNumber = value;
                }
            }
            public int PathIndex
            {
                get { return _pathIndex; }

                set
                {
                    _pathIndex = value;
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
            public uint Size1
            {
                get { return _size1; }

                set
                {
                    _size1 = value;
                }
            }
            public uint Size2
            {
                get { return _size2; }

                set
                {
                    _size2 = value;
                }
            }
            public uint Size3
            {
                get { return _size3; }

                set
                {
                    _size3 = value;
                }
            }
            public byte[] Unk28
            {
                get { return _unk28; }

                set
                {
                    _unk28 = value;
                }
            }
            public uint HashName
            {
                get { return _hashName; }

                set
                {
                    _hashName = value;
                }
            }
            public Tbl M_Root
            {
                get { return m_root; }

                set
                {
                    m_root = value;
                }
            }
            public Tbl.FileInfoBody M_Parent
            {
                get { return m_parent; }

                set
                {
                    m_parent = value;
                }
            }
        }

        private bool f_filePaths;
        private List<FilePathBody> _filePaths;
        public List<FilePathBody> FilePaths
        {
            get
            {
                if (f_filePaths)
                    return _filePaths;
                _filePaths = new List<FilePathBody>();
                for (var i = 0; i < FilePathCount; i++)
                {
                    _filePaths.Add(new FilePathBody(i, m_io, this, m_root));
                }
                f_filePaths = true;
                return _filePaths;
            }

            set
            {
                _filePaths = value;
            }
        }
        private bool f_fileInfos;
        private List<FileInfoBody> _fileInfos;
        public List<FileInfoBody> FileInfos
        {
            get
            {
                if (f_fileInfos)
                    return _fileInfos;
                _fileInfos = new List<FileInfoBody>();
                for (var i = 0; i < CumulativeFileCount; i++)
                {
                    _fileInfos.Add(new FileInfoBody(i, m_io, this, m_root));
                }
                f_fileInfos = true;
                return _fileInfos;
            }

            set
            {
                _fileInfos = value;
            }
        }
        private byte[] _fileMagic;
        private byte[] _flag1;
        private byte[] _flag2;
        private int _filePathCount;
        private uint _cumulativeFileCount;
        private List<FilePathOffsetBody> _filePathOffsets;
        private List<uint> _fileInfoOffsets;
        private ushort _totalFileSize;
        private Tbl m_root;
        private KaitaiStruct m_parent;
        public byte[] FileMagic
        {
            get { return _fileMagic; }

            set
            {
                _fileMagic = value;
            }
        }
        public byte[] Flag1
        {
            get { return _flag1; }

            set
            {
                _flag1 = value;
            }
        }
        public byte[] Flag2
        {
            get { return _flag2; }

            set
            {
                _flag2 = value;
            }
        }
        public int FilePathCount
        {
            get { return _filePathCount; }

            set
            {
                _filePathCount = value;
            }
        }
        public uint CumulativeFileCount
        {
            get { return _cumulativeFileCount; }

            set
            {
                _cumulativeFileCount = value;
            }
        }
        public List<FilePathOffsetBody> FilePathOffsets
        {
            get { return _filePathOffsets; }

            set
            {
                _filePathOffsets = value;
            }
        }
        public List<uint> FileInfoOffsets
        {
            get { return _fileInfoOffsets; }

            set
            {
                _fileInfoOffsets = value;
            }
        }
        public ushort TotalFileSize
        {
            get { return _totalFileSize; }

            set
            {
                _totalFileSize = value;
            }
        }
        public Tbl M_Root
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