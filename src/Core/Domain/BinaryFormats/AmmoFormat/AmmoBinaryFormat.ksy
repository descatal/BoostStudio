meta:
  id: ammo_binary_format
  endian: be
  file-extension: ammo
seq:
  - id: file_magic
    contents: [ 0x1D, 0x25, 0x8F, 0xF7 ]
  - id: property_count
    type: s4
  - id: unk_8
    type: u4
  - id: unk_12
    type: u4
  - id: ammo_count
    type: s4
instances:
  ammo:
    type: ammo_body(_index, property_count, ammo_count)
    repeat: expr
    repeat-expr: ammo_count
types:
  ammo_body:
    params:
      - id: index
        type: s4
      - id: property_count
        type: s4
      - id: ammo_count
        type: s4
    seq:
      - id: hash
        type: u4
    instances:
      offset:
        # For some reason property_count needs to - 1
        value: (ammo_count * 4) + (index * (property_count - 1) * 4) + 0x14
      ammo_properties:
        pos: offset
        type: ammo_properties_body
  
  ammo_properties_body:
    seq:
      - id: ammo_type
        type: u4
      - id: max_ammo
        type: u4
      - id: initial_ammo
        type: u4
      - id: timed_duration_frame
        type: u4
      - id: unk_16
        type: u4
      - id: reload_type
        type: u4
      - id: cooldown_duration_frame
        type: u4
      - id: reload_duration_frame
        type: u4
      - id: assault_burst_reload_duration_frame
        type: u4
      - id: blast_burst_reload_duration_frame
        type: u4
      - id: unk_40
        type: u4
      - id: unk_44
        type: u4
      - id: inactive_unk_48
        type: u4
      - id: inactive_cooldown_duration_frame
        type: u4
      - id: inactive_reload_duration_frame
        type: u4
      - id: inactive_assault_burst_reload_duration_frame
        type: u4
      - id: inactive_blast_burst_reload_duration_frame
        type: u4
      - id: inactive_unk_68
        type: u4
      - id: inactive_unk_72
        type: u4
      - id: burst_replenish
        type: u4
      - id: unk_80
        type: u4
      - id: unk_84
        type: u4
      - id: unk_88
        type: u4
      - id: charge_input
        type: u4
      - id: charge_duration_frame
        type: u4
      - id: assault_burst_charge_duration_frame
        type: u4
      - id: blast_burst_charge_duration_frame
        type: u4
      - id: unk_108
        type: u4
      - id: unk_112
        type: u4
      - id: release_charge_linger_duration_frame
        type: u4
      - id: max_charge_level
        type: u4
      - id: unk_124
        type: u4
      - id: unk_128
        type: u4