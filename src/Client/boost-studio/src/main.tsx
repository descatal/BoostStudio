import React from "react";
import ReactDOM from "react-dom/client";
import {TooltipProvider} from "./components/ui/tooltip";
import {ThemeProvider} from "./components/theme-provider";
import {Toaster} from "./components/ui/toaster";
import {RouterProvider} from "react-router-dom";
import router from "@/router";
import {Menu} from "@/components/menu";
import "./styles/globals.css";

ReactDOM.createRoot(document.getElementById("root") as HTMLElement).render(
  <React.StrictMode>
    <ThemeProvider defaultTheme='system' storageKey='vite-ui-theme' attribute="class" enableSystem>
      <TooltipProvider>
        {'__TAURI__' in window ? <Menu/> : <></>}
        <RouterProvider router={router}/>
        <Toaster/>
      </TooltipProvider>
    </ThemeProvider>
  </React.StrictMode>
);
