meta:
  id: list_info_binary_format
  endian: be
  file-extension: list
seq:
  - id: list_info_name_string_offset
    type: u4
    doc: |
      Name of the list info, will determine which list info schema to use
  - id: count
    type: u2
    doc: Number of info items in the list
  - id: unk_6
    contents: [ 0, 0 ]
    doc: Always 0 from observed patterns
instances:
  list_info_name:
    pos: list_info_name_string_offset
    terminator: 0
    type: str
    encoding: UTF-8
  body:
    repeat: expr
    repeat-expr: count
    type:
      switch-on: list_info_name
      cases:
        '"SCharacterList"': character_info
        '"SSeriesList"': series_info
types:
  character_info:
    doc: Playable characters (unit) metadata info
    seq:
      - id: unit_index
        type: u1
      - id: series_id
        type: u1
      - id: unk_2
        type: u2
        doc: Always double 0xFF from observed patterns
      - id: unit_id
        type: u4
      - id: release_string_offset
        type: u4
        doc: |
          Always after 'SCharacterList.' which is 'Release' in Japanese 'リリース'
      - id: f_string_offset
        type: u4
        doc: |
          Format: F_{{unit_id}}
      - id: f_out_string_offset
        type: u4
        doc: |
          Format: F_OUT_{{unit_id}}
      - id: p_string_offset
        type: u4
        doc: |
          Format: P_{{unit_id}}
      - id: unit_select_order_in_series
        type: u1
        doc: |
          Placement of unit's selection order in its series, starts from 0
      - id: arcade_small_sprite_index
        type: u1
        doc: |
          Placement of unit's arcade small select sprite texture in the 'ArcadeSelectSmallSprites' asset file
      - id: arcade_unit_name_sprite_index
        type: u1
        doc: |
          Placement of unit's arcade name select texture in the 'ArcadeSelectUnitNameSprites' asset file
      - id: unk_27
        type: u1
      - id: arcade_selection_costume_1_sprite_asset_hash
        type: u4
        doc: |
          Asset hash for arcade selection sprites
          Used when selecting unit in arcade mode
          Asset contains both unit and pilot sprites (costume 1)
      - id: arcade_selection_costume_2_sprite_asset_hash
        type: u4
        doc: |
          Asset hash for arcade selection sprites (optional)
          Used when selecting unit in arcade mode
          Asset contains both unit and pilot sprites (costume 2)
      - id: arcade_selection_costume_3_sprite_asset_hash
        type: u4
        doc: |
          Asset hash for arcade selection sprites (optional)
          Used when selecting unit in arcade mode
          Asset contains both unit and pilot sprites (costume 3)
      - id: loading_left_costume_1_sprite_asset_hash
        type: u4
        doc: |
          Asset hash for loading screen (left) sprites 
          Used during VS loading screen when the unit is on the left side
          Asset contains both unit and pilot sprites (costume 1)
      - id: loading_left_costume_2_sprite_asset_hash
        type: u4
        doc: |
          Asset hash for loading screen (left) sprites (optional)
          Used during VS loading screen when the unit is on the left side
          Asset contains both unit and pilot sprites (costume 2)
      - id: loading_left_costume_3_sprite_asset_hash
        type: u4
        doc: |
          Asset hash for loading screen (left) sprites (optional)
          Used during VS loading screen when the unit is on the left side
          Asset contains both unit and pilot sprite (costume 3)
      - id: loading_right_costume_1_sprite_asset_hash
        type: u4
        doc: |
          Asset hash for loading screen (right) sprites
          Used during VS loading screen when the unit is on the right side
          Asset contains both unit and pilot sprites (costume 1)
      - id: loading_right_costume_2_sprite_asset_hash
        type: u4
        doc: |
          Asset hash for loading screen (right) sprites (optional)
          Used during VS loading screen when the unit is on the right side
          Asset contains both unit and pilot sprites (costume 2)
      - id: loading_right_costume_3_sprite_asset_hash
        type: u4
        doc: |
          Asset hash for loading screen (right) sprites (optional)
          Used during VS loading screen when the unit is on the right side
          Asset contains both unit and pilot sprites (costume 3)
      - id: generic_selection_costume_1_sprite_asset_hash
        type: u4
        doc: |
          Asset hash for generic selection sprites
          Mostly used in vertical unit selection menu e.g. Free Battle / FB Missions
          Asset contains both unit and pilot sprite (costume 1)
      - id: generic_selection_costume_2_sprite_asset_hash
        type: u4
        doc: |
          Asset hash for generic selection sprites (optional)
          Mostly used in vertical unit selection menu e.g. Free Battle / FB Missions
          Asset contains both unit and pilot sprites (costume 2)
      - id: generic_selection_costume_3_sprite_asset_hash
        type: u4
        doc: |
          Asset hash for generic selection sprites (optional)
          Mostly used in vertical unit selection menu e.g. Free Battle / FB Missions
          Asset contains both unit and pilot sprites (costume 3)
      - id: loading_target_unit_sprite_asset_hash
        type: u4
        doc: |
          Asset hash for loading screen target unit sprite
          Mainly used in Arcade / CPU battles where this unit is the designated target, usually is the same sprite as loading right but bigger
          Asset only contains unit sprite
      - id: loading_target_pilot_costume_1_sprite_asset_hash
        type: u4
        doc: |
          Asset hash for loading screen pilot target pilot sprite
          Mainly used in Arcade / CPU battles where this unit is the designated target, usually is the same sprite as loading right but bigger
          Asset only contains pilot costume 1 sprite
      - id: loading_target_pilot_costume_2_sprite_asset_hash
        type: u4
        doc: |
          Asset hash for loading screen pilot target pilot sprite (optional)
          Mainly used in Arcade / CPU battles where this unit is the designated target, usually is the same sprite as loading right but bigger
          Asset only contains pilot costume 2 sprite
      - id: loading_target_pilot_costume_3_sprite_asset_hash
        type: u4
        doc: |
          Asset hash for loading screen pilot target pilot sprite (optional)
          Mainly used in Arcade / CPU battles where this unit is the designated target, usually is the same sprite as loading right but bigger
          Asset only contains pilot costume 3 sprite
      - id: in_game_sortie_and_awakening_pilot_costume_1_sprite_asset_hash
        type: u4
        doc: |
          Asset hash for in game sortie and awakening sprites
          Used during initial sortie bottom left pilot costume 1 speaking and awakening cut-in
          Asset contains two folders: 
            1. Bottom left pilot costume 1 speaking sprite with mouth piece frame sprites, alongside sprite placement / script file LMB
            2. Awakening cut-in pilot costume 1 sprite with background / effects etc, alongside sprite placement / script file LMB
      - id: in_game_sortie_and_awakening_pilot_costume_2_sprite_asset_hash
        type: u4
        doc: |
          Asset hash for in game sortie and awakening sprites (optional)
          Used during initial sortie bottom left pilot costume 2 speaking and awakening cut-in
          Asset contains two folders: 
            1. Bottom left pilot costume 2 speaking sprite with mouth piece frame sprites, alongside sprite placement / script file LMB
            2. Awakening cut-in pilot costume 2 sprite with background / effects etc, alongside sprite placement / script file LMB
      - id: in_game_sortie_and_awakening_pilot_costume_3_sprite_asset_hash
        type: u4
        doc: |
          Asset hash for in game sortie and awakening sprites (optional)
          Used during initial sortie bottom left pilot costume 3 speaking and awakening cut-in
          Asset contains two folders: 
            1. Bottom left pilot costume 3 speaking sprite with mouth piece frame sprites, alongside sprite placement / script file LMB
            2. Awakening cut-in pilot costume 3 sprite with background / effects etc, alongside sprite placement / script file LMB
      - id: sprite_frames_asset_hash
        type: u4
        doc: |
          Asset hash for sprite frame data, also known as KPKP format
          In game sortie sprite's mouth piece sprite "movement" is controlled by this file
      - id: result_small_unit_sprite_hash
        type: u4
        doc: |
          Asset hash for result screen sidebar scoreboard's unit sprite
          Asset only contains unit sprite
      - id: unk_112
        type: u1
        doc: Always single 0 from observed patterns
      - id: figurine_sprite_index
        type: u1
        doc: |
          Placement of unit's figure sprite texture in the 'FigurineSprites' asset file
      - id: unk_114
        type: u2
        doc: Always double 0xFF from observed patterns
      - id: figurine_sprite_asset_hash
        type: u4
        doc: |
          Asset hash for unit's standalone figurine sprite
          Unused / deprecated in game, the game respects the figurine index instead of this
      - id: loading_target_unit_small_sprite_asset_hash
        type: u4
        doc: |
          Asset hash for loading screen small target unit sprite
          More compact version of `loading_target_unit_sprite_asset_hash`, probably used in similar scenarios
          Asset only contains unit sprite
      - id: unk_124
        type: u4
      - id: unk_128
        type: u4
      - id: catalog_store_pilot_costume_2_sprite_asset_hash
        type: u4
        doc: |
          Asset hash for catalog store pilot costume 2 sprite (optional)
          Used as a preview on the online catalog store for users to purchase these
      - id: catalog_store_pilot_costume_2_t_string_offset
        type: u4
        doc: |
          Format: IS_COSTUME_{{costume_id}}_T
      - id: catalog_store_pilot_costume_2_string_offset
        type: u4
        doc: |
          Format: IS_COSTUME_{{costume_id}}
      - id: catalog_store_pilot_costume_3_sprite_asset_hash
        type: u4
        doc: |
          Asset hash for catalog store pilot costume 3 sprite (optional)
          Used as a preview on the online catalog store for users to purchase these
      - id: catalog_store_pilot_costume_3_t_string_offset
        type: u4
        doc: |
          Format: IS_COSTUME_{{costume_id}}_T
      - id: catalog_store_pilot_costume_3_string_offset
        type: u4
        doc: |
          Format: IS_COSTUME_{{costume_id}}
      - id: unk_156
        type: u4
    instances:
      release_string:
        pos: release_string_offset
        terminator: 0
        type: str
        encoding: UTF-8
      f_string:
        pos: f_string_offset
        terminator: 0
        type: str
        encoding: UTF-8
      f_out_string:
        pos: f_out_string_offset
        terminator: 0
        type: str
        encoding: UTF-8
      p_string:
        pos: p_string_offset
        terminator: 0
        type: str
        encoding: UTF-8
      catalog_store_pilot_costume_2_t_string:
        pos: catalog_store_pilot_costume_2_t_string_offset
        terminator: 0
        type: str
        encoding: UTF-8
      catalog_store_pilot_costume_2_string:
        pos: catalog_store_pilot_costume_2_string_offset
        terminator: 0
        type: str
        encoding: UTF-8
      catalog_store_pilot_costume_3_t_string:
        pos: catalog_store_pilot_costume_3_t_string_offset
        terminator: 0
        type: str
        encoding: UTF-8
      catalog_store_pilot_costume_3_string:
        pos: catalog_store_pilot_costume_3_string_offset
        terminator: 0
        type: str
        encoding: UTF-8
  series_info:
    doc: Series metadata info
    seq:
      - id: series_id
        type: u1
      - id: unk_2
        type: u1
        doc: Not sure what this is, but closely related to the series_id
      - id: unk_3
        type: u1
        doc: Always 0x80 from observed patterns
      - id: unk_4
        type: u1
        doc: Always 0xFF from observed patterns
      - id: release_string_offset
        type: u4
        doc: |
          Always after 'SSeriesList.' which is 'Release' in Japanese 'リリース'
      - id: select_order
        type: u1
        doc: |
          Placement of series's selection order, starts from 0
      - id: logo_sprite_index
        type: u1
        doc: |
          Placement of series's select sprite texture in the 'SeriesLogoSprites' asset file
      - id: logo_sprite_2_index
        type: u1
        doc: |
          Placement of series's select sprite texture in the 'SeriesLogoSprites2' asset file
      - id: unk_11
        type: u1
        doc: Always 0xFF from observed patterns
      - id: movie_asset_hash
        type: u4
        doc: |
          Asset hash for the series movie / pv
          Played after selection of unit in arcade mode
    instances:
      release_string:
        pos: release_string_offset
        terminator: 0
        type: str
        encoding: UTF-8