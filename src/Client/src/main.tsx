import React from "react";
import ReactDOM from "react-dom/client";
import {ThemeProvider} from "./components/theme-provider";
import "./styles/globals.css";
import App from "@/App";

ReactDOM.createRoot(document.getElementById("root") as HTMLElement).render(
  <React.StrictMode>
    <ThemeProvider defaultTheme='system' storageKey='vite-ui-theme' attribute="class" enableSystem>
      <App/>
    </ThemeProvider>
  </React.StrictMode>
);
