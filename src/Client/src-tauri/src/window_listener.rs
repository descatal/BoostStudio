// use serde::Serialize;
// use std::{
//     sync::atomic::{AtomicBool, Ordering},
//     thread,
//     time::Duration,
// };
// use tauri::{webview, Emitter, LogicalPosition, LogicalSize, Manager, Position, Size};
// use windows::{Win32::Foundation::*, Win32::UI::WindowsAndMessaging::*};
//
// struct WindowInfo {
//     title: String,
//     left: i32,
//     top: i32,
//     width: i32,
//     height: i32,
//     is_foreground: bool,
// }
//
// // Simple structure to pass data to callback
// struct EnumWindowsData {
//     found_windows: Vec<WindowInfo>,
//     keywords: Vec<String>,
// }
//
// // Global running flag
// static RUNNING: AtomicBool = AtomicBool::new(false);
// static mut MONITOR_THREAD: Option<thread::JoinHandle<()>> = None;
//
// #[derive(Clone, Serialize)]
// #[serde(rename_all = "camelCase")]
// struct OverlayListenerStarted {
//     listener_interval: u64,
//     target_window_titles: Vec<String>,
// }
//
// #[derive(Clone, Serialize)]
// #[serde(rename_all = "camelCase")]
// struct OverlayListenerProgress {
//     target_window_found: bool,
// }
//
// #[derive(Clone, Serialize)]
// #[serde(rename_all = "camelCase")]
// struct OverlayListenerStopped {}
//
// #[tauri::command]
// pub async fn get_listener() -> bool {
//     // return true if the listener is running
//     RUNNING.load(Ordering::SeqCst)
// }
//
// #[tauri::command]
// pub async fn start_listening(
//     app: tauri::AppHandle,
//     interval: u64,
//     keywords: Vec<String>,
//     path: String,
// ) {
//     // if keywords is empty, coalesce to default values
//     let keywords = if keywords.is_empty() {
//         vec!["BLJS10250".to_string(), "NPJB00512".to_string()]
//     } else {
//         keywords
//     };
//
//     // if interval is 0, coalesce to default value
//     let interval = if interval == 0 { 1000 } else { interval };
//
//     // if path is empty, coalesce to default value
//     let path = if path.is_empty() {
//         "/overlay/match-info".to_string()
//     } else {
//         path
//     };
//
//     println!("Starting listener...");
//
//     let webview_window =
//         tauri::WebviewWindowBuilder::new(&app, "overlay", tauri::WebviewUrl::App(path.into()))
//             .closable(false)
//             .decorations(false)
//             .resizable(false)
//             .minimizable(false)
//             .maximizable(false)
//             .transparent(true)
//             .build()
//             .unwrap();
//
//     start_monitoring(&app, keywords, interval, webview_window.clone());
//     webview_window.show().unwrap();
// }
//
// #[tauri::command]
// pub async fn stop_listening(app: tauri::AppHandle) {
//     println!("Stopping listener...");
//     stop_monitoring(&app);
//
//     let webview_window = app.get_webview_window("overlay").unwrap();
//     webview_window.close().unwrap();
// }
//
// fn start_monitoring(
//     app: &tauri::AppHandle,
//     keywords: Vec<String>,
//     interval_ms: u64,
//     webview_window: webview::WebviewWindow,
// ) {
//     // Don't start if already running
//     if RUNNING.load(Ordering::SeqCst) {
//         println!("Monitor is already running");
//         return;
//     }
//
//     // Set running flag
//     RUNNING.store(true, Ordering::SeqCst);
//
//     app.emit(
//         "overlay-started",
//         OverlayListenerStarted {
//             listener_interval: interval_ms,
//             target_window_titles: keywords.to_vec(),
//         },
//     )
//     .unwrap();
//
//     println!(
//         "Starting monitor for windows every {} ms with titles containing \"{}\"",
//         interval_ms,
//         keywords.join("\" or \"")
//     );
//
//     // Clone the app handle before moving it into the thread
//     let app_handle = app.clone();
//
//     // Create and store thread handle
//     let handle = thread::spawn(move || {
//         while RUNNING.load(Ordering::SeqCst) {
//             // Create data structure with empty vector and keywords
//             let mut callback_data = EnumWindowsData {
//                 found_windows: Vec::new(),
//                 keywords: keywords.clone(),
//             };
//
//             // Find matching windows
//             unsafe {
//                 let _ = EnumWindows(
//                     Some(find_matching_windows),
//                     LPARAM(&mut callback_data as *mut _ as isize),
//                 );
//             }
//
//             // Print information for each found window
//             if callback_data.found_windows.is_empty() {
//                 webview_window.hide().ok();
//                 app_handle
//                     .emit(
//                         "overlay-progress",
//                         OverlayListenerProgress {
//                             target_window_found: false,
//                         },
//                     )
//                     .unwrap();
//             } else {
//                 for window in &callback_data.found_windows {
//                     let pos = Position::new(LogicalPosition::new(window.left, window.top + 30));
//                     webview_window.set_position(pos).ok();
//
//                     let size = Size::new(LogicalSize::new(window.width - 15, window.height - 38));
//                     webview_window.set_size(size).ok();
//
//                     let is_webview_focused = webview_window.is_focused().unwrap_or(false);
//
//                     if window.is_foreground {
//                         // println!("In foreground, adding always on top: {}", window.title);
//                         webview_window.set_always_on_top(true).ok();
//                         webview_window.show().ok();
//                     } else {
//                         // println!(
//                         //     "Not in foreground, removing always on top: {}",
//                         //     window.title
//                         // );
//                         if !is_webview_focused {
//                             webview_window.set_always_on_top(false).ok();
//                             webview_window.hide().ok();
//                         }
//                     }
//
//                     app_handle
//                         .emit(
//                             "overlay-progress",
//                             OverlayListenerProgress {
//                                 target_window_found: true,
//                             },
//                         )
//                         .unwrap();
//                 }
//             }
//
//             // Wait before checking again
//             thread::sleep(Duration::from_millis(interval_ms));
//         }
//
//         println!("Monitor handler stopped");
//     });
//
//     // Store thread handle
//     unsafe {
//         MONITOR_THREAD = Some(handle);
//     }
// }
//
// fn stop_monitoring(app: &tauri::AppHandle) {
//     if !RUNNING.load(Ordering::SeqCst) {
//         println!("Monitor is not running");
//         return;
//     }
//
//     // Set flag to stop the loop
//     RUNNING.store(false, Ordering::SeqCst);
//
//     // Wait for thread to finish
//     unsafe {
//         if let Some(handle) = MONITOR_THREAD.take() {
//             let _ = handle.join();
//         }
//     }
//
//     app.emit("overlay-stopped", OverlayListenerStopped {})
//         .unwrap();
//
//     println!("Monitor stopped");
// }
//
// extern "system" fn find_matching_windows(window: HWND, lparam: LPARAM) -> BOOL {
//     unsafe {
//         let data = &mut *(lparam.0 as *mut EnumWindowsData);
//
//         let mut text: [u16; 512] = [0; 512];
//         let len = GetWindowTextW(window, &mut text);
//         let text = String::from_utf16_lossy(&text[..len as usize]).to_lowercase();
//
//         let mut info = WINDOWINFO {
//             cbSize: core::mem::size_of::<WINDOWINFO>() as u32,
//             ..Default::default()
//         };
//
//         if GetWindowInfo(window, &mut info).is_ok() {
//             if !text.is_empty()
//                 && info.dwStyle.contains(WS_VISIBLE)
//                 && (data
//                     .keywords
//                     .iter()
//                     .any(|keyword| text.contains(keyword.to_lowercase().as_str())))
//             {
//                 let mut is_foreground = false;
//                 if GetForegroundWindow() == window {
//                     is_foreground = true;
//                 }
//
//                 // Store window information
//                 data.found_windows.push(WindowInfo {
//                     title: text,
//                     left: info.rcWindow.left,
//                     top: info.rcWindow.top,
//                     width: info.rcWindow.right - info.rcWindow.left,
//                     height: info.rcWindow.bottom - info.rcWindow.top,
//                     is_foreground,
//                 });
//             }
//         }
//
//         true.into()
//     }
// }
