// mod window_listener;
// use window_listener::{get_listener, start_listening, stop_listening};

use tauri::{generate_handler, plugin::TauriPlugin, Runtime};

mod sidecar;

#[cfg_attr(mobile, tauri::mobile_entry_point)]
pub fn run() {
    let mut builder = tauri::Builder::default()
        .plugin(tauri_plugin_opener::init())
        .plugin(tauri_plugin_shell::init());

    // Setup
    builder = builder.setup(|app| {
        if cfg!(debug_assertions) {
            app.handle().plugin(build_log_plugin())?;
        }

        let app_handle_copy = app.handle().clone();

        tauri::async_runtime::spawn(async move {
            sidecar::spawn(&app_handle_copy).await;
        });

        Ok(())
    });

    builder = builder.invoke_handler(generate_handler![
        // add commands to be invoked in frontend code
        // start_listening,
        // stop_listening,
        // get_listener
    ]);

    builder
        .run(tauri::generate_context!())
        .expect("error while running tauri application");
}

fn build_log_plugin<R: Runtime>() -> TauriPlugin<R> {
    tauri_plugin_log::Builder::default()
        .level(log::LevelFilter::Debug)
        .build()
}