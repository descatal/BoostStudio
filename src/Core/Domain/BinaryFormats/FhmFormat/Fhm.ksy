meta:
  id: fhm
  endian: be
  file-extension: fhm
params:
  - id: binary_size
    type: u4
seq:
  - id: body
    type: file_body(binary_size)
    size-eos: true
types:
  file_body:
    params:
      - id: section_size
        type: u4
    seq:
      - id: file_magic
        type: u4
        enum: file_magic_enums
      - id: file_content
        type:
          switch-on: file_magic
          cases:
            "file_magic_enums::fhm": fhm_body(section_size)
            "_": generic_body

  fhm_body:
    params:
      - id: section_size
        type: u4
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
      - id: file_offsets
        type: u4
        repeat: expr
        repeat-expr: num_files
      - id: file_sizes
        type: u4
        repeat: expr
        repeat-expr: num_files
      - id: file_asset_load_types
        type: u4
        enum: asset_load_enum
        repeat: expr
        repeat-expr: num_files
      - id: file_unk_types
        type: u4
        enum: unk_enum
        repeat: expr
        repeat-expr: num_files
    instances:
      files:
        type: fhm_file(_index)
        repeat: expr
        repeat-expr: num_files

  fhm_file:
    params:
      - id: index
        type: s4
    instances:
      offset:
        value: _parent.file_offsets[index]
      # this is to cater for the case where the size of the file is not given
      # the only way to know if the fhm section has ended is by calculating the size using the next offset
      # if the next offset is the end of the section, use the passed in size as the offset
      size:
        value: "_parent.file_sizes[index] == 0
          ? _parent.num_files != (index + 1)
            ? _parent.file_offsets[index + 1] - _parent.file_offsets[index]
            : _parent.section_size - _parent.file_offsets[index]
          : _parent.file_sizes[index]"
      asset_load_type:
        value: _parent.file_asset_load_types[index]
      unk_type:
        value: _parent.file_unk_types[index]
      body:
        type: file_body(size.as<u4>)
        pos: offset
        size: size.as<u4>

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
    0x1: image
    0x2: model
    0x3: unknown # occasionally found on nested Fhm, but not always
  unk_enum:
    0x0: unknown
  file_magic_enums:
    0x46484D20: fhm
