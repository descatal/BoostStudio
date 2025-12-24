meta:
  id: nud_binary_format
  file-extension: nud
seq:
  - id: magic
    size: 4
  - id: body
    type: nud_body
types:
  nud_body:
    meta:
      endian:
        switch-on: _root.magic
        cases:
          '[0x4E, 0x44, 0x57, 0x44]': le # NDWD
          _: be # NDP3
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
        repeat-expr: header.num_polysets
    types:
      header_data:
        seq:
          - id: length
            type: u4
            doc: Total length of the NUD, in bytes.
          - id: version
            type: u2
            doc: |
              Indicates the version of the NUD container format.
              For this specification, should be set to 2.
          - id: num_polysets
            type: u2
            doc: |
              Number of polysets
          - id: bone_index_start
            type: u2
            doc: The lowest bone index referenced by the objects and vertices in this NUD.
          - id: bone_index_end
            type: u2
            doc: The highest bone index referenced by the objects and vertices in this NUD.
          - id: toc
            type: u4
            repeat: expr
            repeat-expr: 4
        instances:
          section_1_pointer:
            value:  0x30 + toc[0]
          section_2_pointer:
            value: section_1_pointer + toc[1]
          section_3_pointer:
            value: section_2_pointer + toc[2]
          section_4_pointer:
            value: section_3_pointer + toc[3]
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
          - id: single_bind_bone_index
            type: s2
          - id: num_polygons
            type: u2
          - id: position_b
            type: u4
          - id: polygons
            type: polygon_data
            repeat: expr
            repeat-expr: num_polygons
        instances:
          name:
            type: str
            terminator: 0
            encoding: UTF-8
            pos: name_offset + _parent.header.section_4_pointer
      polygon_data:
        seq:
          - id: vertex_index_offset
            type: u4
          - id: vertex_color_uv_offset
            type: u4
          - id: vertex_geometry_offset
            type: u4
          - id: num_vertices
            type: u2
          - id: flags
            type: vertex_flag
          - id: material_offsets
            type: u4
            repeat: expr
            repeat-expr: 4
          - id: num_vertex_indices
            type: u2
          - id: polygon_size
            type: u1
          - id: polygon_flag
            type: u1
          - id: padding
            size: '(_io.pos % 0x10) == 0 ? 0 : 0x10 - _io.pos % 0x10'
        instances:
          vertex_indices:
            type: u2
            pos: vertex_index_offset + _parent._parent.header.section_1_pointer
            repeat: expr
            repeat-expr: num_vertex_indices
          vertex_color_uv_raw:
            type: vertex_color_uv(flags)
            pos: vertex_color_uv_offset + _parent._parent.header.section_1_pointer
            repeat: expr
            repeat-expr: num_vertices
            if: 'flags.bone_type != vertex_bone_type::no_bone'
          vertices_raw:
            type:
              switch-on: flags.bone_type
              cases:
                'vertex_bone_type::no_bone': vertex_attributes_no_bone(flags)
                _: vertex_attributes_with_bone(flags)
            pos: 'vertex_geometry_offset + (flags.bone_type == vertex_bone_type::no_bone ? _parent._parent.header.section_2_pointer : _parent._parent.header.section_3_pointer)'
            repeat: expr
            repeat-expr: num_vertices
          vertices:
            doc:
              Single source of truth for all the vertices data
            type: vertex_attributes(_index, flags)
            repeat: expr
            repeat-expr: num_vertices
          materials:
            type: material_data(_index)
            repeat: expr
            repeat-expr: 4
      vertex_flag:
        seq:
          - id: vertex_flags
            type: u1
          - id: uv_flags
            type: u1
        instances:
          bone_type:
            doc: |
              Extracts upper 4 bits
            value: (vertex_flags & 0xF0).as<u4>
            enum: vertex_bone_type
          geometry_type:
            doc: |
              Extracts lower 4 bits
            value: (vertex_flags & 0xF).as<u4>
            enum: vertex_geometry_type
          uv_count:
            value: uv_flags >> 4
          color_type:
            value: (uv_flags & 0xE).as<u4>
            enum: vertex_color_type
          uv_type:
            value: (uv_flags & 0x1).as<u4>
            enum: vertex_uv_type
      vertex_attributes_with_bone:
        params:
          - id: flags
            type: vertex_flag
        seq:
          - id: geometry
            type: vertex_geometry(flags)
          - id: bone_indices
            type:
              switch-on: flags.bone_type
              cases:
                'vertex_bone_type::float': s4
                'vertex_bone_type::half_float': s2
                'vertex_bone_type::byte': b1
                _: s4
            repeat: expr
            repeat-expr: 4
          - id: bone_weights
            type:
              switch-on: flags.bone_type
              cases:
                'vertex_bone_type::float': f4
                'vertex_bone_type::half_float': f2
                'vertex_bone_type::byte': b1
                _: f4
            repeat: expr
            repeat-expr: 4
        instances:
          bindings:
            type: vertex_binding(bone_indices[_index].as<s4>, bone_weights[_index].as<f4>)
            repeat: expr
            repeat-expr: 4
      vertex_attributes_no_bone:
        params:
          - id: flags
            type: vertex_flag
        seq:
          - id: geometry
            type: vertex_geometry(flags)
          - id: color_uv
            type: vertex_color_uv(flags)
        instances:
          bindings:
            type: vertex_binding(_parent._parent.single_bind_bone_index.as<s4>, 1.0.as<f4>)
            repeat: expr
            repeat-expr: 1
      vertex_attributes:
        params:
          - id: index
            type: s4
          - id: flags
            type: vertex_flag
        instances:
          geometry:
            value: 'flags.bone_type == vertex_bone_type::no_bone ? _parent.vertices_raw[index].as<vertex_attributes_no_bone>.geometry : _parent.vertices_raw[index].as<vertex_attributes_with_bone>.geometry'
          color_uv:
            value: 'flags.bone_type == vertex_bone_type::no_bone ? _parent.vertices_raw[index].as<vertex_attributes_no_bone>.color_uv : _parent.vertex_color_uv_raw[index]'
          bindings:
            value: 'flags.bone_type == vertex_bone_type::no_bone ? _parent.vertices_raw[index].as<vertex_attributes_no_bone>.bindings : _parent.vertices_raw[index].as<vertex_attributes_with_bone>.bindings'
      vertex_color_uv:
        params:
          - id: flags
            type: vertex_flag
        seq:
          - id: color
            type:
              switch-on: flags.color_type
              cases:
                'vertex_color_type::half_float': vector_4_half_float
                _: vector_4_byte
            if: 'flags.color_type != vertex_color_type::none'
          - id: uv
            type:
              switch-on: flags.uv_type
              cases:
                'vertex_uv_type::float': vector_2
                _: vector_2_half_float
            repeat: expr
            repeat-expr: flags.uv_count
      vertex_geometry:
        params:
          - id: flags
            type: vertex_flag
        seq:
          - id: position
            type: vector_3
          - id: unk_pos
            type: f4
            doc: |
              The mysterious n1, only appear in non-half float scenarios
            if: 'flags.geometry_type != vertex_geometry_type::normals_half_float and flags.geometry_type != vertex_geometry_type::normals_tan_bitan_half_float'
          - id: normal
            type:
              switch-on: flags.geometry_type
              cases:
                'vertex_geometry_type::normals_half_float': vector_3_half_float
                'vertex_geometry_type::normals_tan_bitan_half_float': vector_3_half_float
                _: vector_3
            if: 'flags.geometry_type != vertex_geometry_type::no_normals'
          - id: unk_normal
            type:
              switch-on: flags.geometry_type
              cases:
                'vertex_geometry_type::normals_half_float': f2
                'vertex_geometry_type::normals_tan_bitan_half_float': f2
                _: f4
            doc: |
              The mysterious n1, but for normal, it follows the size of the normal
            if: 'flags.geometry_type != vertex_geometry_type::no_normals'
          - id: bitangent
            type:
              switch-on: flags.geometry_type
              cases:
                'vertex_geometry_type::normals_tan_bitan_half_float': vector_4_half_float
                _: vector_4
            if: 'flags.geometry_type == vertex_geometry_type::normals_tan_bitan_float or flags.geometry_type == vertex_geometry_type::normals_tan_bitan_half_float'
          - id: tangent
            type:
              switch-on: flags.geometry_type
              cases:
                'vertex_geometry_type::normals_tan_bitan_half_float': vector_4_half_float
                _: vector_4
            if: 'flags.geometry_type == vertex_geometry_type::normals_tan_bitan_float or flags.geometry_type == vertex_geometry_type::normals_tan_bitan_half_float'
      vertex_binding:
        params:
          - id: index
            type: s4
          - id: weight
            type: f4
        instances:
          bone_index:
            value: index.as<u4>
          bone_weight:
            value: weight.as<f4>
      vector_3:
        seq:
          - id: x
            type: f4
          - id: y
            type: f4
          - id: z
            type: f4
      vector_3_half_float:
        seq:
          - id: x
            type: f2
          - id: y
            type: f2
          - id: z
            type: f2
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
      vector_4_half_float:
        seq:
          - id: x
            type: f2
          - id: y
            type: f2
          - id: z
            type: f2
          - id: w
            type: f2
      vector_2:
        seq:
          - id: x
            type: f4
          - id: y
            type: f4
      vector_2_half_float:
        seq:
          - id: x
            type: f2
          - id: y
            type: f2
      vector_4_byte:
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
          - id: index
            type: s4
        instances:
          body:
            pos: _parent.material_offsets[index]
            type: material_instance
            if: _parent.material_offsets[index] != 0
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
          - id: destination_factor
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
      f2:
        seq:
          - id: value_raw
            type: u2
        instances:
          value:
            doc: |
              Taken from https://github.com/kaitai-io/kaitai_struct/issues/1013
              Probably the best case scenario, user need to manually convert the hex value to float
              The value here is in hexadecimal f4
            value: ((value_raw & 0x8000) << 16) | (((value_raw & 0x7c00) + 0x1c000) << 13) | ((value_raw & 0x03ff) << 13)
enums:
  vertex_bone_type:
    0x0: no_bone
    0x1: float
    0x2: half_float
    0x4: byte
  vertex_geometry_type:
    0x0: no_normals
    0x1: normals_float
    0x3: normals_tan_bitan_float
    0x6: normals_half_float
    0x7: normals_tan_bitan_half_float
  vertex_uv_type:
    0x0: half_float
    0x1: float
  vertex_color_type:
    0x0: none
    0x2: byte
    0x4: half_float
    