// mod window_listener;
// use window_listener::{get_listener, start_listening, stop_listening};

#[cfg_attr(mobile, tauri::mobile_entry_point)]
pub fn run() {
    tauri::Builder::default()
        .plugin(tauri_plugin_opener::init())
//         .invoke_handler(tauri::generate_handler![
//             start_listening,
//             stop_listening,
//             get_listener
//         ])
        .run(tauri::generate_context!())
        .expect("error while running tauri application");
}
