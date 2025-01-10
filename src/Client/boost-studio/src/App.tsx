import React, {useRef} from "react"

import {Menu} from "@/components/menu"
import {TooltipProvider} from "./components/ui/tooltip"
import {createAppStore} from "@/stores/app-store";
import {AppContext} from "@/providers/app-store-provider";
import {RouterProvider} from "react-router-dom";
import router from "@/router";
import {Toaster} from "@/components/ui/toaster";
import {QueryClient, QueryClientProvider} from "@tanstack/react-query";

const queryClient = new QueryClient()

function App() {
  const store = useRef(createAppStore()).current

  return (
    <AppContext.Provider value={store}>
      <QueryClientProvider client={queryClient}>
        <TooltipProvider>
          {'__TAURI__' in window ? <Menu/> : <></>}
          <RouterProvider router={router}/>
          <Toaster/>
        </TooltipProvider>
      </QueryClientProvider>
    </AppContext.Provider>
  )
}

export default App
