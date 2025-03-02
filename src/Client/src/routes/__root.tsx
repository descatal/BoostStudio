import * as React from "react";
import { createRootRoute, Outlet } from "@tanstack/react-router";
import { AppContext } from "@/providers/app-store-provider";
import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import { TooltipProvider } from "@/components/ui/tooltip";
import { NuqsAdapter } from "nuqs/adapters/react";
import { SearchProvider } from "@/context/search-context";
import { Menu } from "@/components/menu";
import Sidebar from "@/components/layout/sidebar";
import { Toaster } from "@/components/ui/toaster";
import { useRef } from "react";
import { createAppStore } from "@/stores/app-store";
import useIsCollapsed from "@/hooks/use-is-collapsed";
import { ThemeProvider } from "@/context/theme-context";
import { SidebarProvider } from "@/components/ui/sidebar";

export const Route = createRootRoute({
  component: RootComponent,
});

const queryClient = new QueryClient();

function RootComponent() {
  const store = useRef(createAppStore()).current;
  const [isCollapsed, setIsCollapsed] = useIsCollapsed();

  return (
    <ThemeProvider defaultTheme="light" storageKey="vite-ui-theme">
      <AppContext.Provider value={store}>
        <QueryClientProvider client={queryClient}>
          <TooltipProvider>
            <NuqsAdapter>
              <SearchProvider>
                {"__TAURI__" in window ? <Menu /> : <></>}
                <div className="relative h-screen overflow-hidden bg-background">
                  <Sidebar
                    isCollapsed={isCollapsed}
                    setIsCollapsed={setIsCollapsed}
                  />
                  <main
                    id="content"
                    className={`
                      overflow-x-hidden pb-20 pt-16 transition-[margin] md:pt-0 ${isCollapsed ? "md:ml-14" : "md:ml-64"} h-full
                    `}
                  >
                    <React.Fragment>
                      <Outlet />
                    </React.Fragment>
                  </main>
                </div>
                <Toaster />
              </SearchProvider>
            </NuqsAdapter>
          </TooltipProvider>
        </QueryClientProvider>
      </AppContext.Provider>
    </ThemeProvider>
  );
}
