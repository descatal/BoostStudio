meta:
  id: ndp3_binary_format
  file-extension: ndp3
  endian: be
seq:
  - id: header
    type: header_data
  - id: bounding_sphere_1
    type: bounding_sphere
    doc: |
      Bounding sphere information (Render?)
  - id: bounding_sphere_2
    type: bounding_sphere
    doc: |
      Bounding sphere information (Physics?)
  - id: bounding_sphere_3
    type: bounding_sphere
    doc: |
      Bounding sphere information (Distance?)
  - id: meshes
    type: mesh_data
    repeat: expr
    repeat-expr: header.polysets_count

types:
  header_data:
    seq:
      - id: magic
        contents: NDP3
      - id: length
        type: u4
        doc: Total length of the Binary ndp3, including Header and all Chunks, in bytes.
      - id: version
        type: u2
        doc: |
          Indicates the version of the Binary ndp3 container format.
          For this specification, should be set to 2.
      - id: polysets_count
        type: u2
        doc: |
          Number of polysets
      - id: bone_index_start
        type: u2
        doc: The lowest bone index referenced by the objects and vertices in this ndp3.
      - id: bone_index_end
        type: u2
        doc: The highest bone index referenced by the objects and vertices in this ndp3.
      - id: polyclump_start
        type: u4
      - id: polyclump_size
        type: u4
      - id: vertclump_size
        type: u4
      - id: vertaddclump_size
        type: u4
    instances:
      polyclump_pointer:
        value:  0x30 + polyclump_start
      vertclump_pointer:
        value: polyclump_pointer + polyclump_size
      vertaddclump_pointer:
        value: vertclump_pointer + vertclump_size
      nameclump_pointer:
        value: vertaddclump_pointer + vertaddclump_size
  bounding_sphere:
    seq:
      - id: center_x
        type: f4
      - id: center_y
        type: f4
      - id: center_z
        type: f4
      - id: radius
        type: f4
  mesh_data:
    seq:
      - id: name_offset
        type: u4
      - id: unk_6
        type: u2
      - id: bone_flag
        type: s2
      - id: single_bound_flag
        type: s2
      - id: num_polygons
        type: u2
      - id: position_b
        type: u4
      - id: polygons
        type: poly_data
        repeat: expr
        repeat-expr: num_polygons
  poly_data:
    seq:
      - id: poly_start
        type: u4
      - id: vert_start
        type: u4
      - id: vert_add_start
        type: u4
      - id: num_vertices
        type: u2
      - id: vert_size
        type: u1
      - id: uv_size
        type: u1
      - id: text_props
        type: u4
        repeat: expr
        repeat-expr: 4
      - id: num_vertex_indices
        type: u2
      - id: poly_size
        type: u1
      - id: poly_flag
        type: u1
      - id: align
        size: 0xC
      - id: vertices
        type: vertex_data
        repeat: expr
        repeat-expr: num_vertices
    instances:
      materials:
        type: material_data(_index)
        repeat: expr
        repeat-expr: 4
      vertex_indices:
        type: u2
        pos: _parent._parent.header.polyclump_pointer
        repeat: expr
        repeat-expr: num_vertex_indices
  vertex_data:
    instances:
      uv:
        type: uv_data
        pos: _parent._parent._parent.header.vertclump_pointer
        repeat: expr
        repeat-expr: _parent.uv_size >> 4
      vertex:
        type: pos_data
        pos: _parent._parent._parent.header.vertaddclump_pointer
  uv_data:
    seq:
      - id: color
        type: vector_4
      - id: uv
        type: vector_2
  pos_data:
    seq:
      - id: pos
        type: float_pos
      - id: nrm
        type: float_pos
      - id: bitan
        type: float_pos
      - id: tan
        type: float_pos
      - id: bone
        type: bone_data
  bone_data:
    seq:
      - id: ids
        type: u4
        repeat: expr
        repeat-expr: 4
      - id: weights
        type: f4
        repeat: expr
        repeat-expr: 4
  float_pos:
    seq:
      - id: x
        type: f4
      - id: y
        type: f4
      - id: z
        type: f4
      - id: w
        type: f4
  vector_2:
    seq:
      - id: x
        type: u2
      - id: y
        type: u2
  vector_4:
    seq:
      - id: x
        type: u1
      - id: y
        type: u1
      - id: z
        type: u1
      - id: w
        type: u1
  material_data:
    params:
      - id: i
        type: s4
    instances:
      body:
        pos: _parent.text_props[i]
        type: material_instance
        if: _parent.text_props[i] != 0
  material_instance:
    seq:
      - id: flag
        type: u4
      - id: unk_4
        type: u4
      - id: src_factor
        type: u2
      - id: num_textures
        type: u2
      - id: dst_factor
        type: u2
      - id: alpha_test
        type: s1
      - id: alpha_function
        type: s1
      - id: ref_alpha
        type: u2
      - id: cull_mode
        type: u2
      - id: unk_20
        type: u4
      - id: unk_24
        type: u4
      - id: z_buffer_offset
        type: u4
      - id: textures
        type: texture_data
        repeat: expr
        repeat-expr: num_textures
  texture_data:
    seq:
      - id: hash
        type: u4
      - id: alignment
        size: 6
      - id: map_mode
        type: u2
      - id: wrap_mode_s
        type: s1
      - id: wrap_mode_t
        type: s1
      - id: min_filter
        type: s1
      - id: mag_filter
        type: s1
      - id: mip_detail
        type: s1
      - id: unk_49
        type: s1
      - id: unk_50
        type: u2
      - id: unk_52
        type: s4