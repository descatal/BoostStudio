meta:
  id: asset_files_binary_format
  endian: be
  file-extension: asset
seq:
  - id: files
    type: file_hashes
    repeat: eos
types:
  file_hashes:
    seq:
      - id: unit_id
        type: u4
      - id: dummy_hash
        type: u4
      - id: data_hash
        type: u4
      - id: model_hash
        type: u4
      - id: animation_hash
        type: u4
      - id: effects_hash
        type: u4
      - id: sound_effects_hash
        type: u4
      - id: in_game_pilot_voice_lines_hash
        type: u4
      - id: weapon_sprites_hash
        type: u4
      - id: in_game_cut_in_sprites_hash
        type: u4
      - id: sprite_frames_hash
        type: u4
      - id: voice_lines_metadata_hash
        type: u4
      - id: pilot_voice_lines_hash
        type: u4