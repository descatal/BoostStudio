meta:
  id: tbl
  endian: be
  file-extension: tbl
params:
  - id: total_file_size
    type: u2
seq:
  - id: file_magic
    contents: [ 0x54, 0x42, 0x4C, 0x20 ]
  - id: flag_1
    # not sure what these are, for now put validation first
    contents: [ 0x01, 0x01 ]
  - id: flag_2
    # not sure what these are, for now put validation first
    contents: [ 0x00, 0x00 ]
  - id: file_path_count
    type: s4
   # The total number of files combined with previous tbl
  - id: cumulative_file_count
    type: u4
  - id: file_path_offsets
    type: file_path_offset_body
    repeat: expr
    repeat-expr: file_path_count
  - id: file_info_offsets
    type: u4
    repeat: expr
    repeat-expr: cumulative_file_count
instances:
  file_paths:
    type: file_path_body(_index)
    repeat: expr
    repeat-expr: file_path_count
  file_infos:
    type: file_info_body(_index)
    repeat: expr
    repeat-expr: cumulative_file_count
types:
  file_path_offset_body:
    seq:
      - id: subfolder_flag
        type: u2
      - id: path_pointer
        type: u2
        
  file_path_body:
    params:
      - id: index
        type: s4
    instances:
      pointer:
        value: _parent.file_path_offsets[index].path_pointer
      size:
        value: '_parent.file_path_count == (index + 1) ? _parent.total_file_size - pointer : _parent.file_path_offsets[index + 1].path_pointer - pointer'
      path:
        type: str
        encoding: UTF-8
        terminator: 0
        pos: pointer
        size: size
        
  file_info_body:
    params:
      - id: index
        type: s4
    instances:
      offset:
        value: _parent.file_info_offsets[index]
      file_info:
        type: file_info
        pos: offset
        if: offset != 0
        
  file_info:
    seq:
      - id: patch_number
        type: u4
      - id: path_index
        type: s4
      - id: unk_8
        contents: [ 0x00, 0x04, 0x00, 0x00 ]
      - id: size_1
        type: u4
      - id: size_2
        type: u4
      - id: size_3
        type: u4
      - id: unk_28
        contents: [ 0x00, 0x00, 0x00, 0x00 ]
      - id: hash_name
        type: u4
    instances:
      path_body:
        value: _parent._parent.file_paths[path_index]
        if: (path_index == 0 and size_1 == 0 and size_2 == 0 and size_3 == 0) != true
        
