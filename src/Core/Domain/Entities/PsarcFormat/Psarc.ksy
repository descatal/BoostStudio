meta:
  id: psarc
  file-extension: psarc
  application: Sony PlayStation archive
  endian: be

seq:
  - id: header
    type: toc_header
  - id: tables
    type: toc_tables
    size: header.toc_size - 32

enums:
  compression_type:
    0: none
    0x7a6c6962: zlib
    0x6c7a6d61: lzma

types:
  toc_header:
    seq:
      - id: magic
        contents: PSAR
      - id: version_major
        type: u2
      - id: version_minor
        type: u2
      - id: compression_type
        type: u4
        enum: compression_type
      - id: toc_size
        type: u4
      - id: toc_entry_size
        type: u4
      - id: toc_entries
        type: u4
      - id: block_size
        type: u4
      - id: archive_flags
        type: u4

  toc_tables:
    seq:
      - id: entries
        type: toc_entry
        size: _root.header.toc_entry_size
        repeat: expr
        repeat-expr: _root.header.toc_entries
      - id: blocks
        type: u2
        repeat: eos

  toc_entry:
    seq:
      - id: name_digest
        size: 16
      - id: start_block_index
        type: u4
      - id: size
        type: b40
      - id: offset
        type: b40
    instances:
      body:
        pos: offset
        size: _root.tables.blocks[start_block_index]
        io: _root._io
        process: zlib
        repeat: until
        repeat-until: body.size == size # FIXME this doesn't work in ksy