meta:
  id: projectile_binary_format
  endian: be
  file-extension: projectile
seq:
  - id: file_magic
    type: u4
  # some kind of flag, either 0x9AEBA80A for FB or 0x67F019CE for MBON
  - id: unk_4
    type: u4
  - id: unit_id
    type: u4
  - id: data_pointer
    type: u4
  - id: projectile_count
    type: u4
instances:
  # Fixed property count for projectile: 70
  property_count:
    value: 70
  projectile:
    type: projectile_body(_index)
    repeat: expr
    repeat-expr: projectile_count
types:
  projectile_body:
    params:
      - id: index
        type: s4
    seq:
      - id: hash
        type: u4
    instances:
      offset:
        value: (_parent.projectile_count * 4) + (index * (_parent.property_count) * 4) + 0x14
      projectile_properties:
        pos: offset
        type: projectile_properties_body
  projectile_properties_body:
    seq:
      - id: projectile_type
        type: u4
      - id: hitbox_hash
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
        type: f4
      - id: initial_speed
        type: f4
      - id: acceleration
        type: f4
      - id: acceleration_start_frame
        type: u4
      - id: unk_84
        type: u4
      - id: max_speed
        type: f4
      - id: reserved_92
        type: f4
      - id: reserved_96
        type: f4
      - id: reserved_100
        type: f4
      - id: reserved_104
        type: f4
      - id: reserved_108
        type: f4
      - id: reserved_112
        type: f4
      - id: reserved_116
        type: u4
      - id: horizontal_guidance
        type: f4
      - id: horizontal_guidance_angle
        type: f4
      - id: vertical_guidance
        type: f4
      - id: vertical_guidance_angle
        type: f4
      - id: reserved_136
        type: u4
      - id: reserved_140
        type: u4
      - id: reserved_144
        type: f4
      - id: reserved_148
        type: f4
      - id: reserved_152
        type: f4
      - id: reserved_156
        type: f4
      - id: reserved_160
        type: f4
      - id: reserved_164
        type: f4
      - id: reserved_168
        type: u4
      - id: reserved_172
        type: f4
      - id: size
        type: f4
      - id: reserved_180
        type: u4
      - id: reserved_184
        type: u4
      - id: sound_effect_hash
        type: u4
      - id: reserved_192
        type: u4
      - id: reserved_196
        type: u4
      - id: chained_projectile_hash
        type: u4
      - id: reserved_204
        type: u4
      - id: reserved_208
        type: u4
      - id: reserved_212
        type: u4
      - id: reserved_216
        type: u4
      - id: reserved_220
        type: f4
      - id: reserved_224
        type: f4
      - id: reserved_228
        type: f4
      - id: reserved_232
        type: f4
      - id: reserved_236
        type: f4
      - id: reserved_240
        type: f4
      - id: reserved_244
        type: f4
      - id: reserved_248
        type: f4
      - id: reserved_252
        type: f4
      - id: reserved_256
        type: f4
      - id: reserved_260
        type: f4
      - id: reserved_264
        type: f4
      - id: reserved_268
        type: f4
      - id: reserved_272
        type: f4
      - id: reserved_276
        type: f4