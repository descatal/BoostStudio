use log::{debug, info};
use tauri::AppHandle;
use tauri_plugin_shell::process::CommandEvent;
use tauri_plugin_shell::ShellExt;


pub(crate) async fn spawn(app_handle: &AppHandle) {
    info!("Spawning sidecar process");
    let command = app_handle
        .shell()
        .sidecar("BoostStudio.Web")
        .expect("couldn't get sidecar executable");

    let (mut rx, mut _child) = command.spawn().expect("failed to spawn sidecar");

    tauri::async_runtime::spawn(async move {
        while let Some(event) = rx.recv().await {
            match event {
                CommandEvent::Stdout(bytes) => {
                    debug!("{}", String::from_utf8_lossy(&bytes).trim());
                }
                _ => {}
            }
        }
    });
}