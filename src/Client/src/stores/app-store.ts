import { createStore } from "zustand";

interface LinkProps {
  label: string;
  path: string;
}

export interface AppProps {
  transparent: boolean;
  showDevtools: boolean;
  showTopbar: boolean;
  showSidebar: boolean;
  topbarLinks: LinkProps[];
  topbarShowSearch: boolean;
  topbarShowTheme: boolean;
}

export interface AppState extends AppProps {
  setTransparent: (transparent: boolean) => void;
  setShowDevtools: (show: boolean) => void;
  setShowTopbar: (show: boolean) => void;
  setShowSidebar: (show: boolean) => void;
  setTopbarLinks: (links: LinkProps[]) => void;
  setTopbarShowTheme: (show: boolean) => void;
  setTopbarShowSearch: (show: boolean) => void;
}

export type AppStore = ReturnType<typeof createAppStore>;

export const createAppStore = (initProps?: Partial<AppProps>) => {
  const DEFAULT_PROPS: AppProps = {
    transparent: false,
    showDevtools: false,
    showTopbar: true,
    showSidebar: true,
    topbarLinks: [],
    topbarShowSearch: true,
    topbarShowTheme: true,
  };
  return createStore<AppState>()((set) => ({
    ...DEFAULT_PROPS,
    ...initProps,
    setTransparent: (transparent: boolean) => {
      set(() => ({ transparent: transparent }));
    },
    setShowDevtools: (show: boolean) => {
      set(() => ({ showDevtools: show }));
    },
    setShowTopbar: (show: boolean) => {
      set(() => ({ showTopbar: show }));
    },
    setShowSidebar: (show: boolean) => {
      set(() => ({ showSidebar: show }));
    },
    setTopbarLinks: (links: LinkProps[]) => {
      set(() => ({ topbarLinks: links }));
    },
    setTopbarShowTheme: (show: boolean) => {
      set(() => ({ topbarShowSearch: show }));
    },
    setTopbarShowSearch: (show: boolean) => {
      set(() => ({ topbarShowTheme: show }));
    },
  }));
};
