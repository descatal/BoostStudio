meta:
  id: fhm
  endian: be
  file-extension: fhm
seq:
  - id: body
    type: file_body
    size-eos: true
types:
  file_body:
    seq:
      - id: file_magic
        type: u4
        enum: file_magic_enums
      - id: file_content
        type:
          switch-on: file_magic
          cases:
            "file_magic_enums::fhm": fhm_body
            "_": generic_body

  fhm_body:
    seq:
      - id: flag_1
        # not sure what these are, for now put validation first
        contents: [0x01, 0x01]
      - id: flag_2
        contents: [0x00, 0x10]
      - id: unk_c
        type: uint_zero
      - id: total_file_size
        type: u4
      - id: num_files
        type: u4
      - id: files
        type: fhm_file((0x14 + (_index * 4)), (num_files * 4))
        repeat: expr
        repeat-expr: num_files
      - id: alignment
        size: 0xC

  fhm_file:
    params:
      - id: index_ofs
        type: s4
      - id: region_size
        type: u4
    instances:
      offset:
        type: u4
        pos: index_ofs
      size:
        type: u4
        pos: index_ofs + (region_size)
      asset_load_type:
        type: u4
        enum: asset_load_enum
        pos: index_ofs + (region_size * 2)
      unk_type:
        type: u4
        enum: unk_enum
        pos: index_ofs + (region_size * 3)
      body:
        type: file_body
        pos: offset
        size: size

  generic_body:
    instances:
      body:
        size-eos: true

  # for validation purposes
  uint_zero:
    seq:
      - id: zero
        contents: [0, 0, 0, 0]

enums:
  asset_load_enum:
    0x0: normal
    0x1: model
    0x2: image
    0x3: unknown # occasionally found on nested Fhm, but not always
  unk_enum:
    0x0: unknown
  file_magic_enums:
    0x46484D20: fhm
