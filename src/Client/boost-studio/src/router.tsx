import { createBrowserRouter, Navigate } from "react-router-dom"

import App from "./App"
import GeneralError from "./pages/errors/general-error"
import MaintenanceError from "./pages/errors/maintenance-error"
import NotFoundError from "./pages/errors/not-found-error"
import UnauthorisedError from "./pages/errors/unauthorised-error"

const router = createBrowserRouter([
  // Main routes
  {
    path: "/",
    lazy: async () => {
      const AppShell = await import("./components/app-shell")
      return { Component: AppShell.default }
    },
    errorElement: <GeneralError />,
    children: [
      {
        index: true,
        element: <Navigate to="/units" replace />,
      },
      {
        path: "units",
        lazy: async () => ({
          Component: (await import("./pages/units")).default,
        }),
      },
      {
        path: "units/:unitId/customize/",
        lazy: async () => ({
          Component: (await import("./pages/units/customize")).default,
        }),
        errorElement: <GeneralError />,
        children: [
          {
            index: true,
            element: <Navigate to="info" replace />,
          },
          {
            path: "info",
            element: <Navigate to="stats" replace />,
          },
          {
            path: "info/:infoTab",
            lazy: async () => ({
              Component: (await import("./pages/units/customize/information"))
                .default,
            }),
          },
          {
            path: "script",
            lazy: async () => ({
              Component: (await import("./pages/units/customize/scripts"))
                .default,
            }),
          },
          {
            path: "assets",
            lazy: async () => ({
              Component: (await import("./pages/units/customize/assets"))
                .default,
            }),
          },
        ],
      },
      {
        path: "live",
        lazy: async () => ({
          Component: (await import("./pages/live")).default,
        }),
      },
      {
        path: "patches",
        children: [
          {
            index: true,
            element: <Navigate to="All" replace />,
          },
          {
            path: ":patchId",
            lazy: async () => ({
              Component: (await import("./pages/patches")).default,
            }),
          },
        ],
      },
      {
        path: "tools",
        lazy: async () => ({
          Component: (await import("./pages/tools")).default,
        }),
        children: [
          {
            index: true,
            element: <Navigate to="/tools/assets" replace />,
          },
          {
            path: "assets",
            lazy: async () => ({
              Component: (await import("./pages/tools/assets")).default,
            }),
          },
          {
            path: "psarc",
            lazy: async () => ({
              Component: (await import("./pages/tools/psarc")).default,
            }),
          },
          {
            path: "scripts",
            lazy: async () => ({
              Component: (await import("./pages/tools/scripts")).default,
            }),
          },
        ],
      },
      {
        path: "settings",
        lazy: async () => ({
          Component: (await import("./pages/settings")).default,
        }),
        errorElement: <GeneralError />,
        children: [
          {
            index: true,
            element: <Navigate to="/settings/general" replace />,
          },
          {
            path: "general",
            lazy: async () => ({
              Component: (await import("./pages/settings/general")).default,
            }),
          },
          {
            path: "appearance",
            lazy: async () => ({
              Component: (await import("./pages/settings/appearance")).default,
            }),
          },
        ],
      },
    ],
  },

  // Error routes
  { path: "/500", Component: GeneralError },
  { path: "/404", Component: NotFoundError },
  { path: "/503", Component: MaintenanceError },
  { path: "/401", Component: UnauthorisedError },

  // Fallback 404 route
  { path: "*", Component: NotFoundError },
])

export default router
