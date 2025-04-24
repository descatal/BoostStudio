import * as React from "react";
import { useRef } from "react";
import { createRootRoute, Outlet } from "@tanstack/react-router";
import { AppContext } from "@/providers/app-store-provider";
import {
  MutationCache,
  QueryCache,
  QueryClient,
  QueryClientProvider,
} from "@tanstack/react-query";
import { TooltipProvider } from "@/components/ui/tooltip";
import { NuqsAdapter } from "nuqs/adapters/react";
import { SearchProvider } from "@/context/search-context";
import { Menu } from "@/components/menu";
import Sidebar from "@/components/layout/sidebar";
import { createAppStore } from "@/stores/app-store";
import useIsCollapsed from "@/hooks/use-is-collapsed";
import { ThemeProvider } from "@/context/theme-context";
import { ReactQueryDevtools } from "@tanstack/react-query-devtools";
import { TanStackRouterDevtools } from "@tanstack/router-devtools";
import Topbar from "@/components/layout/topbar";
import { Toaster } from "@/components/ui/toaster";
import { toast } from "@/hooks/use-toast";
import { ShowErrorToast } from "@/lib/errors";

export const Route = createRootRoute({
  component: RootComponent,
});

const queryClient = new QueryClient({
  queryCache: new QueryCache({
    onError: async (error) => {
      await ShowErrorToast(error);
    },
  }),
  mutationCache: new MutationCache({
    onSettled: async (data, error) => {
      if (error) {
        await ShowErrorToast(error);
      }
    },
  }),
});

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
                <Toaster />
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
                    <Topbar />
                    <React.Fragment>
                      <Outlet />
                    </React.Fragment>
                  </main>
                </div>
              </SearchProvider>
            </NuqsAdapter>
          </TooltipProvider>
          <TanStackRouterDevtools />
          <ReactQueryDevtools initialIsOpen={false} />
        </QueryClientProvider>
      </AppContext.Provider>
    </ThemeProvider>
  );
}
