import { useRef } from "react";
import { createRootRoute, Outlet } from "@tanstack/react-router";
import { AppContext, useAppContext } from "@/providers/app-store-provider";
import {
  MutationCache,
  QueryCache,
  QueryClient,
  QueryClientProvider,
} from "@tanstack/react-query";
import { TooltipProvider } from "@/components/ui/tooltip";
import { NuqsAdapter } from "nuqs/adapters/react";
import { SearchProvider } from "@/context/search-context";
import Sidebar from "@/components/layout/sidebar";
import { createAppStore } from "@/stores/app-store";
import useIsCollapsed from "@/hooks/use-is-collapsed";
import { ThemeProvider } from "@/context/theme-context";
import { ReactQueryDevtools } from "@tanstack/react-query-devtools";
import Topbar from "@/components/layout/topbar";
import { Toaster } from "@/components/ui/sonner";
import { ShowErrorToast } from "@/features/errors/toast-errors.tsx";
import GeneralError from "@/features/errors/general-error";
import NotFoundError from "@/features/errors/not-found-error";
import { cn } from "@/lib/utils";
import { TanStackRouterDevtools } from "@tanstack/react-router-devtools";

export const Route = createRootRoute({
  component: RootComponent,
  notFoundComponent: NotFoundError,
  errorComponent: GeneralError,
});

const queryClient = new QueryClient({
  queryCache: new QueryCache({
    onError: async (error) => {
      await ShowErrorToast(error);
    },
  }),
  mutationCache: new MutationCache({
    onSettled: async (_, error) => {
      if (error) {
        await ShowErrorToast(error);
      }
    },
  }),
});

function RootComponent() {
  const store = useRef(createAppStore()).current;

  return (
    <ThemeProvider defaultTheme="light" storageKey="vite-ui-theme">
      <AppContext.Provider value={store}>
        <QueryClientProvider client={queryClient}>
          <TooltipProvider>
            <NuqsAdapter>
              <SearchProvider>
                <Toaster />
                <RouterComponent />
              </SearchProvider>
            </NuqsAdapter>
          </TooltipProvider>
        </QueryClientProvider>
      </AppContext.Provider>
    </ThemeProvider>
  );
}

function RouterComponent() {
  const [isCollapsed, setIsCollapsed] = useIsCollapsed();
  const showSidebar = useAppContext((state) => state.showSidebar);
  const showTopBar = useAppContext((state) => state.showTopbar);
  const showDevtools = useAppContext((state) => state.showDevtools);
  const transparent = useAppContext((state) => state.transparent);

  return (
    <>
      <div
        className={`overflow-x-hidden transition-[margin] ${showSidebar && cn(isCollapsed ? "md:ml-14" : "md:ml-64")} ${showSidebar && "md:pt-0 pb-20 pt-16"}`}
      >
        {showTopBar && <Topbar />}
        <div
          className={`relative ${showTopBar ? `h-[calc(100vh-128px)] md:h-[calc(100vh-64px)]` : "h-screen"} overflow-auto ${transparent ? "bg-transparent" : "bg-background"}`}
        >
          {showSidebar && (
            <Sidebar
              isCollapsed={isCollapsed}
              setIsCollapsed={setIsCollapsed}
            />
          )}
          <main id="content" className={"h-full"}>
            <Outlet />
          </main>
        </div>
      </div>
      {showDevtools && (
        <>
          <TanStackRouterDevtools />
          <ReactQueryDevtools initialIsOpen={false} />
        </>
      )}
    </>
  );
}
