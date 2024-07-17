import React from "react";
import ReactDOM from "react-dom/client";
import App from "./App";
import "./styles/globals.css";
import {TooltipProvider} from "./components/ui/tooltip";
import {ThemeProvider} from "./components/theme-provider";
import {Toaster} from "./components/ui/toaster";
import {RouterProvider} from "react-router-dom";
import router from "@/router";
import AppShell from "@/components/app-shell";
import {Menu} from "@/components/menu";

ReactDOM.createRoot(document.getElementById("root") as HTMLElement).render(
  <React.StrictMode>
    <ThemeProvider defaultTheme='system' storageKey='vite-ui-theme' attribute="class" enableSystem>
      <TooltipProvider>
        {'__TAURI__' in window ? <Menu></Menu> : <></>}
        <RouterProvider router={router}/>
        <Toaster/>
      </TooltipProvider>
    </ThemeProvider>
  </React.StrictMode>
);
