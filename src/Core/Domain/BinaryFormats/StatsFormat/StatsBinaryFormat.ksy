meta:
  id: stats_binary_format
  endian: be
  file-extension: stats
seq:
  # The magic is dynamic
  - id: magic
    type: u4
  - id: unit_id
    type: u4
  - id: unk_8
    contents: [ 0x1D, 0x77, 0x26, 0xBC ]
  # Usually is zero, but repurposed as the index slot
  - id: ammo_slot_5
    type: ammo_info_body(0x30)
    doc: |
      Usually is zero, but repurposed as the initial spawn fifth index slot.
      Since the design of ammo_info_body is to be parsed after the ammo_hash_chunk_pointer is read
      Make a special case for this and directly put the pointer, which is usually 0x30
  - id: ammo_hash_chunk_pointer
    type: s4
    doc: |
      Pointer to the start of the ammo hash chunk, which contains all of the ammo hashes to be loaded by this unit
  - id: property_metadata_chunk_pointer
    type: s4
    doc: |
      Pointer to the start of property chunk, which contains the property name hash and the data type
  - id: set_info_chunk_pointer
    type: s4
    doc: |
      Pointer to the start of set info chunk, which indicates the number of sets and the ordering index
  - id: value_chunk_pointer
    type: s4
    doc: |
      Pointer to the values chunk, which contains one value per property per set
  - id: ammo_slot_1
    type: ammo_info_body(ammo_hash_chunk_pointer)
    doc: |
      The initial spawn first ammo slot
  - id: ammo_slot_2
    type: ammo_info_body(ammo_hash_chunk_pointer)
    doc: |
      The initial spawn second ammo slot
  - id: ammo_slot_3
    type: ammo_info_body(ammo_hash_chunk_pointer)
    doc: |
      The initial spawn third ammo slot
  - id: ammo_slot_4
    type: ammo_info_body(ammo_hash_chunk_pointer)
    doc: |
      The initial spawn fourth ammo slot
instances:
  ammo_hashes:
    pos: ammo_hash_chunk_pointer
    type: ammo_hashes_body
    doc: |
      All ammo hashes that's loaded by this unit
  property_count:
    pos: property_metadata_chunk_pointer
    type: s4
    doc: |
      The number of property on each set
  set_count:
    pos: set_info_chunk_pointer
    type: s4
    doc: |
      The number of sets for this unit
  sets:
    pos: value_chunk_pointer
    type: set_body(_index, property_count, property_metadata_chunk_pointer, value_chunk_pointer)
    repeat: expr
    repeat-expr: set_count
    doc: |
      List of stats set
types:
  ammo_info_body:
    params:
      - id: ammo_hash_chunk_pointer
        type: s4
    seq:
      - id: slot_index
        type: s4
    instances:
      ammo_hash:
        pos: (ammo_hash_chunk_pointer + 0x4) + (slot_index * 4)
        type: u4
        if: slot_index != -1
  ammo_hashes_body:
    seq:
      - id: count
        type: u4
      - id: hashes
        type: u4
        repeat: expr
        repeat-expr: count
  set_body:
    params:
      - id: set_index
        type: s4
      - id: property_count
        type: s4
      - id: property_metadata_chunk_pointer
        type: s4
      - id: value_chunk_pointer
        type: s4
    seq:
      - id: stats
        type: stats_body(_index, set_index, property_count, property_metadata_chunk_pointer, value_chunk_pointer)
        repeat: expr
        repeat-expr: property_count
  stats_body:
    params:
      - id: index
        type: s4
      - id: set_index
        type: s4
      - id: property_count
        type: s4
      - id: property_metadata_chunk_pointer
        type: s4
      - id: value_chunk_pointer
        type: s4
    instances:
      property_type:
        pos: (property_metadata_chunk_pointer + 4) + (property_count * 4) + (index * 4)
        type: s4
        enum: property_type_enum
      property_name:
        pos: (property_metadata_chunk_pointer + 4) + (index * 4)
        type: u4
        enum: properties_enum
      property_value:
        pos: (value_chunk_pointer) + (set_index * property_count) + (index * 4)
        type:
          switch-on: property_type
          cases:
            "property_type_enum::float": f4
            "property_type_enum::integer": s4
            "_": s4

enums:
  property_type_enum:
    0x0: float
    0x3: integer
    0x6: unknown_6
  properties_enum:
    0xA604B3A9: unit_cost
    0x286D9774: unit_cost_2
    0x1C752781: max_hp
    0xD46754E5: down_value_threshold
    0x69C8066F: yoruke_value_threshold
    0xC6660AA0: unk_20
    0x3B5D4B23: unk_24
    0x2BFDB9D0: unk_28
    0xC6CEDEC1: max_boost
    0xAE74C5D3: unk_36
    0xC34600F0: unk_40
    0xA5AD4BD2: unk_44
    0xAD72831A: gravity_multiplier_air
    0xDBBEFF25: gravity_multiplier_land
    0x6B46FBE2: unk_56
    0xEF5DB298: unk_60
    0xBC682CAB: 
      id: unk_64
      doc: It was noted as curve beam speed, but not 100% sure what it is  
    0xBB752069: 
      id: unk_68
      doc: Also related to curve beam speed above
    0x4EA45799:
      id: unk_72
      doc: Related to camera
    0x7FF1DD24:
      id: unk_76
      doc: Related to camera on Y axis
    0xE7299A80: unk_80
    0xD80ABFAC: camera_zoom_multiplier
    0xC8039DD2: unk_88
    0xC3540521: unk_92
    0x8D29490D: unk_96
    0xB24DDB35: unk_100
    0xAB56EA74: unk_104
    0x807BB9B7: unk_108
    0x25C84010: size_multiplier
    0x1EFF2B0E: unk_116
    0x63C85750: 
      id: unk_120
      doc: Related to landing touch down, set it to 0 will never land
    0x548E6151: unk_124
    0x4D955010: unk_128
    0x66B803D3: unk_132
    0xBA70CE78: unk_136
    0x915D9DBB: unk_140
    0x8846ACFA: unk_144
    0xB9B71147: unk_148
    0x929A4284: unk_152
    0x8B8173C5: unk_156
    0x72EBC2E2: unk_160
    0x59C69121: unk_164
    0x40DDA060: unk_168
    0xF47FB04C: unk_172
    0xDF52E38F: unk_176
    0xC649D2CE: unk_180
    0x2F29B476: unk_184
    0xD94F31E8: red_lock_range_melee
    0x526F7318: red_lock_range
    0x1837F91C: 
      id: unk_196
      doc: Related to red lock range
    0xC11B6424: 
      id: unk_200
      doc: Related to red lock range
    0xA858CE56: unk_204
    0xFF4EA025: unk_208
    0xF22D3F5D: boost_replenish
    0xA53B512E: unk_216
    0x0220CBAD:
      id: boost_initial_consumption
      doc: Applicable to boost dash or fly up (one time)
    0x3166F669: boost_fuwa_initial_consumption
    0x3564F0FA: boost_fly_consumption
    0x4CE31F3E: boost_ground_step_initial_consumption
    0xFEC14975: boost_ground_step_consumption
    0x862DB789: boost_air_step_initial_consumption
    0x82C34A83: boost_air_step_consumption
    0xB61EC986: boost_bd_initial_consumption
    0x4673A81F: boost_bd_consumption
    0x1D6DFDCC: unk_256
    0xCCA0C1F0: unk_260
    0x48EC863D: unk_264
    0x294108DC: unk_268
    0x64754BDA: boost_transform_initial_consumption
    0x3C79E7B6: boost_transform_consumption
    0xD73777F8: boost_non_vernier_action_consumption
    0x3E18EEDE: boost_post_action_consumption
    0xA449D488: boost_rainbow_step_initial_consumption
    0x15D48179: 
      id: unk_292
      doc: Related to boost consumption
    0xAEDA0E89:
      id: unk_296
      doc: Related to boost consumption 
    0x2CB1FF0F:
      id: unk_300
      doc: Related to boost consumption
    0x4A2EE4C8:
      id: unk_304
      doc: Related to boost consumption
    0x6103B70B:
      id: unk_308
      doc: Related to boost consumption
    0x7818864A: unk_312
    0x3759108D: unk_316
    0x2E4221CC: unk_320
    0x056F720F: unk_324
    0x1C74434E: unk_328
    0x9BEC5F81: unk_332
    0x83A09DD3: assault_burst_red_lock_melee
    0x10EFE695: assault_burst_red_lock
    0xBFD7B8DB: assault_burst_damage_dealt_multiplier
    0x86302075: assault_burst_damage_taken_multiplier
    0xE48ECC9A: assault_burst_mobility_multiplier
    0x0197DF49: assault_burst_down_value_dealt_multiplier
    0xB2AC12DA: assault_burst_boost_consumption_multiplier
    0x801145CC:
      id: unk_364
      doc: Related to assault burst
    0xCE8EA6A0:
      id: unk_368
      doc: Related to assault burst
    0xF73F72CF: assault_burst_damage_dealt_burst_gauge_increase_multiplier
    0x85B796C1: assault_burst_damage_taken_burst_gauge_increase_multiplier
    0xF23C5041:
      id: unk_380
      doc: Related to assault burst
    0x09FA2526:
      id: unk_384
      doc: Related to assault burst
    0x6B108DAF:
      id: unk_388
      doc: Related to assault burst
    0xD18006B1:
      id: unk_392
      doc: Related to assault burst
    0x92DD9C5C:
      id: unk_396
      doc: Related to assault burst
    0xA88DCE10: blast_burst_red_lock_melee
    0x3BC2B556: blast_burst_red_lock
    0x86AF159B: blast_burst_damage_dealt_multiplier
    0xC291056D: blast_burst_damage_taken_multiplier
    0xA02FE982: blast_burst_mobility_multiplier
    0x38EF7209: blast_burst_down_value_dealt_multiplier
    0xEAB2BBF2: blast_burst_boost_consumption_multiplier
    0x7246A834:
      id: unk_428
      doc: Related to blast burst
    0x96900F88: 
      id: unk_432
      doc: Related to blast burst
    0x93DF0931: blast_burst_damage_dealt_burst_gauge_increase_multiplier
    0xBD8C1DB2: blast_burst_damage_taken_burst_gauge_increase_multiplier
    0xCB44FD01: 
      id: unk_444
      doc: Related to blast burst
    0x30828866: 
      id: unk_448
      doc: Related to blast burst
    0xC4B9C065: 
      id: unk_452
      doc: Related to blast burst
    0xE8F8ABF1: 
      id: unk_456
      doc: Related to blast burst
    0x3D74D196: 
      id: unk_460
      doc: Related to blast burst
    0xB196FF51: third_burst_red_lock_melee
    0x22D98417: third_burst_red_lock
    0x9187715B: third_burst_damage_dealt_multiplier
    0xFEF1E665: third_burst_damage_taken_multiplier
    0x9C4F0A8A: third_burst_mobility_multiplier
    0x2FC716C9: third_burst_down_value_dealt_multiplier
    0x6B97DED5: third_burst_boost_consumption_multiplier
    0x955B0EA3: 
      id: unk_492
      doc: Related to third burst
    0x17B56AAF: 
      id: unk_496
      doc: Related to third burst
    0x06AFDDA4: third_burst_damage_dealt_burst_gauge_increase_multiplier
    0x1CB5995C: third_burst_damage_taken_burst_gauge_increase_multiplier
    0xDC6C99C1: 
      id: unk_508
      doc: Related to third burst
    0x27AAECA6: 
      id: unk_512
      doc: Related to third burst
    0xA1DEFB23: 
      id: unk_516
      doc: Related to third burst
    0xFFD0CF31: 
      id: unk_520
      doc: Related to third burst
    0x5813EAD0: 
      id: unk_524
      doc: Related to third burst
    0x9872A871: fourth_burst_red_lock_melee
    0xF9867B09: fourth_burst_red_lock
    0x10E5CA5B: fourth_burst_damage_dealt_multiplier
    0x1539CA4C: fourth_burst_damage_taken_multiplier
    0x778726A3: fourth_burst_mobility_multiplier
    0xAEA5ADC9: fourth_burst_down_value_dealt_multiplier
    0x372435B3: fourth_burst_boost_consumption_multiplier
    0x9124A5C1: 
      id: unk_572
      doc: Related to fourth burst
    0x4B0681C9: 
      id: unk_576
      doc: Related to fourth burst
    0xAECA6071: fourth_burst_damage_dealt_burst_gauge_increase_multiplier
    0x6D82D38C: fourth_burst_damage_taken_burst_gauge_increase_multiplier
    0xA540F511: 
      id: unk_588
      doc: Related to fourth burst
    0x5E868076: 
      id: unk_592
      doc: Related to fourth burst
    0x7A39B6B3: 
      id: unk_596
      doc: Related to fourth burst
    0x86FCA3E1: 
      id: unk_600
      doc: Related to fourth burst
    0x83F4A740: 
      id: unk_604
      doc: Related to fourth burst
    0xC74FAFAE: unk_608

# Footnote for future reference:
# The type enum is actually as follows:
# 0 - Float
# 3 - Integer
# 6 - Unknown
# Here will also list down all the known hashes, and their respective stats usage.
#    Hash    -  Type -   Name
# 0xA604B3A9 -   I   - unit_cost
# 0x286D9774 -   I   - unit_cost_2
# 0x1C752781 -   I   - max_hp
# 0xD46754E5 -   I   - down_value_threshold
# 0x69C8066F -   I   - yoruke_value_threshold
# 0xC6660AA0 -   I   - unk_20
# 0x3B5D4B23 -   I   - unk_24
# 0x2BFDB9D0 -   I   - unk_28
# 0xC6CEDEC1 -   I   - max_boost
# 0xAE74C5D3 -   F   - unk_36
# 0xC34600F0 -   F   - unk_40
# 0xA5AD4BD2 -   I   - unk_44
# 0xAD72831A -   F   - gravity_multiplier_air
# 0xDBBEFF25 -   F   - gravity_multiplier_land
# 0x6B46FBE2 -   F   - unk_56
# 0xEF5DB298 -   F   - unk_60
# 0xBC682CAB -   F   - unk_64
# 0xBB752069 -   F   - unk_68
# 0x4EA45799 -   F   - unk_72
# 0x7FF1DD24 -   F   - unk_76
# 0xE7299A80 -   F   - unk_80
# 0xD80ABFAC -   F   - camera_zoom_multiplier
# 0xC8039DD2 -   F   - unk_88
# 0xC3540521 -   F   - unk_92
# 0x8D29490D -   F   - unk_96
# 0xB24DDB35 -   F   - unk_100
# 0xAB56EA74 -   F   - unk_104
# 0x807BB9B7 -   F   - unk_108
# 0x25C84010 -   F   - size_multiplier
# 0x1EFF2B0E -   F   - unk_116
# 0x63C85750 -   F   - unk_120
# 0x548E6151 -   F   - unk_124
# 0x4D955010 -   F   - unk_128
# 0x66B803D3 -   F   - unk_132
# 0xBA70CE78 -   F   - unk_136
# 0x915D9DBB -   F   - unk_140
# 0x8846ACFA -   F   - unk_144
# 0xB9B71147 -   F   - unk_148
# 0x929A4284 -   F   - unk_152
# 0x8B8173C5 -   F   - unk_156
# 0x72EBC2E2 -   F   - unk_160
# 0x59C69121 -   F   - unk_164
# 0x40DDA060 -   F   - unk_168
# 0xF47FB04C -   F   - unk_172
# 0xDF52E38F -   F   - unk_176
# 0xC649D2CE -   F   - unk_180
# 0x2F29B476 -   F   - unk_184
# 0xD94F31E8 -   F   - red_lock_range_melee
# 0x526F7318 -   F   - red_lock_range
# 0x1837F91C -   F   - unk_196
# 0xC11B6424 -   F   - unk_200
# 0xA858CE56 -   I   - unk_204
# 0xFF4EA025 -   I   - unk_208
# 0xF22D3F5D -   I   - boost_replenish
# 0xA53B512E -   I   - unk_216
# 0x0220CBAD -   I   - boost_initial_consumption
# 0x3166F669 -   I   - boost_fuwa_initial_consumption
# 0x3564F0FA -   I   - boost_fly_consumption
# 0x4CE31F3E -   I   - boost_ground_step_initial_consumption
# 0xFEC14975 -   I   - boost_ground_step_consumption
# 0x862DB789 -   I   - boost_air_step_initial_consumption
# 0x82C34A83 -   I   - boost_air_step_consumption
# 0xB61EC986 -   I   - boost_bd_initial_consumption
# 0x4673A81F -   I   - boost_bd_consumption
# 0x1D6DFDCC -   I   - unk_256
# 0xCCA0C1F0 -   I   - unk_260
# 0x48EC863D -   I   - unk_264
# 0x294108DC -   I   - unk_268
# 0x64754BDA -   I   - boost_transform_initial_consumption
# 0x3C79E7B6 -   I   - boost_transform_consumption
# 0xD73777F8 -   I   - boost_non_vernier_action_consumption
# 0x3E18EEDE -   I   - boost_post_action_consumption
# 0xA449D488 -   I   - boost_rainbow_step_initial_consumption
# 0x15D48179 -   I   - unk_292
# 0xAEDA0E89 -   I   - unk_296
# 0x2CB1FF0F -   I   - unk_300
# 0x4A2EE4C8 -   I   - unk_304
# 0x6103B70B -   I   - unk_308
# 0x7818864A -   I   - unk_312
# 0x3759108D -   I   - unk_316
# 0x2E4221CC -   I   - unk_320
# 0x056F720F -   I   - unk_324
# 0x1C74434E -   I   - unk_328
# 0x9BEC5F81 -   I   - unk_332
# 0x83A09DD3 -   F   - assault_burst_red_lock_melee
# 0x10EFE695 -   F   - assault_burst_red_lock
# 0xBFD7B8DB -   F   - assault_burst_damage_dealt_multiplier
# 0x86302075 -   F   - assault_burst_damage_taken_multiplier
# 0xE48ECC9A -   F   - assault_burst_mobility_multiplier
# 0x0197DF49 -   F   - assault_burst_down_value_dealt_multiplier
# 0xB2AC12DA -   F   - assault_burst_boost_consumption_multiplier
# 0x801145CC -   I   - unk_364
# 0xCE8EA6A0 -   I   - unk_368
# 0xF73F72CF -   F   - assault_burst_damage_dealt_burst_gauge_increase_multiplier
# 0x85B796C1 -   F   - assault_burst_damage_taken_burst_gauge_increase_multiplier
# 0xF23C5041 -   I   - unk_380
# 0x09FA2526 -   F   - unk_384
# 0x6B108DAF -   F   - unk_388
# 0xD18006B1 -   F   - unk_392
# 0x92DD9C5C -   F   - unk_396
# 0xA88DCE10 -   F   - blast_burst_red_lock_melee
# 0x3BC2B556 -   F   - blast_burst_red_lock
# 0x86AF159B -   F   - blast_burst_damage_dealt_multiplier
# 0xC291056D -   F   - blast_burst_damage_taken_multiplier
# 0xA02FE982 -   F   - blast_burst_mobility_multiplier
# 0x38EF7209 -   F   - blast_burst_down_value_dealt_multiplier
# 0xEAB2BBF2 -   F   - blast_burst_boost_consumption_multiplier
# 0x7246A834 -   I   - unk_428
# 0x96900F88 -   I   - unk_432
# 0x93DF0931 -   F   - blast_burst_damage_dealt_burst_gauge_increase_multiplier
# 0xBD8C1DB2 -   F   - blast_burst_damage_taken_burst_gauge_increase_multiplier
# 0xCB44FD01 -   I   - unk_444
# 0x30828866 -   F   - unk_448
# 0xC4B9C065 -   F   - unk_452
# 0xE8F8ABF1 -   F   - unk_456
# 0x3D74D196 -   F   - unk_460
# 0xB196FF51 -   F   - third_burst_red_lock_melee
# 0x22D98417 -   F   - third_burst_red_lock
# 0x9187715B -   F   - third_burst_damage_dealt_multiplier
# 0xFEF1E665 -   F   - third_burst_damage_taken_multiplier
# 0x9C4F0A8A -   F   - third_burst_mobility_multiplier
# 0x2FC716C9 -   F   - third_burst_down_value_dealt_multiplier
# 0x6B97DED5 -   F   - third_burst_boost_consumption_multiplier
# 0x955B0EA3 -   F   - unk_492
# 0x17B56AAF -   I   - unk_496
# 0x06AFDDA4 -   F   - third_burst_damage_dealt_burst_gauge_increase_multiplier
# 0x1CB5995C -   F   - third_burst_damage_taken_burst_gauge_increase_multiplier
# 0xDC6C99C1 -   I   - unk_508
# 0x27AAECA6 -   F   - unk_512
# 0xA1DEFB23 -   F   - unk_516
# 0xFFD0CF31 -   F   - unk_520
# 0x5813EAD0 -   F   - unk_524
# 0x9872A871 -   F   - fourth_burst_red_lock_melee
# 0xF9867B09 -   F   - fourth_burst_red_lock
# 0x10E5CA5B -   F   - fourth_burst_damage_dealt_multiplier
# 0x1539CA4C -   F   - fourth_burst_damage_taken_multiplier
# 0x778726A3 -   F   - fourth_burst_mobility_multiplier
# 0xAEA5ADC9 -   F   - fourth_burst_down_value_dealt_multiplier
# 0x372435B3 -   F   - fourth_burst_boost_consumption_multiplier
# 0x9124A5C1 -   I   - unk_572
# 0x4B0681C9 -   I   - unk_576
# 0xAECA6071 -   F   - fourth_burst_damage_dealt_burst_gauge_increase_multiplier
# 0x6D82D38C -   F   - fourth_burst_damage_taken_burst_gauge_increase_multiplier
# 0xA540F511 -   I   - unk_588
# 0x5E868076 -   F   - unk_592
# 0x7A39B6B3 -   F   - unk_596
# 0x86FCA3E1 -   F   - unk_600
# 0x83F4A740 -   F   - unk_604
# 0xC74FAFAE -   U   - unk_608