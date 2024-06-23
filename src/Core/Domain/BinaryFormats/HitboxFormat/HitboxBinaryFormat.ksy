meta:
  id: hitbox_binary_format
  endian: be
  file-extension: hitbox
seq:
  - id: file_magic
    type: u4
  - id: hitbox_hashes_pointer
    type: u4
  - id: hitbox_data_pointer
    type: u4
  - id: unk_12
    type: u4
  - id: property_count
    type: u4
    doc: |
      The number of property on each hitbox, usually 29
  - id: property_hashes
    type: u4
    enum: properties_enum
    repeat: expr
    repeat-expr: property_count
  - id: hitbox_count
    type: u4
instances:
  hitbox:
    type: hitbox_body(_index)
    repeat: expr
    repeat-expr: hitbox_count
types:
  hitbox_body:
    params:
      - id: index
        type: s4
    seq:
      - id: hash
        type: u4
    instances:
      pos:
        value: (0x10 + 0x4 + _parent.property_count * 4) + (0x4 + _parent.hitbox_count * 4) + (index * _parent.property_count * 4)
      properties:
        pos: pos
        type: hitbox_property(_index)
        repeat: expr
        repeat-expr: _parent.property_count
  hitbox_property:
    params:
      - id: index
        type: s4
    seq:
      - id: value
        type:
          switch-on: _parent._parent.property_hashes[index].as<u4>
          cases:
            0x33EF2CCB: u4
            0xA964CCA4: u4
            0x07431A19: u4
            0x0EDBFE57: u4
            0xD196FC95: u4
            0xD5B8EA1F: u4
            0x54058D5D: u4
            0x1A107AB7: f4
            0xFAE45595: u4
            0xBEDC2392: u4
            0xEE43A562: u4
            0x38BEA931: u4
            0xE392B8D6: u4
            0xA7C78487: u4
            0x7BE01C98: u4
            0x0408DD77: u4
            0x29941888: u4
            0xC47A5D38: u4
            0x502E9BAF: u4
            0x46CED294: u4
            0xBF000953: u4
            0x8B954576: u4
            0x57B2DD69: u4
            0xE252D228: u4
            0xC0EB5412: f4
            0xDDCB9D74: u4
            0x8823E502: u4
            0x4F1F46C1: u4
            0xB3419082: u4
            _: u4
    instances:
      name:
        value: _parent._parent.property_hashes[index]

enums:
  properties_enum:
    0x33EF2CCB: hitbox_type
    0xA964CCA4: damage
    0x07431A19: unk_8
    0x0EDBFE57: down_value_threshold
    0xD196FC95: yoruke_value_threshold
    0xD5B8EA1F: unk_20
    0x54058D5D: unk_24
    0x1A107AB7: damage_correction
    0xFAE45595: special_effect
    0xBEDC2392: hit_effect
    0xEE43A562: fly_direction_1
    0x38BEA931: fly_direction_2
    0xE392B8D6: fly_direction_3
    0xA7C78487: enemy_camera_shake_multiplier
    0x7BE01C98: player_camera_shake_multiplier
    0x0408DD77: unk_56
    0x29941888: knock_up_angle
    0xC47A5D38: knock_up_range
    0x502E9BAF: unk_68
    0x46CED294: multiple_hit_interval_frame
    0xBF000953: multiple_hit_count
    0x8B954576: enemy_stun_duration
    0x57B2DD69: player_stun_duration
    0xE252D228: hit_visual_effect
    0xC0EB5412: hit_visual_effect_size_multiplier
    0xDDCB9D74: hit_sound_effect_hash
    0x8823E502: unk_100
    0x4F1F46C1: friendly_damage_flag
    0xB3419082: unk_108