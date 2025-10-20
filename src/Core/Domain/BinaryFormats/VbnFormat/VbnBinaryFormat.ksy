meta:
  id: vbn_binary_format
  file-extension: vbn
  endian: be
seq:
  - id: header
    type: header_data
  - id: bones
    type: bone_data(_index)
    repeat: expr
    repeat-expr: header.num_bones
  - id: local_transforms
    type: local_transform_data
    repeat: expr
    repeat-expr: header.num_bones
  - id: padding
    size: -_io.pos % 8
  - id: inverse_bind_matrices
    type: matrix_4x4
    repeat: expr
    repeat-expr: header.num_bones
  - id: bind_matrices
    type: matrix_4x4
    repeat: expr
    repeat-expr: header.num_bones
types:
  header_data:
    seq:
      - id: magic
        contents: [VBN, 0x20]
      - id: version
        type: u2
      - id: unk_6
        type: u2
      - id: flags
        type: u4
        doc: |
          0x1 for main body
          0x190 for T-Pose for guns and other parts
          0x191 is also observed
      - id: num_bones
        type: u4
      - id: num_animation_bones
        type: u4
        doc: |
          Number of main bones (type 0) that'll be used in animations
      - id: num_attachment_bones
        type: u4
        doc: |
          Number of attachment bones (type 1) that are not used in animations
      - id: alignment
        size: 8
  bone_data:
    params:
      - id: i
        type: s4
    seq:
      - id: name
        type: str
        size: 16
        encoding: UTF-8
      - id: type
        type: u4
      - id: parent_bone_index
        type: s4
    instances:
      local_transform:
        value: _parent.local_transforms[i]
      inverse_bind_matrix:
        value: _parent.inverse_bind_matrices[i]
      bind_matrix:
        value: _parent.bind_matrices[i]
      parent_bone:
        value: _parent.bones[parent_bone_index]
  local_transform_data:
    seq:
      - id: translation
        type: vector_3
      - id: rotation
        type: vector_3
      - id: scale
        type: vector_3
  matrix_4x4:
    seq:
      - id: row0
        type: vector_4
      - id: row1
        type: vector_4
      - id: row2
        type: vector_4
      - id: row3
        type: vector_4
  vector_3:
    seq:
      - id: x
        type: f4
      - id: y
        type: f4
      - id: z
        type: f4
  vector_4:
    seq:
      - id: x
        type: f4
      - id: y
        type: f4
      - id: z
        type: f4
      - id: w
        type: f4