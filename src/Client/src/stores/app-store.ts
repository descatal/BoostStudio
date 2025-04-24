import { createStore } from "zustand";

interface LinkProps {
  label: string;
  path: string;
}

export interface AppProps {
  topbarLinks: LinkProps[];
  topbarShowSearch: boolean;
  topbarShowTheme: boolean;
}

export interface AppState extends AppProps {
  setTopbarLinks: (links: LinkProps[]) => void;
  setTopbarShowTheme: (show: boolean) => void;
  setTopbarShowSearch: (show: boolean) => void;
}

export type AppStore = ReturnType<typeof createAppStore>;

export const createAppStore = (initProps?: Partial<AppProps>) => {
  const DEFAULT_PROPS: AppProps = {
    topbarLinks: [],
    topbarShowSearch: true,
    topbarShowTheme: true,
  };
  return createStore<AppState>()((set) => ({
    ...DEFAULT_PROPS,
    ...initProps,
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
