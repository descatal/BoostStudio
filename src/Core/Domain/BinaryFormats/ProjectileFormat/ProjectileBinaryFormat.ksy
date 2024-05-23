meta:
  id: projectile_binary_format
  endian: be
  file-extension: projectile
seq:
  - id: file_magic
    type: u4
  - id: unk_4
    contents: [ 0x67, 0xF0, 0x19, 0xCE ]
  - id: unit_id
    type: s4
  - id: data_pointer
    type: s4
  - id: projectile_count
    type: s4
instances:
  # Fixed property count for projectile: 70
  property_count:
    value: 70
  projectile:
    type: projectile_body(_index, property_count, projectile_count)
    repeat: expr
    repeat-expr: projectile_count
types:
  projectile_body:
    params:
      - id: index
        type: s4
      - id: projectile_count
        type: s4
    seq:
      - id: hash
        type: u4
    instances:
      offset:
        # For some reason property_count needs to - 1
        value: (projectile_count * 4) + (index * (property_count - 1) * 4) + 0x14
      projectile_properties:
        pos: offset
        type: projectile_properties_body
  
  projectile_properties_body:
    seq:
      - id: projectile_type
        type: u4
      - id: hit_hash
        type: u4
      - id: model_hash
        type: u4
      - id: skeleton_index
        type: u4
      - id: aim_type
        type: u4
      - id: translate_y
        type: u4
      - id: translate_z
        type: u4
      - id: translate_x
        type: u4
      - id: rotate_x
        type: u4
      - id: rotate_z
        type: u4
      - id: cosmetic_hash
        type: u4
      - id: unk_44
        type: u4
      - id: unk_48
        type: u4
      - id: unk_52
        type: u4
      - id: unk_56
        type: u4
      - id: ammo_consumption
        type: u4
      - id: duration_frame
        type: u4
      - id: max_travel_distance
        type: u4
      # finish this